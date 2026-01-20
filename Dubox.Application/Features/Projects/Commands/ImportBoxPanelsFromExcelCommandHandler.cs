using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.IO;

namespace Dubox.Application.Features.Projects.Commands;

public class ImportBoxPanelsFromExcelCommandHandler : IRequestHandler<ImportBoxPanelsFromExcelCommand, Result<BoxPanelsImportResultDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDbContext _dbContext;
    private readonly IExcelService _excelService;

    public ImportBoxPanelsFromExcelCommandHandler(
        IUnitOfWork unitOfWork,
        IDbContext dbContext,
        IExcelService excelService)
    {
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
        _excelService = excelService;
    }

    public async Task<Result<BoxPanelsImportResultDto>> Handle(ImportBoxPanelsFromExcelCommand request, CancellationToken cancellationToken)
    {
        if (request.FileStream == null)
            return Result.Failure<BoxPanelsImportResultDto>("No file stream provided");

        var fileExtension = Path.GetExtension(request.FileName).ToLower();
        if (fileExtension != ".xlsx" && fileExtension != ".xls")
            return Result.Failure<BoxPanelsImportResultDto>("Invalid file format. Please upload an Excel file (.xlsx or .xls)");

        var project = await _unitOfWork.Repository<Project>().GetByIdAsync(request.ProjectId, cancellationToken);
        if (project == null)
            return Result.Failure<BoxPanelsImportResultDto>("Project not found");

        var errors = new List<string>();
        var successCount = 0;
        var failureCount = 0;

        try
        {
            var stream = request.FileStream;
            stream.Position = 0;

            // Read Excel file
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            using var package = new ExcelPackage(stream);
            var worksheet = package.Workbook.Worksheets.FirstOrDefault();

            if (worksheet == null || worksheet.Dimension == null)
                return Result.Failure<BoxPanelsImportResultDto>("Invalid Excel file or empty worksheet");

            // Read headers
            var headers = new List<string>();
            for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
            {
                var headerValue = worksheet.Cells[1, col].Value?.ToString()?.Trim();
                if (!string.IsNullOrWhiteSpace(headerValue))
                {
                    headers.Add(headerValue);
                }
            }

            // Validate required headers
            if (!headers.Any(h => h.Equals("BoxSerialNumber", StringComparison.OrdinalIgnoreCase)))
            {
                return Result.Failure<BoxPanelsImportResultDto>("Missing required column: BoxSerialNumber");
            }

            // Find panel columns (columns starting with "Panel_")
            var panelColumns = headers
                .Select((h, index) => new { Header = h, Index = index + 1 })
                .Where(x => x.Header.StartsWith("Panel_", StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (!panelColumns.Any())
            {
                return Result.Failure<BoxPanelsImportResultDto>("No panel columns found. Columns must start with 'Panel_'");
            }

            // Get panel types for the project
            var panelTypes = await _dbContext.PanelTypes
                .Where(pt => pt.ProjectId == request.ProjectId && pt.IsActive)
                .ToListAsync(cancellationToken);

            // Create a mapping from column header to panel type
            var panelTypesByColumn = new Dictionary<string, PanelType?>(StringComparer.OrdinalIgnoreCase);
            foreach (var panelCol in panelColumns)
            {
                // Extract type code from "Panel_XXX" header
                var typeCode = panelCol.Header.Substring(6).Trim(); // Remove "Panel_" prefix
                var panelType = panelTypes.FirstOrDefault(pt => 
                    pt.PanelTypeCode.Equals(typeCode, StringComparison.OrdinalIgnoreCase));
                panelTypesByColumn[panelCol.Header] = panelType;
            }

            // Get all boxes for the project by SerialNumber
            var boxesBySerialNumber = await _dbContext.Boxes
                .Where(b => b.ProjectId == request.ProjectId)
                .Where(b => !string.IsNullOrEmpty(b.SerialNumber))
                .ToDictionaryAsync(b => b.SerialNumber!.ToUpper(), b => b, cancellationToken);

            // Get existing panels for all boxes
            var existingPanels = await _dbContext.BoxPanels
                .Where(p => p.ProjectId == request.ProjectId)
                .ToListAsync(cancellationToken);

            // Group existing panels by box ID
            var existingPanelsByBoxId = existingPanels
                .GroupBy(p => p.BoxId)
                .ToDictionary(g => g.Key, g => g.ToList());

            // Process each data row
            for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
            {
                try
                {
                    // Get BoxSerialNumber
                    var serialNumberCell = worksheet.Cells[row, 1].Value?.ToString()?.Trim();
                    if (string.IsNullOrWhiteSpace(serialNumberCell))
                    {
                        errors.Add($"Row {row}: BoxSerialNumber is required");
                        failureCount++;
                        continue;
                    }

                    var serialNumberUpper = serialNumberCell.ToUpper();
                    if (!boxesBySerialNumber.ContainsKey(serialNumberUpper))
                    {
                        errors.Add($"Row {row}: Box with SerialNumber '{serialNumberCell}' not found in project");
                        failureCount++;
                        continue;
                    }

                    var box = boxesBySerialNumber[serialNumberUpper];

                    // Collect panel values from Panel_ columns with their types
                    var panelData = new List<(string PanelName, Guid? PanelTypeId)>();
                    foreach (var panelCol in panelColumns)
                    {
                        var cellValue = worksheet.Cells[row, panelCol.Index].Value?.ToString()?.Trim();
                        if (!string.IsNullOrWhiteSpace(cellValue))
                        {
                            var panelType = panelTypesByColumn[panelCol.Header];
                            panelData.Add((cellValue, panelType?.PanelTypeId));
                        }
                    }

                    // If no panels provided, skip this row
                    if (!panelData.Any())
                    {
                        continue; 
                    }

                    // Delete existing panels for this box
                    if (existingPanelsByBoxId.ContainsKey(box.BoxId))
                    {
                        var panelsToDelete = existingPanelsByBoxId[box.BoxId];
                        _dbContext.BoxPanels.RemoveRange(panelsToDelete);
                    }

                    // Create new panel records with auto-generated barcodes
                    var newPanels = panelData.Select((data, index) => new BoxPanel
                    {
                        BoxId = box.BoxId,
                        ProjectId = request.ProjectId,
                        PanelName = data.PanelName,
                        PanelTypeId = data.PanelTypeId,
                        PanelStatus = Domain.Enums.PanelStatusEnum.Manufacturing,
                        Barcode = $"PANEL-{project.ProjectCode}-{box.SerialNumber}-{(index + 1):D3}", // ProjectCode-SerialNumber-PanelIndex
                        CreatedDate = DateTime.UtcNow,
                        ModifiedDate = DateTime.UtcNow
                    }).ToList();

                    await _dbContext.BoxPanels.AddRangeAsync(newPanels, cancellationToken);
                    successCount++;
                }
                catch (Exception ex)
                {
                    errors.Add($"Row {row}: {ex.Message}");
                    failureCount++;
                }
            }

            // Save all changes
            await _dbContext.SaveChangesAsync(cancellationToken);

            var result = new BoxPanelsImportResultDto
            {
                SuccessCount = successCount,
                FailureCount = failureCount,
                Errors = errors
            };

            return Result.Success(result);
        }
        catch (Exception ex)
        {
            return Result.Failure<BoxPanelsImportResultDto>($"Error processing Excel file: {ex.Message}");
        }
    }
}
