using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.Boxes.Commands;

public class UpdateBoxCommandHandler : IRequestHandler<UpdateBoxCommand, Result<BoxDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IQRCodeService _qrCodeService;
    private readonly IDbContext _dbContext;
    private readonly IBoxActivityService _boxActivityService;
    private readonly ICurrentUserService _currentUserService;

    public UpdateBoxCommandHandler(IUnitOfWork unitOfWork, IQRCodeService qrCodeService, IDbContext dbContext, IBoxActivityService boxActivityService, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _qrCodeService = qrCodeService;
        _dbContext = dbContext;
        _boxActivityService = boxActivityService;
        _currentUserService = currentUserService;
    }

    public async Task<Result<BoxDto>> Handle(UpdateBoxCommand request, CancellationToken cancellationToken)
    {
        var box = await _unitOfWork.Repository<Box>()
            .GetByIdAsync(request.BoxId, cancellationToken);

        if (box == null)
            return Result.Failure<BoxDto>("Box not found");

        if (!string.IsNullOrEmpty(request.BoxTag) && box.BoxTag != request.BoxTag)
        {
            var tagExists = await _unitOfWork.Repository<Box>()
                .IsExistAsync(b => b.ProjectId == box.ProjectId && b.BoxTag == request.BoxTag && b.BoxId != request.BoxId, cancellationToken);

            if (tagExists)
                return Result.Failure<BoxDto>("Box tag already exists in the project");
        }

        var project = await _unitOfWork.Repository<Project>().GetByIdAsync(box.ProjectId, cancellationToken);
        if (project == null)
            return Result.Failure<BoxDto>("Project associated with the box not found.");

        DateTime? newBoxPlannedStartDate = request.PlannedStartDate ?? box.PlannedStartDate;
        int? newBoxDuration = request.Duration ?? box.Duration;
        DateTime? newBoxPlannedEndDate = null;

        if (newBoxPlannedStartDate.HasValue && newBoxDuration.HasValue && newBoxDuration.Value > 0)
            newBoxPlannedEndDate = newBoxPlannedStartDate.Value.AddDays(newBoxDuration.Value);

        if (!project.PlannedStartDate.HasValue || !project.PlannedEndDate.HasValue)
            return Result.Failure<BoxDto>("Cannot update box schedule: Project planned schedule is incomplete.");

        if (newBoxPlannedStartDate.HasValue && newBoxPlannedStartDate.Value < project.PlannedStartDate.Value)
            return Result.Failure<BoxDto>("Box planned start date cannot be before the Project planned start date.");

        if (newBoxPlannedEndDate.HasValue && newBoxPlannedEndDate.Value > project.PlannedEndDate.Value)
            return Result.Failure<BoxDto>("Box planned end date cannot be after the Project planned end date.");

        var boxTypeChange = ApplyBoxUpdates(box, request);

        box.QRCodeString = $"{project.ProjectCode}_{box.BoxTag}";

        var activitiesChanged = false;
        if (boxTypeChange)
        {
            activitiesChanged = true;
            var currentActivities = _unitOfWork.Repository<BoxActivity>().Get().Where(ba => ba.BoxId == request.BoxId).ToList();
            if (currentActivities.Any())
                _unitOfWork.Repository<BoxActivity>().DeleteRange(currentActivities);

            await _boxActivityService.CopyActivitiesToBox(box, cancellationToken);
        }
        var currentUserId = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());
        box.ModifiedDate = DateTime.UtcNow;
        box.ModifiedBy = currentUserId;
        _unitOfWork.Repository<Box>().Update(box);
        if (_changes.Any())
        {
            var oldValues = string.Join(" | ", _changes.Select(c => $"{c.Key}: {c.Value.OldValue?.ToString()}"));
            var newValues = string.Join(" | ", _changes.Select(c => $"{c.Key}: {c.Value.NewValue?.ToString()}"));

            var description = $"Box details updated. ({_changes.Count} properties changed).";
            if (activitiesChanged)
            {
                description += " Box activities were reset due to Box Type change.";
            }

            var log = new AuditLog
            {
                TableName = nameof(Box),
                RecordId = box.BoxId,
                Action = activitiesChanged ? "UpdateAndTypeChange" : "Update",
                OldValues = oldValues,
                NewValues = newValues,
                ChangedBy = currentUserId,
                ChangedDate = DateTime.UtcNow,
                Description = description
            };
            await _unitOfWork.Repository<AuditLog>().AddAsync(log, cancellationToken);
        }
        await _unitOfWork.CompleteAsync(cancellationToken);

        BoxDto response = box.Adapt<BoxDto>() with
        {
            ProjectCode = project.ProjectCode,
            QRCodeImage = _qrCodeService.GenerateQRCodeBase64(box.QRCodeString)
        };

        return Result.Success(response);
    }

    private Dictionary<string, (object OldValue, object NewValue)> _changes = new();
    private bool ApplyBoxUpdates(Box box, UpdateBoxCommand request)
    {
        bool boxTypeChanged = false;
        _changes.Clear();
        void RecordChange<T>(string propertyName, T oldValue, T newValue)
        {
            if (!EqualityComparer<T>.Default.Equals(oldValue, newValue))
            {
                _changes.Add(propertyName, (oldValue, newValue));
            }
        }
        if (!string.IsNullOrEmpty(request.BoxTag) && box.BoxTag != request.BoxTag)
        {
            RecordChange("BoxTag", box.BoxTag, request.BoxTag);
            box.BoxTag = request.BoxTag;
        };

        if (!string.IsNullOrEmpty(request.BoxName) && box.BoxName != request.BoxName)
        {
            RecordChange("BoxName", box.BoxName, request.BoxName);
            box.BoxName = request.BoxName;
        }
        if (!string.IsNullOrEmpty(request.BoxType) && box.BoxType != request.BoxType)
        {
            if (box.BoxType != request.BoxType)
                boxTypeChanged = true;

            RecordChange("BoxType", box.BoxType, request.BoxType);
            box.BoxType = request.BoxType;
        }

        if (!string.IsNullOrEmpty(request.Floor) && box.Floor != request.Floor)
        {
            RecordChange("Floor", box.Floor, request.Floor);
            box.Floor = request.Floor;
        }
        if (!string.IsNullOrEmpty(request.Building) && box.Building != request.Building)
        {
            RecordChange("Building", box.Building, request.Building);
            box.Building = request.Building;
        }
        if (!string.IsNullOrEmpty(request.Zone) && box.Zone != request.Zone)
        {
            RecordChange("Zone", box.Zone, request.Zone);
            box.Zone = request.Zone;
        }
        if (!string.IsNullOrEmpty(request.Notes) && box.Notes != request.Notes)
        {
            RecordChange("Notes", box.Notes, request.Notes);
            box.Notes = request.Notes;
        }

        if (request.Length.HasValue && box.Length != request.Length.Value)
        {
            RecordChange("Length", box.Length, request.Length.Value);
            box.Length = request.Length.Value;
        }

        if (request.Width.HasValue && box.Width != request.Width.Value)
        {
            RecordChange("Width", box.Width, request.Width.Value);
            box.Width = request.Width.Value;
        }

        if (request.Height.HasValue && box.Height != request.Height.Value)
        {
            RecordChange("Height", box.Height, request.Height.Value);
            box.Height = request.Height.Value;
        }
        if (request.PlannedStartDate.HasValue && box.PlannedStartDate != request.PlannedStartDate.Value)
        {
            RecordChange("PlannedStartDate", box.PlannedStartDate, request.PlannedStartDate.Value);
            box.PlannedStartDate = request.PlannedStartDate.Value;
        }
        if (request.Duration.HasValue && box.Duration != request.Duration.Value)
        {
            RecordChange("Duration", box.Duration, request.Duration.Value);
            box.Duration = request.Duration.Value;
        }

        var oldPlannedEndDate = box.PlannedEndDate;
        if (box.PlannedStartDate.HasValue && box.Duration.HasValue && box.Duration > 0)
            box.PlannedEndDate = box.PlannedStartDate.Value.AddDays(box.Duration.Value);

        if (oldPlannedEndDate != box.PlannedEndDate)
            RecordChange("PlannedEndDate", oldPlannedEndDate, box.PlannedEndDate);

        return boxTypeChanged;
    }
}

