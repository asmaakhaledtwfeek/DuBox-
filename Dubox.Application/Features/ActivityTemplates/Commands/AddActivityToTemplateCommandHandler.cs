using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.ActivityTemplates.Commands;

public class AddActivityToTemplateCommandHandler : IRequestHandler<AddActivityToTemplateCommand, Result<Guid>>
{
    private readonly IDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public AddActivityToTemplateCommandHandler(IDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Result<Guid>> Handle(AddActivityToTemplateCommand request, CancellationToken cancellationToken)
    {
        var template = await _context.ActivityTemplates
            .FirstOrDefaultAsync(t => t.ActivityTemplateId == request.ActivityTemplateId, cancellationToken);

        if (template == null)
        {
            return Result.Failure<Guid>(new Error("ActivityTemplate.NotFound", "Activity template not found"));
        }

        // If using ActivityMaster as template, load it to copy properties
        ActivityMaster? activityMaster = null;
        if (request.SourceActivityMasterId.HasValue)
        {
            activityMaster = await _context.ActivityMasters
                .FirstOrDefaultAsync(am => am.ActivityMasterId == request.SourceActivityMasterId, cancellationToken);

            if (activityMaster == null)
            {
                return Result.Failure<Guid>(new Error("ActivityMaster.NotFound", "Activity master not found"));
            }
        }

        // Create activity (MASTER if from template, CUSTOM if created manually)
        // No FK constraint to ActivityMaster - stored for display/tracking only
        var templateActivity = new ActivityTemplateActivity
        {
            ActivityTemplateId = request.ActivityTemplateId,
            SourceActivityMasterId = request.SourceActivityMasterId,  // Store for MASTER badge display
            IsCustomActivity = request.IsCustomActivity,  // Store for CUSTOM badge display
            
            // Copy properties from ActivityMaster template OR use provided values
            ActivityCode = activityMaster?.ActivityCode ?? request.ActivityCode,
            ActivityName = activityMaster?.ActivityName ?? request.ActivityName,
            Stage = request.Stage,
            StageNumber = request.StageNumber,
            SequenceInStage = activityMaster?.SequenceInStage ?? request.SequenceInStage,
            OverallSequence = activityMaster?.OverallSequence ?? request.OverallSequence,
            Description = activityMaster?.Description ?? request.Description,
            EstimatedDurationDays = activityMaster?.EstimatedDurationDays ?? request.EstimatedDurationDays,
            IsWIRCheckpoint = activityMaster?.IsWIRCheckpoint ?? request.IsWIRCheckpoint,
            WIRCode = activityMaster?.WIRCode ?? request.WIRCode,
            ApplicableBoxTypes = activityMaster?.ApplicableBoxTypes ?? request.ApplicableBoxTypes,
            DependsOnActivities = activityMaster?.DependsOnActivities ?? request.DependsOnActivities,
            
            CreatedBy = _currentUserService.UserId,
            CreatedDate = DateTime.UtcNow
        };

        _context.ActivityTemplateActivities.Add(templateActivity);
        await _context.SaveChangesAsync(cancellationToken);

        // Create ActivityCheckListItem records if checklist items are selected
        if (request.SelectedChecklistItemIds != null && request.SelectedChecklistItemIds.Any())
        {
            var sequence = 1;
            foreach (var checklistItemId in request.SelectedChecklistItemIds)
            {
                var activityCheckListItem = new ActivityCheckListItem
                {
                    ActivityTemplateActivityId = templateActivity.ActivityTemplateActivityId,
                    PredefinedChecklistItemId = checklistItemId,
                    Sequence = sequence++,
                    IsMandatory = true,
                    IsActive = true,
                    CreatedBy = _currentUserService.UserId,
                    CreatedDate = DateTime.UtcNow
                };

                _context.ActivityCheckListItems.Add(activityCheckListItem);
            }

            await _context.SaveChangesAsync(cancellationToken);
        }

        return Result.Success(templateActivity.ActivityTemplateActivityId);
    }
}
