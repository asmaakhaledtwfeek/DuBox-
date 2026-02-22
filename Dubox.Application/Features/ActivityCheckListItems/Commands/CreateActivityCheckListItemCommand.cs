using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.ActivityCheckListItems.Commands;

public record CreateActivityCheckListItemCommand : IRequest<Result<Guid>>
{
    public Guid? ActivityMasterId { get; init; }
    public Guid? ActivityTemplateActivityId { get; init; }
    public Guid PredefinedChecklistItemId { get; init; }
    public int Sequence { get; init; }
    public bool IsMandatory { get; init; } = true;
    public bool IsActive { get; init; } = true;
}
