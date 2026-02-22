using Microsoft.AspNetCore.Mvc;

namespace Dubox.Api.Models;

/// <summary>
/// Request model for extracting panels from PDF using AI
/// </summary>
public class ExtractPanelsFromPdfRequest
{
    /// <summary>
    /// Main PDF file containing panel data
    /// </summary>
    [FromForm(Name = "file")]
    public IFormFile File { get; set; } = null!;

    /// <summary>
    /// Optional second PDF file for box tags extraction
    /// </summary>
    [FromForm(Name = "boxTagsFile")]
    public IFormFile? BoxTagsFile { get; set; }

    /// <summary>
    /// Optional Box ID to associate extracted panels with
    /// </summary>
    [FromForm(Name = "boxId")]
    public Guid? BoxId { get; set; }
}
