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
           .Map(dest => dest.ActivitiesCount, src => src.BoxActivities != null ? src.BoxActivities.Count : 0)
           .Map(dest => dest.BoxType, src => src.BoxType != null && src.BoxType.BoxTypeName != null ? src.BoxType.BoxTypeName : string.Empty)
           .Map(dest => dest.BoxTypeId, src => src.BoxTypeId)
           .Map(dest => dest.BoxSubTypeId, src => src.BoxSubTypeId)
           .Map(dest => dest.BoxSubTypeName, src => src.BoxSubType != null && src.BoxSubType.BoxSubTypeName != null ? src.BoxSubType.BoxSubTypeName : null)
           .Map(dest => dest.Status, src => src.Status.ToString())
           .Map(dest => dest.Zone, src => src.Zone != null ? src.Zone.ToString() : null)
           .Map(dest => dest.UnitOfMeasure, src => src.UnitOfMeasure != null ? src.UnitOfMeasure.ToString() : null)
           .Map(dest => dest.ProjectCode, src => src.Project != null && src.Project.ProjectCode != null ? src.Project.ProjectCode : string.Empty)
           .Map(dest => dest.Client, src => src.Project != null && src.Project.ClientName != null ? src.Project.ClientName : string.Empty)
           .Map(dest => dest.CurrentLocationCode, src => src.CurrentLocation != null && src.CurrentLocation.LocationCode != null ? src.CurrentLocation.LocationCode : null)
           .Map(dest => dest.CurrentLocationName, src => src.CurrentLocation != null && src.CurrentLocation.LocationName != null ? src.CurrentLocation.LocationName : null)
           .Map(dest => dest.FactoryId, src => src.FactoryId)
           .Map(dest=>dest.FactoryCode,src=>src.Factory!=null && src.Factory.FactoryCode != null ? src.Factory.FactoryCode : null)
           .Map(dest => dest.FactoryName, src => src.Factory != null && src.Factory.FactoryName != null ? src.Factory.FactoryName : null);


        }
    }
}
