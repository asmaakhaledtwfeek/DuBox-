using Dubox.Application.Features.Schedule.Commands;
using Dubox.Application.Features.Schedule.Queries;
using Dubox.Api.Configurations;
using Dubox.Api.Utilities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Dubox.Domain.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ScheduleController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IDbContext _context;

    public ScheduleController(IMediator mediator, IDbContext context)
    {
        _mediator = mediator;
        _context = context;
    }

    /// <summary>
    /// Get all schedule activities
    /// </summary>
    [HttpGet("activities")]
    public async Task<IActionResult> GetScheduleActivities(CancellationToken cancellationToken)
    {
        var query = new GetScheduleActivitiesQuery();
        var result = await _mediator.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Get schedule activities by project ID
    /// </summary>
    [HttpGet("activities/project/{projectId}")]
    public async Task<IActionResult> GetScheduleActivitiesByProject(Guid projectId, CancellationToken cancellationToken)
    {
        var query = new GetScheduleActivitiesByProjectQuery(projectId);
        var result = await _mediator.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Get schedule activity details by ID
    /// </summary>
    [HttpGet("activities/{id}")]
    public async Task<IActionResult> GetScheduleActivityDetails(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetScheduleActivityDetailsQuery(id);
        var result = await _mediator.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    /// <summary>
    /// Create a new schedule activity
    /// </summary>
    [HttpPost("activities")]
    public async Task<IActionResult> CreateScheduleActivity(
        [FromBody] CreateScheduleActivityCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsSuccess)
        {
            return CreatedAtAction(
                nameof(GetScheduleActivityDetails),
                new { id = result.Data },
                result);
        }

        return BadRequest(result);
    }

    /// <summary>
    /// Assign a team to a schedule activity
    /// </summary>
    [HttpPost("activities/{activityId}/assign-team")]
    public async Task<IActionResult> AssignTeam(
        Guid activityId,
        [FromBody] AssignTeamRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AssignTeamCommand(activityId, request.TeamId, request.Notes);
        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Assign a material to a schedule activity
    /// </summary>
    [HttpPost("activities/{activityId}/assign-material")]
    public async Task<IActionResult> AssignMaterial(
        Guid activityId,
        [FromBody] AssignMaterialRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AssignMaterialCommand(
            activityId,
            request.MaterialName,
            request.MaterialCode,
            request.Quantity,
            request.Unit,
            request.Notes);

        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Update activity progress with automatic actual date tracking
    /// </summary>
    [HttpPut("activities/{activityId}/progress")]
    public async Task<IActionResult> UpdateActivityProgress(
        Guid activityId,
        [FromBody] UpdateActivityProgressRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateActivityProgressCommand(
            activityId,
            request.PercentComplete,
            request.Status,
            request.ActualStartDate,
            request.ActualFinishDate
        );

        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Inspect the baseline Excel file structure and preview data
    /// </summary>
    /// <remarks>
    /// This endpoint inspects the Excel file and returns its structure:
    /// - All worksheet names
    /// - Column headers and their positions
    /// - Sample data for each column
    /// - Statistics about the file
    /// - Mapping of columns to schedule activity fields
    /// 
    /// Use this to verify the Excel file structure before importing.
    /// </remarks>
    [HttpGet("activities/inspect-excel")]
    public IActionResult InspectBaselineExcelFile(
        [FromQuery] string? filePath = null,
        [FromQuery] int previewRows = 10)
    {
        try
        {
            var excelPath = filePath ?? ScheduleActivitySeedingHelper.FindExcelFile("Documentation/KJ-158 - Revised Baseline Schedule.xlsx");
            
            if (excelPath == null || !System.IO.File.Exists(excelPath))
            {
                return NotFound(new
                {
                    success = false,
                    message = "Excel file not found. Default path: Documentation/KJ-158 - Revised Baseline Schedule.xlsx",
                    triedPaths = new[]
                    {
                        Path.Combine(Directory.GetCurrentDirectory(), "Documentation/KJ-158 - Revised Baseline Schedule.xlsx"),
                        Path.Combine(AppContext.BaseDirectory, "Documentation/KJ-158 - Revised Baseline Schedule.xlsx"),
                        Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Documentation/KJ-158 - Revised Baseline Schedule.xlsx")
                    }
                });
            }

            // Create a logger for the inspection
            var logger = HttpContext.RequestServices.GetRequiredService<ILogger<ScheduleController>>();
            
            // Inspect and log to console
            ExcelFileInspector.InspectExcelFile(excelPath, logger, previewRows);
            
            return Ok(new
            {
                success = true,
                message = "Excel file inspection complete. Check the application logs for detailed output.",
                filePath = excelPath,
                fileSize = new FileInfo(excelPath).Length,
                note = "A detailed inspection report has been written to the application logs."
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                success = false,
                message = $"Error inspecting Excel file: {ex.Message}",
                error = ex.ToString()
            });
        }
    }

    /// <summary>
    /// Export schedule activities to Excel file with hierarchical structure
    /// </summary>
    [HttpGet("activities/export/{projectId}")]
    public async Task<IActionResult> ExportScheduleActivitiesToExcel(
        Guid projectId,
        CancellationToken cancellationToken)
    {
        var command = new ExportScheduleActivitiesToExcelCommand(projectId);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsSuccess && result.Data != null)
        {
            var fileName = $"Schedule_Activities_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

            return File(
                result.Data,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName,
                enableRangeProcessing: false
            );
        }

        result.Data?.Dispose();

        return BadRequest(new
        {
            success = false,
            message = result.Error?.Description ?? "Export failed"
        });
    }

    /// <summary>
    /// Import schedule activities from an Excel file with hierarchical (Parent/Child) support
    /// </summary>
    /// <remarks>
    /// The Excel file should contain the following columns:
    /// - Activity ID or Activity Code (required): Unique identifier for the activity
    /// - Activity Name (required): Name/description of the activity
    /// - Parent Activity or Parent Code (optional): Code of the parent activity for hierarchy
    /// - BL1 Start or Start (optional): Planned start date
    /// - BL1 Finish or Finish (optional): Planned finish date
    /// - Original Duration or Duration (optional): Duration in days
    /// - Stage (optional): Stage name
    /// - Status (optional): Activity status
    /// 
    /// The importer will automatically:
    /// - Create hierarchical relationships based on Parent Code
    /// - Prevent duplicates by checking Activity Code
    /// - Link activities to the specified project (if provided)
    /// </remarks>
    [HttpPost("activities/import")]
    [RequestSizeLimit(100_000_000)] // 100MB limit for large schedules
    public async Task<IActionResult> ImportScheduleActivitiesFromExcel(
        [FromForm] ImportScheduleActivitiesRequest request,
        CancellationToken cancellationToken)
    {
        if (request.ExcelFile == null || request.ExcelFile.Length == 0)
        {
            return BadRequest(new { success = false, message = "No file was uploaded or the file is empty." });
        }

        using var stream = request.ExcelFile.OpenReadStream();
        var command = new ImportScheduleActivitiesFromExcelCommand(
            stream,
            request.ExcelFile.FileName,
            request.ProjectId,
            request.SheetName);

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsSuccess && result.Data != null)
        {
            return Ok(new
            {
                success = true,
                message = $"Successfully imported {result.Data.SuccessfullyImported} out of {result.Data.TotalProcessed} activities",
                data = result.Data
            });
        }

        return BadRequest(new
        {
            success = false,
            message = result.Error?.Description ?? "Import failed",
            errors = result.Data?.Errors ?? new List<string>(),
            warnings = result.Data?.Warnings ?? new List<string>()
        });
    }

    /// <summary>
    /// Diagnostic endpoint to verify schedule activity hierarchy structure in the database
    /// </summary>
    /// <remarks>
    /// This endpoint provides detailed information about the hierarchy structure:
    /// - Total activity count
    /// - Root vs child activity counts
    /// - Parent-child relationship statistics
    /// - Sample parent-child pairs
    /// - Orphaned activities (if any)
    /// 
    /// Use this to troubleshoot hierarchy loading issues.
    /// </remarks>
    [HttpGet("activities/diagnostics/hierarchy")]
    public async Task<IActionResult> DiagnoseHierarchy(
        [FromQuery] Guid? projectId = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Get activities filtered by project if specified
            var query = _context.ScheduleActivities.AsQueryable();
            if (projectId.HasValue)
            {
                query = query.Where(a => a.ProjectId == projectId.Value);
            }

            var allActivities = await query.ToListAsync(cancellationToken);

            var totalCount = allActivities.Count;
            var rootCount = allActivities.Count(a => a.ParentActivityId == null);
            var childCount = allActivities.Count(a => a.ParentActivityId != null);

            // Group children by parent
            var parentChildGroups = allActivities
                .Where(a => a.ParentActivityId != null)
                .GroupBy(a => a.ParentActivityId)
                .Select(g => new
                {
                    ParentId = g.Key,
                    Parent = allActivities.FirstOrDefault(a => a.ScheduleActivityId == g.Key),
                    ChildCount = g.Count(),
                    Children = g.Take(5).Select(c => new
                    {
                        c.ActivityCode,
                        c.ActivityName
                    }).ToList()
                })
                .OrderByDescending(g => g.ChildCount)
                .Take(10)
                .ToList();

            // Find orphaned activities
            var orphanedActivities = allActivities
                .Where(a => a.ParentActivityId != null &&
                           !allActivities.Any(p => p.ScheduleActivityId == a.ParentActivityId))
                .Select(a => new
                {
                    a.ActivityCode,
                    a.ActivityName,
                    InvalidParentId = a.ParentActivityId
                })
                .ToList();

            // Sample parent-child pairs
            var samplePairs = allActivities
                .Where(child => child.ParentActivityId != null)
                .Join(
                    allActivities,
                    child => child.ParentActivityId,
                    parent => (Guid?)parent.ScheduleActivityId,
                    (child, parent) => new
                    {
                        ParentCode = parent.ActivityCode,
                        ParentName = parent.ActivityName,
                        ChildCode = child.ActivityCode,
                        ChildName = child.ActivityName
                    })
                .Take(20)
                .ToList();

            // Root activities with their child counts
            var rootActivitiesWithCounts = allActivities
                .Where(a => a.ParentActivityId == null)
                .Select(root => new
                {
                    root.ActivityCode,
                    root.ActivityName,
                    DirectChildCount = allActivities.Count(c => c.ParentActivityId == root.ScheduleActivityId)
                })
                .OrderBy(a => a.ActivityCode)
                .ToList();

            return Ok(new
            {
                success = true,
                projectId,
                summary = new
                {
                    totalActivities = totalCount,
                    rootActivities = rootCount,
                    childActivities = childCount,
                    parentsWithChildren = parentChildGroups.Count,
                    orphanedActivities = orphanedActivities.Count
                },
                rootActivities = rootActivitiesWithCounts,
                topParentsWithMostChildren = parentChildGroups.Select(g => new
                {
                    parentId = g.ParentId,
                    parentCode = g.Parent?.ActivityCode,
                    parentName = g.Parent?.ActivityName,
                    childCount = g.ChildCount,
                    sampleChildren = g.Children
                }),
                sampleParentChildPairs = samplePairs,
                orphanedActivities = orphanedActivities.Any() ? orphanedActivities : null,
                diagnosticNotes = new[]
                {
                    $"✓ Found {totalCount} total activities",
                    $"✓ {rootCount} root activities (no parent)",
                    $"✓ {childCount} child activities (have parent)",
                    orphanedActivities.Any()
                        ? $"✗ WARNING: {orphanedActivities.Count} orphaned activities found (invalid parent references)"
                        : "✓ No orphaned activities found",
                    parentChildGroups.Any()
                        ? $"✓ Hierarchy structure exists: {parentChildGroups.Count} parents have children"
                        : "✗ WARNING: No parent-child relationships found - hierarchy may be broken"
                }
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                success = false,
                message = $"Error diagnosing hierarchy: {ex.Message}",
                error = ex.ToString()
            });
        }
    }
}

// Request DTOs for API
public record AssignTeamRequest(Guid TeamId, string? Notes);
public record AssignMaterialRequest(
    string MaterialName,
    string? MaterialCode,
    decimal Quantity,
    string? Unit,
    string? Notes);

public record UpdateActivityProgressRequest(
    decimal PercentComplete,
    string Status,
    DateTime? ActualStartDate = null,
    DateTime? ActualFinishDate = null
);

public class ImportScheduleActivitiesRequest
{
    public IFormFile ExcelFile { get; set; } = null!;
    public Guid? ProjectId { get; set; }
    public string? SheetName { get; set; }
}

