using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Activities.Commands;

public record UpdateBoxActivityCommand(
    Guid BoxActivityId,
    string Status,
    decimal ProgressPercentage,
    string? WorkDescription,
    string? IssuesEncountered,
    string? AssignedTeam,
    bool MaterialsAvailable
) : IRequest<Result<BoxActivityDto>>;

