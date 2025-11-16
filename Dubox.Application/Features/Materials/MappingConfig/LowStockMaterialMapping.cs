using Dubox.Application.DTOs;
using Dubox.Domain.Entities;
using Mapster;

namespace Dubox.Application.Features.Materials.MappingConfig
{

    public class LowStockMaterialMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Material, LowStockMaterialDto>()
                .Map(dest => dest.Shortage,
                     src => (src.MinimumStock.GetValueOrDefault(0)) - (src.CurrentStock.GetValueOrDefault(0)));
        }
    }
}
