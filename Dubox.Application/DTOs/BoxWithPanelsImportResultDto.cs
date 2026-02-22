namespace Dubox.Application.DTOs;

public class BoxWithPanelsImportResultDto
{
    public int SuccessCount { get; set; }
    public int FailureCount { get; set; }
    public int TotalPanelsCreated { get; set; }
    public List<string> Errors { get; set; } = new();
    public List<BoxWithPanelsDto> ImportedBoxes { get; set; } = new();
}

public class BoxWithPanelsDto
{
    public Guid BoxId { get; set; }
    public string BoxTag { get; set; } = string.Empty;
    public string Floor { get; set; } = string.Empty;
    public string BoxType { get; set; } = string.Empty;
    public int PanelCount { get; set; }
    public List<PanelDto> Panels { get; set; } = new();
}

public class PanelDto
{
    public Guid PanelId { get; set; }
    public string PanelName { get; set; } = string.Empty;
    public string PanelPrefix { get; set; } = string.Empty;
    public int SequenceNumber { get; set; }
}










