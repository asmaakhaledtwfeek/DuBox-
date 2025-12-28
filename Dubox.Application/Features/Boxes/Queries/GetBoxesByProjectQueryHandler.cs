using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Boxes.Queries;

public class GetBoxesByProjectQueryHandler : IRequestHandler<GetBoxesByProjectQuery, Result<List<BoxDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProjectTeamVisibilityService _visibilityService;

    public GetBoxesByProjectQueryHandler(IUnitOfWork unitOfWork, IProjectTeamVisibilityService visibilityService)
    {
        _unitOfWork = unitOfWork;
        _visibilityService = visibilityService;
    }

    public async Task<Result<List<BoxDto>>> Handle(GetBoxesByProjectQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Verify user has access to the requested project
            var canAccessProject = await _visibilityService.CanAccessProjectAsync(request.ProjectId, cancellationToken);
            if (!canAccessProject)
            {
                return Result.Failure<List<BoxDto>>("Access denied. You do not have permission to view boxes for this project.");
            }

            var boxes = _unitOfWork.Repository<Box>().GetWithSpec(new GetBoxesByProjectIdSpecification(request.ProjectId)).Data.ToList();

            var boxDtos = new List<BoxDto>();

            foreach (var box in boxes)
            {
                try
                {
                    var dto = MapBoxToDto(box);
                    boxDtos.Add(dto);
                }
                catch (Exception ex)
                {
                    // Log the error for this specific box
                    return Result.Failure<List<BoxDto>>($"Error mapping box {box.BoxId}: {ex.Message}. Inner exception: {ex.InnerException?.Message}. Stack trace: {ex.StackTrace}");
                }
            }

            return Result.Success(boxDtos);
        }
        catch (Exception ex)
        {
            return Result.Failure<List<BoxDto>>($"Error in GetBoxesByProjectQueryHandler: {ex.Message}. Inner exception: {ex.InnerException?.Message}. Stack trace: {ex.StackTrace}");
        }
    }

    private BoxDto MapBoxToDto(Box box)
    {
        try
        {
            // Safely get Project information
            var projectCode = box.Project?.ProjectCode ?? string.Empty;
            var client = box.Project?.ClientName ?? string.Empty;

            // Get BoxType and BoxSubType names from project configuration
            var boxTypeId = box.BoxTypeId;
            var boxSubTypeId = box.BoxSubTypeId;
            string boxType = string.Empty;
            string? boxSubTypeName = null;
            
            // Fetch BoxType name from ProjectBoxTypes
            if (boxTypeId.HasValue)
            {
                var projectBoxType = _unitOfWork.Repository<ProjectBoxType>()
                    .Get()
                    .FirstOrDefault(pbt => pbt.Id == boxTypeId.Value && pbt.ProjectId == box.ProjectId);
                boxType = projectBoxType?.TypeName ?? string.Empty;
            }
            
            // Fetch BoxSubType name from ProjectBoxSubTypes
            if (boxSubTypeId.HasValue)
            {
                var projectBoxSubType = _unitOfWork.Repository<ProjectBoxSubType>()
                    .Get()
                    .FirstOrDefault(pbst => pbst.Id == boxSubTypeId.Value);
                boxSubTypeName = projectBoxSubType?.SubTypeName;
            }

            // Get Zone - stored as ZoneCode string in database
            string? zoneString = null;
            if (!string.IsNullOrEmpty(box.Zone))
            {
                zoneString = box.Zone;
            }

            // Safely get Status (enum conversion)
            var statusString = box.Status.ToString();

            // Safely get UnitOfMeasure (enum conversion)
            string? unitOfMeasureString = null;
            if (box.UnitOfMeasure.HasValue)
            {
                unitOfMeasureString = box.UnitOfMeasure.Value.ToString();
            }

            // Safely get CurrentLocation information
            var currentLocationId = box.CurrentLocationId;
            var currentLocationCode = box.CurrentLocation?.LocationCode;
            var currentLocationName = box.CurrentLocation?.LocationName;

            // Safely get Factory information
            var factoryId = box.FactoryId;
            var factoryCode = box.Factory?.FactoryCode;
            var factoryName = box.Factory?.FactoryName;

            // Get ActivitiesCount
            var activitiesCount = box.BoxActivities?.Count ?? 0;

            return new BoxDto
            {
                BoxId = box.BoxId,
                ProjectId = box.ProjectId,
                ProjectCode = projectCode,
                Client = client,
                BoxTag = box.BoxTag ?? string.Empty,
                SerialNumber = box.SerialNumber,
                BoxName = box.BoxName,
                BoxType = boxType,
                BoxTypeId = boxTypeId,
                BoxSubTypeId = boxSubTypeId,
                BoxSubTypeName = boxSubTypeName,
                Floor = box.Floor,
                BuildingNumber = box.BuildingNumber,
                BoxFunction = box.BoxFunction,
                Zone = zoneString,
                QRCodeString = box.QRCodeString ?? string.Empty,
                QRCodeImage = box.QRCodeImageUrl,
                ProgressPercentage = box.ProgressPercentage,
                Status = statusString,
                Length = box.Length,
                Width = box.Width,
                Height = box.Height,
                UnitOfMeasure = unitOfMeasureString,
                RevitElementId = box.RevitElementId,
                Duration = box.Duration,
                PlannedStartDate = box.PlannedStartDate,
                ActualStartDate = box.ActualStartDate,
                PlannedEndDate = box.PlannedEndDate,
                ActualEndDate = box.ActualEndDate,
                CreatedDate = box.CreatedDate,
                ActivitiesCount = activitiesCount,
                Notes = box.Notes,
                CurrentLocationId = currentLocationId,
                CurrentLocationCode = currentLocationCode,
                CurrentLocationName = currentLocationName,
                FactoryId = factoryId,
                FactoryCode = factoryCode,
                FactoryName = factoryName,
                Bay = box.Bay,
                Row = box.Row,
                Position = box.Position
            };
        }
        catch (Exception ex)
        {
            throw new Exception($"Error mapping Box {box.BoxId} to BoxDto. Property causing error: {ex.Message}", ex);
        }
    }
}

