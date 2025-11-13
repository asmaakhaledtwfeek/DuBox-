using Dubox.Application.Features.ProgressUpdates.Commands;
using Dubox.Domain.Entities;
using Mapster;

namespace Dubox.Application.Features.ProgressUpdates.MappingConfig
{
    public class ProgressUpdateMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<CreateProgressUpdateCommand, ProgressUpdate>()
                .Map(dest => dest.PhotoUrls,
                     src => src.PhotoUrls != null ? string.Join(",", src.PhotoUrls) : null)
                .Ignore(dest => dest.UpdatedBy)
                .Ignore(dest => dest.UpdateDate)
                .Ignore(dest => dest.CreatedDate);
        }
    }
}
