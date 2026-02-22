using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Schedule.Commands;

/// <summary>
/// Create a Schedule Activity by cloning from ActivityMaster
/// This will snapshot all important properties from ActivityMaster
/// </summary>
public record CreateScheduleActivityFromMasterCommand : IRequest<Result<Guid>>
{
    public Guid ActivityMasterId { get; init; }
    public DateTime PlannedStartDate { get; init; }
    public DateTime PlannedFinishDate { get; init; }
    public Guid? ProjectId { get; init; }
}
