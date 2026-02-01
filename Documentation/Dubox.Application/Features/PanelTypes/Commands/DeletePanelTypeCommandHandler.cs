using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.PanelTypes.Commands;

public class DeletePanelTypeCommandHandler : IRequestHandler<DeletePanelTypeCommand, Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDbContext _dbContext;

    public DeletePanelTypeCommandHandler(
        IUnitOfWork unitOfWork,
        IDbContext dbContext)
    {
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
    }

    public async Task<Result<bool>> Handle(DeletePanelTypeCommand request, CancellationToken cancellationToken)
    {
        var panelType = await _unitOfWork.Repository<PanelType>()
            .GetByIdAsync(request.PanelTypeId, cancellationToken);

        if (panelType == null)
            return Result.Failure<bool>("Panel type not found");

        // Check if there are any panels using this panel type
        var hasAssociatedPanels = await _dbContext.BoxPanels
            .AnyAsync(p => p.PanelTypeId == request.PanelTypeId, cancellationToken);

        if (hasAssociatedPanels)
            return Result.Failure<bool>("Cannot delete panel type that is being used by panels. Please remove all associated panels first.");

        _unitOfWork.Repository<PanelType>().Delete(panelType);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success(true);
    }
}

