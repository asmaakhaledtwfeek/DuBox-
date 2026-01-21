using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.BoxPanels.Commands;

public class ScanPanelBarcodeCommandHandler : IRequestHandler<ScanPanelBarcodeCommand, Result<BoxPanelDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;

    public ScanPanelBarcodeCommandHandler(
        IUnitOfWork unitOfWork,
        IDbContext dbContext,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
        _currentUserService = currentUserService;
    }

    public async Task<Result<BoxPanelDto>> Handle(ScanPanelBarcodeCommand request, CancellationToken cancellationToken)
    {
        // Find panel by barcode
        var panel = await _dbContext.BoxPanels
            .Include(p => p.Box)
            .FirstOrDefaultAsync(p => p.Barcode == request.Barcode, cancellationToken);

        if (panel == null)
            return Result.Failure<BoxPanelDto>($"Panel with barcode '{request.Barcode}' not found");

        // Check if box is dispatched - cannot modify panels
        if (panel.Box.Status == BoxStatusEnum.Dispatched)
            return Result.Failure<BoxPanelDto>("Cannot scan panel. Box is dispatched and read-only.");

        // Check if panel status is Second Approval Approved - cannot scan again
        if (panel.PanelStatus == PanelStatusEnum.SecondApprovalApproved)
            return Result.Failure<BoxPanelDto>("Cannot scan panel. Panel has already been approved with Second Approval and cannot be scanned again.");

        // Check if second approval is pending - cannot scan again until approved or rejected
        if (panel.PanelStatus == PanelStatusEnum.FirstApprovalApproved && panel.SecondApprovalStatus == "Pending")
            return Result.Failure<BoxPanelDto>("Cannot scan panel. Second approval is pending. Please approve or reject the second approval before scanning again.");

        var currentUserId = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());
        var scanTime = DateTime.UtcNow;

        // Check if this is the first scan (NotStarted status) - automatically approve first approval
        bool isFirstScan = panel.PanelStatus == PanelStatusEnum.NotStarted;
        
        if (isFirstScan)
        {
            // Automatically approve first approval on first scan
            panel.FirstApprovalStatus = "Approved";
            panel.FirstApprovalBy = currentUserId;
            panel.FirstApprovalDate = scanTime;
            panel.FirstApprovalNotes = $"Automatically approved on first scan at {scanTime:yyyy-MM-dd HH:mm:ss} UTC";
            panel.PanelStatus = PanelStatusEnum.FirstApprovalApproved;
            panel.SecondApprovalStatus = "Pending";
        }
        // Handle second scan: when panel has FirstApprovalApproved status, create second scan opportunity
        else if (panel.PanelStatus == PanelStatusEnum.FirstApprovalApproved)
        {
            // If second approval was rejected, set it to Pending for approval/rejection
            // Note: We already checked above that SecondApprovalStatus is not "Pending"
            bool wasRejected = panel.SecondApprovalStatus == "Rejected";
            if (wasRejected || string.IsNullOrEmpty(panel.SecondApprovalStatus))
            {
                panel.SecondApprovalStatus = "Pending";
                // Clear previous rejection data to allow fresh second approval
                if (wasRejected)
                {
                    panel.SecondApprovalDate = null;
                    panel.SecondApprovalBy = null;
                    panel.SecondApprovalNotes = null;
                }
            }
        }
        // Handle re-scanning after second approval rejection: allow scanning again
        else if (panel.PanelStatus == PanelStatusEnum.SecondApprovalRejected)
        {
            // Reset second approval to Pending to allow re-scanning and new approval/rejection
            panel.SecondApprovalStatus = "Pending";
            panel.SecondApprovalDate = null;
            panel.SecondApprovalBy = null;
            panel.SecondApprovalNotes = null;
            // Keep status as FirstApprovalApproved to allow second scan workflow
            panel.PanelStatus = PanelStatusEnum.FirstApprovalApproved;
        }

        // Create scan log
        var scanLog = new PanelScanLog
        {
            BoxPanelId = panel.BoxPanelId,
            Barcode = request.Barcode,
            ScanType = request.ScanType,
            ScanLocation = request.ScanLocation,
            ScannedBy = currentUserId,
            ScannedDate = scanTime,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            Notes = request.Notes
        };

        await _dbContext.PanelScanLogs.AddAsync(scanLog, cancellationToken);

       

        panel.ModifiedDate = scanTime;
        panel.ModifiedBy = currentUserId;

        _unitOfWork.Repository<BoxPanel>().Update(panel);
        await _unitOfWork.CompleteAsync(cancellationToken);

        var dto = new BoxPanelDto
        {
            BoxPanelId = panel.BoxPanelId,
            BoxId = panel.BoxId,
            ProjectId = panel.ProjectId,
            PanelName = panel.PanelName,
            PanelStatus = panel.PanelStatus,
            CreatedDate = panel.CreatedDate,
            ModifiedDate = panel.ModifiedDate
        };

        return Result.Success(dto);
    }
}

