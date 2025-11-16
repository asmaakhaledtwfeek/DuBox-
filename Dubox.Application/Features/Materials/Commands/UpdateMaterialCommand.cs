using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Materials.Commands
{
    public record UpdateMaterialCommand(
      int MaterialId,
      string? MaterialCode,
      string? MaterialName,
      string? MaterialCategory,
      string? Unit,
      string? SupplierName,
      decimal? UnitCost,
      decimal? MinimumStock,
      decimal? ReorderLevel,
      bool? IsActive
  ) : IRequest<Result<MaterialDto>>;
}
