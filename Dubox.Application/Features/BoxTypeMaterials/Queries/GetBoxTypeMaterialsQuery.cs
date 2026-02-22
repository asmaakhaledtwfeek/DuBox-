using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.BoxTypeMaterials.Queries;

public record GetBoxTypeMaterialsQuery(
    int ProjectBoxTypeId
) : IRequest<Result<List<BoxTypeMaterialDto>>>;






