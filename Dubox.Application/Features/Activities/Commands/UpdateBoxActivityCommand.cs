using Dubox.Application.DTOs;
using Dubox.Domain.Enums;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Activities.Commands;

public record UpdateBoxActivityCommand(
    Guid BoxActivityId,
    BoxStatusEnum Status,
    decimal ProgressPercentage,
    string? WorkDescription,
    string? IssuesEncountered,
    int? AssignedTeam,
    bool MaterialsAvailable
) : IRequest<Result<BoxActivityDto>>;

