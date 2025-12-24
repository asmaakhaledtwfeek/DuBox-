using Dubox.Application.DTOs;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Projects.Commands;

public record CreateProjectCommand(
    string ProjectCode,
    string ProjectName,
    string? ClientName,
    ProjectLocationEnum Location,
    int ProjectCategoryId,
    int Duration,
    DateTime PlannedStartDate,
    string? Description,
    string? BimLink
) : IRequest<Result<ProjectDto>>;

