using Dubox.Domain.Shared;
using System.Text.Json.Serialization;

namespace Dubox.Domain.Services;

/// <summary>
/// Service for extracting panel data from PDF files using AI
/// </summary>
public interface IPanelPdfExtractionService
{
    /// <summary>
    /// Extracts panel data from a PDF file using AI (Google Gemini)
    /// </summary>
    /// <param name="pdfStream">PDF file stream</param>
    /// <param name="fileName">Original filename for logging</param>
    /// <param name="boxTagsPdfStream">Optional second PDF file stream for box tags extraction</param>
    /// <param name="boxTagsFileName">Optional second filename for logging</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Result containing extracted panel data or error messages</returns>
    Task<Result<PanelExtractionResult>> ExtractPanelDataFromPdfAsync(
        Stream pdfStream,
        string fileName,
        Stream? boxTagsPdfStream = null,
        string? boxTagsFileName = null,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// Result of panel extraction containing list of panels and warnings
/// </summary>
public class PanelExtractionResult
{
    public List<ExtractedPanelData> Panels { get; set; } = new();
    public List<string> Warnings { get; set; } = new();
    public List<string> BoxTags { get; set; } = new();
}


public class ExtractedPanelData
{
    [JsonPropertyName("tag")]
    public string Tag { get; set; } = string.Empty;

    [JsonPropertyName("volume_m3")]
    public decimal? VolumeM3 { get; set; }

    [JsonPropertyName("weight_ton")]
    public decimal? WeightTon { get; set; }

    [JsonPropertyName("concrete_grade")]
    public string? ConcreteGrade { get; set; }

    [JsonPropertyName("cover_mm")]
    public int? CoverMm { get; set; }

    [JsonPropertyName("quantity_in_this_box")]
    public int QuantityInThisBox { get; set; } = 1;

    [JsonPropertyName("embeds")]
    public List<EmbedData> Embeds { get; set; } = new();
}

public class EmbedData
{
    [JsonPropertyName("symbol")]
    public string Symbol { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("qty")]
    public int Qty { get; set; }
}





