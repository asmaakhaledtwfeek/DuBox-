using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.QualityIssues.Queries
{
    public class GetQualityIssuesQueryHandler : IRequestHandler<GetQualityIssuesQuery, Result<List<QualityIssueDetailsDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDbContext _dbContext;
        
        public GetQualityIssuesQueryHandler(IUnitOfWork unitOfWork, IDbContext dbContext)
        {
            _unitOfWork = unitOfWork;
            _dbContext = dbContext;
        }

        public async Task<Result<List<QualityIssueDetailsDto>>> Handle(GetQualityIssuesQuery request, CancellationToken cancellationToken)
        {
            // Use AsNoTracking and ToListAsync for better performance
            var qualityIssues = await _unitOfWork.Repository<QualityIssue>()
                .GetWithSpec(new GetQualityIssuesSpecification(request)).Data
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var dtos = qualityIssues.Select(issue =>
            {
                var dto = issue.Adapt<QualityIssueDetailsDto>();
                dto.Images = new List<QualityIssueImageDto>(); // Will be populated below
                return dto;
            }).ToList();
            
            // Load image metadata separately (without base64 ImageData) for performance
            await PopulateImageMetadata(dtos, cancellationToken);

            return Result.Success(dtos);
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
