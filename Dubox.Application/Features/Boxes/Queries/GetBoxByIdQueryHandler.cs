using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Boxes.Queries;

public class GetBoxByIdQueryHandler : IRequestHandler<GetBoxByIdQuery, Result<BoxDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IQRCodeService _qrCodeService;
    private readonly IProjectTeamVisibilityService _visibilityService;

    public GetBoxByIdQueryHandler(
        IUnitOfWork unitOfWork,
        IQRCodeService qRCodeService,
        IProjectTeamVisibilityService visibilityService)
    {
        _unitOfWork = unitOfWork;
        _qrCodeService = qRCodeService;
        _visibilityService = visibilityService;
    }

    public async Task<Result<BoxDto>> Handle(GetBoxByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var box = _unitOfWork.Repository<Box>().GetEntityWithSpec(new GetBoxByIdWithIncludesSpecification(request.BoxId));

            if (box == null)
                return Result.Failure<BoxDto>("Box not found");

            var canAccessProject = await _visibilityService.CanAccessProjectAsync(box.ProjectId, cancellationToken);
            if (!canAccessProject)
            {
                return Result.Failure<BoxDto>("Access denied. You do not have permission to view this box.");
            }

            var boxDto = MapBoxToDto(box);

            return Result.Success(boxDto);
        }
        catch (Exception ex)
        {
            return Result.Failure<BoxDto>($"Error in GetBoxByIdQueryHandler: {ex.Message}. Inner exception: {ex.InnerException?.Message}. Stack trace: {ex.StackTrace}");
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

            string? zoneString = null;
            if (!string.IsNullOrEmpty(box.Zone))
                zoneString = box.Zone;

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

            // Generate QR Code Image
            string? qrCodeImage = null;
            try
            {
                qrCodeImage = _qrCodeService.GenerateQRCodeBase64(box.QRCodeString);
            }
            catch (Exception ex)
            {
                // Log but don't fail if QR code generation fails
                Console.WriteLine($"Warning: Failed to generate QR code for box {box.BoxId}: {ex.Message}");
            }

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
                QRCodeImage = qrCodeImage,
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

