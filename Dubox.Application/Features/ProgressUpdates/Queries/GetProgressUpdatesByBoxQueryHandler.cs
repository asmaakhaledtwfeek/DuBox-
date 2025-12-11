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

        var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
        var pageSize = request.PageSize < 1 ? 10 : (request.PageSize > 100 ? 100 : request.PageSize);

        var query = request with
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var specification = new GetProgressUpdatesByBoxSpecification(query);
        var updatesResult = _unitOfWork.Repository<ProgressUpdate>().GetWithSpec(specification);
        var updates = await updatesResult.Data.AsNoTracking().ToListAsync(cancellationToken);

        var highestPerActiviry = updates
            .GroupBy(up => up.BoxActivityId)
            .Select(h => h.OrderByDescending(g => g.ProgressPercentage).OrderByDescending(g => g.UpdateDate).FirstOrDefault());
        var totalCount = highestPerActiviry.ToList().Count;
        var updateDtos = highestPerActiviry.Select(u =>
        {
            var dto = u.Adapt<ProgressUpdateDto>();
            return dto with
            {
                ActivityName = u.BoxActivity.ActivityMaster.ActivityName,
                UpdatedByName = u.UpdatedByUser.FullName ?? u.UpdatedByUser.Email,
            };
        }).ToList();

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

    //private async Task PopulateImageMetadata(List<ProgressUpdateDto> updates, CancellationToken cancellationToken)
    //{
    //    if (updates.Count == 0) return;

    //    var progressUpdateIds = updates.Select(u => u.ProgressUpdateId).ToList();

    //    // Load image metadata (without ImageData) in a separate lightweight query
    //    // Use /file endpoint so browser can load images directly as <img src>
    //    var images = await _dbContext.Set<ProgressUpdateImage>()
    //        .AsNoTracking()
    //        .Where(img => progressUpdateIds.Contains(img.ProgressUpdateId))
    //        .Select(img => new ProgressUpdateImageDto
    //        {
    //            ProgressUpdateImageId = img.ProgressUpdateImageId,
    //            ProgressUpdateId = img.ProgressUpdateId,
    //            // Include base64 so frontend can render even if file URL fails
    //            ImageData = img.ImageData,
    //            ImageType = img.ImageType,
    //            OriginalName = img.OriginalName,
    //            FileSize = img.FileSize,
    //            Sequence = img.Sequence,
    //            CreatedDate = img.CreatedDate,
    //            ImageUrl = $"/api/images/ProgressUpdate/{img.ProgressUpdateImageId}/file"
    //        })
    //        .ToListAsync(cancellationToken);

    //    var imagesByUpdateId = images.GroupBy(i => i.ProgressUpdateId)
    //        .ToDictionary(g => g.Key, g => g.OrderBy(i => i.Sequence).ToList());

    //    // Updates are records, so we need to replace them
    //    for (int i = 0; i < updates.Count; i++)
    //    {
    //        if (imagesByUpdateId.TryGetValue(updates[i].ProgressUpdateId, out var updateImages))
    //        {
    //            updates[i] = updates[i] with { Images = updateImages };
    //        }
    //    }
    //}
}

