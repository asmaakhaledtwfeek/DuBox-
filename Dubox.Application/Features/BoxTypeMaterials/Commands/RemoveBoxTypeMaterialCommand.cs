using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.BoxTypeMaterials.Commands;

/// <summary>
/// Command to remove a specific material assignment from a box type
/// This allows users to remove materials that were auto-assigned from project-level or template selections
/// </summary>
public record RemoveBoxTypeMaterialCommand(Guid BoxTypeMaterialId) : IRequest<Result<bool>>;






