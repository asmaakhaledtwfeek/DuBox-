using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace Dubox.Application.Features.Boxes.Queries;

public class GetBoxAttachmentsQueryHandler : IRequestHandler<GetBoxAttachmentsQuery, Result<BoxAttachmentsDto>>
{
    private readonly IDbContext _context;
    private readonly IBlobStorageService _blobStorageService;
    private const string _containerName = "images";
    public GetBoxAttachmentsQueryHandler(IDbContext context, IBlobStorageService blobStorageService)
    {
        _context = context;
        _blobStorageService = blobStorageService;
    }

    public async Task<Result<BoxAttachmentsDto>> Handle(GetBoxAttachmentsQuery request, CancellationToken cancellationToken)
    {
        var boxExists = await _context.Boxes.AnyAsync(b => b.BoxId == request.BoxId, cancellationToken);
        if (!boxExists)
            return Result.Failure<BoxAttachmentsDto>("Box not found");

        var result = new BoxAttachmentsDto
        {
            WIRCheckpointImages = await GetWIRCheckpointImagesAsync(request.BoxId, cancellationToken),
            ProgressUpdateImages = await GetProgressUpdateImagesAsync(request.BoxId, cancellationToken),
            QualityIssueImages = await GetQualityIssueImagesAsync(request.BoxId, cancellationToken)
        };

        result.TotalCount = result.WIRCheckpointImages.Count +
                           result.ProgressUpdateImages.Count +
                           result.QualityIssueImages.Count;

        return Result.Success(result);
    }

    private async Task<List<BoxAttachmentDto>> GetWIRCheckpointImagesAsync(Guid boxId, CancellationToken cancellationToken)
    {
        var wirCheckpointImages = await _context.WIRCheckpoints
            .Where(wir => wir.BoxId == boxId)
            .SelectMany(wir => wir.Images.Select(img => new
            {
                Image = img,
                WIRId = wir.WIRId,
                WIRCode = wir.WIRCode,
                CreatedBy = wir.CreatedBy
            }))
            .OrderByDescending(x => x.Image.CreatedDate)
            .ToListAsync(cancellationToken);

        return wirCheckpointImages.Select(x =>
        {
            var dto = MapCommonImageData(
                x.Image,
                x.CreatedBy,
                x.WIRId,
                "WIRCheckpoint",
                x.WIRCode
            );
            dto.ImageId = x.Image.WIRCheckpointImageId;
            return dto;
        }).ToList();
    }

    private async Task<List<BoxAttachmentDto>> GetProgressUpdateImagesAsync(Guid boxId, CancellationToken cancellationToken)
    {
        var progressUpdateImages = await _context.ProgressUpdates
            .Where(pu => pu.BoxId == boxId)
            .Include(pu => pu.BoxActivity)
            .ThenInclude(ba => ba.ActivityMaster)
            .SelectMany(pu => pu.Images.Select(img => new
            {
                Image = img,
                ProgressUpdateId = pu.ProgressUpdateId,
                UpdateTitle = "Progress Update",
                UpdatedBy = pu.UpdatedBy,
                BoxActivityId = pu.BoxActivityId,
                ActivityName = pu.BoxActivity != null && pu.BoxActivity.ActivityMaster != null
                    ? pu.BoxActivity.ActivityMaster.ActivityName
                    : "Unknown Activity"
            }))
            .OrderByDescending(x => x.Image.CreatedDate)
            .ToListAsync(cancellationToken);

        return progressUpdateImages.Select(x =>
        {
            var dto = MapCommonImageData(
                x.Image,
                x.UpdatedBy,
                x.ProgressUpdateId,
                "ProgressUpdate",
                x.UpdateTitle
            );

            dto.ImageId = x.Image.ProgressUpdateImageId;
            dto.BoxActivityId = x.BoxActivityId;
            dto.ActivityName = x.ActivityName;

            return dto;
        }).ToList();
    }

    private async Task<List<BoxAttachmentDto>> GetQualityIssueImagesAsync(Guid boxId, CancellationToken cancellationToken)
    {
        var qualityIssueImages = await _context.QualityIssues
            .Where(qi => qi.BoxId == boxId)
            .SelectMany(qi => qi.Images.Select(img => new
            {
                Image = img,
                IssueId = qi.IssueId,
                IssueDescription = qi.IssueDescription,
                IssueType = qi.IssueType,
                IssueSeverity = qi.Severity,
                CreatedBy = qi.CreatedBy,
                BoxTag = qi.Box != null ? qi.Box.BoxTag : null,
                WIRCode = qi.WIRCheckpoint != null ? qi.WIRCheckpoint.WIRCode : null
            }))
            .OrderByDescending(x => x.Image.CreatedDate)
            .ToListAsync(cancellationToken);

        return qualityIssueImages.Select(x =>
        {
            var dto = MapCommonImageData(
                x.Image,
                x.CreatedBy,
                x.IssueId,
                "QualityIssue",
                x.IssueDescription
            );

            dto.ImageId = x.Image.QualityIssueImageId;
            dto.IssueType = x.IssueType?.ToString();
            dto.IssueSeverity = x.IssueSeverity?.ToString();
            dto.BoxTag = x.BoxTag;
            dto.WIRCode = x.WIRCode;

            return dto;
        }).ToList();

    }

    private string? GetImageUrl(string? imageFileName)
    {
        return !string.IsNullOrEmpty(imageFileName)
            ? _blobStorageService.GetImageUrl(_containerName,imageFileName)
            : null;
    }
    private BoxAttachmentDto MapCommonImageData(dynamic image,Guid? createdBy,Guid referenceId, string referenceType,string? referenceName)
    {
        var attachment= new BoxAttachmentDto
        {
            ImageFileName = image.ImageFileName,
            ImageUrl = GetImageUrl(image.ImageFileName),
            ImageType = image.ImageType,
            OriginalName = image.OriginalName,
            FileSize = image.FileSize,
            Sequence = image.Sequence,
            Version = image.Version,
            CreatedDate = image.CreatedDate,
            CreatedBy = createdBy,
            ReferenceId = referenceId,
            ReferenceType = referenceType,
            ReferenceName = referenceName
        }; ;
        return attachment;
    }

}
