using Dubox.Application.Features.Boxes.Commands;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Mapster;

namespace Dubox.Application.Features.Boxes.MappingConfig
{
    internal class CreateBoxCommandMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<CreateBoxCommand, Box>()
            //.Map(dest => dest.BoxAssets, src => src.Assets == null
            //   ? new List<BoxAsset>()
            //   : src.Assets.Adapt<List<BoxAsset>>())
           .Map(dest => dest.ProjectId, src => src.ProjectId)
           .Map(dest => dest.BoxTag, src => src.BoxTag)
           .Map(dest => dest.BoxName, src => src.BoxName)
           .Map(dest => dest.BoxType, src => src.BoxType)
           .Map(dest => dest.Floor, src => src.Floor)
           .Map(dest => dest.Building, src => src.Building)
           .Map(dest => dest.Zone, src => src.Zone)
           .Map(dest => dest.BIMModelReference, src => src.BIMModelReference)
           .Map(dest => dest.RevitElementId, src => src.RevitElementId)
           .Map(dest => dest.Length, src => src.Length)
           .Map(dest => dest.Width, src => src.Width)
           .Map(dest => dest.Height, src => src.Height)
           .Map(dest => dest.UnitOfMeasure, _ => UnitOfMeasureEnum.m)
           .Map(dest => dest.ProgressPercentage, _ => 0)
           .Map(dest => dest.Status, _ => BoxStatusEnum.NotStarted)
           .Ignore(dest => dest.QRCodeString)
           .Map(dest => dest.IsActive, _ => true)
           .Map(dest => dest.CreatedDate, _ => DateTime.UtcNow);

        }
    }
}
