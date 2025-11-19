using Dubox.Application.DTOs;
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
            .Map(dest => dest.Stage, src => src.ActivityMaster.Stage);

        }
    }
}
