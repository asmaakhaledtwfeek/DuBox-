using Dubox.Application.DTOs;
using Dubox.Domain.Entities;
using Mapster;

public class TeamMembersMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {


        config.NewConfig<TeamMember, TeamMemberDto>()
            .Map(dest => dest.UserId, src => src.UserId)
            .Map(dest => dest.Email, src => src.User.Email)
            .Map(dest => dest.FullName, src => src.User.FullName)
            .Map(dest => dest.EmployeeCode, src => src.EmployeeCode)
            .Map(dest => dest.EmployeeName, src => src.EmployeeName)
            .Map(dest => dest.MobileNumber, src => src.MobileNumber)
            .Map(dest => dest.TeamName, src => src.Team.TeamName)
            .Map(dest => dest.TeamCode, src => src.Team.TeamCode);

        config.NewConfig<(Team team, List<TeamMember> members), TeamMembersDto>()
            .Map(dest => dest.TeamId, src => src.team.TeamId)
            .Map(dest => dest.TeamName, src => src.team.TeamName)
            .Map(dest => dest.TeamCode, src => src.team.TeamCode)
           .Map(dest => dest.TeamSize, src => src.members.Count(m => m.IsActive))
           .Map(dest => dest.Members, src => src.members);
    }
}

