using Dubox.Application.DTOs;
using Dubox.Domain.Entities;
using Mapster;

namespace Dubox.Application.Features.Boxes.MappingConfig
{
    internal class BoxDtoMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Box, BoxDto>()

           .Map(dest => dest.ActivitiesCount, src => src.BoxActivities.Count);

        }
    }
}
