using Dubox.Application.Features.BoxMaterials.Commands;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.BoxMaterials.Queries;

public record GetBoxMaterialsQuery(
    Guid BoxId,
    bool? OnlyOverdue = null, // Filter: only overdue materials
    bool? OnlyPending = null  // Filter: only pending (not arrived) materials
) : IRequest<Result<List<BoxMaterialDto>>>;






