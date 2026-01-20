using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Schedule.Commands;

public record AssignTeamCommand(
    Guid ScheduleActivityId,
    Guid TeamId,
    string? Notes
) : IRequest<Result<Guid>>;




