using Dubox.Application.Features.Projects.Commands;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Mapster;

namespace Dubox.Application.Features.Projects.MappingConfig
{
    internal class CreateProjectCommandMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<CreateProjectCommand, Project>()
                  .Map(dest => dest.Status, src => ProjectStatusEnum.Active)
                  .Map(dest => dest.IsActive, src => true)
                  .Map(dest => dest.TotalBoxes, src => 0)
                  .Map(dest => dest.CreatedDate, src => DateTime.UtcNow)
               .PreserveReference(true);
        }
    }
}
