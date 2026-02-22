using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.ActivityTemplates.Commands;

public record CreateActivityTemplateCommand : IRequest<Result<Guid>>
{
    public string TemplateName { get; init; } = string.Empty;
    public string? Description { get; init; }
    public int StageCount { get; init; } = 1;
    public List<CreateActivityTemplateActivityDto> Activities { get; init; } = new();
}

/// <summary>
/// DTO for creating custom activities within a template.
/// All activities are custom - Activity Master can be referenced for template purposes only.
/// </summary>
public record CreateActivityTemplateActivityDto
{
    public Guid? SourceActivityMasterId { get; init; }
    public bool IsCustomActivity { get; init; }
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
    public Guid? AssignedTeamId { get; init; }
    public List<ActivityChecklistItemDto>? SelectedChecklistItems { get; init; }
}

public record ActivityChecklistItemDto
{
    public Guid PredefinedChecklistItemId { get; init; }
    public int Sequence { get; init; }
    public bool IsMandatory { get; init; }
}
