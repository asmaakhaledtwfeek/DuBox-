using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Activities.Commands
{
    public record SetBoxActivityScheduleCommand(
    Guid ActivityId,
    DateTime PlannedStartDate,
    int Duration
) : IRequest<Result<BoxActivityDto>>;
}
