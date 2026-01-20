using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.BoxPanels.Commands;

public class ApprovePanelFirstApprovalCommandHandler : IRequestHandler<ApprovePanelFirstApprovalCommand, Result<BoxPanelDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;

    public ApprovePanelFirstApprovalCommandHandler(
        IUnitOfWork unitOfWork,
        IDbContext dbContext,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
        _currentUserService = currentUserService;
    }

    public async Task<Result<BoxPanelDto>> Handle(ApprovePanelFirstApprovalCommand request, CancellationToken cancellationToken)
    {
        var panel = await _dbContext.BoxPanels
            .Include(p => p.Box)
            .FirstOrDefaultAsync(p => p.BoxPanelId == request.BoxPanelId, cancellationToken);

        if (panel == null)
            return Result.Failure<BoxPanelDto>("Panel not found");

        // Check if box is dispatched
        if (panel.Box.Status == BoxStatusEnum.Dispatched)
            return Result.Failure<BoxPanelDto>("Cannot approve panel. Box is dispatched and read-only.");

        // Validate approval status
        if (request.ApprovalStatus != "Approved" && request.ApprovalStatus != "Rejected")
            return Result.Failure<BoxPanelDto>("Invalid approval status. Must be 'Approved' or 'Rejected'");

        var currentUserId = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());
        var approvalTime = DateTime.UtcNow;

        panel.FirstApprovalStatus = request.ApprovalStatus;
        panel.FirstApprovalBy = currentUserId;
        panel.FirstApprovalDate = approvalTime;
        panel.FirstApprovalNotes = request.Notes;

        // Update panel status based on approval
        if (request.ApprovalStatus == "Approved")
        {
            panel.PanelStatus = PanelStatusEnum.FirstApprovalApproved;
            // Auto-set to second approval pending
            panel.SecondApprovalStatus = "Pending";
            panel.PanelStatus = PanelStatusEnum.SecondApprovalPending;
        }
        else
        {
            panel.PanelStatus = PanelStatusEnum.FirstApprovalRejected;
            panel.CurrentLocationStatus = "Rejected";
        }

        panel.ModifiedDate = approvalTime;
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

