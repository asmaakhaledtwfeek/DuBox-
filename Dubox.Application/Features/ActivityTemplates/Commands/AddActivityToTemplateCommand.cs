using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.ActivityTemplates.Commands;

/// <summary>
/// Command to add a custom activity to an activity template.
/// Activity Master can be used as a template source (SourceActivityMasterId) to copy properties,
/// but all activities are stored as independent custom activities with no FK relationship.
/// </summary>
public record AddActivityToTemplateCommand : IRequest<Result<Guid>>
{
    public Guid ActivityTemplateId { get; init; }
    
    /// <summary>
    /// Optional Activity Master ID to use as template (properties will be copied, no FK stored)
    /// If set, activity is marked as MASTER; if null, marked as CUSTOM
    /// </summary>
    public Guid? SourceActivityMasterId { get; init; }
    
    /// <summary>
    /// Flag to indicate if this is a custom activity (true) or from Activity Master template (false)
    /// Used for display badges: MASTER vs CUSTOM
    /// </summary>
    public bool IsCustomActivity { get; init; } = true;
    
    public string ActivityCode { get; init; } = string.Empty;
    public string ActivityName { get; init; } = string.Empty;
    public string Stage { get; init; } = string.Empty;
    public int StageNumber { get; init; }
    public int SequenceInStage { get; init; }
    public int OverallSequence { get; init; }
    public string? Description { get; init; }
    public int EstimatedDurationDays { get; init; } = 1;
    public bool IsWIRCheckpoint { get; init; }
    public string? WIRCode { get; init; }
    public string? ApplicableBoxTypes { get; init; }
    public string? DependsOnActivities { get; init; }
    public List<Guid>? SelectedChecklistItemIds { get; init; }
}
