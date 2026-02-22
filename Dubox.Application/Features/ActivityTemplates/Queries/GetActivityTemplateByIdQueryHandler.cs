using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.ActivityTemplates.Queries;

public class GetActivityTemplateByIdQueryHandler : IRequestHandler<GetActivityTemplateByIdQuery, Result<ActivityTemplateDetailsDto>>
{
    private readonly IDbContext _context;

    public GetActivityTemplateByIdQueryHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<Result<ActivityTemplateDetailsDto>> Handle(GetActivityTemplateByIdQuery request, CancellationToken cancellationToken)
    {
        var template = await _context.ActivityTemplates
            .Include(t => t.TemplateActivities)
                .ThenInclude(ta => ta.ChecklistItems)
            .FirstOrDefaultAsync(t => t.ActivityTemplateId == request.ActivityTemplateId, cancellationToken);

        if (template == null)
        {
            return Result.Failure<ActivityTemplateDetailsDto>(new Error("ActivityTemplate.NotFound", "Activity template not found"));
        }

        var activities = template.TemplateActivities.Select(a => new ActivityTemplateActivityDto(
            a.ActivityTemplateActivityId,
            a.ActivityTemplateId,
            a.SourceActivityMasterId,
            a.IsCustomActivity,
            a.ActivityCode,
            a.ActivityName,
            a.Stage,
            a.StageNumber,
            a.SequenceInStage,
            a.OverallSequence,
            a.Description,
            a.EstimatedDurationDays,
            a.IsWIRCheckpoint,
            a.WIRCode,
            a.ApplicableBoxTypes,
            a.DependsOnActivities,
            a.AssignedTeamId,
            a.ChecklistItems?.Select(ci => new ActivityChecklistItemDto(
                ci.PredefinedChecklistItemId,
                ci.Sequence,
                ci.IsMandatory
            )).ToList()
        )).ToList();

        var result = new ActivityTemplateDetailsDto(
            template.ActivityTemplateId,
            template.TemplateName,
            template.Description,
            template.StageCount,
            template.IsActive,
            activities,
            template.CreatedDate,
            template.CreatedBy,
            template.ModifiedDate,
            template.ModifiedBy
        );

        return Result.Success(result);
    }
}
