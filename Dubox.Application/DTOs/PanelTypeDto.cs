namespace Dubox.Application.DTOs;

public record PanelTypeDto
{
    public Guid PanelTypeId { get; init; }
    public Guid ProjectId { get; init; }
    public string PanelTypeName { get; init; } = string.Empty;
    public string PanelTypeCode { get; init; } = string.Empty;
    public string? Description { get; init; }
    public bool IsActive { get; init; }
    public int DisplayOrder { get; init; }
    public DateTime CreatedDate { get; init; }
    public DateTime? ModifiedDate { get; init; }
}

public record CreatePanelTypeDto
{
    public Guid ProjectId { get; init; }
    public string PanelTypeName { get; init; } = string.Empty;
    public string PanelTypeCode { get; init; } = string.Empty;
    public string? Description { get; init; }
    public int DisplayOrder { get; init; } = 0;
}

public record UpdatePanelTypeDto
{
    public Guid PanelTypeId { get; init; }
    public string PanelTypeName { get; init; } = string.Empty;
    public string PanelTypeCode { get; init; } = string.Empty;
    public string? Description { get; init; }
    public bool IsActive { get; init; } = true;
    public int DisplayOrder { get; init; } = 0;
}

