using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Schedule.Commands;

public record UpdateActivityProgressCommand(
    Guid ScheduleActivityId,
    decimal PercentComplete,
    string Status,
    DateTime? ActualStartDate,
    DateTime? ActualFinishDate
) : IRequest<Result<bool>>;
