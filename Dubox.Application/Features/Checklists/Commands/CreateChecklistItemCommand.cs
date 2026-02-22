using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Checklists.Commands;

public record CreateChecklistItemCommand(
    Guid ChecklistSectionId,
    string Description,
    int Sequence,
    string? Reference
) : IRequest<Result<Guid>>;
