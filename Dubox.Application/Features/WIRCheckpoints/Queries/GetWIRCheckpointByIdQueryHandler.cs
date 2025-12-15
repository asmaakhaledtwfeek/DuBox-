using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.WIRCheckpoints.Queries
{
    public class GetWIRCheckpointByIdQueryHandler
    : IRequestHandler<GetWIRCheckpointByIdQuery, Result<WIRCheckpointDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProjectTeamVisibilityService _visibilityService;

        public GetWIRCheckpointByIdQueryHandler(IUnitOfWork unitOfWork, IProjectTeamVisibilityService visibilityService)
        {
            _unitOfWork = unitOfWork;
            _visibilityService = visibilityService;
        }

        public async Task<Result<WIRCheckpointDto>> Handle(GetWIRCheckpointByIdQuery request, CancellationToken cancellationToken)
        {

            var checkpoint = _unitOfWork.Repository<WIRCheckpoint>().
                GetEntityWithSpec(new GetWIRCheckpointByIdSpecification(request.WIRId));

            if (checkpoint == null)
                return Result.Failure<WIRCheckpointDto>("WIR checkpoint not found");

            // Verify user has access to the project this WIR checkpoint belongs to
            var canAccessProject = await _visibilityService.CanAccessProjectAsync(checkpoint.Box.ProjectId, cancellationToken);
            if (!canAccessProject)
            {
                return Result.Failure<WIRCheckpointDto>("Access denied. You do not have permission to view this WIR checkpoint.");
            }

            // Load category information for checklist items
            var predefinedItemIds = checkpoint.ChecklistItems
                .Where(ci => ci.PredefinedItemId.HasValue)
                .Select(ci => ci.PredefinedItemId.Value)
                .Distinct()
                .ToList();

            Dictionary<Guid, (Guid? CategoryId, string? CategoryName)> categoryMap = new();

            if (predefinedItemIds.Any())
            {
                var predefinedSpec = new GetPredefinedItemsByCategorySpecification(predefinedItemIds);
                var predefinedItems = await _unitOfWork.Repository<PredefinedChecklistItem>()
                    .GetWithSpec(predefinedSpec).Data
                    .ToListAsync(cancellationToken);


            }

            var dto = checkpoint.Adapt<WIRCheckpointDto>();

            // Enrich checklist items with category information
            if (dto.ChecklistItems != null)
            {
                foreach (var item in dto.ChecklistItems)
                {
                    if (item.PredefinedItemId.HasValue && categoryMap.TryGetValue(item.PredefinedItemId.Value, out var category))
                    {
                        item.CategoryId = category.CategoryId;
                        item.CategoryName = category.CategoryName;
                    }
                }
            }

            return Result.Success(dto);
        }
    }

}
