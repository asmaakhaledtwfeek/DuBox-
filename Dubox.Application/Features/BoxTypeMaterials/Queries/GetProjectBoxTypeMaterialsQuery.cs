using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.BoxTypeMaterials.Queries;

/// <summary>
/// Gets all box type materials for a project (across all box types)
/// </summary>
public record GetProjectBoxTypeMaterialsQuery(
    Guid ProjectId,
    string? BuildingNumber = null,
    string? Floor = null
) : IRequest<Result<List<BoxTypeMaterialDto>>>;






