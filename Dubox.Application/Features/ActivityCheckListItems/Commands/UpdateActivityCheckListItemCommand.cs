using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.ActivityCheckListItems.Commands;

public record UpdateActivityCheckListItemCommand : IRequest<Result>
{
    public Guid ActivityCheckListItemId { get; init; }
    public int Sequence { get; init; }
    public bool IsMandatory { get; init; }
    public bool IsActive { get; init; }
}
