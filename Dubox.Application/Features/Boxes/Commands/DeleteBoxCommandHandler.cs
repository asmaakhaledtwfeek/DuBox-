using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Dubox.Domain.Abstraction;
using MediatR;

namespace Dubox.Application.Features.Boxes.Commands;

public class DeleteBoxCommandHandler : IRequestHandler<DeleteBoxCommand, Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBoxCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(DeleteBoxCommand request, CancellationToken cancellationToken)
    {
        var box = await _unitOfWork.Repository<Box>()
            .GetByIdAsync(request.BoxId, cancellationToken);

        if (box == null)
            return Result.Failure<bool>("Box not found");

        var projectId = box.ProjectId;

        // Delete box (cascade will handle related records)
        _unitOfWork.Repository<Box>().Delete(box);
        await _unitOfWork.CompleteAsync(cancellationToken);

        // Update project total boxes count
        var project = await _unitOfWork.Repository<Project>().GetByIdAsync(projectId, cancellationToken);
        if (project != null)
        {
            var boxCount = await _unitOfWork.Repository<Box>()
                .CountAsync(b => b.ProjectId == projectId, cancellationToken);
            project.TotalBoxes = boxCount;
            _unitOfWork.Repository<Project>().Update(project);
            await _unitOfWork.CompleteAsync(cancellationToken);
        }

        return Result.Success(true);
    }
}

