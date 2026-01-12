namespace Dubox.Application.DTOs;

public record BoxDrawingDto
{
    public Guid BoxDrawingId { get; init; }
    public Guid BoxId { get; init; }
    public string? DrawingUrl { get; init; }
    public string? DrawingFileName { get; init; } 
    public string? DownloadUrl { get; init; } 
    public string? OriginalFileName { get; init; }
    public string? FileExtension { get; init; }
    public string? FileType { get; init; }
    public long? FileSize { get; init; }
    public int Version { get; init; } = 1; // Version number for files with same name
    public DateTime CreatedDate { get; init; }
    public Guid? CreatedBy { get; init; }
    public string? CreatedByName { get; init; } // User name who created this drawing
}

