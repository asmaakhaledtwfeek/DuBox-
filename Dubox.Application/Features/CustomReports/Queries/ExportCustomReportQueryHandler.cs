using Dubox.Application.DTOs;
using Dubox.Application.Features.CustomReports.DataSources;
using Dubox.Domain.Shared;
using MediatR;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Style;
using System.Drawing;

namespace Dubox.Application.Features.CustomReports.Queries;

public class ExportCustomReportQueryHandler : IRequestHandler<ExportCustomReportQuery, Result<Stream>>
{
    private readonly IMediator _mediator;

    public ExportCustomReportQueryHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<Result<Stream>> Handle(ExportCustomReportQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var executeQuery = new ExecuteCustomReportQuery(request.Config, Page: 1, PageSize: 5000);
            var result       = await _mediator.Send(executeQuery, cancellationToken);

            if (!result.IsSuccess)
                return Result.Failure<Stream>(result.Message);

            var data          = result.Data;
            var dataSourceDef = DataSourceRegistry.Get(request.Config.DataSource);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage();

            // ── Data worksheet ─────────────────────────────────────────────────
            var ws = package.Workbook.Worksheets.Add("Data");

            var brandColor = Color.FromArgb(27, 154, 170);  // AMANA teal

            // Header row
            for (int col = 0; col < data.Columns.Count; col++)
            {
                var colKey = data.Columns[col];
                var meta   = dataSourceDef?.Columns.FirstOrDefault(c => c.Key == colKey);
                var label  = meta?.Label ?? colKey;

                var hdr = ws.Cells[1, col + 1];
                hdr.Value = label;
                hdr.Style.Font.Bold = true;
                hdr.Style.Fill.PatternType = ExcelFillStyle.Solid;
                hdr.Style.Fill.BackgroundColor.SetColor(brandColor);
                hdr.Style.Font.Color.SetColor(Color.White);
                hdr.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                hdr.Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                hdr.Style.Border.Bottom.Color.SetColor(Color.FromArgb(21, 120, 132));
            }

            // Data rows
            for (int row = 0; row < data.Rows.Count; row++)
            {
                for (int col = 0; col < data.Columns.Count; col++)
                {
                    var colKey    = data.Columns[col];
                    var cellValue = data.Rows[row].TryGetValue(colKey, out var val) ? val : null;
                    var cell      = ws.Cells[row + 2, col + 1];

                    if (cellValue is DateTime dt)
                    {
                        cell.Value = dt.ToString("yyyy-MM-dd HH:mm");
                    }
                    else if (cellValue is decimal dec)
                    {
                        cell.Value = (double)dec;
                        cell.Style.Numberformat.Format = "0.00";
                    }
                    else if (cellValue is int or long)
                    {
                        cell.Value = Convert.ToDouble(cellValue);
                    }
                    else
                    {
                        cell.Value = cellValue?.ToString() ?? string.Empty;
                    }

                    // Alternate row tint
                    if (row % 2 == 1)
                    {
                        cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        cell.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(240, 249, 250));
                    }

                    // Light border
                    cell.Style.Border.Bottom.Style = ExcelBorderStyle.Hair;
                    cell.Style.Border.Bottom.Color.SetColor(Color.FromArgb(220, 220, 220));
                }
            }

            if (data.Columns.Count > 0 && data.Rows.Count > 0)
                ws.Cells[ws.Dimension.Address].AutoFitColumns();

            // ── Chart worksheet (when a chart type other than table is set) ────
            var chartType  = request.Config.ChartType?.ToLower() ?? "table";
            var groupByKey = request.Config.GroupBy;
            var yAxisKey   = request.Config.ChartYAxis;

            bool hasChartData = chartType != "table"
                && !string.IsNullOrEmpty(groupByKey)
                && data.Rows.Count > 0;

            if (hasChartData)
            {
                // Build aggregated series: group by groupByKey, sum yAxisKey (or count)
                var grouped = data.Rows
                    .GroupBy(r => r.TryGetValue(groupByKey!, out var gv) ? gv?.ToString() ?? "(blank)" : "(blank)")
                    .Select(g =>
                    {
                        double sum = 0;
                        if (!string.IsNullOrEmpty(yAxisKey))
                        {
                            sum = g.Sum(r =>
                            {
                                if (r.TryGetValue(yAxisKey, out var yv) && yv != null)
                                    return Convert.ToDouble(yv);
                                return 0;
                            });
                        }
                        else
                        {
                            sum = g.Count();
                        }
                        return (Label: g.Key, Value: sum);
                    })
                    .OrderByDescending(x => x.Value)
                    .Take(20)   // cap at 20 series items for readability
                    .ToList();

                if (grouped.Count > 0)
                {
                    var wsCht = package.Workbook.Worksheets.Add("Chart Data");

                    // Write the aggregated data
                    wsCht.Cells[1, 1].Value = "Category";
                    wsCht.Cells[1, 2].Value = string.IsNullOrEmpty(yAxisKey) ? "Count" : yAxisKey;

                    for (int i = 0; i < grouped.Count; i++)
                    {
                        wsCht.Cells[i + 2, 1].Value = grouped[i].Label;
                        wsCht.Cells[i + 2, 2].Value = grouped[i].Value;
                    }

                    wsCht.Cells[1, 1, 1, 2].Style.Font.Bold = true;
                    wsCht.Cells[1, 1, 1, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    wsCht.Cells[1, 1, 1, 2].Style.Fill.BackgroundColor.SetColor(brandColor);
                    wsCht.Cells[1, 1, 1, 2].Style.Font.Color.SetColor(Color.White);
                    wsCht.Column(1).AutoFit();
                    wsCht.Column(2).AutoFit();

                    // Determine EPPlus chart type
                    eChartType epplusType = chartType switch
                    {
                        "line" => eChartType.Line,
                        "pie"  => eChartType.Pie,
                        _      => eChartType.ColumnClustered
                    };

                    var chart = wsCht.Drawings.AddChart("ReportChart", epplusType);
                    chart.Title.Text = $"{groupByKey} Distribution";
                    chart.SetPosition(grouped.Count + 3, 0, 0, 0);
                    chart.SetSize(700, 380);

                    var series = chart.Series.Add(
                        wsCht.Cells[2, 2, grouped.Count + 1, 2],
                        wsCht.Cells[2, 1, grouped.Count + 1, 1]);
                    series.Header = string.IsNullOrEmpty(yAxisKey) ? "Count" : yAxisKey;

                    // Style
                    chart.Legend.Position = eLegendPosition.Bottom;
                }
            }

            var stream = new MemoryStream();
            package.SaveAs(stream);
            stream.Position = 0;

            return Result.Success<Stream>(stream);
        }
        catch (Exception ex)
        {
            return Result.Failure<Stream>($"Export failed: {ex.Message}");
        }
    }
}
