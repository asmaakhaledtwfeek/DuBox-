using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Dubox.Application.Features.WIRCheckpoints.Queries
{
    public class GetWIRCheckpointsQueryHandler : IRequestHandler<GetWIRCheckpointsQuery, Result<PaginatedWIRCheckpointsResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDbContext _dbContext;
        private readonly IProjectTeamVisibilityService _visibilityService;

        public GetWIRCheckpointsQueryHandler(
            IUnitOfWork unitOfWork, 
            IDbContext dbContext,
            IProjectTeamVisibilityService visibilityService)
        {
            _unitOfWork = unitOfWork;
            _dbContext = dbContext;
            _visibilityService = visibilityService;
        }

        public async Task<Result<PaginatedWIRCheckpointsResponseDto>> Handle(GetWIRCheckpointsQuery request, CancellationToken cancellationToken)
        {
            // Get accessible project IDs based on user role
            var accessibleProjectIds = await _visibilityService.GetAccessibleProjectIdsAsync(cancellationToken);
            
            // Normalize pagination parameters
            var (page, pageSize) = new PaginatedRequest
            {
                Page = request.Page,
                PageSize = request.PageSize
            }.GetNormalizedPagination();
            
            // Get checkpoints with specification (includes pagination)
            var checkpointsResult = _unitOfWork.Repository<WIRCheckpoint>()
                .GetWithSpec(new GetWIRCheckpointsSpecification(request, accessibleProjectIds));
            
            var checkPoints = await checkpointsResult.Data
                .AsNoTracking()
                .ToListAsync(cancellationToken);
            
            var totalCount = checkpointsResult.Count;

            var checkpointDtos = checkPoints.Adapt<List<WIRCheckpointDto>>();

            // Load image metadata separately (without base64 ImageData) for performance
            await PopulateImageMetadata(checkpointDtos, cancellationToken);

            await PopulateActivityMetadata(checkpointDtos, cancellationToken);

            // Calculate summary statistics for ALL checkpoints (not just current page)
            var summary = await CalculateSummaryAsync(request, accessibleProjectIds, cancellationToken);

            // Calculate total pages
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            var response = new PaginatedWIRCheckpointsResponseDto
            {
                Items = checkpointDtos,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                Summary = summary
            };

            return Result.Success(response);
        }

        private async Task<WIRCheckpointsSummary> CalculateSummaryAsync(
            GetWIRCheckpointsQuery request, 
            List<Guid>? accessibleProjectIds, 
            CancellationToken cancellationToken)
        {
            // Get all checkpoints matching the filter criteria (without pagination)
            var allCheckpointsResult = _unitOfWork.Repository<WIRCheckpoint>()
                .GetWithSpec(new GetWIRCheckpointsSummarySpecification(request, accessibleProjectIds));

            // Get status counts using GroupBy
            var statusCounts = await allCheckpointsResult.Data
                .AsNoTracking()
                .GroupBy(c => c.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToListAsync(cancellationToken);

            var summary = new WIRCheckpointsSummary
            {
                TotalCheckpoints = statusCounts.Sum(s => s.Count),
                PendingReviews = statusCounts
                    .Where(s => s.Status == Domain.Enums.WIRCheckpointStatusEnum.Pending)
                    .Sum(s => s.Count),
                Approved = statusCounts
                    .Where(s => s.Status == Domain.Enums.WIRCheckpointStatusEnum.Approved)
                    .Sum(s => s.Count),
                Rejected = statusCounts
                    .Where(s => s.Status == Domain.Enums.WIRCheckpointStatusEnum.Rejected)
                    .Sum(s => s.Count),
                ConditionalApproval = statusCounts
                    .Where(s => s.Status == Domain.Enums.WIRCheckpointStatusEnum.ConditionalApproval)
                    .Sum(s => s.Count)
            };

            return summary;
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
