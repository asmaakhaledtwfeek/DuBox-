using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using Mapster;
using MapsterMapper;
using MediatR;

namespace Dubox.Application.Features.Projects.Commands;

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Result<ProjectDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;
    private readonly IProjectTeamVisibilityService _visibilityService;

    public CreateProjectCommandHandler(
        IUnitOfWork unitOfWork, 
        IMapper mapper, 
        ICurrentUserService currentUserService,
        IProjectTeamVisibilityService visibilityService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
        _visibilityService = visibilityService;
    }

    public async Task<Result<ProjectDto>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        // Authorization: Only SystemAdmin and ProjectManager can create projects
        var canCreate = await _visibilityService.CanCreateProjectOrTeamAsync(cancellationToken);
        if (!canCreate)
        {
            return Result.Failure<ProjectDto>("Access denied. Only System Administrators and Project Managers can create projects.");
        }

        var currentUserId = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());

        var projectExists = await _unitOfWork.Repository<Project>()
            .IsExistAsync(p => p.ProjectCode == request.ProjectCode, cancellationToken);

        if (projectExists)
            return Result.Failure<ProjectDto>("Project with this code already exists");

        var project = _mapper.Map<Project>(request);

        project.PlannedEndDate = request.PlannedStartDate.AddDays(request.Duration);

        project.ActualStartDate = null;
        project.ActualEndDate = null;

        await _unitOfWork.Repository<Project>().AddAsync(project, cancellationToken);

        var projectLog = new AuditLog
        {
            TableName = nameof(Project),
            Action = "Creation",
            OldValues = "N/A",
            NewValues = $"Code: {request.ProjectCode}, Name: {request.ProjectName}, Duration: {request.Duration} days, Start: {request.PlannedStartDate:yyyy-MM-dd}",
            ChangedBy = currentUserId,
            ChangedDate = DateTime.UtcNow,
            Description = $"New Project '{request.ProjectName}' created with code {request.ProjectCode}."
        };
        await _unitOfWork.Repository<AuditLog>().AddAsync(projectLog, cancellationToken);

        await _unitOfWork.CompleteAsync(cancellationToken);

        projectLog.RecordId = project.ProjectId;
        _unitOfWork.Repository<AuditLog>().Update(projectLog);

        await _unitOfWork.CompleteAsync(cancellationToken);
        return Result.Success(project.Adapt<ProjectDto>());
    }
}
