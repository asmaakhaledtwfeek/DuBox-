using Dubox.Application.Abstractions;
using Dubox.Domain.Shared;
using MediatR;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Dubox.Application.Features.Schedule.Commands;

/// <summary>
/// Handler for exporting schedule activities to Excel with hierarchical structure.
/// Exports in the same format as the import template (KJ-158 structure).
/// Uses ADO.NET + vw_ScheduleActivitiesForExport for performance:
/// only the 12 columns needed are read ? no full entity load via EF.
/// </summary>
public class ExportScheduleActivitiesToExcelCommandHandler : IRequestHandler<ExportScheduleActivitiesToExcelCommand, Result<Stream>>
{
    /// <summary>
    /// Slim projection used only inside this handler ? avoids loading the full
    /// ScheduleActivity entity (Description, WIRCode, Weight, audit fields, etc.)
    /// </summary>
    private record ScheduleActivityRow(
        Guid      ScheduleActivityId,
        Guid?     ParentActivityId,
        int       OverallSequence,
        string    ActivityCode,
        string    ActivityName,
        DateTime  PlannedStartDate,
        DateTime  PlannedFinishDate,
        string    Stage,
        string    Status,
        decimal   PercentComplete,
        DateTime? ActualStartDate,
        DateTime? ActualFinishDate
    );

    private readonly IDbQueryExecutor _db;

    public ExportScheduleActivitiesToExcelCommandHandler(IDbQueryExecutor db)
    {
        _db = db;
    }

    // SQL kept as a constant so it is easy to review / tune.
    private const string Sql = @"
        SELECT
            ScheduleActivityId,
            ParentActivityId,
            OverallSequence,
            ActivityCode,
            ActivityName,
            PlannedStartDate,
            PlannedFinishDate,
            Stage,
            Status,
            PercentComplete,
            ActualStartDate,
            ActualFinishDate
        FROM [dbo].[vw_ScheduleActivitiesForExport]
        WHERE ProjectId = @projectId
        ORDER BY OverallSequence;";

