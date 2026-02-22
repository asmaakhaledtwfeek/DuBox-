using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Checklists.Commands;

public record CloneChecklistItemsCommand(
    List<Guid> ItemIds,
    string TargetWIRCode
) : IRequest<Result<CloneChecklistItemsResult>>;

public record CloneChecklistItemsResult(
    int ChecklistsCloned,
    int SectionsCloned,
    int ItemsCloned,
    List<Guid> NewChecklistIds
);
