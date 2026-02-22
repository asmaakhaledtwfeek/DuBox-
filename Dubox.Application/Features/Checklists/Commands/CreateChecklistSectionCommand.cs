using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Checklists.Commands;

public record CreateChecklistSectionCommand(
    Guid ChecklistId,
    string Title,
    int Order
) : IRequest<Result<Guid>>;
