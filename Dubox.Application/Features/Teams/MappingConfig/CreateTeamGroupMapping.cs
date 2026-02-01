using Dubox.Application.DTOs;
using Dubox.Domain.Entities;
using Mapster;

namespace Dubox.Application.Features.Teams.MappingConfig;

internal class CreateTeamGroupMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<TeamGroup, TeamGroupDto>()
             .Map(dest => dest.TeamGroupId, src => src.TeamGroupId)
             .Map(dest => dest.TeamId, src => src.TeamId)
             .Map(dest => dest.TeamName, src => src.Team != null ? src.Team.TeamName : string.Empty)
             .Map(dest => dest.TeamCode, src => src.Team != null ? src.Team.TeamCode : string.Empty)
             .Map(dest => dest.GroupTag, src => src.GroupTag)
             .Map(dest => dest.GroupType, src => src.GroupType)
             .Map(dest => dest.GroupLeaderId, src => src.GroupLeaderId)
             .Map(dest => dest.GroupLeaderName, src => src.GroupLeader != null && src.GroupLeader.User != null ? src.GroupLeader.User.FullName : null)
             .Map(dest => dest.IsActive, src => src.IsActive)
             .Map(dest => dest.CreatedDate, src => src.CreatedDate)
             .Map(dest => dest.CreatedBy, src => src.CreatedBy)
             .Map(dest => dest.MemberCount, src => src.Members != null ? src.Members.Count(m => m.IsActive) : 0);
    }
}

