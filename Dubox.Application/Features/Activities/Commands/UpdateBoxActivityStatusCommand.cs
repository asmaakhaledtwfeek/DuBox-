using Dubox.Application.DTOs;
using Dubox.Domain.Enums;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Activities.Commands;

public record UpdateBoxActivityStatusCommand(
    Guid BoxActivityId,
    BoxStatusEnum Status,
    string? WorkDescription,
    string? IssuesEncountered
) : IRequest<Result<BoxActivityDto>>;

