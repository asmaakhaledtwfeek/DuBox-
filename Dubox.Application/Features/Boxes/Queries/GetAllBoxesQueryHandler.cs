using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Boxes.Queries;

public class GetAllBoxesQueryHandler : IRequestHandler<GetAllBoxesQuery, Result<List<BoxDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IQRCodeService _qrCodeService;
    private readonly IProjectTeamVisibilityService _visibilityService;

    public GetAllBoxesQueryHandler(
        IUnitOfWork unitOfWork, 
        IQRCodeService qRCodeService,
        IProjectTeamVisibilityService visibilityService)
    {
        _unitOfWork = unitOfWork;
        _qrCodeService = qRCodeService;
        _visibilityService = visibilityService;
    }

    public async Task<Result<List<BoxDto>>> Handle(GetAllBoxesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Get accessible project IDs based on user role
            var accessibleProjectIds = await _visibilityService.GetAccessibleProjectIdsAsync(cancellationToken);
            
            // Use AsNoTracking and ToListAsync for better performance
            var boxes = await _unitOfWork.Repository<Box>()
                .GetWithSpec(new GetAllBoxesWithIncludesSpecification(accessibleProjectIds)).Data
                .AsNoTracking()
                .ToListAsync(cancellationToken);

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
                    // Log the error for this specific box but continue processing others
                    Console.WriteLine($"Warning: Error mapping box {box.BoxId}: {ex.Message}");
                    // Optionally, you could skip this box or add a partial DTO
                    // For now, we'll skip boxes that fail to map
                }
            }

            return Result.Success(boxDtos);
        }
        catch (Exception ex)
        {
            return Result.Failure<List<BoxDto>>($"Error in GetAllBoxesQueryHandler: {ex.Message}. Inner exception: {ex.InnerException?.Message}. Stack trace: {ex.StackTrace}");
        }
    }

    private BoxDto MapBoxToDto(Box box)
    {
        try
        {
            // Safely get Project information
            var projectCode = box.Project?.ProjectCode ?? string.Empty;
            var client = box.Project?.ClientName ?? string.Empty;

            // Safely get BoxType information
            var boxType = box.BoxType?.BoxTypeName ?? string.Empty;
            var boxTypeId = box.BoxTypeId;
            var boxSubTypeId = box.BoxSubTypeId;
            var boxSubTypeName = box.BoxSubType?.BoxSubTypeName;

            // Safely get Zone (enum conversion)
            string? zoneString = null;
            if (box.Zone.HasValue)
            {
                zoneString = box.Zone.Value.ToString();
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
                BIMModelReference = box.BIMModelReference,
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

