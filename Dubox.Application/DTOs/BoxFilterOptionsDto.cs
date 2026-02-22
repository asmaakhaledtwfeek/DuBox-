namespace Dubox.Application.DTOs;

public class BoxFilterOptionsDto
{
    public Guid ProjectId { get; set; }
    public List<FilterOptionDto> SubTypes { get; set; } = new();
    public List<FilterOptionDto> Buildings { get; set; } = new();
    public List<FilterOptionDto> Floors { get; set; } = new();
    public List<FilterOptionDto> Zones { get; set; } = new();
}

public class FilterOptionDto
{
    public string Value { get; set; } = string.Empty;
    public int Count { get; set; }
}
