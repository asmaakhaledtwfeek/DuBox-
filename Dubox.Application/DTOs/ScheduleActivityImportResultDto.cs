namespace Dubox.Application.DTOs;

/// <summary>
/// Result of the schedule activity import operation
/// </summary>
public class ScheduleActivityImportResultDto
{
    public int TotalProcessed { get; set; }
    public int SuccessfullyImported { get; set; }
    public int Skipped { get; set; }
    public List<string> Errors { get; set; } = new();
    public List<string> Warnings { get; set; } = new();
    public bool IsSuccess => !Errors.Any();
}
