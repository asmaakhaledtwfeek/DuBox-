using Dubox.Application.DTOs;
using Dubox.Domain.Entities;
using Mapster;

namespace Dubox.Application.Features.Teams.MappingConfig
{
    internal class CreateTeamMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Team, TeamDto>()
                 .Map(dest => dest.TeamId, src => src.TeamId)
                 .Map(dest => dest.DepartmentId, src => src.DepartmentId)
                 .Map(dest => dest.DepartmentName, src => src.Department != null ? src.Department.DepartmentName : string.Empty)
                 .Map(dest => dest.TeamSize, src => src.Members != null ? src.Members.Count : 0)
                 .Map(dest => dest.TeamLeaderName, src => src.TeamLeader != null ? src.TeamLeader.EmployeeName : null);
        }
    }
}
