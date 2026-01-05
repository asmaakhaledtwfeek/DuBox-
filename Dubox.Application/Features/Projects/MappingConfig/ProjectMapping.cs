using Dubox.Application.DTOs;
using Dubox.Domain.Entities;
using Mapster;

namespace Dubox.Application.Features.Projects.MappingConfig
{
    public class ProjectMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            TypeAdapterConfig<Project, ProjectDto>.NewConfig()
                .Map(dest => dest.StartDate, src => src.ActualStartDate ?? src.CompressionStartDate ?? src.PlannedStartDate)
                .Map(dest => dest.PlannedStartDate, src => src.PlannedStartDate)
                .Map(dest => dest.PlannedEndDate, src => src.PlannedEndDate)
                .Map(dest => dest.ActualEndDate, src => src.ActualEndDate)
                .Map(dest => dest.CompressionStartDate, src => src.CompressionStartDate)
                .Map(dest => dest.CategoryId, src => src.CategoryId)
                .Map(dest => dest.BimLink, src => src.BimLink);
        }
    }
}


