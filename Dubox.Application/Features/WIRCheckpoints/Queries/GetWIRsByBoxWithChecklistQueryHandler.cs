using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Dubox.Application.Features.WIRCheckpoints.Queries;

public class GetWIRsByBoxWithChecklistQueryHandler : IRequestHandler<GetWIRsByBoxWithChecklistQuery, Result<List<WIRWithChecklistDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetWIRsByBoxWithChecklistQueryHandler> _logger;

    public GetWIRsByBoxWithChecklistQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetWIRsByBoxWithChecklistQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<List<WIRWithChecklistDto>>> Handle(GetWIRsByBoxWithChecklistQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Use Specification to get WIRs with ChecklistItems
            var spec = new GetWIRsWithChecklistByBoxIdSpecification(request.BoxId);
            var wirCheckpoints = await _unitOfWork.Repository<WIRCheckpoint>()
                .GetWithSpec(spec).Data
                .ToListAsync(cancellationToken);

            if (!wirCheckpoints.Any())
            {
                return Result.Failure<List<WIRWithChecklistDto>>($"No WIRs found for Box {request.BoxId}");
            }

            var result = new List<WIRWithChecklistDto>();

            foreach (var wir in wirCheckpoints)
            {
                // Get predefined items with categories to group checklist items
                var predefinedItemIds = wir.ChecklistItems
                    .Where(ci => ci.PredefinedItemId.HasValue)
                    .Select(ci => ci.PredefinedItemId.Value)
                    .ToList();

                if (predefinedItemIds.Any())
                {
                    var predefinedSpec = new GetPredefinedItemsByCategorySpecification(predefinedItemIds);
                    var predefinedItems = await _unitOfWork.Repository<PredefinedChecklistItem>()
                        .GetWithSpec(predefinedSpec).Data
                        .ToListAsync(cancellationToken);

                    // Group by category
                    var sections = wir.ChecklistItems
                        .OrderBy(ci => ci.Sequence)
                        .Select(ci =>
                        {
                            var predefined = predefinedItems.FirstOrDefault(p => p.PredefinedItemId == ci.PredefinedItemId);
                            return new
                            {
                                ChecklistItem = ci,
                                CategoryName = predefined?.Category?.CategoryName ?? "General",
                                ItemNumber = predefined?.ItemNumber ?? ci.Sequence.ToString()
                            };
                        })
                        .GroupBy(x => x.CategoryName)
                        .Select((g, index) => new ChecklistSectionDto
                        {
                            SectionLetter = GetSectionLetter(index),
                            SectionName = g.Key,
                            Items = g.Select(x => new ChecklistItemDetailDto
                            {
                                ChecklistItemId = x.ChecklistItem.ChecklistItemId,
                                ItemNumber = x.ItemNumber,
                                Description = x.ChecklistItem.CheckpointDescription,
                                ReferenceDocument = x.ChecklistItem.ReferenceDocument,
                                Status = x.ChecklistItem.Status.ToString(),
                                Remarks = x.ChecklistItem.Remarks,
                                Sequence = x.ChecklistItem.Sequence
                            }).ToList()
                        })
                        .ToList();

                    var totalItems = wir.ChecklistItems.Count;
                    var completedItems = wir.ChecklistItems.Count(ci =>
                        ci.Status == CheckListItemStatusEnum.Pass ||
                        ci.Status == CheckListItemStatusEnum.Fail);

                    result.Add(new WIRWithChecklistDto
                    {
                        WIRId = wir.WIRId,
                        WIRNumber = wir.WIRNumber,
                        WIRName = wir.WIRName ?? string.Empty,
                        WIRDescription = wir.WIRDescription,
                        Status = wir.Status.ToString(),
                        RequestedDate = wir.RequestedDate,
                        InspectionDate = wir.InspectionDate,
                        InspectorName = wir.InspectorName,
                        InspectorRole = wir.InspectorRole,
                        Comments = wir.Comments,
                        Sections = sections,
                        TotalItems = totalItems,
                        CompletedItems = completedItems,
                        ProgressPercentage = totalItems > 0 ? (int)Math.Round((double)completedItems / totalItems * 100) : 0
                    });
                }
            }

            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving WIRs with checklist for Box {BoxId}", request.BoxId);
            return Result.Failure<List<WIRWithChecklistDto>>($"Error retrieving WIRs: {ex.Message}");
        }
    }

    private static string GetSectionLetter(int index)
    {
        if (index < 26)
            return ((char)('A' + index)).ToString();
        return $"Section {index + 1}";
    }
}
