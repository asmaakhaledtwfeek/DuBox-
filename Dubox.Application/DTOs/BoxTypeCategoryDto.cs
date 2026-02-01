namespace Dubox.Application.DTOs;

public record ProjectTypeCategoryDto
{
    public int CategoryId { get; init; }
    public string CategoryName { get; init; } = string.Empty;
    public string? Abbreviation { get; init; }
}

public record BoxTypeDto
{
    public int BoxTypeId { get; init; }
    public string BoxTypeName { get; init; } = string.Empty;
    public string? Abbreviation { get; init; }
    public int CategoryId { get; init; }
    public bool HasSubTypes { get; init; }
}

public record BoxSubTypeDto
{
    public int BoxSubTypeId { get; init; }
    public string BoxSubTypeName { get; init; } = string.Empty;
    public string? Abbreviation { get; init; }
    public int BoxTypeId { get; init; }
}

