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
                Images = u.Images.OrderBy(img => img.Sequence).Select(img => new ProgressUpdateImageDto
                {
                    ProgressUpdateImageId = img.ProgressUpdateImageId,
                    ProgressUpdateId = img.ProgressUpdateId,
                    ImageData = img.ImageData,
                    ImageType = img.ImageType,
                    OriginalName = img.OriginalName,
                    FileSize = img.FileSize,
                    Sequence = img.Sequence,
                    CreatedDate = img.CreatedDate
                }).ToList()
            };
        }).ToList();

        return Result.Success(updateDtos);
    }
}

