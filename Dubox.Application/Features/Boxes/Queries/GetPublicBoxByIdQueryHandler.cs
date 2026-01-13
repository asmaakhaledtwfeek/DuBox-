using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Boxes.Queries;

public class GetPublicBoxByIdQueryHandler : IRequestHandler<GetPublicBoxByIdQuery, Result<PublicBoxDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetPublicBoxByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PublicBoxDto>> Handle(GetPublicBoxByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var box = _unitOfWork.Repository<Box>()
                .GetEntityWithSpec(new GetBoxWithIncludesSpecification(request.BoxId));

            if (box == null)
                return Result.Failure<PublicBoxDto>("Box not found");

            // Check if box is active
            if (!box.IsActive)
                return Result.Failure<PublicBoxDto>("Box is no longer available");

            // Get BoxType and BoxSubType names from project configuration
            var boxTypeId = box.ProjectBoxTypeId;
            var boxSubTypeId = box.ProjectBoxSubTypeId;
            string boxType = string.Empty;
            string? boxSubTypeName = null;

            if (boxTypeId.HasValue)
            {
                var projectBoxType = _unitOfWork.Repository<ProjectBoxType>()
                    .Get()
                    .FirstOrDefault(pbt => pbt.Id == boxTypeId.Value && pbt.ProjectId == box.ProjectId);
                boxType = projectBoxType?.TypeName ?? string.Empty;
            }

            if (boxSubTypeId.HasValue)
            {
                var projectBoxSubType = _unitOfWork.Repository<ProjectBoxSubType>()
                    .Get()
                    .FirstOrDefault(pbst => pbst.Id == boxSubTypeId.Value);
                boxSubTypeName = projectBoxSubType?.SubTypeName;
            }

            var publicBoxDto = new PublicBoxDto
            {
                BoxId = box.BoxId,
                ProjectId = box.ProjectId,
                ProjectCode = box.Project?.ProjectCode ?? string.Empty,
                ProjectName = box.Project?.ProjectName ?? string.Empty,
                ClientName = box.Project?.ClientName ?? string.Empty,
                BoxTag = box.BoxTag ?? string.Empty,
                SerialNumber = box.SerialNumber,
                BoxName = box.BoxName,
                BoxType = boxType,
                BoxSubTypeName = boxSubTypeName,
                Floor = box.Floor,
                BuildingNumber = box.BuildingNumber,
                BoxFunction = box.BoxFunction,
                Zone = box.Zone,
                ProgressPercentage = box.ProgressPercentage,
                Status = box.Status.ToString(),
                Length = box.Length,
                Width = box.Width,
                Height = box.Height,
                UnitOfMeasure = box.UnitOfMeasure?.ToString(),
                PlannedStartDate = box.PlannedStartDate,
                ActualStartDate = box.ActualStartDate,
                PlannedEndDate = box.PlannedEndDate,
                ActualEndDate = box.ActualEndDate,
                ActivitiesCount = box.BoxActivities?.Count ?? 0,
                CurrentLocationName = box.CurrentLocation?.LocationName,
                FactoryName = box.Factory?.FactoryName,
                Bay = box.Bay,
                Row = box.Row,
                Position = box.Position
            };

            return Result.Success(publicBoxDto);
        }
        catch (Exception ex)
        {
            return Result.Failure<PublicBoxDto>($"Error retrieving box information: {ex.Message}");
        }
    }
}

