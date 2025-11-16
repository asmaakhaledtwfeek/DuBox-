using Dubox.Application.DTOs;
using Dubox.Domain.Entities;
using Mapster;

namespace Dubox.Application.Features.Boxes.MappingConfig
{
    public class BoxMaterialMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            TypeAdapterConfig<BoxMaterial, BoxMaterialDto>.NewConfig()
                .Map(dest => dest.MaterialCode, src => src.Material.MaterialCode)
                .Map(dest => dest.MaterialName, src => src.Material.MaterialName)
                .Map(dest => dest.BoxTag, src => src.Box.BoxTag)

                .MaxDepth(2); ;
        }
    }
}
