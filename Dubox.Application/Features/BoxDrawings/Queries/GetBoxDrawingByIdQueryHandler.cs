using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.BoxDrawings.Queries;

public class GetBoxDrawingByIdQueryHandler : IRequestHandler<GetBoxDrawingByIdQuery, Result<BoxDrawingDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBlobStorageService _blobStorageService;
    private const string _containerName = "images";
    public GetBoxDrawingByIdQueryHandler(IUnitOfWork unitOfWork, IBlobStorageService blobStorageService)
    {
        _unitOfWork = unitOfWork;
      _blobStorageService = blobStorageService;
    }

    public async Task<Result<BoxDrawingDto>> Handle(GetBoxDrawingByIdQuery request, CancellationToken cancellationToken )
    {
        var drawing = await _unitOfWork.Repository<BoxDrawing>()
            .GetByIdAsync(request.DrawingId, cancellationToken);

        if (drawing == null || !drawing.IsActive)
        {
            return Result.Failure<BoxDrawingDto>("Drawing not found");
        }

        // Get user name
        string? createdByName = "Unknown";
        if (drawing.CreatedBy.HasValue)
        {
            var user = await _unitOfWork.Repository<User>().GetByIdAsync(drawing.CreatedBy.Value, cancellationToken);
            if (user != null)
            {
                createdByName = user.FullName ?? user.Email;
            }
        }

        var drawingDto = drawing.Adapt<BoxDrawingDto>() with 
        { 
            CreatedByName = createdByName,
            DownloadUrl = !string.IsNullOrEmpty(drawing.DrawingFileName)
                                                 ? _blobStorageService.GetImageUrl(_containerName,drawing.DrawingFileName)
                                                 : drawing.DrawingUrl
        };
        return Result.Success(drawingDto);
    }
}

