using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Schedule.Commands;

public class AssignMaterialCommandHandler : IRequestHandler<AssignMaterialCommand, Result<Guid>>
{
    private readonly IDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public AssignMaterialCommandHandler(IDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Result<Guid>> Handle(AssignMaterialCommand request, CancellationToken cancellationToken)
    {
        // Check if activity exists
        var activityExists = await _context.ScheduleActivities
            .AnyAsync(a => a.ScheduleActivityId == request.ScheduleActivityId, cancellationToken);

        if (!activityExists)
        {
            return Result.Failure<Guid>(new Error("ScheduleActivity.NotFound", "Schedule activity not found"));
        }

        var material = new ScheduleActivityMaterial
        {
            ScheduleActivityId = request.ScheduleActivityId,
            MaterialName = request.MaterialName,
            MaterialCode = request.MaterialCode,
            Quantity = request.Quantity,
            Unit = request.Unit,
            Notes = request.Notes,
            CreatedBy = _currentUserService.UserId,
            CreatedDate = DateTime.UtcNow
        };

        _context.ScheduleActivityMaterials.Add(material);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(material.ScheduleActivityMaterialId);
    }
}



