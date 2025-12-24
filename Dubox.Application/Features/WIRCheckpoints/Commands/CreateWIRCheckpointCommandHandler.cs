using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;


namespace Dubox.Application.Features.WIRCheckpoints.Commands
{
    public class CreateWIRCheckpointCommandHandler : IRequestHandler<CreateWIRCheckpointCommand, Result<CreateWIRCheckpointDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IProjectTeamVisibilityService _visibilityService;
        private readonly IImageProcessingService _imageProcessingService;

        public CreateWIRCheckpointCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IProjectTeamVisibilityService visibilityService,
            IImageProcessingService imageProcessingService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _visibilityService = visibilityService;
            _imageProcessingService = imageProcessingService;
        }

        public async Task<Result<CreateWIRCheckpointDto>> Handle(CreateWIRCheckpointCommand request, CancellationToken cancellationToken)
        {
            // Check if user can modify data (Viewer role cannot)
            var canModify = await _visibilityService.CanModifyDataAsync(cancellationToken);
            if (!canModify)
            {
                return Result.Failure<CreateWIRCheckpointDto>("Access denied. Viewer role has read-only access and cannot create WIR checkpoints.");
            }

            var boxActicity = await _unitOfWork.Repository<BoxActivity>().GetByIdAsync(request.BoxActivityId);
            if (boxActicity == null)
                return Result.Failure<CreateWIRCheckpointDto>("Box Activity not fount");

            // Load box to get project ID for authorization check
            var box = await _unitOfWork.Repository<Box>().GetByIdAsync(boxActicity.BoxId, cancellationToken);
            if (box == null)
                return Result.Failure<CreateWIRCheckpointDto>("Box not found");

            // Verify user has access to the project
            var canAccessProject = await _visibilityService.CanAccessProjectAsync(box.ProjectId, cancellationToken);
            if (!canAccessProject)
            {
                return Result.Failure<CreateWIRCheckpointDto>("Access denied. You do not have permission to create WIR checkpoints for this project.");
            }

            var currentUserId = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());
            var user = await _unitOfWork.Repository<User>().GetByIdAsync(currentUserId);
            var currentUserName = user != null ? user.FullName : string.Empty;

            var existCheckpoint = _unitOfWork.Repository<WIRCheckpoint>().Get().Where(c => c.BoxId == boxActicity.BoxId && c.WIRCode == request.WIRNumber).FirstOrDefault();

            if (existCheckpoint != null)
            {
                var existCheckpointId = existCheckpoint.WIRId;
                var oldStatus = existCheckpoint.Status.ToString();
                var oldWIRName = existCheckpoint.WIRName ?? "N/A";
                var oldWIRDescription = existCheckpoint.WIRDescription ?? "N/A";

               //existCheckpoint.WIRId = existCheckpointId;
                existCheckpoint.RequestedBy = currentUserName;
                existCheckpoint.BoxId = boxActicity.BoxId;
                existCheckpoint.WIRCode = request.WIRNumber;
                existCheckpoint.CreatedBy = currentUserId;
                
                _unitOfWork.Repository<WIRCheckpoint>().Update(existCheckpoint);
                await _unitOfWork.CompleteAsync(cancellationToken);

                // Process images - save to WIRCheckpointImage table (same logic as ReviewWIRCheckPoint)
                if (request.Files != null && request.Files.Any() || request.ImageUrls != null && request.ImageUrls.Any())
                {
                    int sequence = 0;
                    var existingImages = await _unitOfWork.Repository<WIRCheckpointImage>()
                        .FindAsync(img => img.WIRId == existCheckpoint.WIRId, cancellationToken);
                    if (existingImages.Any())
                    {
                        sequence = existingImages.Max(img => img.Sequence) + 1;
                    }

                    (bool, string) imagesProcessResult = await _imageProcessingService.ProcessImagesAsync<WIRCheckpointImage>(
                        existCheckpoint.WIRId, 
                        request.Files, 
                        request.ImageUrls, 
                        cancellationToken, 
                        sequence,
                        fileNames: request.FileNames);
                    
                    if (!imagesProcessResult.Item1)
                    {
                        return Result.Failure<CreateWIRCheckpointDto>(imagesProcessResult.Item2);
                    }

                    try
                    {
                        await _unitOfWork.CompleteAsync(cancellationToken);
                    }
                    catch (Microsoft.EntityFrameworkCore.DbUpdateException dbEx)
                    {
                        return Result.Failure<CreateWIRCheckpointDto>($"Database error saving images: {dbEx.Message}. Inner exception: {dbEx.InnerException?.Message}.");
                    }
                    catch (Exception ex)
                    {
                        return Result.Failure<CreateWIRCheckpointDto>($"Error saving images: {ex.Message}. Inner exception: {ex.InnerException?.Message}");
                    }
                }
                
                // Create audit log for update
                var auditLog = new AuditLog
                {
                    TableName = nameof(WIRCheckpoint),
                    RecordId = existCheckpoint.WIRId,
                    Action = "UPDATE",
                    OldValues = $"Status: {oldStatus}, WIRName: {oldWIRName}, WIRDescription: {oldWIRDescription}",
                    NewValues = $"Status: {existCheckpoint.Status}, WIRName: {existCheckpoint.WIRName ?? "N/A"}, WIRDescription: {existCheckpoint.WIRDescription ?? "N/A"}",
                    ChangedBy = currentUserId,
                    ChangedDate = DateTime.UtcNow,
                    Description = $"WIR Checkpoint {request.WIRNumber} updated."
                };
                await _unitOfWork.Repository<AuditLog>().AddAsync(auditLog, cancellationToken);
                await _unitOfWork.CompleteAsync(cancellationToken);
                
                var existDto = existCheckpoint.Adapt<CreateWIRCheckpointDto>();
                existDto.WIRNumber = existCheckpoint.WIRCode;
                return Result.Success(existDto);
            }
            
                var checkpoint = request.Adapt<WIRCheckpoint>();
            checkpoint.WIRCode = request.WIRNumber;
            checkpoint.BoxId = boxActicity.BoxId;
            checkpoint.Status = WIRCheckpointStatusEnum.Pending;
            checkpoint.CreatedDate = DateTime.UtcNow;
            checkpoint.RequestedDate = DateTime.UtcNow;
            checkpoint.RequestedBy = currentUserName;
            checkpoint.CreatedBy = currentUserId;
            await _unitOfWork.Repository<WIRCheckpoint>().AddAsync(checkpoint);
            await _unitOfWork.CompleteAsync(cancellationToken);

            // Process images - save to WIRCheckpointImage table (same logic as ReviewWIRCheckPoint)
            if (request.Files != null && request.Files.Any() || request.ImageUrls != null && request.ImageUrls.Any())
            {
                int sequence = 0;
                var existingImages = await _unitOfWork.Repository<WIRCheckpointImage>()
                    .FindAsync(img => img.WIRId == checkpoint.WIRId, cancellationToken);
                if (existingImages.Any())
                {
                    sequence = existingImages.Max(img => img.Sequence) + 1;
                }

                (bool, string) imagesProcessResult = await _imageProcessingService.ProcessImagesAsync<WIRCheckpointImage>(
                    checkpoint.WIRId, 
                    request.Files, 
                    request.ImageUrls, 
                    cancellationToken, 
                    sequence,
                    fileNames: request.FileNames);
                
                if (!imagesProcessResult.Item1)
                {
                    return Result.Failure<CreateWIRCheckpointDto>(imagesProcessResult.Item2);
                }

                try
                {
                    await _unitOfWork.CompleteAsync(cancellationToken);
                }
                catch (Microsoft.EntityFrameworkCore.DbUpdateException dbEx)
                {
                    return Result.Failure<CreateWIRCheckpointDto>($"Database error saving images: {dbEx.Message}. Inner exception: {dbEx.InnerException?.Message}.");
                }
                catch (Exception ex)
                {
                    return Result.Failure<CreateWIRCheckpointDto>($"Error saving images: {ex.Message}. Inner exception: {ex.InnerException?.Message}");
                }
            }
            
            // Create audit log for creation
            var createAuditLog = new AuditLog
            {
                TableName = nameof(WIRCheckpoint),
                RecordId = checkpoint.WIRId,
                Action = "INSERT",
                OldValues = null,
                NewValues = $"WIRCode: {checkpoint.WIRCode}, Status: {checkpoint.Status}, WIRName: {checkpoint.WIRName ?? "N/A"}, WIRDescription: {checkpoint.WIRDescription ?? "N/A"}",
                ChangedBy = currentUserId,
                ChangedDate = DateTime.UtcNow,
                Description = $"WIR Checkpoint {checkpoint.WIRCode} created for Box {box.BoxTag ?? box.BoxName}."
            };
            await _unitOfWork.Repository<AuditLog>().AddAsync(createAuditLog, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);
            
            var dto = checkpoint.Adapt<CreateWIRCheckpointDto>();
            dto.WIRNumber = checkpoint.WIRCode;
            return Result.Success(dto);
        }
    }
}
