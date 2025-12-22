using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Interfaces;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.BoxDrawings.Commands;

public class UploadBoxDrawingCommandHandler : IRequestHandler<UploadBoxDrawingCommand, Result<BoxDrawingDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public UploadBoxDrawingCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<Result<BoxDrawingDto>> Handle(UploadBoxDrawingCommand request, CancellationToken cancellationToken)
    {
        // Validate that the box exists
        var box = await _unitOfWork.Repository<Box>().GetByIdAsync(request.BoxId, cancellationToken);
        if (box == null)
        {
            return Result.Failure<BoxDrawingDto>("Box not found.");
        }

        // Get current user ID
        Guid? currentUserId = null;
        if (Guid.TryParse(_currentUserService.UserId, out var userId))
        {
            currentUserId = userId;
        }

        // Determine version number: Check for existing files with same name
        int version = 1;
        string fileName = request.FileName ?? request.DrawingUrl ?? "unknown";
        
        var existingDrawings = _unitOfWork.Repository<BoxDrawing>()
            .Get()
            .Where(d => d.BoxId == request.BoxId && d.IsActive)
            .ToList();
        
        // Find all drawings with the same filename
        var sameNameDrawings = existingDrawings
            .Where(d => 
                (d.OriginalFileName != null && d.OriginalFileName.Equals(fileName, StringComparison.OrdinalIgnoreCase)) ||
                (d.DrawingUrl != null && d.DrawingUrl.Equals(fileName, StringComparison.OrdinalIgnoreCase))
            )
            .ToList();
        
        if (sameNameDrawings.Any())
        {
            // Get the highest version number and increment
            version = sameNameDrawings.Max(d => d.Version) + 1;
        }

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

        // Process file if provided
        if (request.File != null && request.File.Length > 0)
        {
            try
            {
                // Convert file to base64
                var base64String = Convert.ToBase64String(request.File);
                
                // Determine file type based on extension
                var extension = Path.GetExtension(request.FileName ?? "").ToLowerInvariant();
                string mimeType = extension switch
                {
                    ".pdf" => "application/pdf",
                    ".dwg" => "application/acad",
                    _ => "application/octet-stream"
                };

                // Store as data URL
                boxDrawing.FileData = $"data:{mimeType};base64,{base64String}";
                boxDrawing.OriginalFileName = request.FileName;
                boxDrawing.FileExtension = extension;
                boxDrawing.FileType = "file";
                boxDrawing.FileSize = request.File.Length;
            }
            catch (Exception ex)
            {
                return Result.Failure<BoxDrawingDto>($"Error processing file: {ex.Message}");
            }
        }
        else if (!string.IsNullOrWhiteSpace(request.DrawingUrl))
        {
            boxDrawing.FileType = "url";
        }

        // Save to database
        try
        {
            await _unitOfWork.Repository<BoxDrawing>().AddAsync(boxDrawing, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return Result.Failure<BoxDrawingDto>($"Failed to save drawing: {ex.Message}");
        }

        // Get user name for the response
        string? createdByName = "Unknown";
        if (currentUserId.HasValue)
        {
            var user = await _unitOfWork.Repository<User>().GetByIdAsync(currentUserId.Value, cancellationToken);
            if (user != null)
            {
                createdByName = user.FullName ?? user.Email;
            }
        }

        // Map to DTO and return
        var dto = boxDrawing.Adapt<BoxDrawingDto>() with 
        { 
            CreatedByName = createdByName 
        };
        return Result.Success(dto);
    }
}

