using Dubox.Application.DTOs;
using Dubox.Domain.Enums;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Projects.Commands;

public record UpdateProjectCommand(
    Guid ProjectId,
    string? ProjectCode,
    string? ProjectName,
    string? ClientName,
    ProjectLocationEnum? Location,
    int? CategoryId,
    string? Description,
    bool? IsActive,
    DateTime? PlannedStartDate,
    int? Duration
) : IRequest<Result<ProjectDto>>;

