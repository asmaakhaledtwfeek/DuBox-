using Dubox.Application.DTOs;
using Dubox.Domain.Entities;
using Mapster;

namespace Dubox.Application.Features.Teams.MappingConfig
{
    internal class CreateTeamMapping : IRegister
    {
        void IRegister.Register(TypeAdapterConfig config)
        {
            TypeAdapterConfig<Team, TeamDto>
                 .NewConfig()
                 .Map(dest => dest.DepartmentName, src => src.Department!.DepartmentName)
                 .Map(dest => dest.TeamSize, src => src.Members.Count);
        }
    }
}
