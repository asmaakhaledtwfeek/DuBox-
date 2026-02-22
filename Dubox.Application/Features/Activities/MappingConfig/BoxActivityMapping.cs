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
                // Map source IDs for traceability
                .Map(dest => dest.ActivityMasterId, src => src.ActivityMasterId)
                .Map(dest => dest.ActivityTemplateActivityId, src => src.ActivityTemplateActivityId)
                
                // Map from ActivityMaster if available, otherwise from ActivityTemplateActivity
                .Map(dest => dest.ActivityName, src => 
                    src.ActivityMaster != null 
                        ? src.ActivityMaster.ActivityName 
                        : (src.ActivityTemplateActivity != null ? src.ActivityTemplateActivity.ActivityName : null))
                .Map(dest => dest.ActivityCode, src => 
                    src.ActivityMaster != null 
                        ? src.ActivityMaster.ActivityCode 
                        : (src.ActivityTemplateActivity != null ? src.ActivityTemplateActivity.ActivityCode : null))
                .Map(dest => dest.Stage, src => 
                    src.ActivityMaster != null 
                        ? src.ActivityMaster.Stage 
                        : (src.ActivityTemplateActivity != null ? src.ActivityTemplateActivity.Stage : null))
                .Map(dest => dest.IsWIRCheckpoint, src => 
                    src.ActivityMaster != null 
                        ? src.ActivityMaster.IsWIRCheckpoint 
                        : (src.ActivityTemplateActivity != null ? src.ActivityTemplateActivity.IsWIRCheckpoint : false))
                .Map(dest => dest.WIRCode, src => 
                    src.ActivityMaster != null 
                        ? src.ActivityMaster.WIRCode 
                        : (src.ActivityTemplateActivity != null ? src.ActivityTemplateActivity.WIRCode : null))
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
                .Map(dest => dest.ActualDuration, src =>
                    DurationFormatter.CalculateDurationInDays(src.ActualStartDate, src.ActualEndDate))
                .Map(dest => dest.ActualDurationFormatted, src =>
                    DurationFormatter.FormatDuration(src.ActualStartDate, src.ActualEndDate));
        }
    }
}
