using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.ActivityTemplates.Commands;

public class CreateActivityTemplateCommandHandler : IRequestHandler<CreateActivityTemplateCommand, Result<Guid>>
{
    private readonly IDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CreateActivityTemplateCommandHandler(IDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Result<Guid>> Handle(CreateActivityTemplateCommand request, CancellationToken cancellationToken)
    {
        // Validate: Check for duplicate OverallSequence values
        var sequenceGroups = request.Activities
            .GroupBy(a => a.OverallSequence)
            .Where(g => g.Count() > 1)
            .ToList();

        if (sequenceGroups.Any())
        {
            var duplicateSequences = string.Join(", ", sequenceGroups.Select(g => g.Key));
            return Result.Failure<Guid>(new Error(
                "ActivityTemplate.DuplicateSequence",
                $"Duplicate sequence numbers found: {duplicateSequences}. Each activity must have a unique OverallSequence value."));
        }

        // Validate stage count
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

        // Add custom activities to the template (normalize stage to "Stage 01", "Stage 02" format)
        var addedActivities = new List<ActivityTemplateActivity>();
        foreach (var activityDto in request.Activities)
        {
            var stageLabel = $"Stage {activityDto.StageNumber:D2}";
            var templateActivity = new ActivityTemplateActivity
            {
                ActivityTemplateId = template.ActivityTemplateId,
                SourceActivityMasterId = activityDto.SourceActivityMasterId,  // Store for MASTER badge display
                IsCustomActivity = activityDto.IsCustomActivity,  // Store for CUSTOM badge display
                ActivityCode = activityDto.ActivityCode,
                ActivityName = activityDto.ActivityName,
                Stage = stageLabel,
                StageNumber = activityDto.StageNumber,
                SequenceInStage = activityDto.SequenceInStage,
                OverallSequence = activityDto.OverallSequence,
                Description = activityDto.Description,
                EstimatedDurationDays = activityDto.EstimatedDurationDays,
                IsWIRCheckpoint = activityDto.IsWIRCheckpoint,
                WIRCode = activityDto.WIRCode,
                ApplicableBoxTypes = activityDto.ApplicableBoxTypes,
                DependsOnActivities = activityDto.DependsOnActivities,
                AssignedTeamId = activityDto.AssignedTeamId,
                CreatedBy = _currentUserService.UserId,
                CreatedDate = DateTime.UtcNow
            };

            _context.ActivityTemplateActivities.Add(templateActivity);
            addedActivities.Add(templateActivity);
        }

        // Save to get the ActivityTemplateActivityId
        await _context.SaveChangesAsync(cancellationToken);

        // Add checklist items for each activity
        foreach (var activityDto in request.Activities)
        {
            if (activityDto.SelectedChecklistItems != null && activityDto.SelectedChecklistItems.Any())
            {
                var activity = addedActivities.FirstOrDefault(a => 
                    a.ActivityCode == activityDto.ActivityCode && 
                    a.StageNumber == activityDto.StageNumber);

                if (activity != null)
                {
                    foreach (var checklistItem in activityDto.SelectedChecklistItems)
                    {
                        var activityChecklistItem = new ActivityCheckListItem
                        {
                            ActivityTemplateActivityId = activity.ActivityTemplateActivityId,
                            PredefinedChecklistItemId = checklistItem.PredefinedChecklistItemId,
                            Sequence = checklistItem.Sequence,
                            IsMandatory = checklistItem.IsMandatory,
                            IsActive = true,
                            CreatedBy = _currentUserService.UserId,
                            CreatedDate = DateTime.UtcNow
                        };

                        _context.ActivityCheckListItems.Add(activityChecklistItem);
                    }
                }
            }
        }

        // Save checklist items
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(template.ActivityTemplateId);
    }
}