    public async Task<Result<Stream>> Handle(ExportScheduleActivitiesToExcelCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Fetch only the columns needed for the export via ADO.NET.
            var activities = await _db.QueryListAsync(
                Sql,
                reader => new ScheduleActivityRow(
                    ScheduleActivityId: reader.GetGuid(0),
                    ParentActivityId:   reader.IsDBNull(1) ? null : reader.GetGuid(1),
                    OverallSequence:    reader.GetInt32(2),
                    ActivityCode:       reader.GetString(3),
                    ActivityName:       reader.GetString(4),
                    PlannedStartDate:   reader.GetDateTime(5),
                    PlannedFinishDate:  reader.GetDateTime(6),
                    Stage:              reader.GetString(7),
                    Status:             reader.GetString(8),
                    PercentComplete:    reader.GetDecimal(9),
                    ActualStartDate:    reader.IsDBNull(10) ? null : reader.GetDateTime(10),
                    ActualFinishDate:   reader.IsDBNull(11) ? null : reader.GetDateTime(11)
                ),
                parameters: [("@projectId", request.ProjectId)],
                cancellationToken: cancellationToken);

            if (!activities.Any())
            {
                return Result.Failure<Stream>(new Error("Export.NoData", "No activities found for this project"));
            }

            // Create children lookup (O(n) hierarchy build instead of O(n?))
            var childrenLookup = activities
                .Where(a => a.ParentActivityId.HasValue)
                .GroupBy(a => a.ParentActivityId!.Value)
                .ToDictionary(
                    g => g.Key,
                    g => g.OrderBy(a => a.OverallSequence).ToList()
                );

            // Create Excel file
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Schedule Activities");

            // Set up headers (matching import structure)
            SetupHeaders(worksheet);

            // ?? Perf: apply date/number formats once per column instead of per cell ??
            // This alone eliminates ~55,000 individual style operations for 11k rows.
            const string dateFormat = "dd-mmm-yy";
            worksheet.Column(3).Style.Numberformat.Format = dateFormat;  // PlannedStartDate
            worksheet.Column(4).Style.Numberformat.Format = dateFormat;  // PlannedFinishDate
            worksheet.Column(8).Style.Numberformat.Format = "0\"%\"";    // PercentComplete
            worksheet.Column(9).Style.Numberformat.Format = dateFormat;  // ActualStartDate
            worksheet.Column(10).Style.Numberformat.Format = dateFormat; // ActualFinishDate

            // ── Pass 1: collect ordered flat list (no EPPlus interaction yet) ──────
            // Tree traversal that just builds a List<> – zero EPPlus overhead here.
            var orderedRows = new List<(ScheduleActivityRow Activity, int IndentLevel)>(activities.Count);
            foreach (var root in activities
                         .Where(a => !a.ParentActivityId.HasValue)
                         .OrderBy(a => a.OverallSequence))
            {
                CollectOrderedRows(root, childrenLookup, orderedRows, 0);
            }

            int totalRows = orderedRows.Count;
            int lastRow   = 1 + totalRows; // 1-based, header is row 1

            // ── Pass 2: bulk-write all cell values via LoadFromArrays ─────────────
            // ONE EPPlus call instead of 110,000 individual Cells[r,c].Value = ...
            var dataArrays = new List<object?[]>(totalRows);
            foreach (var (a, indent) in orderedRows)
            {
                dataArrays.Add(new object?[]
                {
                    GetIndent(indent) + a.ActivityCode,  // col 1
                    a.ActivityName,                       // col 2
                    a.PlannedStartDate,                   // col 3  (format: column-level)
                    a.PlannedFinishDate,                  // col 4  (format: column-level)
                    (a.PlannedFinishDate - a.PlannedStartDate).Days, // col 5
                    a.Stage,                              // col 6
                    a.Status,                             // col 7
                    a.PercentComplete,                    // col 8  (format: column-level)
                    a.ActualStartDate,                    // col 9  (format: column-level)
                    a.ActualFinishDate                    // col 10 (format: column-level)
                });
            }
            worksheet.Cells[2, 1].LoadFromArrays(dataArrays);

            // ── Pass 3: outline levels + row colours (unavoidable per-row work) ───
            // Only root (level 0) and level-1 rows get background colours – typically
            // a small fraction of 11k rows, so this loop is cheap in practice.
            for (int i = 0; i < totalRows; i++)
            {
                var (activity, indentLevel) = orderedRows[i];
                int row = i + 2;

                var excelRow = worksheet.Row(row);
                excelRow.OutlineLevel = indentLevel;

                if (indentLevel > 0)
                {
                    excelRow.Hidden    = true;
                    excelRow.Collapsed = true;
                }

                if (indentLevel == 0)
                {
                    var rowRange = worksheet.Cells[row, 1, row, 10];
                    rowRange.Style.Font.Bold = true;
                    rowRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    var color = GetStageColor(activity.Stage);
                    rowRange.Style.Fill.BackgroundColor.SetColor(color);
                    if (IsDarkColor(color))
                        rowRange.Style.Font.Color.SetColor(System.Drawing.Color.White);
                }
                else if (indentLevel == 1)
                {
                    var rowRange = worksheet.Cells[row, 1, row, 10];
                    rowRange.Style.Font.Bold = true;
                    rowRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rowRange.Style.Fill.BackgroundColor.SetColor(GetLightStageColor(activity.Stage));
                }
            }

            // Configure worksheet for outline/grouping
            worksheet.OutLineSummaryBelow = false;
            worksheet.OutLineSummaryRight = false;

            // ── Borders: one range operation on the entire data block ─────────────
            if (totalRows > 0)
            {
                var dataRange = worksheet.Cells[2, 1, lastRow, 10];
                dataRange.Style.Border.Top.Style    = ExcelBorderStyle.Thin;
                dataRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                dataRange.Style.Border.Left.Style   = ExcelBorderStyle.Thin;
                dataRange.Style.Border.Right.Style  = ExcelBorderStyle.Thin;
                dataRange.Style.Border.Top.Color.SetColor(System.Drawing.Color.LightGray);
                dataRange.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.LightGray);
                dataRange.Style.Border.Left.Color.SetColor(System.Drawing.Color.LightGray);
                dataRange.Style.Border.Right.Color.SetColor(System.Drawing.Color.LightGray);
            }

            // Auto-fit columns only for reasonable dataset size
            if (activities.Count < 10000)
            {
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
            }

            // Set minimum column widths
            worksheet.Column(1).Width = Math.Max(worksheet.Column(1).Width, 30);
            worksheet.Column(2).Width = Math.Max(worksheet.Column(2).Width, 50);
            worksheet.Column(3).Width = Math.Max(worksheet.Column(3).Width, 15);
            worksheet.Column(4).Width = Math.Max(worksheet.Column(4).Width, 15);
            worksheet.Column(5).Width = Math.Max(worksheet.Column(5).Width, 12);
            worksheet.Column(6).Width = Math.Max(worksheet.Column(6).Width, 20);
            worksheet.Column(7).Width = Math.Max(worksheet.Column(7).Width, 15);
            worksheet.Column(8).Width = Math.Max(worksheet.Column(8).Width, 12);
            worksheet.Column(9).Width = Math.Max(worksheet.Column(9).Width, 15);
            worksheet.Column(10).Width = Math.Max(worksheet.Column(10).Width, 15);

