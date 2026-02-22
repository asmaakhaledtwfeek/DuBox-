using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.ActivityCheckListItems.Queries;

public class GetActivityCheckListItemsByActivityTemplateActivityIdQueryHandler 
    : IRequestHandler<GetActivityCheckListItemsByActivityTemplateActivityIdQuery, Result<List<ActivityCheckListItemDetailsDto>>>
{
    private readonly IDbContext _context;

    public GetActivityCheckListItemsByActivityTemplateActivityIdQueryHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<ActivityCheckListItemDetailsDto>>> Handle(
        GetActivityCheckListItemsByActivityTemplateActivityIdQuery request, 
        CancellationToken cancellationToken)
    {
        var checklistItems = await _context.ActivityCheckListItems
            .Include(aci => aci.ActivityTemplateActivity)
            .Include(aci => aci.PredefinedChecklistItem)
            .Where(aci => aci.ActivityTemplateActivityId == request.ActivityTemplateActivityId)
            .OrderBy(aci => aci.Sequence)
            .Select(aci => new ActivityCheckListItemDetailsDto(
                aci.ActivityCheckListItemId,
                aci.ActivityMasterId,
                null, // ActivityMasterName
                null, // ActivityMasterCode
                aci.ActivityTemplateActivityId,
                aci.ActivityTemplateActivity != null ? aci.ActivityTemplateActivity.ActivityName : null,
                aci.ActivityTemplateActivity != null ? aci.ActivityTemplateActivity.ActivityCode : null,
                aci.PredefinedChecklistItemId,
                aci.PredefinedChecklistItem.Description,
                aci.PredefinedChecklistItem.Reference,
                aci.Sequence,
                aci.IsMandatory,
                aci.IsActive,
                aci.CreatedDate,
                aci.CreatedBy,
                aci.ModifiedDate,
                aci.ModifiedBy
            ))
            .ToListAsync(cancellationToken);

        return Result.Success(checklistItems);
    }
}
