using Dubox.Application.DTOs;
using Dubox.Domain.Entities;
using Mapster;

namespace Dubox.Application.Features.Activities.MappingConfig
{
    internal class BoxActivityMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<BoxActivity, BoxActivityDto>()
            .Map(dest => dest.ActivityName, src => src.ActivityMaster.ActivityName)
            .Map(dest => dest.ActivityCode, src => src.ActivityMaster.ActivityCode)
            .Map(dest => dest.Stage, src => src.ActivityMaster.Stage)
            .Map(dest => dest.ActualDuration, src => 
                !src.ActualStartDate.HasValue || !src.ActualEndDate.HasValue
                    ? null
                    : src.ActualStartDate.Value.Date == src.ActualEndDate.Value.Date
                        ? (int?)1
                        : (int?)Math.Max(1, (int)Math.Ceiling((src.ActualEndDate.Value.Date - src.ActualStartDate.Value.Date).TotalDays) + 1));

        }
    }
}
