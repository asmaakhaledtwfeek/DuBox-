using Dubox.Domain.Abstraction;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

namespace Dubox.Application.Features.BoxTypeMaterials.Queries;

public class GenerateBoxTypeMaterialsExcelQueryHandler 
    : IRequestHandler<GenerateBoxTypeMaterialsExcelQuery, Result<byte[]>>
{
    private readonly IDbContext _context;

    public GenerateBoxTypeMaterialsExcelQueryHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<Result<byte[]>> Handle(
        GenerateBoxTypeMaterialsExcelQuery request, 
        CancellationToken cancellationToken)
    {
        try
        {
            // Set the license context for EPPlus
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // Get the box type
            var projectBoxType = await _context.ProjectBoxTypes
                .FirstOrDefaultAsync(pbt => pbt.Id == request.ProjectBoxTypeId, cancellationToken);

            if (projectBoxType == null)
                return Result.Failure<byte[]>($"Box type with ID {request.ProjectBoxTypeId} not found");

            // Get all materials for this box type
            var boxTypeMaterials = await _context.BoxTypeMaterials
                .Include(btm => btm.Material)
                .Include(btm => btm.ProjectBoxType)
                .Where(btm => btm.ProjectBoxTypeId == request.ProjectBoxTypeId)
                .OrderBy(btm => btm.Material.MaterialName)
                .ToListAsync(cancellationToken);

            // Get box count for this box type
            var boxCount = await _context.Boxes
                .Where(b => b.ProjectBoxTypeId == request.ProjectBoxTypeId 
                    && b.ProjectId == projectBoxType.ProjectId
                    && b.IsActive)
                .CountAsync(cancellationToken);

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add($"Box Type Summary: {boxCount} boxes, {boxTypeMaterials.Count} materials");

            // Add headers
            var headers = new[]
            {
                "Material Code",
                "Material Name",
                "Category",
                "Unit",
                "Quantity Per Box",
                "Total Quantity (Auto-calculated)",
                "Status",
                "Delivered Quantity",
                "Required Before (Days)",
                "Notes"
            };

            for (int i = 0; i < headers.Length; i++)
            {
                var cell = worksheet.Cells[1, i + 1];
                cell.Value = headers[i];
                cell.Style.Font.Bold = true;
                cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cell.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(79, 129, 189));
                cell.Style.Font.Color.SetColor(Color.White);
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            }

            // Add instruction row
            var instructionRow = 2;
            var instructionCell = worksheet.Cells[instructionRow, 1];
            instructionCell.Value = $"⚠️ INSTRUCTIONS: Edit 'Quantity Per Box' and 'Delivered Quantity' as needed. Total Quantity = {boxCount} boxes × Quantity Per Box. Delivered Quantity CANNOT exceed Total Quantity. Re-import this file to update the system.";
            instructionCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
            instructionCell.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 242, 204));
            instructionCell.Style.Font.Bold = true;
            instructionCell.Style.Font.Size = 10;
            instructionCell.Style.WrapText = true;
            worksheet.Row(instructionRow).Height = 50;
            worksheet.Cells[instructionRow, 1, instructionRow, headers.Length].Merge = true;

            // Add data rows
            int rowIndex = 3;
            foreach (var btm in boxTypeMaterials)
            {
                var totalQuantity = boxCount * btm.QuantityPerBox;

                worksheet.Cells[rowIndex, 1].Value = btm.Material.MaterialCode; // Material Code
                worksheet.Cells[rowIndex, 2].Value = btm.Material.MaterialName; // Material Name
                worksheet.Cells[rowIndex, 3].Value = btm.Material.MaterialCategory; // Category
                worksheet.Cells[rowIndex, 4].Value = btm.Material.Unit; // Unit
                worksheet.Cells[rowIndex, 5].Value = btm.QuantityPerBox; // Quantity Per Box (editable)
                
                // Set formula for Total Quantity (column 6): Quantity Per Box (column 5) × Box Count
                var totalQtyCell = worksheet.Cells[rowIndex, 6];
                totalQtyCell.Formula = $"E{rowIndex}*{boxCount}";
                totalQtyCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                totalQtyCell.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(240, 240, 240)); // Light gray
                totalQtyCell.Style.Font.Italic = true;
                totalQtyCell.Style.Font.Color.SetColor(Color.FromArgb(102, 102, 102)); // Dark gray
                
                worksheet.Cells[rowIndex, 7].Value = btm.Status; // Status
                worksheet.Cells[rowIndex, 8].Value = btm.DeliveredQuantity ?? 0; // Delivered Quantity (editable)
                worksheet.Cells[rowIndex, 9].Value = $"{btm.RequiredBeforeDays} days"; // Required Before Days
                worksheet.Cells[rowIndex, 10].Value = btm.Notes; // Notes (editable)

                // Style based on status
                var statusCell = worksheet.Cells[rowIndex, 7];
                statusCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                
                if (btm.Status == "DELIVERED")
                {
                    statusCell.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(198, 239, 206)); // Light green
                }
                else if (btm.Status == "PENDING")
                {
                    statusCell.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 235, 156)); // Light yellow
                }

                // Make editable columns highlighted
                var editableCols = new[] { 5, 8, 10 }; // Quantity Per Box, Delivered Quantity, Notes
                foreach (var col in editableCols)
                {
                    var cell = worksheet.Cells[rowIndex, col];
                    cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    cell.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(217, 225, 242)); // Light blue
                }

                rowIndex++;
            }

            // Auto-fit columns
            for (int i = 1; i <= headers.Length; i++)
            {
                worksheet.Column(i).AutoFit();
                if (worksheet.Column(i).Width < 15)
                    worksheet.Column(i).Width = 15;
            }

            // Freeze top rows
            worksheet.View.FreezePanes(3, 1);

            return Result.Success(package.GetAsByteArray());
        }
        catch (Exception ex)
        {
            return Result.Failure<byte[]>($"Error generating Excel file: {ex.Message}");
        }
    }
}
