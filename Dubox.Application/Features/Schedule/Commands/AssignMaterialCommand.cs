using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Schedule.Commands;

public record AssignMaterialCommand(
    Guid ScheduleActivityId,
    string MaterialName,
    string? MaterialCode,
    decimal Quantity,
    string? Unit,
    string? Notes
) : IRequest<Result<Guid>>;




