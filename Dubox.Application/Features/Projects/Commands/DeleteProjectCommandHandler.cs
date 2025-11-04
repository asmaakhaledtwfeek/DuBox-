using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Dubox.Domain.Abstraction;
using MediatR;

namespace Dubox.Application.Features.Projects.Commands;

public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProjectCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _unitOfWork.Repository<Project>()
            .GetByIdAsync(request.ProjectId, cancellationToken);

        if (project == null)
            return Result.Failure<bool>("Project not found");

        // Check if project has boxes
        var hasBoxes = await _unitOfWork.Repository<Box>()
            .IsExistAsync(b => b.ProjectId == request.ProjectId, cancellationToken);

        if (hasBoxes)
            return Result.Failure<bool>("Cannot delete project with existing boxes. Delete boxes first.");

        _unitOfWork.Repository<Project>().Delete(project);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success(true);
    }
}

