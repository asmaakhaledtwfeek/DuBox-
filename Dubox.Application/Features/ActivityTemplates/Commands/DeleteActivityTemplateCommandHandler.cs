using Dubox.Domain.Abstraction;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.ActivityTemplates.Commands;

public class DeleteActivityTemplateCommandHandler : IRequestHandler<DeleteActivityTemplateCommand, Result<Unit>>
{
    private readonly IDbContext _context;

    public DeleteActivityTemplateCommandHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Unit>> Handle(DeleteActivityTemplateCommand request, CancellationToken cancellationToken)
    {
        var template = await _context.ActivityTemplates
            .Include(t => t.TemplateActivities)
            .FirstOrDefaultAsync(t => t.ActivityTemplateId == request.ActivityTemplateId, cancellationToken);

        if (template == null)
        {
            return Result.Failure<Unit>(new Error("ActivityTemplate.NotFound", "Activity template not found"));
        }

        _context.ActivityTemplates.Remove(template);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(Unit.Value);
    }
}
