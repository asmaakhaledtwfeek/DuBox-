using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Materials.Commands;

public record CreateMaterialCommand(
    string MaterialCode,
    string MaterialName,
    string? MaterialCategory,
    string? Unit,
    decimal? UnitCost,
    decimal? CurrentStock,
    decimal? MinimumStock,
    decimal? ReorderLevel,
    string? SupplierName
) : IRequest<Result<MaterialDto>>;

