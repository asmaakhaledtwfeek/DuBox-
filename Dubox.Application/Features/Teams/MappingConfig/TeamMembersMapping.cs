using Dubox.Application.DTOs;
using Dubox.Domain.Entities;
using Mapster;

public class TeamMembersMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {

        config.NewConfig<TeamMember, TeamMemberDto>()
            .Map(dest => dest.TeamMemberId, src => src.TeamMemberId)
            .Map(dest => dest.UserId, src => src.UserId)
            .Map(dest => dest.TeamId, src => src.TeamId)
            .Map(dest => dest.TeamCode, src => src.Team != null ? src.Team.TeamCode : string.Empty)
            .Map(dest => dest.TeamName, src => src.Team != null ? src.Team.TeamName : string.Empty)
            .Map(dest => dest.Email, src => src.User == null ? string.Empty : (src.User.Email ?? string.Empty))
            .Map(dest => dest.FullName, src => src.User == null ? (src.EmployeeName ?? string.Empty) : (src.User.FullName ?? src.EmployeeName ?? string.Empty))
            .Map(dest => dest.EmployeeCode, src => src.EmployeeCode)
            .Map(dest => dest.EmployeeName, src => src.EmployeeName)
            .Map(dest => dest.MobileNumber, src => src.MobileNumber);
          

        config.NewConfig<(Team team, List<TeamMember> members), TeamMembersDto>()
            .Map(dest => dest.TeamId, src => src.team.TeamId)
            .Map(dest => dest.TeamName, src => src.team.TeamName)
            .Map(dest => dest.TeamCode, src => src.team.TeamCode)
           .Map(dest => dest.TeamSize, src => src.members.Count(m => m.IsActive))
           .Map(dest => dest.Members, src => src.members);
    }
}

