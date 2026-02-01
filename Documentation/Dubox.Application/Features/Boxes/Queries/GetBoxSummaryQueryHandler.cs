using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Enums;
using Dubox.Domain.Interfaces;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Boxes.Queries;

public class GetBoxSummaryQueryHandler : IRequestHandler<GetBoxSummaryQuery, Result<BoxSummaryDto>>
{
    private readonly IDbContext _context;
    private readonly IProjectTeamVisibilityService _visibilityService;
    private readonly ICurrentUserService _currentUserService;

    public GetBoxSummaryQueryHandler(IDbContext context, IProjectTeamVisibilityService visibilityService, ICurrentUserService currentUserService)
    {
        _context = context;
        _visibilityService = visibilityService;
        _currentUserService = currentUserService;
    }

    public async Task<Result<BoxSummaryDto>> Handle(GetBoxSummaryQuery request, CancellationToken cancellationToken)
    {
        // Verify box exists and get project ID
        var box = await _context.Boxes
            .FirstOrDefaultAsync(b => b.BoxId == request.BoxId, cancellationToken);
        
        if (box == null)
        {
            return Result.Failure<BoxSummaryDto>("Box not found");
        }

        //// Verify user has access to the project (skip check for public/unauthenticated access)
        //if (_currentUserService.IsAuthenticated)
        //{
        //    var canAccessProject = await _visibilityService.CanAccessProjectAsync(box.ProjectId, cancellationToken);
        //    if (!canAccessProject)
        //    {
        //        return Result.Failure<BoxSummaryDto>("Access denied. You do not have permission to view this box.");
        //    }
        //}

        var summary = new BoxSummaryDto();
        var sevenDaysAgo = DateTime.UtcNow.AddDays(-7);

        // Activity Status Summary
        var activities = await _context.BoxActivities
            .Where(ba => ba.BoxId == request.BoxId && ba.IsActive)
            .ToListAsync(cancellationToken);

        summary.ActivityStatus = new ActivityStatusSummaryDto
        {
            Total = activities.Count,
            NotStarted = activities.Count(a => a.Status == BoxStatusEnum.NotStarted),
            InProgress = activities.Count(a => a.Status == BoxStatusEnum.InProgress),
            Completed = activities.Count(a => a.Status == BoxStatusEnum.Completed),
            OnHold = activities.Count(a => a.Status == BoxStatusEnum.OnHold),
            Delayed = activities.Count(a => a.Status == BoxStatusEnum.Delayed),
            AverageProgress = activities.Any() 
                ? (decimal)activities.Average(a => a.ProgressPercentage) 
                : 0
        };

        // WIR Checkpoint Summary
        var wirCheckpoints = await _context.WIRCheckpoints
            .Where(wir => wir.BoxId == request.BoxId)
            .ToListAsync(cancellationToken);

        summary.WIRCheckpoint = new WIRCheckpointSummaryDto
        {
            Total = wirCheckpoints.Count,
            Pending = wirCheckpoints.Count(w => w.Status == WIRCheckpointStatusEnum.Pending),
            Approved = wirCheckpoints.Count(w => w.Status == WIRCheckpointStatusEnum.Approved),
            Rejected = wirCheckpoints.Count(w => w.Status == WIRCheckpointStatusEnum.Rejected),
            ConditionalApproval = wirCheckpoints.Count(w => w.Status == WIRCheckpointStatusEnum.ConditionalApproval),
            UnderReview = wirCheckpoints.Count(w => 
                w.Status == WIRCheckpointStatusEnum.Approved && 
                w.ChecklistItems != null && 
                w.ChecklistItems.Any(ci => ci.Status != CheckListItemStatusEnum.Pass))
        };

        // Quality Issue Summary
        var qualityIssues = await _context.QualityIssues
            .Where(qi => qi.BoxId == request.BoxId)
            .ToListAsync(cancellationToken);

        summary.QualityIssue = new QualityIssueSummaryDto
        {
            Total = qualityIssues.Count,
            Open = qualityIssues.Count(q => q.Status == QualityIssueStatusEnum.Open),
            InProgress = qualityIssues.Count(q => q.Status == QualityIssueStatusEnum.InProgress),
            Resolved = qualityIssues.Count(q => q.Status == QualityIssueStatusEnum.Resolved),
            Closed = qualityIssues.Count(q => q.Status == QualityIssueStatusEnum.Closed),
            Critical = qualityIssues.Count(q => q.Severity == SeverityEnum.Critical),
            Major = qualityIssues.Count(q => q.Severity == SeverityEnum.Major),
            Minor = qualityIssues.Count(q => q.Severity == SeverityEnum.Minor)
        };

        // Attachment Summary (from WIRCheckpointImages, ProgressUpdateImages, QualityIssueImages)
        var wirCheckpointImages = await _context.WIRCheckpointImages
            .Where(img => img.WIRCheckpoint != null && img.WIRCheckpoint.BoxId == request.BoxId)
            .ToListAsync(cancellationToken);

        var progressUpdateImages = await _context.ProgressUpdateImages
            .Where(img => img.ProgressUpdate != null && img.ProgressUpdate.BoxId == request.BoxId)
            .ToListAsync(cancellationToken);

        var qualityIssueImages = await _context.QualityIssueImages
            .Where(img => img.QualityIssue != null && img.QualityIssue.BoxId == request.BoxId)
            .ToListAsync(cancellationToken);

        var allAttachments = wirCheckpointImages
            .Select(img => new { img.CreatedDate })
            .Concat(progressUpdateImages.Select(img => new { img.CreatedDate }))
            .Concat(qualityIssueImages.Select(img => new { img.CreatedDate }))
            .ToList();

        summary.Attachment = new AttachmentSummaryDto
        {
            Total = allAttachments.Count,
          //  Recent = allAttachments.Count(a => a.CreatedDate >= sevenDaysAgo)
        };

        // Drawing Summary (all ProgressUpdateImages are considered drawings based on GetBoxDrawingQuery)
        var drawingImages = await _context.ProgressUpdateImages
            .Where(img => img.ProgressUpdate != null && img.ProgressUpdate.BoxId == request.BoxId)
            .ToListAsync(cancellationToken);

        summary.Drawing = new DrawingSummaryDto
        {
            Total = drawingImages.Count,
           // Recent = drawingImages.Count(d => d.CreatedDate >= sevenDaysAgo)
        };

        return Result.Success(summary);
    }
}

