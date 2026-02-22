namespace Dubox.Application.DTOs;

// ─── Config model stored as JSON in the database ───────────────────────────

public class CustomReportConfig
{
    public string DataSource { get; set; } = string.Empty;
    public List<string> Columns { get; set; } = new();
    public List<ReportFilterConfig> Filters { get; set; } = new();
    /// <summary>bar | line | pie | table</summary>
    public string ChartType { get; set; } = "table";
    public string? ChartXAxis { get; set; }
    public string? ChartYAxis { get; set; }
    public string? GroupBy { get; set; }
}

public class ReportFilterConfig
{
    public string Field { get; set; } = string.Empty;
    /// <summary>eq | neq | gt | lt | contains | between</summary>
    public string Operator { get; set; } = "eq";
    public object? Value { get; set; }
    /// <summary>Second value used only for the "between" operator.</summary>
    public object? Value2 { get; set; }
}

// ─── API DTOs ───────────────────────────────────────────────────────────────

public class CustomReportDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public CustomReportConfig Config { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CustomReportListItemDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string DataSource { get; set; } = string.Empty;
    public string ChartType { get; set; } = string.Empty;
    public int ColumnCount { get; set; }
    public DateTime CreatedAt { get; set; }
}

// ─── Execution result ───────────────────────────────────────────────────────

public class ReportExecutionResultDto
{
    public List<string> Columns { get; set; } = new();
    public List<ColumnMetadataDto> ColumnMeta { get; set; } = new();
    public List<Dictionary<string, object?>> Rows { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public List<ChartDataPointDto>? ChartData { get; set; }
}

public class ChartDataPointDto
{
    public string Label { get; set; } = string.Empty;
    public double Value { get; set; }
}

// ─── Data source / column metadata ─────────────────────────────────────────

public class ColumnMetadataDto
{
    public string Key { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    /// <summary>string | number | date | status | guid</summary>
    public string DataType { get; set; } = string.Empty;
    public bool IsFilterable { get; set; } = true;
    public bool IsGroupable { get; set; } = false;
    public List<string> AllowedOperators { get; set; } = new();
    /// <summary>Pre-defined value options for status/enum fields.</summary>
    public List<OptionDto>? Options { get; set; }
}

public class OptionDto
{
    public string Value { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
}

public class DataSourceDefinitionDto
{
    public string Key { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public List<ColumnMetadataDto> Columns { get; set; } = new();
}
