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

public class UpdateBoxDeliveryInfoCommandHandler : IRequestHandler<UpdateBoxDeliveryInfoCommand, Result<BoxDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IProjectTeamVisibilityService _visibilityService;

    public UpdateBoxDeliveryInfoCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        IProjectTeamVisibilityService visibilityService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _visibilityService = visibilityService;
    }

    public async Task<Result<BoxDto>> Handle(UpdateBoxDeliveryInfoCommand request, CancellationToken cancellationToken)
    {
        var module = PermissionModuleEnum.Boxes;
        var action = PermissionActionEnum.Edit;
        var canModify = await _visibilityService.CanPerformAsync(module, action, cancellationToken);
        if (!canModify)
            return Result.Failure<BoxDto>("Access denied. You do not have permission to update boxes.");

        var box = await _unitOfWork.Repository<Box>()
            .GetByIdAsync(request.BoxId, cancellationToken);

        if (box == null)
            return Result.Failure<BoxDto>("Box not found");

        var canAccessProject = await _visibilityService.CanAccessProjectAsync(box.ProjectId, cancellationToken);
        if (!canAccessProject)
            return Result.Failure<BoxDto>("Access denied. You do not have permission to update this box.");

        var projectStatusValidation = await _visibilityService.GetProjectStatusChecksAsync(box.ProjectId, "update boxes", cancellationToken);

        if (!projectStatusValidation.IsSuccess)
            return Result.Failure<BoxDto>(projectStatusValidation.Error!);

        var boxStatusValidation = await _visibilityService.GetBoxStatusChecksAsync(box.BoxId, "update a dispatched box", cancellationToken);

        if (!boxStatusValidation.IsSuccess)
            return Result.Failure<BoxDto>(boxStatusValidation.Error!);

        var changes = new Dictionary<string, (object? OldValue, object? NewValue)>();

        void RecordChange<T>(string propertyName, T? oldValue, T? newValue)
        {
            if (!EqualityComparer<T>.Default.Equals(oldValue, newValue))
            {
                changes.Add(propertyName, (oldValue, newValue));
            }
        }

        // Update concrete panel delivery fields - only update if explicitly provided (not null)
        if (request.Wall1.HasValue && request.Wall1 != box.Wall1)
        {
            RecordChange("Wall1", box.Wall1, request.Wall1);
            box.Wall1 = request.Wall1;
        }

        if (request.Wall2.HasValue && request.Wall2 != box.Wall2)
        {
            RecordChange("Wall2", box.Wall2, request.Wall2);
            box.Wall2 = request.Wall2;
        }

        if (request.Wall3.HasValue && request.Wall3 != box.Wall3)
        {
            RecordChange("Wall3", box.Wall3, request.Wall3);
            box.Wall3 = request.Wall3;
        }

        if (request.Wall4.HasValue && request.Wall4 != box.Wall4)
        {
            RecordChange("Wall4", box.Wall4, request.Wall4);
            box.Wall4 = request.Wall4;
        }

        if (request.Slab.HasValue && request.Slab != box.Slab)
        {
            RecordChange("Slab", box.Slab, request.Slab);
            box.Slab = request.Slab;
        }

        if (request.Soffit.HasValue && request.Soffit != box.Soffit)
        {
            RecordChange("Soffit", box.Soffit, request.Soffit);
            box.Soffit = request.Soffit;
        }

        // Update pod delivery fields - only update if explicitly provided (not null)
        if (request.PodDeliver.HasValue && request.PodDeliver != box.PodDeliver)
        {
            RecordChange("PodDeliver", box.PodDeliver, request.PodDeliver);
            box.PodDeliver = request.PodDeliver;
        }

        if (request.PodName != null && request.PodName != box.PodName)
        {
            RecordChange("PodName", box.PodName ?? "N/A", request.PodName ?? "N/A");
            box.PodName = request.PodName;
        }

        if (request.PodType != null && request.PodType != box.PodType)
        {
            RecordChange("PodType", box.PodType ?? "N/A", request.PodType ?? "N/A");
            box.PodType = request.PodType;
        }

        var currentUserId = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());
        box.ModifiedDate = DateTime.UtcNow;
        box.ModifiedBy = currentUserId;
        _unitOfWork.Repository<Box>().Update(box);

        if (changes.Any())
        {
            var oldValues = string.Join(" | ", changes.Select(c => $"{c.Key}: {c.Value.OldValue?.ToString() ?? "N/A"}"));
            var newValues = string.Join(" | ", changes.Select(c => $"{c.Key}: {c.Value.NewValue?.ToString() ?? "N/A"}"));

            var description = $"Box delivery information updated. ({changes.Count} properties changed).";

            var log = new AuditLog
            {
                TableName = nameof(Box),
                RecordId = box.BoxId,
                Action = "UpdateDeliveryInfo",
                OldValues = oldValues,
                NewValues = newValues,
                ChangedBy = currentUserId,
                ChangedDate = DateTime.UtcNow,
                Description = description
            };
            await _unitOfWork.Repository<AuditLog>().AddAsync(log, cancellationToken);
        }

        await _unitOfWork.CompleteAsync(cancellationToken);

        // Reload box with includes
        box = _unitOfWork.Repository<Box>().GetEntityWithSpec(new GetBoxWithIncludesSpecification(box.BoxId));
        
        var project = await _unitOfWork.Repository<Project>().GetByIdAsync(box.ProjectId, cancellationToken);

        BoxDto response = box.Adapt<BoxDto>() with
        {
            ProjectCode = project?.ProjectCode ?? string.Empty,
            FactoryId = box.FactoryId,
            FactoryCode = box.Factory?.FactoryCode,
            FactoryName = box.Factory?.FactoryName
        };

        return Result.Success(response);
    }
}

