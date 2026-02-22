using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Boxes.Queries
{
    public class GetBoxesForExchangeQueryHandler : IRequestHandler<GetBoxesForExchangeQuery, Result<List<BoxDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDbContext _dbContext;
        private readonly IProjectTeamVisibilityService _visibilityService;

        public GetBoxesForExchangeQueryHandler(
            IUnitOfWork unitOfWork,
            IDbContext dbContext,
            IProjectTeamVisibilityService visibilityService)
        {
            _unitOfWork = unitOfWork;
            _dbContext = dbContext;
            _visibilityService = visibilityService;
        }

        public async Task<Result<List<BoxDto>>> Handle(GetBoxesForExchangeQuery request, CancellationToken cancellationToken)
        {
            // Permission check
            var module = PermissionModuleEnum.Boxes;
            var action = PermissionActionEnum.View;
            var canView = await _visibilityService.CanPerformAsync(module, action, cancellationToken);
            if (!canView)
                return Result.Failure<List<BoxDto>>("Access denied. You do not have permission to view boxes.");

            // Verify user has access to the project
            var canAccessProject = await _visibilityService.CanAccessProjectAsync(request.ProjectId, cancellationToken);
            if (!canAccessProject)
                return Result.Failure<List<BoxDto>>("Access denied. You do not have permission to access this project.");

            // Get the current box
            var currentBox = await _unitOfWork.Repository<Box>().GetByIdAsync(request.BoxId);
            if (currentBox == null)
                return Result.Failure<List<BoxDto>>("Current box not found.");

            if (currentBox.ProjectId != request.ProjectId)
                return Result.Failure<List<BoxDto>>("Box does not belong to the specified project.");

            // Validate building and floor are provided
            if (string.IsNullOrWhiteSpace(request.BuildingNumber) && string.IsNullOrWhiteSpace(request.Floor))
                return Result.Failure<List<BoxDto>>("At least one of Building or Floor must be specified.");

            // Determine target building and floor
            var targetBuilding = request.BuildingNumber ?? currentBox.BuildingNumber;
            var targetFloor = request.Floor ?? currentBox.Floor;


            var boxes = _unitOfWork.Repository<Box>().GetWithSpec(new GetBoxesByProjectWithFiltersSpecification(request.ProjectId, currentBox, targetBuilding,targetFloor)).Data.ToList();

            if (boxes.Count == 0)
            {
                return Result.Failure<List<BoxDto>>("No eligible boxes found for exchange in the specified building and floor.");
            }

            // Map to DTOs
            var boxDtos = boxes.Select(box => new BoxDto
            {
                BoxId = box.BoxId,
                ProjectId = box.ProjectId,
                BoxTag = box.BoxTag,
                BoxName = box.BoxName,
                BoxType = box.BoxType?.TypeName ?? string.Empty,
                BoxSubTypeName = box.BoxSubType?.SubTypeName,
                BoxNumber = box.BoxNumber,
                BuildingNumber = box.BuildingNumber,
                Floor = box.Floor,
                Zone = box.Zone,
                BoxFunction = box.BoxFunction,
                Status = box.Status.ToString(),
                Length = box.Length,
                Width = box.Width,
                Height = box.Height,
                Notes = box.Notes,
                ProjectCode = string.Empty,
                Client = string.Empty,
                QRCodeString = string.Empty,
                ProgressPercentage = box.ProgressPercentage,
                CreatedDate = box.CreatedDate,
                ActivitiesCount = 0
            }).ToList();

            return Result.Success(boxDtos);
        }
    }
}
