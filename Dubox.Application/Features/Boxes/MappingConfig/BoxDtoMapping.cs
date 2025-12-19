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
           .Map(dest => dest.ActivitiesCount, src => src.BoxActivities.Count)
           .Map(dest => dest.BoxType, src => src.BoxType != null ? src.BoxType.BoxTypeName : string.Empty)
           .Map(dest => dest.BoxTypeId, src => src.BoxTypeId)
           .Map(dest => dest.BoxSubTypeId, src => src.BoxSubTypeId)
           .Map(dest => dest.BoxSubTypeName, src => src.BoxSubType != null ? src.BoxSubType.BoxSubTypeName : null);

        }
    }
}
