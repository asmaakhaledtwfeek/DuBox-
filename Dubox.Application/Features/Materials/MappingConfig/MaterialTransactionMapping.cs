using Dubox.Application.DTOs;
using Dubox.Domain.Entities;
using Mapster;

namespace Dubox.Application.Features.Materials.MappingConfig
{
    public class MaterialTransactionMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            TypeAdapterConfig<MaterialTransaction, MaterialTransactionDto>.NewConfig()
           .Map(dest => dest.TransactionId, src => src.TransactionId)
           .Map(dest => dest.PerformedByUserName, src => src.PerformedBy != null ? src.PerformedBy.FullName : string.Empty)
           .Map(dest => dest.BoxCode, src => src.Box != null ? src.Box.BoxTag : null)
            .Map(dest => dest.ActivityCode, src => src.BoxActivity != null ? src.BoxActivity.ActivityMaster.ActivityCode : null)
           .Map(dest => dest.ActivityName, src => src.BoxActivity != null ? src.BoxActivity.ActivityMaster.ActivityName : null)
           .Map(dest => dest.BoxActivityId, src => src.BoxActivity != null ? src.BoxActivityId : null)
           .Map(dest => dest.MaterialCode, src => src.Material != null ? src.Material.MaterialCode : string.Empty)
           .Map(dest => dest.MaterialName, src => src.Material != null ? src.Material.MaterialName : string.Empty);


        }
    }
}
