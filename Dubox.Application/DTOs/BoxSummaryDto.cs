namespace Dubox.Application.DTOs;

public class BoxSummaryDto
{
    // Activity Status Summary
    public ActivityStatusSummaryDto ActivityStatus { get; set; } = new();
    
    // WIR Checkpoint Summary
    public WIRCheckpointSummaryDto WIRCheckpoint { get; set; } = new();
    
    // Quality Issue Summary
    public QualityIssueSummaryDto QualityIssue { get; set; } = new();
    
    // Attachment Summary
    public AttachmentSummaryDto Attachment { get; set; } = new();
    
    // Drawing Summary
    public DrawingSummaryDto Drawing { get; set; } = new();
}

public class ActivityStatusSummaryDto
{
    public int Total { get; set; }
    public int NotStarted { get; set; }
    public int InProgress { get; set; }
    public int Completed { get; set; }
    public int OnHold { get; set; }
    public int Delayed { get; set; }
    public decimal AverageProgress { get; set; }
}

public class WIRCheckpointSummaryDto
{
    public int Total { get; set; }
    public int Pending { get; set; }
    public int Approved { get; set; }
    public int Rejected { get; set; }
    public int ConditionalApproval { get; set; }
    public int UnderReview { get; set; }
}

public class QualityIssueSummaryDto
{
    public int Total { get; set; }
    public int Open { get; set; }
    public int InProgress { get; set; }
    public int Resolved { get; set; }
    public int Closed { get; set; }
    public int Critical { get; set; }
    public int Major { get; set; }
    public int Minor { get; set; }
}

public class AttachmentSummaryDto
{
    public int Total { get; set; }
    public int Recent { get; set; } // Attachments added in last 7 days
}

public class DrawingSummaryDto
{
    public int Total { get; set; }
    public int Recent { get; set; } // Drawings added in last 7 days
}

