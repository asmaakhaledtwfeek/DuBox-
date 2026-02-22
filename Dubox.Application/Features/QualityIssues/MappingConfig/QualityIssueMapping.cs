using Dubox.Application.DTOs;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Mapster;
namespace Dubox.Application.Features.QualityIssues.MappingConfig
{

    public class QualityIssueMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<QualityIssue, QualityIssueDetailsDto>()
                

                .Map(dest => dest.WIRId, src => src.WIRCheckpoint != null ? (Guid?)src.WIRCheckpoint.WIRId : null)
                
                .Map(dest => dest.WIRId, src => src.WIRCheckpoint != null ? (Guid?)src.WIRCheckpoint.WIRId : null)
                .Map(dest => dest.WIRNumber, src => src.WIRCheckpoint != null ? src.WIRCheckpoint.WIRCode : null)
                .Map(dest => dest.WIRName, src => src.WIRCheckpoint != null ? src.WIRCheckpoint.WIRName : null)
                .Map(dest => dest.WIRStatus, src => src.WIRCheckpoint != null ? (WIRCheckpointStatusEnum?)src.WIRCheckpoint.Status : null)
                .Map(dest => dest.WIRRequestedDate, src => src.WIRCheckpoint != null ? src.WIRCheckpoint.RequestedDate : null)
                .Map(dest => dest.AssignedTo, src => src.AssignedToTeamId != null ? src.AssignedToTeamId : null)
                .Map(dest => dest.AssignedTeamName, src => src.AssignedToTeam != null ? src.AssignedToTeam.TeamName : null)
                .Map(dest => dest.AssignedToUserId, src => src.AssignedToMember != null
                    ? (src.AssignedToMember.UserId ?? src.AssignedToMemberId)
                    : src.AssignedUserId)
                .Map(dest => dest.AssignedToUserName, src => src.AssignedToMember != null ? src.AssignedToMember.EmployeeName : null)
                .Map(dest => dest.CCUserId, src => src.CCUserId)
                .Map(dest => dest.CCUserName, src => src.CCUser != null ? src.CCUser.FullName : null)
                .Map(dest => dest.IsReadOnly, src => src.IsReadOnly)



                ;
        }
    }

}
