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

            // Get existing panels for all boxes to determine max panel count
            var existingPanels = await _dbContext.BoxPanels
                .Where(p => p.ProjectId == request.ProjectId)
                .ToListAsync(cancellationToken);

            // Group panels by box and find max panel count
            var panelsByBox = existingPanels
                .GroupBy(p => p.BoxId)
                .ToDictionary(g => g.Key, g => g.Select(p => p.PanelName).OrderBy(n => n).ToList());

            // Determine the maximum number of panels across all boxes
            int maxPanelCount = Math.Max(6, panelsByBox.Values.Any() ? panelsByBox.Values.Max(p => p.Count) : 0);

            // Generate Excel file
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Box Panels");

            // Create headers
            var headers = new List<string> { "BoxSerialNumber", "BoxTag" };
            for (int i = 1; i <= maxPanelCount; i++)
            {
                headers.Add($"Panel_{i}");
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

                // Panel columns
                var boxPanels = panelsByBox.ContainsKey(box.BoxId) 
                    ? panelsByBox[box.BoxId] 
                    : new List<string>();

                for (int panelIndex = 0; panelIndex < maxPanelCount; panelIndex++)
                {
                    var panelValue = panelIndex < boxPanels.Count ? boxPanels[panelIndex] : string.Empty;
                    worksheet.Cells[dataRow, panelIndex + 3].Value = panelValue;
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
