using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Boxes.Commands;

public record CreateBoxExchangeRequestCommand(
    Guid BoxId,
    string? NewBuildingNumber,
    string? NewFloor,
    string? RequestReason,
    Guid? TargetBoxId  // The box to exchange with
) : IRequest<Result<BoxExchangeDto>>;
