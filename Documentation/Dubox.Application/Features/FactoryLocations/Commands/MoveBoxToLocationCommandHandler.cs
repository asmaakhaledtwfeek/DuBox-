using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.FactoryLocations.Commands;

public class MoveBoxToLocationCommandHandler : IRequestHandler<MoveBoxToLocationCommand, Result<BoxLocationHistoryDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;

    public MoveBoxToLocationCommandHandler(
        IUnitOfWork unitOfWork,
        IDbContext dbContext,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
        _currentUserService = currentUserService;
    }

    public async Task<Result<BoxLocationHistoryDto>> Handle(MoveBoxToLocationCommand request, CancellationToken cancellationToken)
    {
        // Get the box
        var box = await _dbContext.Boxes
            .Include(b => b.Project)
            .FirstOrDefaultAsync(b => b.BoxId == request.BoxId, cancellationToken);

        if (box == null)
            return Result.Failure<BoxLocationHistoryDto>("Box not found");

        // Get the target location
        var targetLocation = await _unitOfWork.Repository<FactoryLocation>()
            .GetByIdAsync(request.ToLocationId, cancellationToken);

        if (targetLocation == null)
            return Result.Failure<BoxLocationHistoryDto>("Target location not found");

        if (!targetLocation.IsActive)
            return Result.Failure<BoxLocationHistoryDto>("Target location is not active");

        // Check capacity if set
        if (targetLocation.Capacity.HasValue && targetLocation.CurrentOccupancy >= targetLocation.Capacity.Value)
            return Result.Failure<BoxLocationHistoryDto>("Target location is at full capacity");

        // Get the previous location if box is currently in a location
        FactoryLocation? previousLocation = null;
        if (box.CurrentLocationId.HasValue)
        {
            previousLocation = await _unitOfWork.Repository<FactoryLocation>()
                .GetByIdAsync(box.CurrentLocationId.Value, cancellationToken);
        }
        var currentUserId = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());

        // Create location history entry
        var historyEntry = new BoxLocationHistory
        {
            BoxId = request.BoxId,
            LocationId = request.ToLocationId,
            MovedFromLocationId = box.CurrentLocationId,
            MovedDate = DateTime.UtcNow,
            Reason = request.Reason,
            MovedBy = currentUserId
        };

        await _unitOfWork.Repository<BoxLocationHistory>().AddAsync(historyEntry, cancellationToken);

        // Update box's current location
        box.CurrentLocationId = request.ToLocationId;
        box.ModifiedDate = DateTime.UtcNow;
        box.ModifiedBy = currentUserId;

        // Update location occupancies
        if (previousLocation != null)
        {
            previousLocation.CurrentOccupancy = Math.Max(0, previousLocation.CurrentOccupancy - 1);
        }

        targetLocation.CurrentOccupancy += 1;

        // Create audit log for location movement
        var userIdForAudit = currentUserId;
        var oldLocationInfo = previousLocation != null
            ? $"Location: {previousLocation.LocationCode} - {previousLocation.LocationName}"
            : "No location assigned";
        var newLocationInfo = $"Location: {targetLocation.LocationCode} - {targetLocation.LocationName}";
        var reasonInfo = !string.IsNullOrWhiteSpace(request.Reason) ? $" Reason: {request.Reason}" : "";

        var auditLog = new AuditLog
        {
            TableName = nameof(Box),
            RecordId = box.BoxId,
            Action = "LocationMove",
            OldValues = oldLocationInfo,
            NewValues = newLocationInfo,
            ChangedBy = userIdForAudit,
            ChangedDate = DateTime.UtcNow,
            Description = $"Box '{box.BoxTag}' moved from {(previousLocation != null ? $"{previousLocation.LocationCode} - {previousLocation.LocationName}" : "no location")} to {targetLocation.LocationCode} - {targetLocation.LocationName}.{reasonInfo}"
        };

        await _unitOfWork.Repository<AuditLog>().AddAsync(auditLog, cancellationToken);

        await _unitOfWork.CompleteAsync(cancellationToken);

        // Reload the history entry with navigation properties for DTO
        var historyWithIncludes = await _dbContext.BoxLocationHistory
            .Include(h => h.Location)
            .Include(h => h.MovedFromLocation)
            .Include(h => h.Box)
            .Include(h => h.MovedByUser)
            .FirstOrDefaultAsync(h => h.HistoryId == historyEntry.HistoryId, cancellationToken);

        if (historyWithIncludes == null)
            return Result.Failure<BoxLocationHistoryDto>("Failed to retrieve location history after creation");

        var dto = new BoxLocationHistoryDto
        {
            HistoryId = historyWithIncludes.HistoryId,
            BoxId = historyWithIncludes.BoxId,
            BoxTag = historyWithIncludes.Box.BoxTag,
            SerialNumber = historyWithIncludes.Box.SerialNumber,
            LocationId = historyWithIncludes.LocationId,
            LocationCode = historyWithIncludes.Location.LocationCode,
            LocationName = historyWithIncludes.Location.LocationName,
            MovedFromLocationId = historyWithIncludes.MovedFromLocationId,
            MovedFromLocationCode = historyWithIncludes.MovedFromLocation?.LocationCode,
            MovedFromLocationName = historyWithIncludes.MovedFromLocation?.LocationName,
            MovedDate = historyWithIncludes.MovedDate,
            Reason = historyWithIncludes.Reason,
            MovedBy = historyWithIncludes.MovedBy,
            MovedByUsername = historyWithIncludes.MovedByUser?.Email,
            MovedByFullName = historyWithIncludes.MovedByUser?.FullName
        };

        return Result.Success(dto);
    }
}

