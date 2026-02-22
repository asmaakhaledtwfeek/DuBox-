using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.BoxTypeMaterials.Commands;

/// <summary>
/// Command to import materials for all box types in a project from an Excel file
/// Updates DeliveredQuantity and QuantityPerBox for materials across multiple box types
/// </summary>
public record ImportProjectMaterialsFromExcelCommand(
    Guid ProjectId,
    Stream FileStream,
    string FileName
) : IRequest<Result<BoxTypeMaterialImportResultDto>>;
