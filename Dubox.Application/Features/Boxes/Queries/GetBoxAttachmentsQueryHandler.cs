using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Boxes.Queries;

public class GetBoxAttachmentsQueryHandler : IRequestHandler<GetBoxAttachmentsQuery, Result<BoxAttachmentsDto>>
{
    private readonly IDbContext _context;

    public GetBoxAttachmentsQueryHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<Result<BoxAttachmentsDto>> Handle(GetBoxAttachmentsQuery request, CancellationToken cancellationToken)
    {
        // Verify box exists
        var boxExists = await _context.Boxes.AnyAsync(b => b.BoxId == request.BoxId, cancellationToken);
        if (!boxExists)
        {
            return Result.Failure<BoxAttachmentsDto>("Box not found");
        }

        var result = new BoxAttachmentsDto();

        // Get WIRCheckpoint Images
        var wirCheckpointImages = await _context.WIRCheckpoints
            .Where(wir => wir.BoxId == request.BoxId)
            .SelectMany(wir => wir.Images.Select(img => new
            {
                Image = img,
                WIRId = wir.WIRId,
                WIRCode = wir.WIRCode,
                CreatedBy = wir.CreatedBy
            }))
            .OrderByDescending(x => x.Image.CreatedDate)
            .ToListAsync(cancellationToken);

        result.WIRCheckpointImages = wirCheckpointImages.Select(x => new BoxAttachmentDto
        {
            ImageId = x.Image.WIRCheckpointImageId,
            ImageData = x.Image.ImageData,
            ImageType = x.Image.ImageType,
            OriginalName = x.Image.OriginalName,
            FileSize = x.Image.FileSize,
            Sequence = x.Image.Sequence,
            CreatedDate = x.Image.CreatedDate,
            CreatedBy = x.CreatedBy, // Use WIRCheckpoint.CreatedBy
            ReferenceId = x.WIRId,
            ReferenceType = "WIRCheckpoint",
            ReferenceName = x.WIRCode
        }).ToList();

        // Get ProgressUpdate Images
        var progressUpdateImages = await _context.ProgressUpdates
            .Where(pu => pu.BoxId == request.BoxId)
            .SelectMany(pu => pu.Images.Select(img => new
            {
                Image = img,
                ProgressUpdateId = pu.ProgressUpdateId,
                UpdateTitle = "Progress Update",
                UpdatedBy = pu.UpdatedBy
            }))
            .OrderByDescending(x => x.Image.CreatedDate)
            .ToListAsync(cancellationToken);

        result.ProgressUpdateImages = progressUpdateImages.Select(x => new BoxAttachmentDto
        {
            ImageId = x.Image.ProgressUpdateImageId,
            ImageData = x.Image.ImageData,
            ImageType = x.Image.ImageType,
            OriginalName = x.Image.OriginalName,
            FileSize = x.Image.FileSize,
            Sequence = x.Image.Sequence,
            CreatedDate = x.Image.CreatedDate,
            CreatedBy = x.UpdatedBy, // Use ProgressUpdate.UpdatedBy as the creator
            ReferenceId = x.ProgressUpdateId,
            ReferenceType = "ProgressUpdate",
            ReferenceName = x.UpdateTitle
        }).ToList();

        // Get QualityIssue Images
        var qualityIssueImages = await _context.QualityIssues
            .Where(qi => qi.WIRCheckpoint!.BoxId == request.BoxId)
            .SelectMany(qi => qi.Images.Select(img => new
            {
                Image = img,
                IssueId = qi.IssueId,
                IssueDescription = qi.IssueDescription,
                CreatedBy = qi.CreatedBy
            }))
            .OrderByDescending(x => x.Image.CreatedDate)
            .ToListAsync(cancellationToken);

        result.QualityIssueImages = qualityIssueImages.Select(x => new BoxAttachmentDto
        {
            ImageId = x.Image.QualityIssueImageId,
            ImageData = x.Image.ImageData,
            ImageType = x.Image.ImageType,
            OriginalName = x.Image.OriginalName,
            FileSize = x.Image.FileSize,
            Sequence = x.Image.Sequence,
            CreatedDate = x.Image.CreatedDate,
            CreatedBy = x.CreatedBy, // Use QualityIssue.CreatedBy
            ReferenceId = x.IssueId,
            ReferenceType = "QualityIssue",
            ReferenceName = x.IssueDescription
        }).ToList();

        result.TotalCount = result.WIRCheckpointImages.Count +
                           result.ProgressUpdateImages.Count +
                           result.QualityIssueImages.Count;

        return Result.Success(result);
    }
}
