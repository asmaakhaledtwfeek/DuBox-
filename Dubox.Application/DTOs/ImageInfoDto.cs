namespace Dubox.Application.DTOs;

/// <summary>
/// Lightweight image DTO without ImageData - used for listings to avoid large payloads.
/// Use the /api/images/{type}/{id} endpoint to fetch full image data when needed.
/// </summary>
public record ImageInfoDto
{
    public Guid ImageId { get; init; }
    public Guid ParentId { get; init; }
    public string ImageType { get; init; } = string.Empty;
    public string? OriginalName { get; init; }
    public long? FileSize { get; init; }
    public int Sequence { get; init; }
    public DateTime CreatedDate { get; init; }
    
    /// <summary>
    /// URL to fetch the full image data: /api/images/{ImageCategory}/{ImageId}
    /// </summary>
    public string ImageUrl { get; init; } = string.Empty;
}

/// <summary>
/// Full image DTO with base64 ImageData - used for detail views or direct image fetch.
/// </summary>
public record ImageDataDto
{
    public Guid ImageId { get; init; }
    public string ImageData { get; init; } = string.Empty;
    public string ImageType { get; init; } = string.Empty;
    public string? OriginalName { get; init; }
    public long? FileSize { get; init; }
}

/// <summary>
/// Categories of images in the system
/// </summary>
public enum ImageCategory
{
    ProgressUpdate,
    QualityIssue,
    WIRCheckpoint
}

