using Dubox.Application.DTOs;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Dubox.Domain.Abstraction;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.Projects.Queries;

public class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, Result<ProjectDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetProjectByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ProjectDto>> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        var project = await _unitOfWork.Repository<Project>()
            .GetByIdAsync(request.ProjectId, cancellationToken);

        if (project == null)
            return Result.Failure<ProjectDto>("Project not found");

        return Result.Success(project.Adapt<ProjectDto>());
    }
}

