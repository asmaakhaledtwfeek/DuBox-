namespace Dubox.Application.DTOs;

public record ProgressUpdateDto
{
    public Guid ProgressUpdateId { get; init; }
    public Guid BoxId { get; init; }
    public string BoxTag { get; init; } = string.Empty;
    public Guid BoxActivityId { get; init; }
    public string ActivityName { get; init; } = string.Empty;
    public DateTime UpdateDate { get; init; }
    public Guid UpdatedBy { get; init; }
    public string UpdatedByName { get; init; } = string.Empty;
    public decimal ProgressPercentage { get; init; }
    public string Status { get; init; } = string.Empty;
    public string? WorkDescription { get; init; }
    public string? IssuesEncountered { get; init; }
    public double? Latitude { get; init; }
    public double? Longitude { get; init; }
    public string? LocationDescription { get; init; }
    public string? PhotoUrls { get; init; }
    public string UpdateMethod { get; init; } = string.Empty;
}

public record CreateProgressUpdateDto
{
    public Guid BoxId { get; init; }
    public Guid BoxActivityId { get; init; }
    public decimal ProgressPercentage { get; init; }
    public string Status { get; init; } = string.Empty;
    public string? WorkDescription { get; init; }
    public string? IssuesEncountered { get; init; }
    public double? Latitude { get; init; }
    public double? Longitude { get; init; }
    public string? LocationDescription { get; init; }
    public List<string>? PhotoUrls { get; init; }
    public string UpdateMethod { get; init; } = "Web";
    public string? DeviceInfo { get; init; }
}

public record ScanQRUpdateDto
{
    public string QRCodeString { get; init; } = string.Empty;
    public Guid BoxActivityId { get; init; }
    public decimal ProgressPercentage { get; init; }
    public string Status { get; init; } = string.Empty;
    public string? WorkDescription { get; init; }
    public string? IssuesEncountered { get; init; }
    public double? Latitude { get; init; }
    public double? Longitude { get; init; }
    public List<string>? PhotoUrls { get; init; }
}

