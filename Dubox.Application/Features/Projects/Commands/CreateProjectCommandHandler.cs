using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Mapster;
using MapsterMapper;
using MediatR;

namespace Dubox.Application.Features.Projects.Commands;

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Result<ProjectDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public CreateProjectCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<ProjectDto>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var projectExists = await _unitOfWork.Repository<Project>()
            .IsExistAsync(p => p.ProjectCode == request.ProjectCode, cancellationToken);

        if (projectExists)
            return Result.Failure<ProjectDto>("Project with this code already exists");

        var project = _mapper.Map<Project>(request);


        await _unitOfWork.Repository<Project>().AddAsync(project, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success(project.Adapt<ProjectDto>());
    }
}

