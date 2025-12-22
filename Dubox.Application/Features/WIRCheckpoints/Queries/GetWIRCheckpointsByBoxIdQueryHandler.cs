using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.WIRCheckpoints.Queries
{
    public class GetWIRCheckpointsByBoxIdQueryHandler
     : IRequestHandler<GetWIRCheckpointsByBoxIdQuery, Result<List<WIRCheckpointDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDbContext _dbContext;
        private readonly IProjectTeamVisibilityService _visibilityService;

        public GetWIRCheckpointsByBoxIdQueryHandler(
            IUnitOfWork unitOfWork,
            IDbContext dbContext,
            IProjectTeamVisibilityService visibilityService)
        {
            _unitOfWork = unitOfWork;
            _dbContext = dbContext;
            _visibilityService = visibilityService;
        }

        public async Task<Result<List<WIRCheckpointDto>>> Handle(GetWIRCheckpointsByBoxIdQuery request, CancellationToken cancellationToken)
        {
            var box = await _unitOfWork.Repository<Box>().GetByIdAsync(request.BoxId);
            if (box == null)
                return Result.Failure<List<WIRCheckpointDto>>("Box not found");

            // Verify user has access to the project this box belongs to
            var canAccessProject = await _visibilityService.CanAccessProjectAsync(box.ProjectId, cancellationToken);
            if (!canAccessProject)
            {
                return Result.Failure<List<WIRCheckpointDto>>("Access denied. You do not have permission to view WIR checkpoints for this box.");
            }

            // Use AsNoTracking and ToListAsync for better performance
            var checkpoints = await _unitOfWork.Repository<WIRCheckpoint>()
                .GetWithSpec(new GetWIRCheckPointsByBoxIdSpecification(request.BoxId)).Data
                .AsNoTracking()
                .ToListAsync(cancellationToken);
            var checkpointDtos = new List<WIRCheckpointDto>();
            try
            {

                 checkpointDtos = checkpoints.Adapt<List<WIRCheckpointDto>>();
            }catch(Exception ex)
            {

            }

            // Enrich checklist items with section and checklist information
            EnrichChecklistItems(checkpoints, checkpointDtos);

            // Load image metadata separately (without base64 ImageData) for performance
           // await PopulateImageMetadata(checkpointDtos, cancellationToken);

            await PopulateActivityMetadata(checkpointDtos, cancellationToken);

            return Result.Success(checkpointDtos);
        }

        private void EnrichChecklistItems(List<WIRCheckpoint> checkpoints, List<WIRCheckpointDto> checkpointDtos)
        {
            for (int i = 0; i < checkpoints.Count && i < checkpointDtos.Count; i++)
            {
                var checkpoint = checkpoints[i];
                var checkpointDto = checkpointDtos[i];

                if (checkpointDto.ChecklistItems == null || checkpoint.ChecklistItems == null)
                    continue;

                foreach (var dtoItem in checkpointDto.ChecklistItems)
                {
                    // Find the corresponding entity item
                    var entityItem = checkpoint.ChecklistItems
                        .FirstOrDefault(ci => ci.ChecklistItemId == dtoItem.ChecklistItemId);

                    if (entityItem?.PredefinedChecklistItem != null)
                    {
                        var predefinedItem = entityItem.PredefinedChecklistItem;

                        // Map section information
                        if (predefinedItem.ChecklistSection != null)
                        {
                            dtoItem.SectionId = predefinedItem.ChecklistSection.ChecklistSectionId;
                            dtoItem.SectionName = predefinedItem.ChecklistSection.Title;
                            dtoItem.SectionOrder = predefinedItem.ChecklistSection.Order;

                            // Map checklist information
                            if (predefinedItem.ChecklistSection.Checklist != null)
                            {
                                dtoItem.ChecklistId = predefinedItem.ChecklistSection.Checklist.ChecklistId;
                                dtoItem.ChecklistName = predefinedItem.ChecklistSection.Checklist.Name;
                                dtoItem.ChecklistCode = predefinedItem.ChecklistSection.Checklist.Code;

                                // Use checklist name as category if not already set
                                if (string.IsNullOrEmpty(dtoItem.CategoryName))
                                {
                                    dtoItem.CategoryName = predefinedItem.ChecklistSection.Checklist.Name;
                                }
                            }
                        }
                    }
                }
            }
        }

        private async Task PopulateImageMetadata(List<WIRCheckpointDto> checkpoints, CancellationToken cancellationToken)
        {
            if (checkpoints.Count == 0) return;

            var wirIds = checkpoints.Select(c => c.WIRId).ToList();

            // Load WIR checkpoint images metadata (without ImageData)
            // Use /file endpoint so browser can load images directly as <img src>
            var wirImages = await _dbContext.Set<WIRCheckpointImage>()
                .AsNoTracking()
                .Where(img => wirIds.Contains(img.WIRId))
                .Select(img => new WIRCheckpointImageDto
                {
                    WIRCheckpointImageId = img.WIRCheckpointImageId,
                    WIRId = img.WIRId,
                    ImageData = null, // Don't load base64 data!
                    ImageType = img.ImageType,
                    OriginalName = img.OriginalName,
                    FileSize = img.FileSize,
                    Sequence = img.Sequence,
                    Version = img.Version,
                    CreatedDate = img.CreatedDate,
                    ImageUrl = $"/api/images/WIRCheckpoint/{img.WIRCheckpointImageId}/file"
                })
                .ToListAsync(cancellationToken);

            // Load quality issue IDs for these checkpoints
            var issueIds = checkpoints.SelectMany(c => c.QualityIssues.Select(q => q.IssueId)).ToList();

            // Load quality issue images metadata (without ImageData)
            // Use /file endpoint so browser can load images directly as <img src>
            var qualityImages = await _dbContext.Set<QualityIssueImage>()
                .AsNoTracking()
                .Where(img => issueIds.Contains(img.IssueId))
                .Select(img => new QualityIssueImageDto
                {
                    QualityIssueImageId = img.QualityIssueImageId,
                    IssueId = img.IssueId,
                    ImageData = null, // Don't load base64 data!
                    ImageType = img.ImageType,
                    OriginalName = img.OriginalName,
                    FileSize = img.FileSize,
                    Sequence = img.Sequence,
                    Version = img.Version,
                    CreatedDate = img.CreatedDate,
                    ImageUrl = $"/api/images/QualityIssue/{img.QualityIssueImageId}/file"
                })
                .ToListAsync(cancellationToken);

            // Map images to checkpoints
            var wirImagesByWirId = wirImages.GroupBy(i => i.WIRId).ToDictionary(g => g.Key, g => g.OrderBy(i => i.Sequence).ToList());
            var qualityImagesByIssueId = qualityImages.GroupBy(i => i.IssueId).ToDictionary(g => g.Key, g => g.OrderBy(i => i.Sequence).ToList());

            foreach (var checkpoint in checkpoints)
            {
                if (wirImagesByWirId.TryGetValue(checkpoint.WIRId, out var cpImages))
                {
                    checkpoint.Images = cpImages;
                }

                foreach (var issue in checkpoint.QualityIssues)
                {
                    if (qualityImagesByIssueId.TryGetValue(issue.IssueId, out var issueImages))
                    {
                        issue.Images = issueImages;
                        issue.ImageCount = issueImages.Count;
                    }
                }
            }
        }

        private async Task PopulateActivityMetadata(List<WIRCheckpointDto> checkpoints, CancellationToken cancellationToken)
        {
            if (checkpoints.Count == 0)
            {
                return;
            }

            var wirCodes = checkpoints
                .Select(cp => cp.WIRNumber)
                .Where(code => !string.IsNullOrWhiteSpace(code))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (!wirCodes.Any())
            {
                return;
            }

            // Use AsNoTracking and ToListAsync for better performance
            var wirRecords = await _unitOfWork.Repository<WIRRecord>()
                .GetWithSpec(new GetWIRRecordsByCodesSpecification(wirCodes)).Data
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var recordMap = wirRecords
                .GroupBy(record => record.WIRCode.ToLower())
                .ToDictionary(
                    group => group.Key,
                    group => group
                        .OrderByDescending(r => r.CreatedDate)
                        .First());

            foreach (var checkpoint in checkpoints)
            {
                var key = checkpoint.WIRNumber?.ToLower();
                if (string.IsNullOrWhiteSpace(key))
                {
                    continue;
                }

                if (!recordMap.TryGetValue(key, out var record))
                {
                    continue;
                }

                checkpoint.BoxActivityId = record.BoxActivityId;
                checkpoint.ProjectId ??= record.BoxActivity?.Box?.ProjectId;
            }
        }
    }

}
