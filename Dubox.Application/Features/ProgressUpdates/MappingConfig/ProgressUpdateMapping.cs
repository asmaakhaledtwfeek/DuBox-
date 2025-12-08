using Dubox.Application.DTOs;
using Dubox.Domain.Entities;
using Mapster;

namespace Dubox.Application.Features.ProgressUpdates.MappingConfig
{
    public class ProgressUpdateMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            // Map Photo field from entity to PhotoUrls in DTO
            TypeAdapterConfig<ProgressUpdate, ProgressUpdateDto>.NewConfig()
                .Map(dest => dest.Photo, src => src.Photo);
        }
    }
}
