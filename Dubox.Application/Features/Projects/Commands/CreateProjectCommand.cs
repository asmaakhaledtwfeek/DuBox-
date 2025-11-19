using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Projects.Commands;

public record CreateProjectCommand(
    string ProjectCode,
    string ProjectName,
    string? ClientName,
    string? Location,
    int Duration,
    DateTime PlannedStartDate,
    string? Description
) : IRequest<Result<ProjectDto>>;

