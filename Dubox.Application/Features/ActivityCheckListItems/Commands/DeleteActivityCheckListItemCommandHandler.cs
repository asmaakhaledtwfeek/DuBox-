using Dubox.Domain.Abstraction;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.ActivityCheckListItems.Commands;

public class DeleteActivityCheckListItemCommandHandler : IRequestHandler<DeleteActivityCheckListItemCommand, Result>
{
    private readonly IDbContext _context;

    public DeleteActivityCheckListItemCommandHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(DeleteActivityCheckListItemCommand request, CancellationToken cancellationToken)
    {
        var checklistItem = await _context.ActivityCheckListItems
            .FirstOrDefaultAsync(aci => aci.ActivityCheckListItemId == request.ActivityCheckListItemId, cancellationToken);

        if (checklistItem == null)
        {
            return Result.Failure(new Error(
                "ActivityCheckListItem.NotFound",
                "The specified ActivityCheckListItem does not exist."));
        }

        _context.ActivityCheckListItems.Remove(checklistItem);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
