using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.ProgressUpdates.Queries;

public class GetProgressUpdatesByActivityQueryHandler : IRequestHandler<GetProgressUpdatesByActivityQuery, Result<List<ProgressUpdateDto>>>
{
    private readonly IDbContext _dbContext;
    private readonly IUnitOfWork _unitOfWork;

    public GetProgressUpdatesByActivityQueryHandler(IDbContext dbContext, IUnitOfWork unitOfWork)
    {
        _dbContext = dbContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<ProgressUpdateDto>>> Handle(GetProgressUpdatesByActivityQuery request, CancellationToken cancellationToken)
    {
        // Use AsNoTracking and ToListAsync for better performance
        var updates = await _unitOfWork.Repository<ProgressUpdate>()
            .GetWithSpec(new GetProgressUpdatesByActivitySpecification(request.BoxActivityId)).Data
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var updateDtos = updates.Select(u =>
        {
            var dto = u.Adapt<ProgressUpdateDto>();
            return dto with
            {
                BoxTag = u.Box.BoxTag,
                ActivityName = u.BoxActivity.ActivityMaster.ActivityName,
                UpdatedByName = u.UpdatedByUser.FullName ?? u.UpdatedByUser.Email,
                Photo = u.Photo, // Keep for backward compatibility
                Images = new List<ProgressUpdateImageDto>() // Will be populated below
            };
        }).ToList();
        
        // Load image metadata separately (without base64 ImageData) for performance
        await PopulateImageMetadata(updateDtos, cancellationToken);

        return Result.Success(updateDtos);
    }
    
    private async Task PopulateImageMetadata(List<ProgressUpdateDto> updates, CancellationToken cancellationToken)
    {
        if (updates.Count == 0) return;
        
        var progressUpdateIds = updates.Select(u => u.ProgressUpdateId).ToList();
        
        // Load image metadata (without ImageData) in a separate lightweight query
        var images = await _dbContext.Set<ProgressUpdateImage>()
            .AsNoTracking()
            .Where(img => progressUpdateIds.Contains(img.ProgressUpdateId))
            .Select(img => new ProgressUpdateImageDto
            {
                ProgressUpdateImageId = img.ProgressUpdateImageId,
                ProgressUpdateId = img.ProgressUpdateId,
                ImageData = null, // Don't load base64 data!
                ImageType = img.ImageType,
                OriginalName = img.OriginalName,
                FileSize = img.FileSize,
                Sequence = img.Sequence,
                CreatedDate = img.CreatedDate,
                ImageUrl = $"/api/images/ProgressUpdate/{img.ProgressUpdateImageId}"
            })
            .ToListAsync(cancellationToken);
        
        var imagesByUpdateId = images.GroupBy(i => i.ProgressUpdateId)
            .ToDictionary(g => g.Key, g => g.OrderBy(i => i.Sequence).ToList());
        
        // Updates are records, so we need to replace them
        for (int i = 0; i < updates.Count; i++)
        {
            if (imagesByUpdateId.TryGetValue(updates[i].ProgressUpdateId, out var updateImages))
            {
                updates[i] = updates[i] with { Images = updateImages };
            }
        }
    }
}

