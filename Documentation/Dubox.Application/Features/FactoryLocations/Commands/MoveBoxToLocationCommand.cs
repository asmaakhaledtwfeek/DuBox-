using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.FactoryLocations.Commands;

public record MoveBoxToLocationCommand(
    Guid BoxId,
    Guid ToLocationId,
    string? Reason
) : IRequest<Result<BoxLocationHistoryDto>>;

