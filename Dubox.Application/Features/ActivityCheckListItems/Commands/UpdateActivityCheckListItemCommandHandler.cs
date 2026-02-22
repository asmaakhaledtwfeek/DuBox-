using Dubox.Domain.Abstraction;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.ActivityCheckListItems.Commands;

public class UpdateActivityCheckListItemCommandHandler : IRequestHandler<UpdateActivityCheckListItemCommand, Result>
{
    private readonly IDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public UpdateActivityCheckListItemCommandHandler(IDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Result> Handle(UpdateActivityCheckListItemCommand request, CancellationToken cancellationToken)
    {
        var checklistItem = await _context.ActivityCheckListItems
            .FirstOrDefaultAsync(aci => aci.ActivityCheckListItemId == request.ActivityCheckListItemId, cancellationToken);

        if (checklistItem == null)
        {
            return Result.Failure(new Error(
                "ActivityCheckListItem.NotFound",
                "The specified ActivityCheckListItem does not exist."));
        }

        checklistItem.Sequence = request.Sequence;
        checklistItem.IsMandatory = request.IsMandatory;
        checklistItem.IsActive = request.IsActive;
        checklistItem.ModifiedBy = _currentUserService.UserId;
        checklistItem.ModifiedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