            worksheet.View.FreezePanes(2, 1);

            // Save directly to stream (avoid double memory allocation)
            var stream = new MemoryStream();
            await package.SaveAsAsync(stream, cancellationToken);
            stream.Position = 0;

            return Result.Success<Stream>(stream);
        }
        catch (Exception ex)
        {
            return Result.Failure<Stream>(
                new Error("Export.Failed", $"Failed to export activities: {ex.Message}")
            );
        }
    }

    private void SetupHeaders(ExcelWorksheet worksheet)
    {
        worksheet.Cells[1, 1].Value = "Activity Code";
        worksheet.Cells[1, 2].Value = "Activity Name";
        worksheet.Cells[1, 3].Value = "BL1 Start";
        worksheet.Cells[1, 4].Value = "BL1 Finish";
        worksheet.Cells[1, 5].Value = "Original Duration";
        worksheet.Cells[1, 6].Value = "Stage";
        worksheet.Cells[1, 7].Value = "Status";
        worksheet.Cells[1, 8].Value = "Progress %";
        worksheet.Cells[1, 9].Value = "Actual Start";
        worksheet.Cells[1, 10].Value = "Actual Finish";

        using (var range = worksheet.Cells[1, 1, 1, 10])
        {
            range.Style.Font.Bold = true;
            range.Style.Font.Size = 11;
            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(27, 154, 170));
            range.Style.Font.Color.SetColor(System.Drawing.Color.White);
            range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            range.AutoFilter = true;
        }
    }

    /// <summary>
    /// Recursively collects the activity tree into a flat ordered list.
    /// No EPPlus interaction ? pure in-memory traversal.
    /// </summary>
    private static void CollectOrderedRows(
        ScheduleActivityRow activity,
        Dictionary<Guid, List<ScheduleActivityRow>> childrenLookup,
        List<(ScheduleActivityRow Activity, int IndentLevel)> result,
        int indentLevel)
    {
        result.Add((activity, indentLevel));

        if (childrenLookup.TryGetValue(activity.ScheduleActivityId, out var children))
            foreach (var child in children)
                CollectOrderedRows(child, childrenLookup, result, indentLevel + 1);
    }

    // Pre-computed indent strings ? avoids allocating a new string per row.
    // 20 levels covers any realistic hierarchy depth; safe fallback beyond that.
    private static readonly string[] IndentCache =
        Enumerable.Range(0, 20).Select(i => new string(' ', i * 2)).ToArray();

    private static string GetIndent(int level) =>
        level < IndentCache.Length ? IndentCache[level] : new string(' ', level * 2);

    private System.Drawing.Color GetStageColor(string stage)
    {
        return stage?.ToUpper() switch
        {
            string s when s.Contains("MILESTONE") => System.Drawing.Color.FromArgb(68, 114, 196),
            string s when s.Contains("DELIVERABLE") => System.Drawing.Color.FromArgb(112, 173, 71),
            string s when s.Contains("PREPARATION") => System.Drawing.Color.FromArgb(255, 192, 0),
            string s when s.Contains("APPROVAL") => System.Drawing.Color.FromArgb(146, 208, 80),
            string s when s.Contains("INVESTIGATION") => System.Drawing.Color.FromArgb(91, 155, 213),
            string s when s.Contains("DESIGN") => System.Drawing.Color.FromArgb(255, 217, 102),
            string s when s.Contains("ENGINEERING") => System.Drawing.Color.FromArgb(237, 125, 49),
            _ => System.Drawing.Color.FromArgb(68, 114, 196)
        };
    }

    private System.Drawing.Color GetLightStageColor(string stage)
    {
        return stage?.ToUpper() switch
        {
            string s when s.Contains("MILESTONE") => System.Drawing.Color.FromArgb(180, 198, 231),
            string s when s.Contains("DELIVERABLE") => System.Drawing.Color.FromArgb(198, 224, 180),
            string s when s.Contains("PREPARATION") => System.Drawing.Color.FromArgb(255, 230, 153),
            string s when s.Contains("APPROVAL") => System.Drawing.Color.FromArgb(208, 224, 227),
            string s when s.Contains("INVESTIGATION") => System.Drawing.Color.FromArgb(189, 215, 238),
            string s when s.Contains("DESIGN") => System.Drawing.Color.FromArgb(255, 242, 204),
            string s when s.Contains("ENGINEERING") => System.Drawing.Color.FromArgb(244, 176, 132),
            _ => System.Drawing.Color.FromArgb(221, 235, 247)
        };
    }

    private bool IsDarkColor(System.Drawing.Color color)
    {
        var brightness = (color.R * 299 + color.G * 587 + color.B * 114) / 1000;
        return brightness < 128;
    }
}
