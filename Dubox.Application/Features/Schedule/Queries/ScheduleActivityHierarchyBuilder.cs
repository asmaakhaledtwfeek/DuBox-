using Dubox.Application.DTOs;
using Dubox.Domain.Entities;

namespace Dubox.Application.Features.Schedule.Queries;

/// <summary>
/// Flat projection row used by the ADO.NET path – contains exactly
/// what vw_ScheduleActivitiesWithCounts returns (no entity graph needed).
/// </summary>
internal record ScheduleActivityFlatRow(
    Guid      ScheduleActivityId,
    Guid?     ParentActivityId,
    int       OverallSequence,
    string    ActivityCode,
    string    ActivityName,
    string    Stage,
    int       StageNumber,
    bool      IsCustomActivity,
    DateTime  PlannedStartDate,
    DateTime  PlannedFinishDate,
    DateTime? ActualStartDate,
    DateTime? ActualFinishDate,
    string    Status,
    decimal   PercentComplete,
    decimal   Weight,
    int       TeamCount,
    int       MaterialCount
);

/// <summary>
/// Helper class to build hierarchical tree structure from flat list of activities
/// </summary>
internal static class ScheduleActivityHierarchyBuilder
{
    // ── ADO.NET path ────────────────────────────────────────────────────────
    /// <summary>
    /// Builds hierarchy from the flat ADO.NET projection rows
    /// (replaces the EF-entity overload for read queries).
    /// </summary>
    public static List<ScheduleActivityListDto> BuildHierarchy(List<ScheduleActivityFlatRow> allActivities)
    {
        var nodeDict = new Dictionary<Guid, ActivityNode>(allActivities.Count);

        foreach (var row in allActivities)
        {
            nodeDict[row.ScheduleActivityId] = new ActivityNode
            {
                ScheduleActivityId = row.ScheduleActivityId,
                ActivityCode       = row.ActivityCode,
                ActivityName       = row.ActivityName,
                Stage              = row.Stage,
                StageNumber        = row.StageNumber,
                OverallSequence    = row.OverallSequence,
                IsCustomActivity   = row.IsCustomActivity,
                PlannedStartDate   = row.PlannedStartDate,
                PlannedFinishDate  = row.PlannedFinishDate,
                ActualStartDate    = row.ActualStartDate,
                ActualFinishDate   = row.ActualFinishDate,
                Status             = row.Status,
                PercentComplete    = row.PercentComplete,
                Weight             = row.Weight,
                TeamCount          = row.TeamCount,
                MaterialCount      = row.MaterialCount,
                ParentActivityId   = row.ParentActivityId,
                Children           = new List<ActivityNode>()
            };
        }

        return BuildFromNodeDict(allActivities.Select(r => (r.ScheduleActivityId, r.ParentActivityId)), nodeDict);
    }

    // ── EF-entity path (kept for backward compatibility) ────────────────────
    /// <summary>
    /// Builds a hierarchical tree structure from a flat list of ScheduleActivity entities
    /// Optimized for performance with minimal logging
    /// </summary>
    public static List<ScheduleActivityListDto> BuildHierarchy(List<ScheduleActivity> allActivities)
    {
        // Use Dictionary for O(1) lookups instead of O(n) searches
        var nodeDict = new Dictionary<Guid, ActivityNode>(allActivities.Count);

        // First pass: Create mutable nodes for all activities
        foreach (var activity in allActivities)
        {
            var node = new ActivityNode
            {
                ScheduleActivityId = activity.ScheduleActivityId,
                ActivityCode = activity.ActivityCode,
                ActivityName = activity.ActivityName,
                Stage = activity.Stage,
                StageNumber = activity.StageNumber,
                OverallSequence = activity.OverallSequence,
                IsCustomActivity = activity.IsCustomActivity,
                PlannedStartDate = activity.PlannedStartDate,
                PlannedFinishDate = activity.PlannedFinishDate,
                ActualFinishDate = activity.ActualFinishDate,
                ActualStartDate = activity.ActualStartDate,
                Status = activity.Status,
                PercentComplete = activity.PercentComplete,
                Weight = activity.Weight,
                TeamCount = activity.AssignedTeams?.Count ?? 0,
                MaterialCount = activity.AssignedMaterials?.Count ?? 0,
                ParentActivityId = activity.ParentActivityId,
                Children = new List<ActivityNode>()
            };

            nodeDict[activity.ScheduleActivityId] = node;
        }

        return BuildFromNodeDict(
            allActivities.Select(a => (a.ScheduleActivityId, a.ParentActivityId)),
            nodeDict);
    }

    // ── Shared second + third pass (used by both overloads) ─────────────────
    private static List<ScheduleActivityListDto> BuildFromNodeDict(
        IEnumerable<(Guid Id, Guid? ParentId)> pairs,
        Dictionary<Guid, ActivityNode> nodeDict)
    {
        var rootNodes = new List<ActivityNode>();

        foreach (var (id, parentId) in pairs)
        {
            var currentNode = nodeDict[id];

            if (parentId.HasValue)
            {
                if (nodeDict.TryGetValue(parentId.Value, out var parentNode))
                    parentNode.Children.Add(currentNode);
                else
                    rootNodes.Add(currentNode); // orphaned – treat as root
            }
            else
            {
                rootNodes.Add(currentNode);
            }
        }

        return rootNodes
            .OrderBy(n => n.OverallSequence)
            .Select(ConvertNodeToDto)
            .ToList();
    }

    /// <summary>
    /// Recursively converts ActivityNode to ScheduleActivityListDto
    /// </summary>
    private static ScheduleActivityListDto ConvertNodeToDto(ActivityNode node)
    {
        // Recursively convert children
        var childrenDtos = node.Children
            .OrderBy(c => c.OverallSequence)
            .Select(child => ConvertNodeToDto(child))
            .ToList();

        return new ScheduleActivityListDto(
            node.ScheduleActivityId,
            node.ActivityCode,
            node.ActivityName,
            node.Stage,
            node.StageNumber,
            node.IsCustomActivity,
            node.PlannedStartDate,
            node.PlannedFinishDate,
            node.ActualStartDate,
            node.ActualFinishDate,
            node.Status,
            node.PercentComplete,
            node.Weight,
            node.TeamCount,
            node.MaterialCount,
            node.ParentActivityId,
            childrenDtos
        );
    }
}

/// <summary>
/// Temporary mutable class for building hierarchy
/// (Records are immutable, so we use a class to build the tree structure)
/// </summary>
internal class ActivityNode
{
    public Guid ScheduleActivityId { get; set; }
    public string ActivityCode { get; set; } = string.Empty;
    public string ActivityName { get; set; } = string.Empty;
    public string Stage { get; set; } = string.Empty;
    public int StageNumber { get; set; }
    public int OverallSequence { get; set; } // ✅ For preserving order
    public bool IsCustomActivity { get; set; }
    public DateTime PlannedStartDate { get; set; }
    public DateTime PlannedFinishDate { get; set; }
    public DateTime? ActualStartDate { get; set; }
    public DateTime? ActualFinishDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal PercentComplete { get; set; }
    public decimal Weight { get; set; }
    public int TeamCount { get; set; }
    public int MaterialCount { get; set; }
    public Guid? ParentActivityId { get; set; }
    public List<ActivityNode> Children { get; set; } = new();
}
