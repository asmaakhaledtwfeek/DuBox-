using Dubox.Application.DTOs;
using Dubox.Application.Features.CustomReports.DataSources;
using Dubox.Application.Features.QualityIssues.Queries;
using Dubox.Application.Features.Reports.Queries;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.CustomReports.Queries;

public class ExecuteCustomReportQueryHandler : IRequestHandler<ExecuteCustomReportQuery, Result<ReportExecutionResultDto>>
{
    private readonly IMediator _mediator;

    public ExecuteCustomReportQueryHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<Result<ReportExecutionResultDto>> Handle(ExecuteCustomReportQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var config = request.Config;

            if (string.IsNullOrWhiteSpace(config.DataSource))
                return Result.Failure<ReportExecutionResultDto>("Data source is required.");

            var dataSourceDef = DataSourceRegistry.Get(config.DataSource);
            if (dataSourceDef is null)
                return Result.Failure<ReportExecutionResultDto>($"Unknown data source: '{config.DataSource}'.");

            // Ensure columns list has entries
            var columns = config.Columns.Any()
                ? config.Columns
                : dataSourceDef.Columns
                    .Where(c => c.Key != "projectId" && c.Key != "teamId" && c.Key != "search")
                    .Take(6)
                    .Select(c => c.Key)
                    .ToList();

            var (rows, totalCount) = config.DataSource switch
            {
                "activities"        => await ExecuteActivitiesAsync(config, request.Page, request.PageSize, cancellationToken),
                "teams_performance" => await ExecuteTeamsPerformanceAsync(config, request.Page, request.PageSize, cancellationToken),
                "boxes"             => await ExecuteBoxesAsync(config, request.Page, request.PageSize, cancellationToken),
                "projects"          => await ExecuteProjectsAsync(config, request.Page, request.PageSize, cancellationToken),
                "quality_issues"    => await ExecuteQualityIssuesAsync(config, request.Page, request.PageSize, cancellationToken),
                _                   => (new List<Dictionary<string, object?>>(), 0)
            };

            var totalPages = request.PageSize > 0
                ? (int)Math.Ceiling((double)totalCount / request.PageSize)
                : 1;

            var chartData = BuildChartData(rows, config.GroupBy, config.ChartYAxis, config.ChartType);

            var columnMeta = dataSourceDef.Columns
                .Where(c => columns.Contains(c.Key))
                .ToList();

            return Result.Success(new ReportExecutionResultDto
            {
                Columns = columns,
                ColumnMeta = columnMeta,
                Rows = rows,
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize,
                TotalPages = totalPages,
                ChartData = chartData
            });
        }
        catch (Exception ex)
        {
            return Result.Failure<ReportExecutionResultDto>($"Failed to execute report: {ex.Message}");
        }
    }

    // ─── Data source executors ──────────────────────────────────────────────

    private async Task<(List<Dictionary<string, object?>> Rows, int TotalCount)> ExecuteActivitiesAsync(
        CustomReportConfig config, int page, int pageSize, CancellationToken ct)
    {
        var query = new GetActivitiesReportQuery
        {
            Page = page,
            PageSize = pageSize,
            ProjectId = GetGuid(config.Filters, "projectId"),
            TeamId = GetGuid(config.Filters, "teamId"),
            Status = GetInt(config.Filters, "status"),
            Search = GetString(config.Filters, "search") ?? GetString(config.Filters, "activityName", "contains") ?? GetString(config.Filters, "boxTag", "contains"),
            PlannedStartDateFrom = GetDate(config.Filters, "plannedStartDate", "gt"),
            PlannedStartDateTo = GetDate(config.Filters, "plannedStartDate", "lt"),
            PlannedEndDateFrom = GetDate(config.Filters, "plannedEndDate", "gt"),
            PlannedEndDateTo = GetDate(config.Filters, "plannedEndDate", "lt"),
        };

        var result = await _mediator.Send(query, ct);
        if (!result.IsSuccess) return (new(), 0);

        var rows = result.Data.Items.Select(a => new Dictionary<string, object?>
        {
            ["activityName"]            = a.ActivityName,
            ["boxTag"]                  = a.BoxTag,
            ["projectName"]             = a.ProjectName,
            ["assignedTeam"]            = a.AssignedTeam,
            ["status"]                  = a.Status,
            ["progressPercentage"]      = a.ProgressPercentage,
            ["plannedStartDate"]        = a.PlannedStartDate,
            ["plannedEndDate"]          = a.PlannedEndDate,
            ["actualStartDate"]         = a.ActualStartDate,
            ["actualEndDate"]           = a.ActualEndDate,
            ["actualDurationFormatted"] = a.ActualDurationFormatted,
            ["delayDaysFormatted"]      = a.DelayDaysFormatted,
        }).ToList();

        return (ProjectColumns(rows, config.Columns), result.Data.TotalCount);
    }

    private async Task<(List<Dictionary<string, object?>> Rows, int TotalCount)> ExecuteTeamsPerformanceAsync(
        CustomReportConfig config, int page, int pageSize, CancellationToken ct)
    {
        var query = new GetTeamsPerformanceReportQuery
        {
            Page = page,
            PageSize = pageSize,
            ProjectId = GetGuid(config.Filters, "projectId"),
            Status = GetInt(config.Filters, "status"),
            Search = GetString(config.Filters, "search") ?? GetString(config.Filters, "teamName", "contains"),
        };

        var result = await _mediator.Send(query, ct);
        if (!result.IsSuccess) return (new(), 0);

        var rows = result.Data.Items.Select(t => new Dictionary<string, object?>
        {
            ["teamName"]         = t.TeamName,
            ["teamCode"]         = t.TeamCode,
            ["membersCount"]     = t.MembersCount,
            ["totalActivities"]  = t.TotalAssignedActivities,
            ["completed"]        = t.Completed,
            ["inProgress"]       = t.InProgress,
            ["pending"]          = t.Pending,
            ["delayed"]          = t.Delayed,
            ["averageProgress"]  = t.AverageTeamProgress,
            ["workloadLevel"]    = t.WorkloadLevel,
        }).ToList();

        return (ProjectColumns(rows, config.Columns), result.Data.TotalCount);
    }

    private async Task<(List<Dictionary<string, object?>> Rows, int TotalCount)> ExecuteBoxesAsync(
        CustomReportConfig config, int page, int pageSize, CancellationToken ct)
    {
        var query = new GetBoxesSummaryReportQuery
        {
            PageNumber = page,
            PageSize = pageSize,
            ProjectId = GetGuid(config.Filters, "projectId"),
            Search = GetString(config.Filters, "search") ?? GetString(config.Filters, "boxTag", "contains"),
            Status = GetIntList(config.Filters, "status"),
            ProgressMin = GetDecimal(config.Filters, "progressPercentage", "gt"),
            ProgressMax = GetDecimal(config.Filters, "progressPercentage", "lt"),
            Floor = GetString(config.Filters, "floor"),
            BuildingNumber = GetString(config.Filters, "buildingNumber"),
            Zone = GetString(config.Filters, "zone"),
            DateFrom = GetDate(config.Filters, "plannedStartDate", "gt"),
            DateTo = GetDate(config.Filters, "plannedStartDate", "lt"),
        };

        var result = await _mediator.Send(query, ct);
        if (!result.IsSuccess) return (new(), 0);

        var rows = result.Data.Items.Select(b => new Dictionary<string, object?>
        {
            ["boxTag"]              = b.BoxTag,
            ["projectName"]         = b.ProjectName,
            ["projectCode"]         = b.ProjectCode,
            ["floor"]               = b.Floor,
            ["buildingNumber"]      = b.BuildingNumber,
            ["zone"]                = b.Zone,
            ["status"]              = b.Status,
            ["progressPercentage"]  = b.ProgressPercentage,
            ["plannedStartDate"]    = b.PlannedStartDate,
            ["plannedEndDate"]      = b.PlannedEndDate,
            ["factoryName"]         = b.FactoryName,
            ["activitiesCount"]     = b.ActivitiesCount,
        }).ToList();

        return (ProjectColumns(rows, config.Columns), result.Data.TotalCount);
    }

    private async Task<(List<Dictionary<string, object?>> Rows, int TotalCount)> ExecuteProjectsAsync(
        CustomReportConfig config, int page, int pageSize, CancellationToken ct)
    {
        var query = new GetProjectsSummaryReportQuery
        {
            PageNumber = page,
            PageSize = pageSize,
            Search = GetString(config.Filters, "search") ?? GetString(config.Filters, "projectName", "contains"),
        };

        var result = await _mediator.Send(query, ct);
        if (!result.IsSuccess) return (new(), 0);

        var rows = result.Data.Items.Select(p => new Dictionary<string, object?>
        {
            ["projectCode"]         = p.ProjectCode,
            ["projectName"]         = p.ProjectName,
            ["clientName"]          = p.ClientName,
            ["location"]            = p.Location,
            ["status"]              = p.Status,
            ["progressPercentage"]  = p.ProgressPercentage,
            ["totalBoxes"]          = p.TotalBoxes,
        }).ToList();

        return (ProjectColumns(rows, config.Columns), result.Data.TotalCount);
    }

    private async Task<(List<Dictionary<string, object?>> Rows, int TotalCount)> ExecuteQualityIssuesAsync(
        CustomReportConfig config, int page, int pageSize, CancellationToken ct)
    {
        var query = new GetQualityIssuesQuery(
            SearchTerm: GetString(config.Filters, "search") ?? GetString(config.Filters, "issueNumber", "contains"),
            Status: null,
            Severity: null,
            IssueType: null,
            IssueNumber: GetString(config.Filters, "issueNumber"),
            BoxTag: GetString(config.Filters, "boxTag"),
            ProjectCode: null,
            AssignedUser: null,
            Page: page,
            PageSize: pageSize
        );

        var result = await _mediator.Send(query, ct);
        if (!result.IsSuccess) return (new(), 0);

        var rows = result.Data.Items.Select(q => new Dictionary<string, object?>
        {
            ["issueNumber"]     = q.IssueNumber,
            ["issueDate"]       = q.IssueDate,
            ["issueType"]       = q.IssueType?.ToString(),
            ["severity"]        = q.Severity?.ToString(),
            ["issueDescription"]= q.IssueDescription,
            ["reportedBy"]      = q.ReportedBy,
            ["assignedTeamName"]= q.AssignedTeamName,
            ["status"]          = q.Status.ToString(),
            ["dueDate"]         = q.DueDate,
            ["boxTag"]          = q.BoxTag,
            ["projectName"]     = q.ProjectName,
            ["isOverdue"]       = q.IsOverdue ? "Yes" : "No",
        }).ToList();

        return (ProjectColumns(rows, config.Columns), result.Data.TotalCount);
    }

    // ─── Helpers ────────────────────────────────────────────────────────────

    /// <summary>Keep only requested columns in each row (preserves ordering).</summary>
    private static List<Dictionary<string, object?>> ProjectColumns(
        List<Dictionary<string, object?>> rows, List<string> columns)
    {
        if (!columns.Any()) return rows;
        return rows.Select(r => columns.ToDictionary(
            c => c,
            c => r.TryGetValue(c, out var v) ? v : null
        )).ToList();
    }

    private static List<ChartDataPointDto>? BuildChartData(
        List<Dictionary<string, object?>> rows,
        string? groupBy,
        string? yAxis,
        string chartType)
    {
        if (chartType == "table" || string.IsNullOrEmpty(groupBy)) return null;

        return rows
            .GroupBy(r => r.TryGetValue(groupBy, out var v) ? v?.ToString() ?? "Unknown" : "Unknown")
            .Select(g =>
            {
                double value;
                if (!string.IsNullOrEmpty(yAxis))
                {
                    value = g.Sum(r =>
                    {
                        if (r.TryGetValue(yAxis, out var v) && v != null)
                            return double.TryParse(v.ToString(), out var d) ? d : 0;
                        return 0;
                    });
                }
                else
                {
                    value = g.Count();
                }

                return new ChartDataPointDto { Label = g.Key, Value = value };
            })
            .OrderByDescending(p => p.Value)
            .ToList();
    }

    private static Guid? GetGuid(List<ReportFilterConfig> filters, string field)
    {
        var f = filters.FirstOrDefault(x => x.Field == field && x.Operator == "eq");
        if (f?.Value == null) return null;
        return Guid.TryParse(f.Value.ToString(), out var g) ? g : null;
    }

    private static int? GetInt(List<ReportFilterConfig> filters, string field)
    {
        var f = filters.FirstOrDefault(x => x.Field == field && x.Operator == "eq");
        if (f?.Value == null) return null;
        return int.TryParse(f.Value.ToString(), out var i) ? i : null;
    }

    private static List<int>? GetIntList(List<ReportFilterConfig> filters, string field)
    {
        var val = GetInt(filters, field);
        return val.HasValue ? new List<int> { val.Value } : null;
    }

    private static decimal? GetDecimal(List<ReportFilterConfig> filters, string field, string op)
    {
        var f = filters.FirstOrDefault(x => x.Field == field && x.Operator == op);
        if (f?.Value == null) return null;
        return decimal.TryParse(f.Value.ToString(), out var d) ? d : null;
    }

    private static string? GetString(List<ReportFilterConfig> filters, string field, string op = "eq")
    {
        var f = filters.FirstOrDefault(x => x.Field == field && x.Operator == op);
        return string.IsNullOrWhiteSpace(f?.Value?.ToString()) ? null : f.Value.ToString();
    }

    private static DateTime? GetDate(List<ReportFilterConfig> filters, string field, string op)
    {
        var f = filters.FirstOrDefault(x => x.Field == field && x.Operator == op);
        if (f?.Value == null) return null;
        return DateTime.TryParse(f.Value.ToString(), out var dt) ? dt : null;
    }
}
