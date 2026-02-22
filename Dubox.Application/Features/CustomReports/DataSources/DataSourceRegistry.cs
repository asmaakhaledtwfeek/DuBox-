using Dubox.Application.DTOs;

namespace Dubox.Application.Features.CustomReports.DataSources;

/// <summary>
/// Central registry of available report data sources and their column/filter metadata.
/// Each data source key maps to an existing report query infrastructure.
/// </summary>
public static class DataSourceRegistry
{
    private static readonly List<string> StringOps = new() { "eq", "contains" };
    private static readonly List<string> NumberOps = new() { "eq", "gt", "lt", "between" };
    private static readonly List<string> DateOps = new() { "gt", "lt", "between" };
    private static readonly List<string> StatusOps = new() { "eq", "neq" };

    private static readonly List<OptionDto> ActivityStatusOptions = new()
    {
        new() { Value = "1", Label = "Not Started" },
        new() { Value = "2", Label = "In Progress" },
        new() { Value = "3", Label = "Completed" },
        new() { Value = "4", Label = "On Hold" },
        new() { Value = "5", Label = "Delayed" }
    };

    private static readonly List<OptionDto> BoxStatusOptions = new()
    {
        new() { Value = "1", Label = "Not Started" },
        new() { Value = "2", Label = "In Progress" },
        new() { Value = "3", Label = "Completed" },
        new() { Value = "4", Label = "On Hold" },
        new() { Value = "5", Label = "Delayed" }
    };

    private static readonly List<OptionDto> QualityStatusOptions = new()
    {
        new() { Value = "0", Label = "Open" },
        new() { Value = "1", Label = "In Progress" },
        new() { Value = "2", Label = "Resolved" },
        new() { Value = "3", Label = "Closed" }
    };

    private static readonly List<OptionDto> SeverityOptions = new()
    {
        new() { Value = "0", Label = "Minor" },
        new() { Value = "1", Label = "Major" },
        new() { Value = "2", Label = "Critical" }
    };

