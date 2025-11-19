using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Shared;
using Mapster;
using MapsterMapper;
using MediatR;

namespace Dubox.Application.Features.Projects.Commands
{
    public class UpdateProjectStatusCommandHandler : IRequestHandler<UpdateProjectStatusCommand, Result<ProjectDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public UpdateProjectStatusCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<Result<ProjectDto>> Handle(UpdateProjectStatusCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());

            var project = await _unitOfWork.Repository<Project>()
                .GetByIdAsync(request.ProjectId, cancellationToken);

            if (project == null)
                return Result.Failure<ProjectDto>("Project not found.");

            var oldStatus = project.Status;
            var newStatus = (ProjectStatusEnum)request.Status;

            if (oldStatus == newStatus)
                return Result.Failure<ProjectDto>($"Project is already in the status: {newStatus}. No update needed.");

            project.Status = newStatus;

            _unitOfWork.Repository<Project>().Update(project);

            var projectLog = new AuditLog
            {
                TableName = nameof(Project),
                RecordId = project.ProjectId,
                Action = "StatusUpdate",
                OldValues = $"Status: {oldStatus.ToString()}",
                NewValues = $"Status: {newStatus.ToString()}",
                ChangedBy = currentUserId,
                ChangedDate = DateTime.UtcNow,
                Description = $"Project status changed from {oldStatus} to {newStatus}."
            };
            await _unitOfWork.Repository<AuditLog>().AddAsync(projectLog, cancellationToken);

            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result.Success(project.Adapt<ProjectDto>());
        }
    }
}
