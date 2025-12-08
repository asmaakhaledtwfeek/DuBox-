using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Images.Queries;

public class GetImageQueryHandler : IRequestHandler<GetImageQuery, Result<ImageDataDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetImageQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ImageDataDto>> Handle(GetImageQuery request, CancellationToken cancellationToken)
    {
        ImageDataDto? result = null;

        switch (request.Category)
        {
            case ImageCategory.ProgressUpdate:
                var progressImage = await _unitOfWork.Repository<ProgressUpdateImage>()
                    .GetByIdAsync(request.ImageId, cancellationToken);
                if (progressImage != null)
                {
                    result = new ImageDataDto
                    {
                        ImageId = progressImage.ProgressUpdateImageId,
                        ImageData = progressImage.ImageData,
                        ImageType = progressImage.ImageType,
                        OriginalName = progressImage.OriginalName,
                        FileSize = progressImage.FileSize
                    };
                }
                break;

            case ImageCategory.QualityIssue:
                var qualityImage = await _unitOfWork.Repository<QualityIssueImage>()
                    .GetByIdAsync(request.ImageId, cancellationToken);
                if (qualityImage != null)
                {
                    result = new ImageDataDto
                    {
                        ImageId = qualityImage.QualityIssueImageId,
                        ImageData = qualityImage.ImageData,
                        ImageType = qualityImage.ImageType,
                        OriginalName = qualityImage.OriginalName,
                        FileSize = qualityImage.FileSize
                    };
                }
                break;

            case ImageCategory.WIRCheckpoint:
                var wirImage = await _unitOfWork.Repository<WIRCheckpointImage>()
                    .GetByIdAsync(request.ImageId, cancellationToken);
                if (wirImage != null)
                {
                    result = new ImageDataDto
                    {
                        ImageId = wirImage.WIRCheckpointImageId,
                        ImageData = wirImage.ImageData,
                        ImageType = wirImage.ImageType,
                        OriginalName = wirImage.OriginalName,
                        FileSize = wirImage.FileSize
                    };
                }
                break;
        }

        if (result == null)
        {
            return Result.Failure<ImageDataDto>("Image not found");
        }

        return Result.Success(result);
    }
}

