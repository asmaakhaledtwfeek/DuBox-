using Dubox.Application.Features.Boxes.Commands;
using Dubox.Domain.Entities;
using Mapster;

namespace Dubox.Application.Features.Boxes.MappingConfig
{
    internal class UpdateBoxCommandMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<UpdateBoxCommand, Box>()
             .Map(dest => dest.BoxTag, src => src.BoxTag)
             .Map(dest => dest.BoxName, src => src.BoxName)
             .Map(dest => dest.BoxTypeId, src => src.BoxTypeId)
             .Map(dest => dest.Floor, src => src.Floor)
             .Map(dest => dest.BuildingNumber, src => src.BuildingNumber)
             .Map(dest => dest.Zone, src => src.Zone)
             .Map(dest => dest.Length, src => src.Length)
             .Map(dest => dest.Width, src => src.Width)
             .Map(dest => dest.Height, src => src.Height)
             .Map(dest => dest.Notes, src => src.Notes)
             .Map(dest => dest.FactoryId, src => src.FactoryId)
             .Map(dest => dest.ModifiedDate, _ => DateTime.UtcNow)

             .Ignore(dest => dest.BoxId)
             .Ignore(dest => dest.ProjectId)
             .Ignore(dest => dest.QRCodeString)
             .Ignore(dest => dest.QRCodeImageUrl)
             .Ignore(dest => dest.CreatedDate);

        }
    }
}
