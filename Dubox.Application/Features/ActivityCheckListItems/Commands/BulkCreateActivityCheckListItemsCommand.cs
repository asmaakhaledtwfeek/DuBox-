using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.ActivityCheckListItems.Commands;

public record BulkCreateActivityCheckListItemsCommand : IRequest<Result<int>>
{
    public Guid? ActivityMasterId { get; init; }
    public Guid? ActivityTemplateActivityId { get; init; }
    public List<ChecklistItemData> ChecklistItems { get; init; } = new();
}

public record ChecklistItemData
{
    public Guid PredefinedChecklistItemId { get; init; }
}
