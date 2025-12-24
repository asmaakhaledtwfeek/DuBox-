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
        
        Console.WriteLine($"🔍 VERSION DEBUG - Uploading file: {fileName}");
        
        // Get existing drawings - use FindAsync to execute immediately and avoid race conditions
        var existingDrawings = await _unitOfWork.Repository<BoxDrawing>()
            .FindAsync(
                filter: d => d.BoxId == request.BoxId && d.IsActive,
                cancellationToken: cancellationToken
            );
        
        Console.WriteLine($"🔍 VERSION DEBUG - Found {existingDrawings.Count} existing drawings for this box");
        Console.WriteLine($"🔍 VERSION DEBUG - Existing files: {string.Join(", ", existingDrawings.Select(d => $"{d.OriginalFileName ?? d.DrawingUrl} (V{d.Version})"))}");
        
        // Find all drawings with the same filename (case-insensitive, in-memory comparison)
        // Note: This is done in-memory after loading, so we can use StringComparison.OrdinalIgnoreCase
        var fileNameLower = fileName.ToLower();
        var sameNameDrawings = existingDrawings
            .Where(d => 
                (d.OriginalFileName != null && d.OriginalFileName.ToLower() == fileNameLower) ||
                (d.DrawingUrl != null && d.DrawingUrl.ToLower() == fileNameLower)
            )
            .ToList();
        
        Console.WriteLine($"🔍 VERSION DEBUG - Found {sameNameDrawings.Count} drawings with same name '{fileName}'");
        
        if (sameNameDrawings.Any())
        {
            // Get the highest version number and increment
            var maxVersion = sameNameDrawings.Max(d => d.Version);
            version = maxVersion + 1;
            Console.WriteLine($"🔍 VERSION DEBUG - Max existing version: V{maxVersion}, New version will be: V{version}");
        }
        else
        {
            Console.WriteLine($"🔍 VERSION DEBUG - No existing files with same name, using V1");
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
                
                Console.WriteLine($"🔍 VERSION DEBUG - Setting OriginalFileName to: {request.FileName}");
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
            // Double-check version right before saving to handle race conditions
            // Load all active drawings for this box and compare in-memory
            var allDrawingsBeforeSave = await _unitOfWork.Repository<BoxDrawing>()
                .FindAsync(
                    filter: d => d.BoxId == request.BoxId && d.IsActive,
                    cancellationToken: cancellationToken
                );
            
            // Compare in-memory (case-insensitive)
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
                    Console.WriteLine($"🔍 VERSION DEBUG - Race condition detected! Updated version to V{version}");
                }
            }
            
            await _unitOfWork.Repository<BoxDrawing>().AddAsync(boxDrawing, cancellationToken);
            Console.WriteLine($"🔍 VERSION DEBUG - Saving drawing with Version: V{boxDrawing.Version}, FileName: {boxDrawing.OriginalFileName ?? boxDrawing.DrawingUrl}");
            await _unitOfWork.CompleteAsync(cancellationToken);
            Console.WriteLine($"✅ VERSION DEBUG - Successfully saved drawing with Version: V{boxDrawing.Version}");
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

