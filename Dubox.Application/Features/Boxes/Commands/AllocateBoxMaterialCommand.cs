using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Boxes.Commands
{
    public record AllocateBoxMaterialCommand(
     Guid BoxMaterialId,
     decimal AllocatedQuantity
 ) : IRequest<Result<BoxMaterialDto>>;
}
