using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.FactoryLocations.Commands;

public record CreateFactoryLocationCommand(
    string LocationCode,
    string LocationName,
    string? LocationType,
    string? Bay,
    string? Row,
    string? Position,
    int? Capacity
) : IRequest<Result<FactoryLocationDto>>;

