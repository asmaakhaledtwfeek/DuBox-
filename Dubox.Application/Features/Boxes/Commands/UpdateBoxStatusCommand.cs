using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Boxes.Commands
{
    public record UpdateBoxStatusCommand(
    Guid BoxId,
    int Status
) : IRequest<Result<BoxDto>>;
}
