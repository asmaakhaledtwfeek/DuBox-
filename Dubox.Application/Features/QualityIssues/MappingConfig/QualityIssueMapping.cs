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
              

                .Map(dest => dest.BoxId, src => src.Box.BoxId)
                .Map(dest => dest.BoxName, src => src.Box.BoxName)
                .Map(dest => dest.BoxTag, src => src.Box.BoxTag)
                .Map(dest => dest.ProjectName, src => src.Box.Project.ProjectName)
                .Map(dest => dest.WIRId, src => src.WIRCheckpoint != null ? (Guid?)src.WIRCheckpoint.WIRId : null)
                
                .Map(dest => dest.WIRId, src => src.WIRCheckpoint != null ? (Guid?)src.WIRCheckpoint.WIRId : null)
                .Map(dest => dest.WIRNumber, src => src.WIRCheckpoint != null ? src.WIRCheckpoint.WIRCode : null)
                .Map(dest => dest.WIRName, src => src.WIRCheckpoint != null ? src.WIRCheckpoint.WIRName : null)
                .Map(dest => dest.WIRStatus, src => src.WIRCheckpoint != null ? (WIRCheckpointStatusEnum?)src.WIRCheckpoint.Status : null)
                .Map(dest => dest.WIRRequestedDate, src => src.WIRCheckpoint != null ? src.WIRCheckpoint.RequestedDate : null)
                .Map(dest => dest.AssignedTo, src => src.AssignedToTeamId != null ? src.AssignedToTeamId : null)
                .Map(dest => dest.AssignedTeamName, src => src.AssignedToTeam != null ? src.AssignedToTeam.TeamName : null)
                .Map(dest => dest.AssignedToUserId, src => src.AssignedToMemberId)
                .Map(dest => dest.AssignedToUserName, src => src.AssignedToMember != null ? src.AssignedToMember.EmployeeName : null)

                ;
        }
    }

}
