using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.ActivityTemplates.Commands;

public class CreateActivityTemplateFromMasterCommandHandler : IRequestHandler<CreateActivityTemplateFromMasterCommand, Result<Guid>>
{
    private readonly IDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CreateActivityTemplateFromMasterCommandHandler(IDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Result<Guid>> Handle(CreateActivityTemplateFromMasterCommand request, CancellationToken cancellationToken)
    {
        // Get the selected activities from ActivityMaster
        var activityMasters = await _context.ActivityMasters
            .Where(am => request.ActivityMasterIds.Contains(am.ActivityMasterId))
            .ToListAsync(cancellationToken);

        if (activityMasters.Count != request.ActivityMasterIds.Count)
        {
            return Result.Failure<Guid>(new Error("ActivityMaster.NotFound", "One or more selected activities not found"));
        }

        if (request.StageCount < 1)
        {
            return Result.Failure<Guid>(new Error(
                "ActivityTemplate.InvalidStageCount",
                "Stage count must be at least 1."));
        }

        // Create the template
        var template = new ActivityTemplate
        {
            TemplateName = request.TemplateName,
            Description = request.Description,
            StageCount = request.StageCount,
            IsActive = true,
            CreatedBy = _currentUserService.UserId,
            CreatedDate = DateTime.UtcNow
        };

        _context.ActivityTemplates.Add(template);

        // Create MASTER activities from ActivityMaster templates (no FK relationship, stored for display)
        foreach (var activityMaster in activityMasters)
        {
            var templateActivity = new ActivityTemplateActivity
            {
                ActivityTemplateId = template.ActivityTemplateId,
                SourceActivityMasterId = activityMaster.ActivityMasterId,  // Store for MASTER badge display
                IsCustomActivity = false,  // Mark as MASTER (not custom)
                
                // Copy all properties from ActivityMaster template
                ActivityCode = activityMaster.ActivityCode,
                ActivityName = activityMaster.ActivityName,
                Stage = activityMaster.Stage,
                StageNumber = activityMaster.StageNumber,
                SequenceInStage = activityMaster.SequenceInStage,
                OverallSequence = activityMaster.OverallSequence,
                Description = activityMaster.Description,
                EstimatedDurationDays = activityMaster.EstimatedDurationDays,
                IsWIRCheckpoint = activityMaster.IsWIRCheckpoint,
                WIRCode = activityMaster.WIRCode,
                ApplicableBoxTypes = activityMaster.ApplicableBoxTypes,
                DependsOnActivities = activityMaster.DependsOnActivities,
                
                CreatedBy = _currentUserService.UserId,
                CreatedDate = DateTime.UtcNow
            };

            _context.ActivityTemplateActivities.Add(templateActivity);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(template.ActivityTemplateId);
    }
}
