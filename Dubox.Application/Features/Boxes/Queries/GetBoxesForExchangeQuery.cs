using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Boxes.Queries;

public record GetBoxesForExchangeQuery(
    Guid ProjectId,
    Guid BoxId,
    string? BuildingNumber = null,
    string? Floor = null
) : IRequest<Result<List<BoxDto>>>;
