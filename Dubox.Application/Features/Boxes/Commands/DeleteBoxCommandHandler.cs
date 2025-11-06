using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Boxes.Commands;

public class DeleteBoxCommandHandler : IRequestHandler<DeleteBoxCommand, Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<Box> _genericRepository;

    public DeleteBoxCommandHandler(IUnitOfWork unitOfWork, IGenericRepository<Box> genericRepository)
    {
        _unitOfWork = unitOfWork;
        _genericRepository = genericRepository;
    }

    public async Task<Result<bool>> Handle(DeleteBoxCommand request, CancellationToken cancellationToken)
    {

        try
        {
            var box = _genericRepository.GetEntityWithSpec(new DeleteBoxWithIncludesSpecification(request.BoxId));
            if (box == null)
                return Result.Failure<bool>("Box not found");
            var projectId = box.ProjectId;
            _unitOfWork.Repository<Box>().Delete(box);
            await _unitOfWork.CompleteAsync(cancellationToken);

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
        catch (Exception ex)
        {
            return Result.Failure<bool>($"Error deleting box: {ex.Message}");
        }

    }
}

