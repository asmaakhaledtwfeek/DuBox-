using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.ProgressUpdates.Queries;

public class GetProgressUpdatesByBoxQueryHandler : IRequestHandler<GetProgressUpdatesByBoxQuery, Result<PaginatedProgressUpdatesResponseDto>>
{
    private readonly IDbContext _dbContext;
    private readonly IUnitOfWork _unitOfWork;

    public GetProgressUpdatesByBoxQueryHandler(IDbContext dbContext, IUnitOfWork unitOfWork)
    {
        _dbContext = dbContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PaginatedProgressUpdatesResponseDto>> Handle(GetProgressUpdatesByBoxQuery request, CancellationToken cancellationToken)
    {
        // Validate pagination parameters
        var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
        var pageSize = request.PageSize < 1 ? 10 : (request.PageSize > 100 ? 100 : request.PageSize);

        // Create query with validated pagination
        var query = request with
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var specification = new GetProgressUpdatesByBoxSpecification(query);
        var updatesResult = _unitOfWork.Repository<ProgressUpdate>().GetWithSpec(specification);
        var updates = await updatesResult.Data.AsNoTracking().ToListAsync(cancellationToken);
        var totalCount = updatesResult.Count;

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

        // Calculate total pages
        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        var response = new PaginatedProgressUpdatesResponseDto
        {
            Items = updateDtos,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPages = totalPages
        };

        return Result.Success(response);
    }
    
    private async Task PopulateImageMetadata(List<ProgressUpdateDto> updates, CancellationToken cancellationToken)
    {
        if (updates.Count == 0) return;
        
        var progressUpdateIds = updates.Select(u => u.ProgressUpdateId).ToList();
        
        // Load image metadata (without ImageData) in a separate lightweight query
        // Use /file endpoint so browser can load images directly as <img src>
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
                ImageUrl = $"/api/images/ProgressUpdate/{img.ProgressUpdateImageId}/file"
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

