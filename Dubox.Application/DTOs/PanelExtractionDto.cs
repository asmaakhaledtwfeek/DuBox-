namespace Dubox.Application.DTOs;

/// <summary>
/// Result of AI extraction from PDF containing panel data
/// </summary>
public class PanelExtractionResult
{
    public List<ExtractedPanelData> Panels { get; set; } = new();
    public List<string> Warnings { get; set; } = new(); // For missing fields, parsing issues, etc.
}

/// <summary>
/// Panel data extracted from PDF by AI
/// </summary>
public class ExtractedPanelData
{
    [System.Text.Json.Serialization.JsonPropertyName("tag")]
    public string Tag { get; set; } = string.Empty; // e.g., "IW-230-1"
    
    [System.Text.Json.Serialization.JsonPropertyName("volume_m3")]
    public decimal? VolumeM3 { get; set; }
    
    [System.Text.Json.Serialization.JsonPropertyName("weight_ton")]
    public decimal? WeightTon { get; set; }
    
    [System.Text.Json.Serialization.JsonPropertyName("concrete_grade")]
    public string? ConcreteGrade { get; set; }
    
    [System.Text.Json.Serialization.JsonPropertyName("cover_mm")]
    public int? CoverMm { get; set; }
    
    [System.Text.Json.Serialization.JsonPropertyName("quantity_in_this_box")]
    public int QuantityInThisBox { get; set; } = 1;
    
    [System.Text.Json.Serialization.JsonPropertyName("embeds")]
    public List<EmbedData> Embeds { get; set; } = new();
}

/// <summary>
/// Embed data (hardware, fixtures) for a panel
/// </summary>
public class EmbedData
{
    [System.Text.Json.Serialization.JsonPropertyName("symbol")]
    public string Symbol { get; set; } = string.Empty;
    
    [System.Text.Json.Serialization.JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;
    
    [System.Text.Json.Serialization.JsonPropertyName("qty")]
    public int Qty { get; set; }
}

/// <summary>
/// Review DTO for extracted panel data - sent to frontend for user review
/// </summary>
public class PanelExtractionReviewDto
{
    public List<ReviewPanelTypeDto> PanelTypes { get; set; } = new();
    public List<ReviewBoxPanelDto> BoxPanels { get; set; } = new();
    public List<string> Warnings { get; set; } = new();
    public List<string> BoxTags { get; set; } = new();
}

/// <summary>
/// Panel type data for review (may be new or existing)
/// </summary>
public class ReviewPanelTypeDto
{
    public Guid? PanelTypeId { get; set; } // null if new
    public string PanelTypeCode { get; set; } = string.Empty; // e.g., "IW-230-1"
    public string PanelTypeName { get; set; } = string.Empty; // e.g., "Interior Wall IW-230-1"
    public decimal? VolumeM3 { get; set; }
    public decimal? WeightTon { get; set; }
    public string? ConcreteGrade { get; set; }
    public int? CoverMm { get; set; }
    public string? EmbedsJson { get; set; }
    public bool IsNew { get; set; } // true if doesn't exist yet
}

/// <summary>
/// Box panel data for review (to be created)
/// </summary>
public class ReviewBoxPanelDto
{
    public string PanelTypeCode { get; set; } = string.Empty;
    public string PanelName { get; set; } = string.Empty;
    public int QuantityInThisBox { get; set; }
}

/// <summary>
/// Confirmation result after saving panels
/// </summary>
public class PanelExtractionConfirmationDto
{
    public int PanelTypesCreated { get; set; }
    public int PanelTypesUpdated { get; set; }
    public int BoxPanelsCreated { get; set; }
    public List<Guid> CreatedPanelTypeIds { get; set; } = new();
    public List<Guid> CreatedBoxPanelIds { get; set; } = new();
    public List<string> Errors { get; set; } = new();
}

