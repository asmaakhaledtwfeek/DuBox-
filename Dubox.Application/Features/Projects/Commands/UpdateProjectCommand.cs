using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Projects.Commands;

public record UpdateProjectCommand(
    Guid ProjectId,
    string ProjectCode,
    string ProjectName,
    string? ClientName,
    string? Location,
    DateTime? StartDate,
    DateTime? PlannedEndDate,
    DateTime? ActualEndDate,
    string Status,
    string? Description,
    bool IsActive
) : IRequest<Result<ProjectDto>>;

