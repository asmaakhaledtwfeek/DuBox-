using Dubox.Application.DTOs;
using Dubox.Domain.Enums;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Projects.Commands;

public record UpdateProjectStatusCommand(Guid ProjectId, ProjectStatusEnum Status) : IRequest<Result<ProjectDto>>;
