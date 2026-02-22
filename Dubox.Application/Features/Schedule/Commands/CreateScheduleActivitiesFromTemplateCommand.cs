using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Schedule.Commands;

/// <summary>
/// Create multiple Schedule Activities from an Activity Template
/// This will clone all activities from the template with their snapshot properties
/// </summary>
public record CreateScheduleActivitiesFromTemplateCommand : IRequest<Result<List<Guid>>>
{
    public Guid ActivityTemplateId { get; init; }
    public DateTime BaseStartDate { get; init; }
    public Guid? ProjectId { get; init; }
}
