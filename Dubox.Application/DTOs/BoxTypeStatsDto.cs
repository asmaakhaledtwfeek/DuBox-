namespace Dubox.Application.DTOs;

public record BoxSubTypeStatDto
{
    public string SubTypeName { get; init; } = string.Empty;
    public string? SubTypeAbbreviation { get; init; }
    public int BoxCount { get; init; }
    public decimal Progress { get; init; }
}

public record BoxTypeStatDto
{
    public string BoxType { get; init; } = string.Empty;
    public int BoxCount { get; init; }
    public decimal OverallProgress { get; init; }
    public List<BoxSubTypeStatDto> SubTypes { get; init; } = new();
}

public record BoxTypeStatsByProjectDto
{
    public Guid ProjectId { get; init; }
    public List<BoxTypeStatDto> BoxTypeStats { get; init; } = new();
}


