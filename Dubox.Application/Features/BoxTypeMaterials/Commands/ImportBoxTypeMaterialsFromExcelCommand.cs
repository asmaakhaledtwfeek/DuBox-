using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.BoxTypeMaterials.Commands;

/// <summary>
/// Command to import box type materials from an Excel file
/// Updates DeliveredQuantity and QuantityPerBox for materials within a specific box type
/// </summary>
public record ImportBoxTypeMaterialsFromExcelCommand(
    int ProjectBoxTypeId,
    Stream FileStream,
    string FileName
) : IRequest<Result<BoxTypeMaterialImportResultDto>>;

/// <summary>
/// Result DTO for box type material import operation
/// </summary>
public class BoxTypeMaterialImportResultDto
{
    public int SuccessCount { get; set; }
    public int FailureCount { get; set; }
    public List<string> Errors { get; set; } = new();
    public List<string> Warnings { get; set; } = new();
}

/// <summary>
/// DTO for mapping Excel rows during import
/// </summary>
public class ImportBoxTypeMaterialRowDto
{
    public string Status { get; set; } = string.Empty;
    public string MaterialCode { get; set; } = string.Empty;
    public string MaterialName { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int QuantityPerBox { get; set; }
    public decimal? DeliveredQuantity { get; set; }
    public decimal TotalQuantity { get; set; }
    public string RequiredBeforeDays { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}
