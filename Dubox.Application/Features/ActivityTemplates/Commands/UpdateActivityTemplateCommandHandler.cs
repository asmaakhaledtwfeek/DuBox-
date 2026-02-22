using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.ActivityTemplates.Commands;

public class UpdateActivityTemplateCommandHandler : IRequestHandler<UpdateActivityTemplateCommand, Result<Unit>>
{
    private readonly IDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public UpdateActivityTemplateCommandHandler(IDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Result<Unit>> Handle(UpdateActivityTemplateCommand request, CancellationToken cancellationToken)
    {
        var template = await _context.ActivityTemplates
            .Include(t => t.TemplateActivities)
            .ThenInclude(ta => ta.ChecklistItems)
            .FirstOrDefaultAsync(t => t.ActivityTemplateId == request.ActivityTemplateId, cancellationToken);

        if (template == null)
        {
            return Result.Failure<Unit>(new Error("ActivityTemplate.NotFound", "Activity template not found"));
        }

        if (request.StageCount < 1)
        {
            return Result.Failure<Unit>(new Error(
                "ActivityTemplate.InvalidStageCount",
                "Stage count must be at least 1."));
        }

        // Update template metadata
        template.TemplateName = request.TemplateName;
        template.Description = request.Description;
        template.StageCount = request.StageCount;
        template.IsActive = request.IsActive;
        template.ModifiedBy = _currentUserService.UserId;
        template.ModifiedDate = DateTime.UtcNow;

        // If activities are provided, update them
        if (request.Activities != null)
        {
            // Remove existing activities and their checklist items
            var existingActivities = template.TemplateActivities.ToList();
            foreach (var activity in existingActivities)
            {
                // Remove associated checklist items
                var checklistItems = activity.ChecklistItems.ToList();
                foreach (var item in checklistItems)
                {
                    _context.ActivityCheckListItems.Remove(item);
                }
                
                _context.ActivityTemplateActivities.Remove(activity);
            }

            await _context.SaveChangesAsync(cancellationToken);

            // Add new activities
            var addedActivities = new List<ActivityTemplateActivity>();
            
            foreach (var activityDto in request.Activities)
            {
                var templateActivity = new ActivityTemplateActivity
                {
                    ActivityTemplateId = template.ActivityTemplateId,
                    SourceActivityMasterId = activityDto.SourceActivityMasterId,
                    IsCustomActivity = activityDto.IsCustomActivity,
                    ActivityCode = activityDto.ActivityCode,
                    ActivityName = activityDto.ActivityName,
                    Stage = activityDto.Stage,
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
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(Unit.Value);
    }
}
