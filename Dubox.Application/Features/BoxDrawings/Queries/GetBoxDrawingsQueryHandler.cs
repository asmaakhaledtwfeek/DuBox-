using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.BoxDrawings.Queries;

public class GetBoxDrawingsQueryHandler : IRequestHandler<GetBoxDrawingsQuery, Result<List<BoxDrawingDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBlobStorageService _blobStorageService;
    private const string _containerName = "images";
    public GetBoxDrawingsQueryHandler(IUnitOfWork unitOfWork, IBlobStorageService blobStorageService)
    {
        _unitOfWork = unitOfWork;
        _blobStorageService = blobStorageService;
    }

    public async Task<Result<List<BoxDrawingDto>>> Handle(GetBoxDrawingsQuery request, CancellationToken cancellationToken)
    {
        // Verify box exists
        var boxExists = await _unitOfWork.Repository<Box>()
            .IsExistAsync(x => x.BoxId == request.BoxId, cancellationToken);
        
        if (!boxExists)
        {
            return Result.Failure<List<BoxDrawingDto>>("Box not found");
        }

        // Get all active box drawings for the specified box with user info
        var boxDrawingsQuery = from bd in _unitOfWork.Repository<BoxDrawing>().Get()
                               join u in _unitOfWork.Repository<User>().Get() 
                                   on bd.CreatedBy equals u.UserId into userGroup
                               from user in userGroup.DefaultIfEmpty()
                               where bd.BoxId == request.BoxId && bd.IsActive
                               orderby bd.CreatedDate descending
                               select new BoxDrawingDto
                               {
                                   BoxDrawingId = bd.BoxDrawingId,
                                   BoxId = bd.BoxId,
                                   DrawingUrl = bd.DrawingUrl,
                                   DrawingFileName = bd.DrawingFileName,
                                   DownloadUrl = !string.IsNullOrEmpty(bd.DrawingFileName)
                                                 ? _blobStorageService.GetImageUrl(_containerName,bd.DrawingFileName)
                                                 : bd.DrawingUrl,
                                   OriginalFileName = bd.OriginalFileName,
                                   FileExtension = bd.FileExtension,
                                   FileType = bd.FileType,
                                   FileSize = bd.FileSize,
                                   Version = bd.Version,
                                   CreatedDate = bd.CreatedDate,
                                   CreatedBy = bd.CreatedBy,
                                   CreatedByName = user != null ? (user.FullName ?? user.Email) : "Unknown"
                               };

        var boxDrawingDtos = boxDrawingsQuery.ToList();

        return Result.Success(boxDrawingDtos);
    }
}

