using Dubox.Application.DTOs;
using Dubox.Domain.Entities;
using Mapster;

namespace Dubox.Application.Features.Boxes.MappingConfig
{
    public class BoxMaterialMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            // Mapping for BoxMaterial entity (box-level material tracking)
            TypeAdapterConfig<BoxMaterial, BoxMaterialDto>.NewConfig()
                .Map(dest => dest.MaterialCode, src => src.Material.MaterialCode)
                .Map(dest => dest.MaterialName, src => src.Material.MaterialName)
                .Map(dest => dest.BoxTag, src => src.Box.BoxTag)
                .MaxDepth(2);

            // Mapping for ProjectMaterial entity (project-level material allocation)
            TypeAdapterConfig<ProjectMaterial, BoxMaterialDto>.NewConfig()
                .Map(dest => dest.MaterialCode, src => src.Material.MaterialCode)
                .Map(dest => dest.MaterialName, src => src.Material.MaterialName)
                .Map(dest => dest.BoxTag, src => (string)null!) // No box for project materials
                .MaxDepth(2);
        }
    }
}
