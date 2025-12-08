using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Dubox.Application.Features.WIRCheckpoints.Queries
{
    public class GetWIRCheckpointsQueryHandler : IRequestHandler<GetWIRCheckpointsQuery, Result<List<WIRCheckpointDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDbContext _dbContext;

        public GetWIRCheckpointsQueryHandler(IUnitOfWork unitOfWork, IDbContext dbContext)
        {
            _unitOfWork = unitOfWork;
            _dbContext = dbContext;
        }

        public async Task<Result<List<WIRCheckpointDto>>> Handle(GetWIRCheckpointsQuery request, CancellationToken cancellationToken)
        {
            // Use AsNoTracking and ToListAsync for better performance
            var checkPoints = await _unitOfWork.Repository<WIRCheckpoint>()
                 .GetWithSpec(new GetWIRCheckpointsSpecification(request)).Data
                 .AsNoTracking()
                 .ToListAsync(cancellationToken);

            var checkpointDtos = checkPoints.Adapt<List<WIRCheckpointDto>>();

            // Load image metadata separately (without base64 ImageData) for performance
            await PopulateImageMetadata(checkpointDtos, cancellationToken);
            
            await PopulateActivityMetadata(checkpointDtos, cancellationToken);

            return Result.Success(checkpointDtos);
        }
        
        private async Task PopulateImageMetadata(List<WIRCheckpointDto> checkpoints, CancellationToken cancellationToken)
        {
            if (checkpoints.Count == 0) return;
            
            var wirIds = checkpoints.Select(c => c.WIRId).ToList();
            
            // Load WIR checkpoint images metadata (without ImageData)
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
                    CreatedDate = img.CreatedDate,
                    ImageUrl = $"/api/images/WIRCheckpoint/{img.WIRCheckpointImageId}"
                })
                .ToListAsync(cancellationToken);
            
            // Load quality issue IDs for these checkpoints
            var issueIds = checkpoints.SelectMany(c => c.QualityIssues.Select(q => q.IssueId)).ToList();
            
            // Load quality issue images metadata (without ImageData)
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
                    CreatedDate = img.CreatedDate,
                    ImageUrl = $"/api/images/QualityIssue/{img.QualityIssueImageId}"
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
