using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Projects.Commands;

public record CreateProjectCommand(
    string ProjectCode,
    string ProjectName,
    string? ClientName,
    string? Location,
    DateTime? StartDate,
    DateTime? PlannedEndDate,
    string? Description
) : IRequest<Result<ProjectDto>>;

