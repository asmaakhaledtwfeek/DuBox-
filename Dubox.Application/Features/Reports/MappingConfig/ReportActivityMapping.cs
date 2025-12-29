using Dubox.Application.DTOs;
using Dubox.Application.Utilities;
using Dubox.Domain.Entities;
using Mapster;

namespace Dubox.Application.Features.Reports.MappingConfig;

/// <summary>
/// Mapster configuration for mapping BoxActivity to ReportActivityDto
/// </summary>
public class ReportActivityMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<BoxActivity, ReportActivityDto>()
            .Map(dest => dest.ActivityId, src => src.BoxActivityId)
            .Map(dest => dest.ActivityName, src => src.ActivityMaster != null ? src.ActivityMaster.ActivityName : string.Empty)
            .Map(dest => dest.BoxTag, src => src.Box != null ? src.Box.BoxTag : string.Empty)
            .Map(dest => dest.ProjectName, src => src.Box != null && src.Box.Project != null ? src.Box.Project.ProjectName : string.Empty)
            .Map(dest => dest.AssignedTeam, src => src.Team != null ? src.Team.TeamName : null)
            .Map(dest => dest.Status, src => src.Status.ToString())
            .Map(dest => dest.ProgressPercentage, src => src.ProgressPercentage)
            .Map(dest => dest.PlannedStartDate, src => src.PlannedStartDate)
            .Map(dest => dest.PlannedEndDate, src => src.PlannedEndDate)
            .Map(dest => dest.ActualStartDate, src => src.ActualStartDate)
            .Map(dest => dest.ActualEndDate, src => src.ActualEndDate)
            // Legacy ActualDuration: calendar days + 1 (for backward compatibility)
            .Map(dest => dest.ActualDuration, src => 
                DurationFormatter.CalculateDurationInDays(src.ActualStartDate, src.ActualEndDate))
            // New ActualDurationFormatted: flexible formatting (hours, days + hours)
            .Map(dest => dest.ActualDurationFormatted, src =>
                DurationFormatter.FormatDuration(src.ActualStartDate, src.ActualEndDate))
            .Map(dest => dest.DelayDays, src => 
                src.PlannedEndDate.HasValue && 
                !src.ActualEndDate.HasValue && 
                src.PlannedEndDate < DateTime.UtcNow
                    ? (int?)(DateTime.UtcNow.Date - src.PlannedEndDate.Value.Date).Days
                    : null)
            .Map(dest => dest.BoxId, src => src.BoxId)
            .Map(dest => dest.ProjectId, src => src.Box != null ? src.Box.ProjectId : Guid.Empty);
    }
}

