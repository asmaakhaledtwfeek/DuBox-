using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Dubox.Application.Features.Panels.Commands;

/// <summary>
/// Handler for extracting panel data from PDF
/// Does NOT save to database - returns data for review
/// </summary>
public class ExtractPanelsFromPdfCommandHandler : IRequestHandler<ExtractPanelsFromPdfCommand, Result<PanelExtractionReviewDto>>
{
    private readonly IPanelPdfExtractionService _extractionService;
    private readonly IDbContext _dbContext;
    private readonly ILogger<ExtractPanelsFromPdfCommandHandler> _logger;

    public ExtractPanelsFromPdfCommandHandler(
        IPanelPdfExtractionService extractionService,
        IDbContext dbContext,
        ILogger<ExtractPanelsFromPdfCommandHandler> logger)
    {
        _extractionService = extractionService;
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<PanelExtractionReviewDto>> Handle(
        ExtractPanelsFromPdfCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation($"Extracting panels from PDF: {request.FileName} for project: {request.ProjectId}");

            // Validate project exists
            var project = await _dbContext.Projects
                .FirstOrDefaultAsync(p => p.ProjectId == request.ProjectId, cancellationToken);

            if (project == null)
            {
                return Result.Failure<PanelExtractionReviewDto>("Project not found");
            }

            // Validate box if provided
            Box? box = null;
            if (request.BoxId.HasValue)
            {
                box = await _dbContext.Boxes
                    .FirstOrDefaultAsync(b => b.BoxId == request.BoxId.Value && b.ProjectId == request.ProjectId, cancellationToken);

                if (box == null)
                {
                    return Result.Failure<PanelExtractionReviewDto>("Box not found or does not belong to this project");
                }
            }

            // Extract panel data using AI
            var extractionResult = await _extractionService.ExtractPanelDataFromPdfAsync(
                request.PdfStream,
                request.FileName,
                request.BoxTagsPdfStream,
                request.BoxTagsFileName,
                cancellationToken);

            if (!extractionResult.IsSuccess)
            {
                return Result.Failure<PanelExtractionReviewDto>(extractionResult.Error);
            }

            var extractedData = extractionResult.Data!;

            // Get existing panel types for this project
            var existingPanelTypes = await _dbContext.PanelTypes
                .Where(pt => pt.ProjectId == request.ProjectId)
                .ToListAsync(cancellationToken);

            var panelTypesDict = existingPanelTypes.ToDictionary(
                pt => pt.PanelTypeCode.ToUpper(),
                pt => pt);

            // Prepare review DTOs
            var reviewDto = new PanelExtractionReviewDto
            {
                PanelTypes = new List<ReviewPanelTypeDto>(),
                BoxPanels = new List<ReviewBoxPanelDto>(),
                Warnings = extractedData.Warnings,
                BoxTags = extractedData.BoxTags
            };

            foreach (var extractedPanel in extractedData.Panels)
            {
                var panelCode = extractedPanel.Tag.ToUpper().Trim();

                // Check if panel type already exists
                var existingPanelType = panelTypesDict.GetValueOrDefault(panelCode);

                // Serialize embeds to JSON
                string? embedsJson = null;
                if (extractedPanel.Embeds != null && extractedPanel.Embeds.Any())
                {
                    embedsJson = JsonSerializer.Serialize(extractedPanel.Embeds);
                }

                if (existingPanelType != null)
                {
                    // Existing panel type - return current data
                    reviewDto.PanelTypes.Add(new ReviewPanelTypeDto
                    {
                        PanelTypeId = existingPanelType.PanelTypeId,
                        PanelTypeCode = existingPanelType.PanelTypeCode,
                        PanelTypeName = existingPanelType.PanelTypeName,
                        VolumeM3 = existingPanelType.VolumeM3,
                        WeightTon = existingPanelType.WeightTon,
                        ConcreteGrade = existingPanelType.ConcreteGrade,
                        CoverMm = existingPanelType.CoverMm,
                        EmbedsJson = existingPanelType.EmbedsJson,
                        IsNew = false
                    });
                }
                else
                {
                    // New panel type - prepare data for creation
                    reviewDto.PanelTypes.Add(new ReviewPanelTypeDto
                    {
                        PanelTypeId = null,
                        PanelTypeCode = panelCode,
                        PanelTypeName = $"Panel {panelCode}",
                        VolumeM3 = extractedPanel.VolumeM3,
                        WeightTon = extractedPanel.WeightTon,
                        ConcreteGrade = extractedPanel.ConcreteGrade,
                        CoverMm = extractedPanel.CoverMm,
                        EmbedsJson = embedsJson,
                        IsNew = true
                    });
                }

                // Prepare box panel data (if box is provided)
                if (box != null)
                {
                    reviewDto.BoxPanels.Add(new ReviewBoxPanelDto
                    {
                        PanelTypeCode = panelCode,
                        PanelName = panelCode,
                        QuantityInThisBox = extractedPanel.QuantityInThisBox
                    });
                }
            }

            _logger.LogInformation($"Prepared review data: {reviewDto.PanelTypes.Count} panel types, {reviewDto.BoxPanels.Count} box panels");

            return Result.Success(reviewDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error processing PDF extraction for {request.FileName}");
            return Result.Failure<PanelExtractionReviewDto>($"Error processing PDF: {ex.Message}");
        }
    }
}