    public static readonly Dictionary<string, DataSourceDefinitionDto> DataSources = new()
    {
        ["activities"] = new DataSourceDefinitionDto
        {
            Key = "activities",
            Label = "Activities",
            Columns = new List<ColumnMetadataDto>
            {
                new() { Key = "activityName",           Label = "Activity Name",        DataType = "string",  IsFilterable = true,  AllowedOperators = StringOps },
                new() { Key = "boxTag",                 Label = "Box Tag",              DataType = "string",  IsFilterable = true,  AllowedOperators = StringOps },
                new() { Key = "projectName",            Label = "Project",              DataType = "string",  IsFilterable = true,  AllowedOperators = StringOps },
                new() { Key = "assignedTeam",           Label = "Assigned Team",        DataType = "string",  IsFilterable = true,  AllowedOperators = StringOps },
                new() { Key = "status",                 Label = "Status",               DataType = "status",  IsFilterable = true,  IsGroupable = true, AllowedOperators = StatusOps, Options = ActivityStatusOptions },
                new() { Key = "progressPercentage",     Label = "Progress %",           DataType = "number",  IsFilterable = true,  IsGroupable = false, AllowedOperators = NumberOps },
                new() { Key = "plannedStartDate",       Label = "Planned Start",        DataType = "date",    IsFilterable = true,  AllowedOperators = DateOps },
                new() { Key = "plannedEndDate",         Label = "Planned End",          DataType = "date",    IsFilterable = true,  AllowedOperators = DateOps },
                new() { Key = "actualStartDate",        Label = "Actual Start",         DataType = "date",    IsFilterable = false, AllowedOperators = new() },
                new() { Key = "actualEndDate",          Label = "Actual End",           DataType = "date",    IsFilterable = false, AllowedOperators = new() },
                new() { Key = "actualDurationFormatted",Label = "Duration",             DataType = "string",  IsFilterable = false, AllowedOperators = new() },
                new() { Key = "delayDaysFormatted",     Label = "Delay",                DataType = "string",  IsFilterable = false, AllowedOperators = new() },
                // Meta-filter fields (rendered as dedicated controls in the UI)
                new() { Key = "projectId",              Label = "Project Filter",       DataType = "guid",    IsFilterable = true,  AllowedOperators = new() { "eq" } },
                new() { Key = "teamId",                 Label = "Team Filter",          DataType = "guid",    IsFilterable = true,  AllowedOperators = new() { "eq" } },
                new() { Key = "search",                 Label = "Search",               DataType = "string",  IsFilterable = true,  AllowedOperators = new() { "contains" } },
            }
        },
        ["teams_performance"] = new DataSourceDefinitionDto
        {
            Key = "teams_performance",
            Label = "Teams Performance",
            Columns = new List<ColumnMetadataDto>
            {
                new() { Key = "teamName",           Label = "Team Name",            DataType = "string",  IsFilterable = true,  AllowedOperators = StringOps },
                new() { Key = "teamCode",           Label = "Team Code",            DataType = "string",  IsFilterable = true,  AllowedOperators = StringOps },
                new() { Key = "membersCount",       Label = "Members",              DataType = "number",  IsFilterable = false, AllowedOperators = new() },
                new() { Key = "totalActivities",    Label = "Total Activities",     DataType = "number",  IsFilterable = false, IsGroupable = false, AllowedOperators = new() },
                new() { Key = "completed",          Label = "Completed",            DataType = "number",  IsFilterable = false, IsGroupable = false, AllowedOperators = new() },
                new() { Key = "inProgress",         Label = "In Progress",          DataType = "number",  IsFilterable = false, AllowedOperators = new() },
                new() { Key = "pending",            Label = "Pending",              DataType = "number",  IsFilterable = false, AllowedOperators = new() },
                new() { Key = "delayed",            Label = "Delayed",              DataType = "number",  IsFilterable = false, AllowedOperators = new() },
                new() { Key = "averageProgress",    Label = "Avg Progress %",       DataType = "number",  IsFilterable = false, AllowedOperators = new() },
                new() { Key = "workloadLevel",      Label = "Workload",             DataType = "status",  IsFilterable = true,  IsGroupable = true, AllowedOperators = StatusOps },
                // Meta-filter fields
                new() { Key = "projectId",          Label = "Project Filter",       DataType = "guid",    IsFilterable = true,  AllowedOperators = new() { "eq" } },
                new() { Key = "search",             Label = "Search",               DataType = "string",  IsFilterable = true,  AllowedOperators = new() { "contains" } },
            }
        },
        ["boxes"] = new DataSourceDefinitionDto
        {
            Key = "boxes",
            Label = "Boxes",
            Columns = new List<ColumnMetadataDto>
            {
                new() { Key = "boxTag",             Label = "Box Tag",              DataType = "string",  IsFilterable = true,  AllowedOperators = StringOps },
                new() { Key = "projectName",        Label = "Project",              DataType = "string",  IsFilterable = true,  AllowedOperators = StringOps },
                new() { Key = "projectCode",        Label = "Project Code",         DataType = "string",  IsFilterable = true,  AllowedOperators = StringOps },
                new() { Key = "floor",              Label = "Floor",                DataType = "string",  IsFilterable = true,  AllowedOperators = StringOps },
                new() { Key = "buildingNumber",     Label = "Building",             DataType = "string",  IsFilterable = true,  AllowedOperators = StringOps },
                new() { Key = "zone",               Label = "Zone",                 DataType = "string",  IsFilterable = true,  AllowedOperators = StringOps },
                new() { Key = "status",             Label = "Status",               DataType = "status",  IsFilterable = true,  IsGroupable = true, AllowedOperators = StatusOps, Options = BoxStatusOptions },
                new() { Key = "progressPercentage", Label = "Progress %",           DataType = "number",  IsFilterable = true,  AllowedOperators = NumberOps },
                new() { Key = "plannedStartDate",   Label = "Planned Start",        DataType = "date",    IsFilterable = true,  AllowedOperators = DateOps },
                new() { Key = "plannedEndDate",     Label = "Planned End",          DataType = "date",    IsFilterable = true,  AllowedOperators = DateOps },
                new() { Key = "factoryName",        Label = "Factory",              DataType = "string",  IsFilterable = false, AllowedOperators = new() },
                new() { Key = "activitiesCount",    Label = "Activities Count",     DataType = "number",  IsFilterable = false, AllowedOperators = new() },
                // Meta-filter fields
                new() { Key = "projectId",          Label = "Project Filter",       DataType = "guid",    IsFilterable = true,  AllowedOperators = new() { "eq" } },
                new() { Key = "search",             Label = "Search",               DataType = "string",  IsFilterable = true,  AllowedOperators = new() { "contains" } },
            }
        },
        ["projects"] = new DataSourceDefinitionDto
        {
            Key = "projects",
            Label = "Projects",
            Columns = new List<ColumnMetadataDto>
            {
                new() { Key = "projectCode",        Label = "Project Code",         DataType = "string",  IsFilterable = true,  AllowedOperators = StringOps },
                new() { Key = "projectName",        Label = "Project Name",         DataType = "string",  IsFilterable = true,  AllowedOperators = StringOps },
                new() { Key = "clientName",         Label = "Client",               DataType = "string",  IsFilterable = false, AllowedOperators = new() },
                new() { Key = "location",           Label = "Location",             DataType = "string",  IsFilterable = false, AllowedOperators = new() },
                new() { Key = "status",             Label = "Status",               DataType = "status",  IsFilterable = true,  IsGroupable = true, AllowedOperators = StatusOps },
                new() { Key = "progressPercentage", Label = "Progress %",           DataType = "number",  IsFilterable = true,  AllowedOperators = NumberOps },
                new() { Key = "totalBoxes",         Label = "Total Boxes",          DataType = "number",  IsFilterable = false, AllowedOperators = new() },
                // Meta-filter fields
                new() { Key = "search",             Label = "Search",               DataType = "string",  IsFilterable = true,  AllowedOperators = new() { "contains" } },
            }
        },
        ["quality_issues"] = new DataSourceDefinitionDto
        {
            Key = "quality_issues",
            Label = "Quality Issues",
            Columns = new List<ColumnMetadataDto>
            {
                new() { Key = "issueNumber",        Label = "Issue #",              DataType = "string",  IsFilterable = true,  AllowedOperators = StringOps },
                new() { Key = "issueDate",          Label = "Issue Date",           DataType = "date",    IsFilterable = true,  AllowedOperators = DateOps },
                new() { Key = "issueType",          Label = "Issue Type",           DataType = "status",  IsFilterable = true,  IsGroupable = true, AllowedOperators = StatusOps },
                new() { Key = "severity",           Label = "Severity",             DataType = "status",  IsFilterable = true,  IsGroupable = true, AllowedOperators = StatusOps, Options = SeverityOptions },
                new() { Key = "issueDescription",   Label = "Description",          DataType = "string",  IsFilterable = false, AllowedOperators = new() },
                new() { Key = "reportedBy",         Label = "Reported By",          DataType = "string",  IsFilterable = true,  AllowedOperators = StringOps },
                new() { Key = "assignedTeamName",   Label = "Assigned Team",        DataType = "string",  IsFilterable = false, AllowedOperators = new() },
                new() { Key = "status",             Label = "Status",               DataType = "status",  IsFilterable = true,  IsGroupable = true, AllowedOperators = StatusOps, Options = QualityStatusOptions },
                new() { Key = "dueDate",            Label = "Due Date",             DataType = "date",    IsFilterable = true,  AllowedOperators = DateOps },
                new() { Key = "boxTag",             Label = "Box Tag",              DataType = "string",  IsFilterable = true,  AllowedOperators = StringOps },
                new() { Key = "projectName",        Label = "Project",              DataType = "string",  IsFilterable = false, AllowedOperators = new() },
                new() { Key = "isOverdue",          Label = "Is Overdue",           DataType = "string",  IsFilterable = false, AllowedOperators = new() },
                // Meta-filter fields
                new() { Key = "projectId",          Label = "Project Filter",       DataType = "guid",    IsFilterable = true,  AllowedOperators = new() { "eq" } },
                new() { Key = "search",             Label = "Search",               DataType = "string",  IsFilterable = true,  AllowedOperators = new() { "contains" } },
            }
        }
    };

    public static DataSourceDefinitionDto? Get(string key)
        => DataSources.TryGetValue(key, out var def) ? def : null;

    public static List<DataSourceDefinitionDto> GetAll()
        => DataSources.Values.ToList();
}
