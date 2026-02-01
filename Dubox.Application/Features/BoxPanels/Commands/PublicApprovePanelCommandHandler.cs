using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
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

    public PublicApprovePanelCommandHandler(
        IUnitOfWork unitOfWork,
        IDbContext dbContext)
    {
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
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

        var approvalTime = DateTime.UtcNow;
        var systemUserId = Guid.Empty; // Use system user for public approvals

        string message = "";
        string approvalStatus = "Approved";
        string actualApprovalType = "";

        // AUTO-DETERMINE approval type based on current panel status (ONE QR CODE FOR BOTH)
        if (panel.PanelStatus == PanelStatusEnum.NotStarted)
        {
            // First approval
            actualApprovalType = "First";
            panel.FirstApprovalStatus = "Approved";
            panel.FirstApprovalBy = systemUserId;
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
            panel.SecondApprovalBy = systemUserId;
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
            return Result.Failure<PublicPanelApprovalDto>("Panel has already been fully approved or is in an invalid state.");
        }

        // Create scan log for tracking
        var scanLog = new PanelScanLog
        {
            BoxPanelId = panel.BoxPanelId,
            Barcode = panel.QRCode ?? "",
            ScanType = $"Public{actualApprovalType}Approval",
            ScanLocation = $"GPS: {request.Latitude}, {request.Longitude}",
            ScannedBy = systemUserId,
            ScannedDate = approvalTime,
            Latitude = request.Latitude != null ? (decimal)request.Latitude : null,
            Longitude = request.Longitude != null ? (decimal)request.Longitude : null,
            Notes = $"Public approval via QR scan. Device: {request.DeviceInfo}"
        };

        await _dbContext.PanelScanLogs.AddAsync(scanLog, cancellationToken);

        panel.ModifiedDate = approvalTime;
        panel.ModifiedBy = systemUserId;

        _unitOfWork.Repository<BoxPanel>().Update(panel);
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

