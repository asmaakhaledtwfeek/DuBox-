using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.BoxTypeMaterials.Queries;

/// <summary>
/// Query to generate an Excel file with all box type materials for a project (all box types)
/// This file can be edited and re-imported to update DeliveredQuantity and QuantityPerBox across all box types
/// </summary>
public record GenerateProjectMaterialsExcelQuery(
    Guid ProjectId
) : IRequest<Result<byte[]>>;
