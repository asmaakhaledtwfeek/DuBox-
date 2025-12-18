using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.QualityIssues.Queries
{
    public class GetQualityIssuesQueryHandler : IRequestHandler<GetQualityIssuesQuery, Result<PaginatedQualityIssuesResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDbContext _dbContext;
        private readonly IProjectTeamVisibilityService _visibilityService;
        
        public GetQualityIssuesQueryHandler(
            IUnitOfWork unitOfWork, 
            IDbContext dbContext,
            IProjectTeamVisibilityService visibilityService)
        {
            _unitOfWork = unitOfWork;
            _dbContext = dbContext;
            _visibilityService = visibilityService;
        }

        public async Task<Result<PaginatedQualityIssuesResponseDto>> Handle(GetQualityIssuesQuery request, CancellationToken cancellationToken)
        {
            var accessibleProjectIds = await _visibilityService.GetAccessibleProjectIdsAsync(cancellationToken);
            
            var (page, pageSize) = new PaginatedRequest
            {
                Page = request.Page,
                PageSize = request.PageSize
            }.GetNormalizedPagination();
            
            var qualityIssuesResult = _unitOfWork.Repository<QualityIssue>()
                .GetWithSpec(new GetQualityIssuesSpecification(request, accessibleProjectIds));
            
            var qualityIssues = await qualityIssuesResult.Data
                .AsNoTracking()
                .ToListAsync(cancellationToken);
            
            var totalCount = qualityIssuesResult.Count;

            var dtos = qualityIssues.Select(issue =>
            {
                var dto = issue.Adapt<QualityIssueDetailsDto>();
               // dto.Images = new List<QualityIssueImageDto>();
                return dto;
            }).ToList();
            
            //await PopulateImageMetadata(dtos, cancellationToken);

            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            var response = new PaginatedQualityIssuesResponseDto
            {
                Items = dtos,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages
            };

            return Result.Success(response);
        }
        
        private async Task PopulateImageMetadata(List<QualityIssueDetailsDto> issues, CancellationToken cancellationToken)
        {
            if (issues.Count == 0) return;
            
            var issueIds = issues.Select(i => i.IssueId).ToList();
            
            // Load image metadata (without ImageData) in a separate lightweight query
            // Use /file endpoint so browser can load images directly as <img src>
            var images = await _dbContext.Set<QualityIssueImage>()
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
                    CreatedDate = img.CreatedDate,
                    ImageUrl = $"/api/images/QualityIssue/{img.QualityIssueImageId}/file"
                })
                .ToListAsync(cancellationToken);
            
            var imagesByIssueId = images.GroupBy(i => i.IssueId)
                .ToDictionary(g => g.Key, g => g.OrderBy(i => i.Sequence).ToList());
            
            foreach (var issue in issues)
            {
                if (imagesByIssueId.TryGetValue(issue.IssueId, out var issueImages))
                {
                    issue.Images = issueImages;
                }
            }
        }
    }

}
