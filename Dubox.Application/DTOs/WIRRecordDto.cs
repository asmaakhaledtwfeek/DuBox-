namespace Dubox.Application.DTOs;

public record WIRRecordDto
{
    public Guid WIRRecordId { get; init; }
    public Guid BoxActivityId { get; init; }
    public Guid BoxId { get; init; }
    public string BoxTag { get; init; } = string.Empty;
    public string ActivityName { get; init; } = string.Empty;
    public string WIRCode { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public DateTime RequestedDate { get; init; }
    public Guid RequestedBy { get; init; }
    public string RequestedByName { get; init; } = string.Empty;
    public Guid? InspectedBy { get; init; }
    public string? InspectedByName { get; init; }
    public DateTime? InspectionDate { get; init; }
    public string? InspectionNotes { get; init; }
    public string? PhotoUrls { get; init; }
    public string? RejectionReason { get; init; }
    public string? Bay { get; init; }
    public string? Row { get; init; }
    public string? Position { get; init; }
}

public record CreateWIRRecordDto
{
    public Guid BoxActivityId { get; init; }
    public string WIRCode { get; init; } = string.Empty;
    public string? PhotoUrls { get; init; }
}

public record ApproveWIRDto
{
    public Guid WIRRecordId { get; init; }
    public string? InspectionNotes { get; init; }
    public string? PhotoUrls { get; init; }
}

public record RejectWIRDto
{
    public Guid WIRRecordId { get; init; }
    public string RejectionReason { get; init; } = string.Empty;
    public string? InspectionNotes { get; init; }
}

