using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.BoxTypeMaterials.Commands;

/// <summary>
/// Command to synchronize BoxTypeMaterial records from existing template assignments
/// This is useful for populating BoxTypeMaterial records for projects that had templates assigned before this feature was implemented
/// </summary>
public record SyncBoxTypeMaterialsCommand(Guid ProjectId) : IRequest<Result<int>>;






