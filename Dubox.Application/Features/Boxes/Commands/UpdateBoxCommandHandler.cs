using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
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
    private readonly IProjectTeamVisibilityService _visibilityService;

    public UpdateBoxCommandHandler(
        IUnitOfWork unitOfWork, 
        IQRCodeService qrCodeService, 
        IDbContext dbContext, 
        IBoxActivityService boxActivityService, 
        ICurrentUserService currentUserService,
        IProjectTeamVisibilityService visibilityService)
    {
        _unitOfWork = unitOfWork;
        _qrCodeService = qrCodeService;
        _dbContext = dbContext;
        _boxActivityService = boxActivityService;
        _currentUserService = currentUserService;
        _visibilityService = visibilityService;
    }

    public async Task<Result<BoxDto>> Handle(UpdateBoxCommand request, CancellationToken cancellationToken)
    {
        var box = await _unitOfWork.Repository<Box>()
            .GetByIdAsync(request.BoxId, cancellationToken);

        if (box == null)
            return Result.Failure<BoxDto>("Box not found");

        // Check if user can edit the project this box belongs to
        // This checks: user created the project, or project was created by PM/SA who created a team user is in
        var canEditProject = await _visibilityService.CanEditProjectAsync(box.ProjectId, cancellationToken);
        if (!canEditProject)
        {
            return Result.Failure<BoxDto>("Access denied. You can only edit boxes in projects you created or projects created by Project Managers/System Admins who created teams you are a member of.");
        }

        // Check if project is archived
        var isArchived = await _visibilityService.IsProjectArchivedAsync(box.ProjectId, cancellationToken);
        if (isArchived)
        {
            return Result.Failure<BoxDto>("Cannot update boxes in an archived project. Archived projects are read-only.");
        }

        // Check if project is on hold
        var isOnHold = await _visibilityService.IsProjectOnHoldAsync(box.ProjectId, cancellationToken);
        if (isOnHold)
        {
            return Result.Failure<BoxDto>("Cannot update boxes in a project on hold. Projects on hold only allow status changes.");
        }

        // Check if project is closed
        var isClosed = await _visibilityService.IsProjectClosedAsync(box.ProjectId, cancellationToken);
        if (isClosed)
        {
            return Result.Failure<BoxDto>("Cannot update boxes in a closed project. Closed projects only allow project status changes.");
        }

        // Check if box is Dispatched - cannot perform any actions
        if (box.Status == BoxStatusEnum.Dispatched)
        {
            return Result.Failure<BoxDto>("Cannot update a dispatched box. Dispatched boxes are read-only and no actions are allowed.");
        }

        // Check if box is OnHold - cannot perform box actions (only status changes allowed)
        if (box.Status == BoxStatusEnum.OnHold)
        {
            return Result.Failure<BoxDto>("Cannot update a box that is on hold. Only status changes are allowed for boxes on hold.");
        }

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

        // Reload box with includes to get Factory info
        box = _unitOfWork.Repository<Box>().GetEntityWithSpec(new GetBoxByIdWithIncludesSpecification(box.BoxId));
        
        BoxDto response = box.Adapt<BoxDto>() with
        {
            ProjectCode = project.ProjectCode,
            FactoryId = box.FactoryId,
            FactoryCode = box.Factory?.FactoryCode,
            FactoryName = box.Factory?.FactoryName
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
        if ( request.BoxTypeId.HasValue && box.ProjectBoxTypeId != request.BoxTypeId)
        {
            if (box.ProjectBoxTypeId != request.BoxTypeId)
                boxTypeChanged = true;

            RecordChange("BoxTypeId", box.ProjectBoxTypeId, request.BoxTypeId);
            box.ProjectBoxTypeId = request.BoxTypeId.Value;
        }
        if (request.BoxSubTypeId.HasValue && box.ProjectBoxSubTypeId != request.BoxSubTypeId.Value)
        {
            RecordChange("BoxSubTypeId", box.ProjectBoxSubTypeId?.ToString() ?? "N/A", request.BoxSubTypeId.Value.ToString());
            box.ProjectBoxSubTypeId = request.BoxSubTypeId.Value;
        }

        if (!string.IsNullOrEmpty(request.Floor) && box.Floor != request.Floor)
        {
            RecordChange("Floor", box.Floor, request.Floor);
            box.Floor = request.Floor;
        }
        if (!string.IsNullOrEmpty(request.BuildingNumber) && box.BuildingNumber != request.BuildingNumber)
        {
            RecordChange("BuildingNumber", box.BuildingNumber, request.BuildingNumber);
            box.BuildingNumber = request.BuildingNumber;
        }
        if (!string.IsNullOrEmpty(request.BoxFunction) && box.BoxFunction != request.BoxFunction)
        {
            RecordChange("BoxFunction", box.BoxFunction, request.BoxFunction);
            box.BoxFunction = request.BoxFunction;
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
        
        // Factory is now automatically assigned based on project location
        // FactoryId updates from the UI are ignored
        // if (request.FactoryId.HasValue && box.FactoryId != request.FactoryId.Value)
        // {
        //     RecordChange("FactoryId", box.FactoryId?.ToString() ?? "N/A", request.FactoryId.Value.ToString());
        //     box.FactoryId = request.FactoryId.Value;
        // }
        // else if (!request.FactoryId.HasValue && box.FactoryId.HasValue)
        // {
        //     RecordChange("FactoryId", box.FactoryId.Value.ToString(), "N/A");
        //     box.FactoryId = null;
        // }

        var oldPlannedEndDate = box.PlannedEndDate;
        if (box.PlannedStartDate.HasValue && box.Duration.HasValue && box.Duration > 0)
            box.PlannedEndDate = box.PlannedStartDate.Value.AddDays(box.Duration.Value);

        if (oldPlannedEndDate != box.PlannedEndDate)
            RecordChange("PlannedEndDate", oldPlannedEndDate, box.PlannedEndDate);

        return boxTypeChanged;
    }
}

