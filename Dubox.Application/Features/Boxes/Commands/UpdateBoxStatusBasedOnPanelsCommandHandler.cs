using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Boxes.Commands;

public class UpdateBoxStatusBasedOnPanelsCommandHandler : IRequestHandler<UpdateBoxStatusBasedOnPanelsCommand, Result<string>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;

    public UpdateBoxStatusBasedOnPanelsCommandHandler(
        IUnitOfWork unitOfWork,
        IDbContext dbContext,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
        _currentUserService = currentUserService;
    }

    public async Task<Result<string>> Handle(UpdateBoxStatusBasedOnPanelsCommand request, CancellationToken cancellationToken)
    {
        var box = await _unitOfWork.Repository<Box>().GetByIdAsync(request.BoxId, cancellationToken);

        if (box == null)
            return Result.Failure<string>("Box not found");

        // Only process boxes that are in NotStarted status
        if (box.Status != BoxStatusEnum.NotStarted)
            return Result.Failure<string>($"Box is not in NotStarted status. Current status: {box.Status}");

        // Get all panels for this box
        var allPanels = await _dbContext.BoxPanels
            .Where(bp => bp.BoxId == request.BoxId)
            .ToListAsync(cancellationToken);

        if (!allPanels.Any())
            return Result.Failure<string>("Box has no panels");

        // Check if all panels are SecondApprovalApproved
        var allPanelsApproved = allPanels.All(bp => bp.PanelStatus == PanelStatusEnum.SecondApprovalApproved);

        if (!allPanelsApproved)
        {
            var approvedCount = allPanels.Count(bp => bp.PanelStatus == PanelStatusEnum.SecondApprovalApproved);
            return Result.Failure<string>($"Not all panels are approved. {approvedCount} out of {allPanels.Count} panels have second approval approved");
        }

        // Update box status to ReadyToStart
        var currentUserId = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());
        box.Status = BoxStatusEnum.ReadyToStart;
        box.ModifiedDate = DateTime.UtcNow;
        box.ModifiedBy = currentUserId;

        _unitOfWork.Repository<Box>().Update(box);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success<string>($"Box status updated to ReadyToStart. All {allPanels.Count} panels are approved.");
    }
}
