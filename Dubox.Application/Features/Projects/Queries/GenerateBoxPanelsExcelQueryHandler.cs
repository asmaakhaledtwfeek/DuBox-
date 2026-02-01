using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

namespace Dubox.Application.Features.Projects.Queries;

public class GenerateBoxPanelsExcelQueryHandler : IRequestHandler<GenerateBoxPanelsExcelQuery, Result<byte[]>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDbContext _dbContext;

    public GenerateBoxPanelsExcelQueryHandler(IUnitOfWork unitOfWork, IDbContext dbContext)
    {
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
    }

    public async Task<Result<byte[]>> Handle(GenerateBoxPanelsExcelQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Validate project exists
            var project = await _unitOfWork.Repository<Project>().GetByIdAsync(request.ProjectId, cancellationToken);
            if (project == null)
                return Result.Failure<byte[]>("Project not found");

            // Get all boxes for the project
            var boxes = await _dbContext.Boxes
                .Where(b => b.ProjectId == request.ProjectId && b.IsActive)
                .OrderBy(b => b.SerialNumber ?? b.BoxTag)
                .ToListAsync(cancellationToken);

            if (!boxes.Any())
                return Result.Failure<byte[]>("No boxes found for this project");

            // Get panel types for the project
            var panelTypes = await _dbContext.PanelTypes
                .Where(pt => pt.ProjectId == request.ProjectId && pt.IsActive)
                .OrderBy(pt => pt.PanelTypeCode)
                .ToListAsync(cancellationToken);

            // Get existing panels for all boxes
            var existingPanels = await _dbContext.BoxPanels
                .Where(p => p.ProjectId == request.ProjectId)
                .Include(p => p.PanelType)
                .ToListAsync(cancellationToken);

            // Group panels by box and panel type
            var panelsByBox = existingPanels
                .GroupBy(p => p.BoxId)
                .ToDictionary(
                    g => g.Key,
                    g => g.ToDictionary(p => p.PanelTypeId ?? Guid.Empty, p => p.PanelName ?? "")
                );

            // Generate Excel file
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Box Panels");

            // Create headers
            var headers = new List<string> { "BoxSerialNumber", "BoxTag" };
            
            // Add dynamic panel type columns
            if (panelTypes.Any())
            {
                foreach (var panelType in panelTypes)
                {
                    headers.Add($"Panel_{panelType.PanelTypeCode}");
                }
            }
            else
            {
                // Fallback to generic columns if no panel types defined
                for (int i = 1; i <= 6; i++)
                {
                    headers.Add($"Panel_{i}");
                }
            }

            // Write headers
            for (int col = 0; col < headers.Count; col++)
            {
                var cell = worksheet.Cells[1, col + 1];
                cell.Value = headers[col];
                cell.Style.Font.Bold = true;
                cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cell.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }

            // Write data rows
            for (int row = 0; row < boxes.Count; row++)
            {
                var box = boxes[row];
                var dataRow = row + 2; // Row 1 is header

                // BoxSerialNumber (required)
                worksheet.Cells[dataRow, 1].Value = box.SerialNumber ?? string.Empty;

                // BoxTag (optional if exists)
                worksheet.Cells[dataRow, 2].Value = box.BoxTag ?? string.Empty;

                // Panel columns based on panel types
                var boxPanelData = panelsByBox.ContainsKey(box.BoxId) 
                    ? panelsByBox[box.BoxId] 
                    : new Dictionary<Guid, string>();

                if (panelTypes.Any())
                {
                    for (int i = 0; i < panelTypes.Count; i++)
                    {
                        var panelType = panelTypes[i];
                        var panelValue = boxPanelData.ContainsKey(panelType.PanelTypeId) 
                            ? boxPanelData[panelType.PanelTypeId] 
                            : string.Empty;
                        worksheet.Cells[dataRow, i + 3].Value = panelValue;
                    }
                }
                else
                {
                    // Fallback for no panel types - populate first 6 columns with empty values
                    for (int i = 0; i < 6; i++)
                    {
                        worksheet.Cells[dataRow, i + 3].Value = string.Empty;
                    }
                }
            }

            // Auto-fit columns
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

            return Result.Success(package.GetAsByteArray());
        }
        catch (Exception ex)
        {
            return Result.Failure<byte[]>($"Error generating Excel file: {ex.Message}");
        }
    }
}
