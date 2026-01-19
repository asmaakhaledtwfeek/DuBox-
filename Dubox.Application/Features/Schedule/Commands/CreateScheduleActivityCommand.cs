using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Schedule.Commands;

public record CreateScheduleActivityCommand(
    string ActivityName,
    string ActivityCode,
    string? Description,
    DateTime PlannedStartDate,
    DateTime PlannedFinishDate,
    Guid? ProjectId
) : IRequest<Result<Guid>>;



