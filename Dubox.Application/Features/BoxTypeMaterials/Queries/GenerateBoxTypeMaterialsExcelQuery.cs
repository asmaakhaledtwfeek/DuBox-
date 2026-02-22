using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.BoxTypeMaterials.Queries;

/// <summary>
/// Query to generate an Excel file with box type materials for a specific box type
/// This file can be edited and re-imported to update DeliveredQuantity and QuantityPerBox
/// </summary>
public record GenerateBoxTypeMaterialsExcelQuery(
    int ProjectBoxTypeId
) : IRequest<Result<byte[]>>;
