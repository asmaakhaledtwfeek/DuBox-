using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Schedule.Commands;

public class UpdateActivityProgressCommandHandler : IRequestHandler<UpdateActivityProgressCommand, Result<bool>>
{
    private readonly IDbContext _context;

    public UpdateActivityProgressCommandHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<Result<bool>> Handle(UpdateActivityProgressCommand request, CancellationToken cancellationToken)
    {
        // Find the activity
        var activity = await _context.ScheduleActivities
            .FirstOrDefaultAsync(a => a.ScheduleActivityId == request.ScheduleActivityId, cancellationToken);

        if (activity == null)
        {
            return Result.Failure<bool>(new Error("Activity.NotFound", "Schedule activity not found"));
        }

        // Validate progress percentage
        if (request.PercentComplete < 0 || request.PercentComplete > 100)
        {
            return Result.Failure<bool>(new Error("Progress.Invalid", "Progress must be between 0 and 100"));
        }

        // Update progress
        activity.PercentComplete = request.PercentComplete;

        // Auto-set status based on progress
        if (request.PercentComplete == 100)
        {
            // 100% = Completed
            activity.Status = "Completed";
        }
        else if (request.PercentComplete > 0 && request.PercentComplete < 100)
        {
            // 1-99% = In Progress
            activity.Status = "In Progress";
        }
        else
        {
            // 0% = Use provided status or keep current
            activity.Status = request.Status;
        }

        // Auto-set actual start date if provided or if progress > 0 and no existing start date
        if (request.ActualStartDate.HasValue)
        {
            activity.ActualStartDate = request.ActualStartDate.Value;
        }
        else if (request.PercentComplete > 0 && !activity.ActualStartDate.HasValue)
        {
            activity.ActualStartDate = DateTime.UtcNow;
        }

        // Auto-set actual finish date if provided or if progress = 100 and no existing finish date
        if (request.ActualFinishDate.HasValue)
        {
            activity.ActualFinishDate = request.ActualFinishDate.Value;
        }
        else if (request.PercentComplete == 100 && !activity.ActualFinishDate.HasValue)
        {
            activity.ActualFinishDate = DateTime.UtcNow;
        }

        // Top-down: push parent value to descendants (only if parentValue > childValue)
        var descendantIds = await GetDescendantIdsAsync(activity, cancellationToken);
        if (descendantIds.Count > 0)
        {
            var newStatus = request.PercentComplete >= 100 ? "Completed"
                : request.PercentComplete > 0 ? "In Progress" : "Planned";
            await _context.ScheduleActivities
                .Where(a => descendantIds.Contains(a.ScheduleActivityId) && a.PercentComplete < request.PercentComplete)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(a => a.PercentComplete, request.PercentComplete)
                    .SetProperty(a => a.Status, newStatus),
                    cancellationToken);
        }

        // CRITICAL: Immediately recalculate parent from its actual children (after top-down push)
        // This ensures parent always reflects true weighted average, even after manual update
        if (activity.ChildActivities?.Count > 0 || descendantIds.Count > 0)
        {
            // Reload direct children from DB (they now have updated values from top-down push)
            var directChildren = await _context.ScheduleActivities
                .Where(a => a.ParentActivityId == activity.ScheduleActivityId)
                .ToListAsync(cancellationToken);

            if (directChildren.Count > 0)
            {
                // Recalculate parent as weighted average of actual children values
                decimal weightedSum = 0;
                decimal totalWeight = 0;
                foreach (var child in directChildren)
                {
                    var childWeight = child.Weight > 0 ? child.Weight : 1.0m;
                    weightedSum += child.PercentComplete * childWeight;
                    totalWeight += childWeight;
                }
                activity.PercentComplete = totalWeight > 0 
                    ? Math.Round(weightedSum / totalWeight, 2) 
                    : 0;

                // Update status based on recalculated progress
                if (activity.PercentComplete >= 100)
                    activity.Status = "Completed";
                else if (activity.PercentComplete > 0)
                    activity.Status = "In Progress";
                else
                    activity.Status = "Planned";
            }
        }

        // Bottom-up: parent's progress = weighted average of all direct children
        Guid? parentId = activity.ParentActivityId;
        while (parentId.HasValue)
        {
            var parent = await _context.ScheduleActivities
                .FirstOrDefaultAsync(a => a.ScheduleActivityId == parentId.Value, cancellationToken);
            if (parent == null)
                break;

            var directChildren = await _context.ScheduleActivities
                .Where(a => a.ParentActivityId == parentId.Value)
                .ToListAsync(cancellationToken);

            int count = directChildren.Count;
            if (count == 0)
                break;

            // Calculate weighted average: sum(child.Progress * child.Weight) / sum(child.Weight)
            decimal weightedSum = 0;
            decimal totalWeight = 0;
            foreach (var child in directChildren)
            {
                // Use in-memory value for the activity we're updating (it was recalculated from its children)
                var childProgress = child.ScheduleActivityId == activity.ScheduleActivityId
                    ? activity.PercentComplete
                    : child.PercentComplete;
                var childWeight = child.Weight > 0 ? child.Weight : 1.0m; // Default to 1.0 if weight is 0 or negative
                
                weightedSum += childProgress * childWeight;
                totalWeight += childWeight;
            }

            parent.PercentComplete = totalWeight > 0 
                ? Math.Round(weightedSum / totalWeight, 2) 
                : 0;

            // Set parent status from rolled-up progress
            if (parent.PercentComplete >= 100)
                parent.Status = "Completed";
            else if (parent.PercentComplete > 0)
                parent.Status = "In Progress";
            else
                parent.Status = "Planned";

            parentId = parent.ParentActivityId;
        }

        // Save changes (activity + all updated parents)
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(true);
    }

    /// <summary>
    /// Gets all descendant activity IDs (children, grandchildren, etc.) of the given activity.
    /// </summary>
    private async Task<HashSet<Guid>> GetDescendantIdsAsync(ScheduleActivity activity, CancellationToken cancellationToken)
    {
        var descendantIds = new HashSet<Guid>();
        if (activity.ProjectId == null)
        {
            // No project: traverse by querying children level by level
            var currentLevel = new List<Guid> { activity.ScheduleActivityId };
            while (currentLevel.Count > 0)
            {
                var childIds = await _context.ScheduleActivities
                    .Where(a => a.ParentActivityId != null && currentLevel.Contains(a.ParentActivityId.Value))
                    .Select(a => a.ScheduleActivityId)
                    .ToListAsync(cancellationToken);
                foreach (var id in childIds)
                    descendantIds.Add(id);
                currentLevel = childIds;
            }
            return descendantIds;
        }

        // Same project: load (Id, ParentId) and BFS in memory
        var pairs = await _context.ScheduleActivities
            .Where(a => a.ProjectId == activity.ProjectId && a.ParentActivityId != null)
            .Select(a => new { a.ScheduleActivityId, a.ParentActivityId })
            .ToListAsync(cancellationToken);

        var childrenByParent = pairs
            .GroupBy(p => p.ParentActivityId!.Value)
            .ToDictionary(g => g.Key, g => g.Select(x => x.ScheduleActivityId).ToList());

        var queue = new Queue<Guid>();
        queue.Enqueue(activity.ScheduleActivityId);
        while (queue.Count > 0)
        {
            var id = queue.Dequeue();
            if (!childrenByParent.TryGetValue(id, out var childIds))
                continue;
            foreach (var cid in childIds)
            {
                descendantIds.Add(cid);
                queue.Enqueue(cid);
            }
        }

        return descendantIds;
    }
}
