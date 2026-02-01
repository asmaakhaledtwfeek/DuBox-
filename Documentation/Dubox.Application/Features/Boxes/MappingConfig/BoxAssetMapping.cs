using Dubox.Application.DTOs;
using Dubox.Domain.Entities;
using Mapster;

namespace Dubox.Application.Features.Boxes.MappingConfig
{
    internal class BoxAssetMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<CreateBoxAssetDto, BoxAsset>()
            .Map(dest => dest.AssetType, src => src.AssetType)
            .Map(dest => dest.AssetCode, src => src.AssetCode)
            .Map(dest => dest.AssetName, src => src.AssetName)
            .Map(dest => dest.Quantity, src => src.Quantity)
            .Map(dest => dest.Unit, src => src.Unit)
            .Map(dest => dest.Specifications, src => src.Specifications)
            .Map(dest => dest.Notes, src => src.Notes)
            .Map(dest => dest.CreatedDate, _ => DateTime.UtcNow);
        }
    }
}
