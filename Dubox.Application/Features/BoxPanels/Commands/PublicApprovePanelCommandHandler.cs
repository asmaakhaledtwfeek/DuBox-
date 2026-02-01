using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Dubox.Application.Features.BoxPanels.Commands;

public class PublicApprovePanelCommandHandler : IRequestHandler<PublicApprovePanelCommand, Result<PublicPanelApprovalDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;

    public PublicApprovePanelCommandHandler(
        IUnitOfWork unitOfWork,
        IDbContext dbContext,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
        _currentUserService = currentUserService;
    }

    public async Task<Result<PublicPanelApprovalDto>> Handle(PublicApprovePanelCommand request, CancellationToken cancellationToken)
    {
        // Find panel by ID
        var panel = await _dbContext.BoxPanels
            .Include(p => p.Box)
            .FirstOrDefaultAsync(p => p.BoxPanelId == request.BoxPanelId, cancellationToken);

        if (panel == null)
            return Result.Failure<PublicPanelApprovalDto>("Panel not found");

        // Validate approval token
        var expectedToken = GenerateApprovalToken(panel.BoxPanelId, panel.QRCode ?? "");
        if (request.ApprovalToken != expectedToken)
            return Result.Failure<PublicPanelApprovalDto>("Invalid approval token. QR code may be tampered or expired.");

        // Check if box is dispatched
        if (panel.Box.Status == BoxStatusEnum.Dispatched)
            return Result.Failure<PublicPanelApprovalDto>("Cannot approve panel. Box is dispatched and read-only.");

        var userId = _currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
            return Result.Failure<PublicPanelApprovalDto>("Authentication required. Please log in to approve the panel.");

        var currentUserId = Guid.Parse(userId);
        var approvalTime = DateTime.UtcNow;

        string message = "";
        string approvalStatus = "Approved";
        string actualApprovalType = "";

        // AUTO-DETERMINE approval type based on current panel status (ONE QR CODE FOR BOTH)
        // First approval only when panel workflow is completed (panel status Completed or FirstApprovalPending)
        if (panel.PanelStatus == PanelStatusEnum.Completed || panel.PanelStatus == PanelStatusEnum.FirstApprovalPending)
        {
            if (panel.FirstApprovalStatus == "Approved")
                return Result.Failure<PublicPanelApprovalDto>("First approval has already been processed for this panel.");

            // First approval
            actualApprovalType = "First";
            panel.FirstApprovalStatus = "Approved";
            panel.FirstApprovalBy = currentUserId;
            panel.FirstApprovalDate = approvalTime;
            panel.FirstApprovalNotes = $"Approved via QR code scan from mobile device. Location: ({request.Latitude}, {request.Longitude}). Device: {request.DeviceInfo}";
            if (!string.IsNullOrEmpty(request.Notes))
            {
                panel.FirstApprovalNotes += $" Notes: {request.Notes}";
            }
            panel.PanelStatus = PanelStatusEnum.FirstApprovalApproved;
            panel.SecondApprovalStatus = "Pending";

            message = "First approval completed successfully. Panel is now awaiting second approval.";
        }
        else if (panel.PanelStatus == PanelStatusEnum.FirstApprovalApproved)
        {
            // Second approval
            actualApprovalType = "Second";

            // Check if second approval is already done
            if (panel.SecondApprovalStatus == "Approved")
            {
                return Result.Failure<PublicPanelApprovalDto>("Second approval has already been processed for this panel.");
            }

            panel.SecondApprovalStatus = "Approved";
            panel.SecondApprovalBy = currentUserId;
            panel.SecondApprovalDate = approvalTime;
            panel.SecondApprovalNotes = $"Approved via QR code scan from mobile device. Location: ({request.Latitude}, {request.Longitude}). Device: {request.DeviceInfo}";
            if (!string.IsNullOrEmpty(request.Notes))
            {
                panel.SecondApprovalNotes += $" Notes: {request.Notes}";
            }
            panel.PanelStatus = PanelStatusEnum.SecondApprovalApproved;

            message = "Second approval completed successfully. Panel is fully approved.";
        }
        else
        {
            if (panel.PanelStatus == PanelStatusEnum.NotStarted || panel.PanelStatus == PanelStatusEnum.InProgress || panel.PanelStatus == PanelStatusEnum.OnHold)
                return Result.Failure<PublicPanelApprovalDto>("Panel must complete workflow (all 4 stages) before first approval.");
            return Result.Failure<PublicPanelApprovalDto>("Panel has already been fully approved or is in an invalid state.");
        }

        // Create scan log for tracking
        var scanLog = new PanelScanLog
        {
            BoxPanelId = panel.BoxPanelId,
            Barcode = panel.QRCode ?? "",
            ScanType = $"Public{actualApprovalType}Approval",
            ScanLocation = $"GPS: {request.Latitude}, {request.Longitude}",
            ScannedBy = currentUserId,
            ScannedDate = approvalTime,
            Latitude = request.Latitude != null ? (decimal)request.Latitude : null,
            Longitude = request.Longitude != null ? (decimal)request.Longitude : null,
            Notes = $"Public approval via QR scan. Device: {request.DeviceInfo}"
        };

        await _dbContext.PanelScanLogs.AddAsync(scanLog, cancellationToken);

        panel.ModifiedDate = approvalTime;
        panel.ModifiedBy = currentUserId;

        _unitOfWork.Repository<BoxPanel>().Update(panel);

        // Check if all panels in the box are SecondApprovalApproved and update box status to ReadyToStart
        if (actualApprovalType == "Second" && panel.Box.Status == BoxStatusEnum.NotStarted)
        {
            // Get all OTHER panels for this box (excluding current panel)
            var otherPanelsInBox = await _dbContext.BoxPanels
                .Where(bp => bp.BoxId == panel.BoxId && bp.BoxPanelId != panel.BoxPanelId)
                .ToListAsync(cancellationToken);

            // Check if all other panels are also SecondApprovalApproved
            // Current panel is already updated to SecondApprovalApproved above
            var allOtherPanelsApproved = !otherPanelsInBox.Any() || 
                otherPanelsInBox.All(bp => bp.PanelStatus == PanelStatusEnum.SecondApprovalApproved);

            if (allOtherPanelsApproved)
            {
                // All panels including current one are approved, update box status
                var box = await _unitOfWork.Repository<Box>().GetByIdAsync(panel.BoxId, cancellationToken);
                if (box != null && box.Status == BoxStatusEnum.NotStarted)
                {
                    box.Status = BoxStatusEnum.ReadyToStart;
                    box.ModifiedDate = approvalTime;
                    box.ModifiedBy = currentUserId;
                    _unitOfWork.Repository<Box>().Update(box);
                }
            }
        }

        // Save all changes (panel + box if applicable) in one transaction
        await _unitOfWork.CompleteAsync(cancellationToken);

        var result = new PublicPanelApprovalDto
        {
            BoxPanelId = panel.BoxPanelId,
            PanelName = panel.PanelName,
            ApprovalType = actualApprovalType,
            ApprovalStatus = approvalStatus,
            ApprovalDate = approvalTime,
            Message = message
        };

        return Result.Success(result);
    }

    /// <summary>
    /// Generates a security token for panel approval
    /// Uses BoxPanelId and QRCode to create a unique, reproducible token
    /// </summary>
    private string GenerateApprovalToken(Guid boxPanelId, string qrCode)
    {
        var data = $"{boxPanelId}|{qrCode}|DUBOX_PANEL_APPROVAL";
        using var sha256 = SHA256.Create();
        var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(data));
        return Convert.ToBase64String(hash).Substring(0, 16); // Take first 16 chars for brevity
    }
}

