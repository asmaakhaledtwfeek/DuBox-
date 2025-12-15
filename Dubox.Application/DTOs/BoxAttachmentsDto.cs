namespace Dubox.Application.DTOs;

public class BoxAttachmentsDto
{
    public List<BoxAttachmentDto> WIRCheckpointImages { get; set; } = new();
    public List<BoxAttachmentDto> ProgressUpdateImages { get; set; } = new();
    public List<BoxAttachmentDto> QualityIssueImages { get; set; } = new();
    public int TotalCount { get; set; }
}

public class BoxAttachmentDto
{
    public Guid ImageId { get; set; }
    public string ImageData { get; set; } = string.Empty;
    public string ImageType { get; set; } = string.Empty;
    public string? OriginalName { get; set; }
    public long? FileSize { get; set; }
    public int Sequence { get; set; }
    public DateTime CreatedDate { get; set; }
    
    // Reference information
    public Guid ReferenceId { get; set; } // WIRId, ProgressUpdateId, or IssueId
    public string ReferenceType { get; set; } = string.Empty; // "WIRCheckpoint", "ProgressUpdate", or "QualityIssue"
    public string? ReferenceName { get; set; } // WIR Code, Progress Update title, or Issue description
}

