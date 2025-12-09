using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.ProgressUpdates.Queries;

public class GetProgressUpdateByIdQueryHandler : IRequestHandler<GetProgressUpdateByIdQuery, Result<ProgressUpdateDto>>
{
    private readonly IDbContext _dbContext;
    private readonly IUnitOfWork _unitOfWork;

    public GetProgressUpdateByIdQueryHandler(IDbContext dbContext, IUnitOfWork unitOfWork)
    {
        _dbContext = dbContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ProgressUpdateDto>> Handle(GetProgressUpdateByIdQuery request, CancellationToken cancellationToken)
    {
        // Use AsNoTracking for better performance
        var update = await _unitOfWork.Repository<ProgressUpdate>()
            .GetWithSpec(new GetProgressUpdateByIdSpecification(request.ProgressUpdateId)).Data
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        if (update == null)
            return Result.Failure<ProgressUpdateDto>("Progress update not found.");

        var dto = update.Adapt<ProgressUpdateDto>();
        var updateDto = dto with
        {
            BoxTag = update.Box.BoxTag,
            ActivityName = update.BoxActivity.ActivityMaster.ActivityName,
            UpdatedByName = update.UpdatedByUser.FullName ?? update.UpdatedByUser.Email,
            Photo = update.Photo, // Keep for backward compatibility
            Images = new List<ProgressUpdateImageDto>() // Will be populated below
        };

        // Load image metadata separately (without base64 ImageData) for performance
        var images = await PopulateImageMetadata(updateDto.ProgressUpdateId, cancellationToken);
        updateDto = updateDto with { Images = images };

        return Result.Success(updateDto);
    }

    private async Task<List<ProgressUpdateImageDto>> PopulateImageMetadata(Guid progressUpdateId, CancellationToken cancellationToken)
    {
        // Load image metadata (without ImageData) in a separate lightweight query
        // Use /file endpoint so browser can load images directly as <img src>
        var images = await _dbContext.Set<ProgressUpdateImage>()
            .AsNoTracking()
            .Where(img => img.ProgressUpdateId == progressUpdateId)
            .Select(img => new ProgressUpdateImageDto
            {
                ProgressUpdateImageId = img.ProgressUpdateImageId,
                ProgressUpdateId = img.ProgressUpdateId,
                ImageData = img.ImageData,
                ImageType = img.ImageType,
                OriginalName = img.OriginalName,
                FileSize = img.FileSize,
                Sequence = img.Sequence,
                CreatedDate = img.CreatedDate,
                ImageUrl = $"/api/images/ProgressUpdate/{img.ProgressUpdateImageId}/file"
            })
            .OrderBy(i => i.Sequence)
            .ToListAsync(cancellationToken);

        return images;
    }
}

