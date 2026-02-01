using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Materials.Commands
{
    public record RestockMaterialCommand(
     Guid MaterialId,
     decimal Quantity,
     string? Reference,
     string? Remarks
 ) : IRequest<Result<RestockMaterialDto>>;
}
