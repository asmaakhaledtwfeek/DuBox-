using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.ActivityCheckListItems.Queries;

public class GetActivityCheckListItemsByActivityMasterIdQueryHandler 
    : IRequestHandler<GetActivityCheckListItemsByActivityMasterIdQuery, Result<List<ActivityCheckListItemDetailsDto>>>
{
    private readonly IDbContext _context;

    public GetActivityCheckListItemsByActivityMasterIdQueryHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<ActivityCheckListItemDetailsDto>>> Handle(
        GetActivityCheckListItemsByActivityMasterIdQuery request, 
        CancellationToken cancellationToken)
    {
        var checklistItems = await _context.ActivityCheckListItems
            .Include(aci => aci.ActivityMaster)
            .Include(aci => aci.PredefinedChecklistItem)
            .Where(aci => aci.ActivityMasterId == request.ActivityMasterId)
            .OrderBy(aci => aci.Sequence)
            .Select(aci => new ActivityCheckListItemDetailsDto(
                aci.ActivityCheckListItemId,
                aci.ActivityMasterId,
                aci.ActivityMaster != null ? aci.ActivityMaster.ActivityName : null,
                aci.ActivityMaster != null ? aci.ActivityMaster.ActivityCode : null,
                aci.ActivityTemplateActivityId,
                null, // ActivityTemplateActivityName
                null, // ActivityTemplateActivityCode
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
