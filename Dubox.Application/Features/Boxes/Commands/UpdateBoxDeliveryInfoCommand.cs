using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Boxes.Commands;

public record UpdateBoxDeliveryInfoCommand(
    Guid BoxId,
    bool? Wall1,
    bool? Wall2,
    bool? Wall3,
    bool? Wall4,
    bool? PodDeliver,
    string? PodName,
    string? PodType
) : IRequest<Result<BoxDto>>;



