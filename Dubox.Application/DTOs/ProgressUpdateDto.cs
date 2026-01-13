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
    public decimal BoxProgressSnapshot { get; init; }
    public string Status { get; init; } = string.Empty;
    public string? WorkDescription { get; init; }
    public string? IssuesEncountered { get; init; }
    public double? Latitude { get; init; }
    public double? Longitude { get; init; }
    public string? LocationDescription { get; init; }
    [Obsolete("Use Images list instead. Kept for backward compatibility.")]
    public string? Photo { get; init; }
    public List<ProgressUpdateImageDto> Images { get; init; } = new();
    public string UpdateMethod { get; init; } = string.Empty;
}

public record ProgressUpdateImageDto
{
    public Guid ProgressUpdateImageId { get; init; }
    public Guid ProgressUpdateId { get; init; }
    public string? ImageFileName { get; set; }
    public string? ImageUrl { get; set; }
    public string ImageType { get; init; } = string.Empty;
    public string? OriginalName { get; init; }
    public long? FileSize { get; init; }
    public int Sequence { get; init; }
    public int Version { get; init; } = 1; // Version number for files with same name
    public DateTime CreatedDate { get; init; }

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

public class PaginatedProgressUpdatesResponseDto
{
    public List<ProgressUpdateDto> Items { get; set; } = new List<ProgressUpdateDto>();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
}

