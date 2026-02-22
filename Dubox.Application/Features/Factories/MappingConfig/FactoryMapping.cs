using Dubox.Application.DTOs;
using Dubox.Domain.Entities;
using Mapster;

namespace Dubox.Application.Features.Factories.MappingConfig;

public class FactoryMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // Map Factory to FactoryDto with Sections
        config.NewConfig<Factory, FactoryDto>()
            .Map(dest => dest.Sections, src => src.Sections);

        // Map FactorySection to FactorySectionDto (includes Parts)
        config.NewConfig<FactorySection, FactorySectionDto>();

        // Map FactorySectionPart to FactorySectionPartDto (includes FreezingCells)
        config.NewConfig<FactorySectionPart, FactorySectionPartDto>()
            .Map(dest => dest.FreezingCells, src => src.FreezingCells);

        // Map FreezingCell to FreezingCellDto
        config.NewConfig<FreezingCell, FreezingCellDto>();
    }
}

