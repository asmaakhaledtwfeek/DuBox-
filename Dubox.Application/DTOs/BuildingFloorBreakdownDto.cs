namespace Dubox.Application.DTOs;

public class BuildingFloorBreakdownDto
{
    public Guid ProjectId { get; set; }
    public List<BuildingBreakdownDto> Buildings { get; set; } = new();
}

public class BuildingBreakdownDto
{
    public string Building { get; set; } = string.Empty;
    public int TotalBoxes { get; set; }
    public int InProgressCount { get; set; }
    public int CompletedCount { get; set; }
    public List<FloorBreakdownDto> Floors { get; set; } = new();
}

public class FloorBreakdownDto
{
    public string Floor { get; set; } = string.Empty;
    public int BoxCount { get; set; }
    public int InProgressCount { get; set; }
    public int CompletedCount { get; set; }
}
