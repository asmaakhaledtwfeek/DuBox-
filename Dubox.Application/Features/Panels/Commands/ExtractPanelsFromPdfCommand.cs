using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Panels.Commands;

/// <summary>
/// Command to extract panel data from PDF using AI
/// Returns data for user review before saving
/// </summary>
public record ExtractPanelsFromPdfCommand(
    Guid ProjectId,
    Guid? BoxId, // Optional - if provided, will create BoxPanels for this box
    Stream PdfStream,
    string FileName,
    Stream? BoxTagsPdfStream = null, // Optional - second PDF for box tags extraction
    string? BoxTagsFileName = null
) : IRequest<Result<PanelExtractionReviewDto>>;






