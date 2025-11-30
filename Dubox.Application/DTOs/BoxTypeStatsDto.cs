namespace Dubox.Application.DTOs;

public record BoxTypeStatDto
{
    public string BoxType { get; init; } = string.Empty;
    public int BoxCount { get; init; }
    public decimal OverallProgress { get; init; }
}

public record BoxTypeStatsByProjectDto
{
    public Guid ProjectId { get; init; }
    public List<BoxTypeStatDto> BoxTypeStats { get; init; } = new();
}

