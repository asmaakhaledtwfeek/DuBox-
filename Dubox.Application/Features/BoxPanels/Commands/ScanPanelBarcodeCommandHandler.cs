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

        var currentUserId = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());
        var scanTime = DateTime.UtcNow;

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

        // Update panel status based on scan type
        switch (request.ScanType.ToLower())
        {
            case "dispatch":
                panel.PanelStatus = PanelStatusEnum.InTransit;
                panel.DispatchedDate = scanTime;
                panel.CurrentLocationStatus = "InTransit";
                break;

            case "sitearrival":
                panel.PanelStatus = PanelStatusEnum.ArrivedFactory;
                panel.ActualArrivalDate = scanTime;
                panel.ScannedAtFactory = scanTime;
                panel.CurrentLocationStatus = "ArrivedSite";
                // Auto-approve on scan (First Approval)
                panel.PanelStatus = PanelStatusEnum.FirstApprovalApproved;
                panel.FirstApprovalStatus = "Approved";
                panel.FirstApprovalBy = currentUserId;
                panel.FirstApprovalDate = scanTime;
                panel.FirstApprovalNotes = "Auto-approved via barcode scan";
                break;

            case "installation":
                panel.PanelStatus = PanelStatusEnum.Installed;
                panel.InstalledDate = scanTime;
                panel.CurrentLocationStatus = "Installed";
                break;

            case "inspection":
                // Inspection scan doesn't change status, just logs the event
                break;
        }

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

