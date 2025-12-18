namespace Dubox.Application.DTOs;

public record BoxDrawingDto
{
    public Guid BoxDrawingId { get; init; }
    public Guid BoxId { get; init; }
    public string? DrawingUrl { get; init; }
    public string? FileData { get; init; }
    public string? OriginalFileName { get; init; }
    public string? FileExtension { get; init; }
    public string? FileType { get; init; }
    public long? FileSize { get; init; }
    public DateTime CreatedDate { get; init; }
    public Guid? CreatedBy { get; init; }
}

