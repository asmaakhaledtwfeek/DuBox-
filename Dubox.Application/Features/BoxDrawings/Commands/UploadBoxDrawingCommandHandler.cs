using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Interfaces;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dubox.Application.Features.BoxDrawings.Commands;

public class UploadBoxDrawingCommandHandler : IRequestHandler<UploadBoxDrawingCommand, Result<BoxDrawingDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IBlobStorageService _blobStorageService;
    private readonly ILogger<UploadBoxDrawingCommandHandler> _logger;
    private const string _containerName = "drawings";
    public UploadBoxDrawingCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IBlobStorageService blobStorageService, ILogger<UploadBoxDrawingCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _blobStorageService = blobStorageService;
        _logger = logger;
    }

    public async Task<Result<BoxDrawingDto>> Handle(UploadBoxDrawingCommand request, CancellationToken cancellationToken)
    {
        var box = await _unitOfWork.Repository<Box>().GetByIdAsync(request.BoxId, cancellationToken);
        if (box == null)
            return Result.Failure<BoxDrawingDto>("Box not found.");

        Guid? currentUserId = null;
        if (Guid.TryParse(_currentUserService.UserId, out var userId))
            currentUserId = userId;

        int version = 1;
        string fileName = request.FileName ?? request.DrawingUrl ?? "unknown";

        _logger.LogInformation($"🔍 VERSION DEBUG - Uploading file: {fileName}");
        
        // Get existing drawings - use FindAsync to execute immediately and avoid race conditions
        var existingDrawings = await _unitOfWork.Repository<BoxDrawing>()
            .FindAsync(
                filter: d => d.BoxId == request.BoxId && d.IsActive,
                cancellationToken: cancellationToken
            );
        
        _logger.LogInformation($"🔍 VERSION DEBUG - Found {existingDrawings.Count} existing drawings for this box");
        _logger.LogInformation($"🔍 VERSION DEBUG - Existing files: {string.Join(", ", existingDrawings.Select(d => $"{d.OriginalFileName ?? d.DrawingUrl} (V{d.Version})"))}");
        
        var fileNameLower = fileName.ToLower();
        var sameNameDrawings = existingDrawings
            .Where(d => 
                (d.OriginalFileName != null && d.OriginalFileName.ToLower() == fileNameLower) ||
                (d.DrawingUrl != null && d.DrawingUrl.ToLower() == fileNameLower)
            )
            .ToList();

        _logger.LogInformation($"🔍 VERSION DEBUG - Found {sameNameDrawings.Count} drawings with same name '{fileName}'");
        
        if (sameNameDrawings.Any())
        {
            // Get the highest version number and increment
            var maxVersion = sameNameDrawings.Max(d => d.Version);
            version = maxVersion + 1;
            _logger.LogInformation($"🔍 VERSION DEBUG - Max existing version: V{maxVersion}, New version will be: V{version}");
        }
        else
            _logger.LogInformation($"🔍 VERSION DEBUG - No existing files with same name, using V1");

        // Create the BoxDrawing entity
        var boxDrawing = new BoxDrawing
        {
            BoxId = request.BoxId,
            DrawingUrl = request.DrawingUrl,
            Version = version,
            IsActive = true,
            CreatedDate = DateTime.UtcNow,
            CreatedBy = currentUserId
        };

        if (request.File != null && request.File.Length > 0)
        {
            try
            {
                var folderName = $"box-drawings/{request.BoxId}";
                var blobFileName = await _blobStorageService.UploadFileAsync(
                    _containerName,
                    request.File,
                    folderName
                );

                _logger.LogInformation($"✅ Uploaded drawing to Blob: {blobFileName}");

                boxDrawing.DrawingFileName = blobFileName;
                boxDrawing.OriginalFileName = request.FileName;
                boxDrawing.FileExtension = Path.GetExtension(request.FileName ?? "").ToLowerInvariant();
                boxDrawing.FileType = "file";
                boxDrawing.FileSize = request.File.Length;

                _logger.LogInformation($"🔍 VERSION DEBUG - OriginalFileName: {request.FileName}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Failed to upload drawing to Blob Storage");
                return Result.Failure<BoxDrawingDto>($"Error uploading file: {ex.Message}");
            }
        }
        else if (!string.IsNullOrWhiteSpace(request.DrawingUrl))
        {
            boxDrawing.DrawingUrl = request.DrawingUrl;
            boxDrawing.FileType = "url";
            _logger.LogInformation($"✅ Saved drawing URL: {request.DrawingUrl}");
        }

        try
        {
            var allDrawingsBeforeSave = await _unitOfWork.Repository<BoxDrawing>()
                .FindAsync(
                    filter: d => d.BoxId == request.BoxId && d.IsActive,
                    cancellationToken: cancellationToken
                );

            var finalCheck = allDrawingsBeforeSave
                .Where(d =>
                    (d.OriginalFileName != null && d.OriginalFileName.ToLower() == fileNameLower) ||
                    (d.DrawingUrl != null && d.DrawingUrl.ToLower() == fileNameLower)
                )
                .ToList();

            if (finalCheck.Any())
            {
                var finalMaxVersion = finalCheck.Max(d => d.Version);
                if (finalMaxVersion >= version)
                {
                    version = finalMaxVersion + 1;
                    boxDrawing.Version = version;
                    _logger.LogInformation($"🔍 VERSION DEBUG - Race condition! Updated to V{version}");
                }
            }

            await _unitOfWork.Repository<BoxDrawing>().AddAsync(boxDrawing, cancellationToken);
            _logger.LogInformation($"🔍 VERSION DEBUG - Saving V{boxDrawing.Version}");

            await _unitOfWork.CompleteAsync(cancellationToken);

            _logger.LogInformation($"✅ VERSION DEBUG - Successfully saved V{boxDrawing.Version}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Failed to save drawing");
            if (!string.IsNullOrEmpty(boxDrawing.DrawingFileName))
            {
                try
                {
                    await _blobStorageService.DeleteFileAsync(_containerName,boxDrawing.DrawingFileName);
                    _logger.LogInformation($"🗑️ Cleaned up Blob file: {boxDrawing.DrawingFileName}");
                }
                catch (Exception deleteEx)
                {
                    _logger.LogWarning(deleteEx, "⚠️ Failed to cleanup Blob file");
                }
            }

            return Result.Failure<BoxDrawingDto>($"Failed to save drawing: {ex.Message}");
        }
        string? createdByName = "Unknown";
        if (currentUserId.HasValue)
        {
            var user = await _unitOfWork.Repository<User>().GetByIdAsync(currentUserId.Value, cancellationToken);
            if (user != null)
            {
                createdByName = user.FullName ?? user.Email;
            }
        }

        var dto = boxDrawing.Adapt<BoxDrawingDto>() with 
        { 
            CreatedByName = createdByName 
        };
        return Result.Success(dto);
    }
}

