using Dubox.Application.DTOs;
using Dubox.Application.Utilities;
using Dubox.Domain.Entities;
using Mapster;

namespace Dubox.Application.Features.Activities.MappingConfig
{
    internal class BoxActivityMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<BoxActivity, BoxActivityDto>()
                .Map(dest => dest.ActivityName, src => src.ActivityMaster.ActivityName)
                .Map(dest => dest.ActivityCode, src => src.ActivityMaster.ActivityCode)
                .Map(dest => dest.Stage, src => src.ActivityMaster.Stage)
                .Map(dest => dest.IsWIRCheckpoint, src => src.ActivityMaster.IsWIRCheckpoint)
                .Map(dest => dest.WIRCode, src => src.ActivityMaster.WIRCode)
                .Map(dest => dest.BoxTag, src => src.Box.BoxTag)
                .Map(dest => dest.TeamId, src => src.TeamId)
                .Map(dest => dest.TeamName, src => src.Team != null ? src.Team.TeamName : null)
                .Map(dest => dest.AssignedMemberId, src => src.AssignedMemberId)
                .Map(dest => dest.AssignedMemberName, src =>
                    src.AssignedMember != null
                        ? (!string.IsNullOrWhiteSpace(src.AssignedMember.EmployeeName)
                            ? src.AssignedMember.EmployeeName
                            : src.AssignedMember.User.FullName)
                        : null)
                .Map(dest => dest.Status, src => src.Status.ToString())
                // Legacy ActualDuration: calendar days + 1 (for backward compatibility)
                .Map(dest => dest.ActualDuration, src =>
                    DurationFormatter.CalculateDurationInDays(src.ActualStartDate, src.ActualEndDate))
                // New ActualDurationFormatted: flexible formatting (hours, days + hours)
                .Map(dest => dest.ActualDurationFormatted, src =>
                    DurationFormatter.FormatDuration(src.ActualStartDate, src.ActualEndDate));
        }
    }
}
