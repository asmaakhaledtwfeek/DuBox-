using Dubox.Application.DTOs;
using Dubox.Application.Utilities;
using Dubox.Domain.Entities;
using Mapster;

namespace Dubox.Application.Features.Reports.MappingConfig;

public class ProjectSummaryMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Project, ProjectSummaryItemDto>()
            .Map(dest => dest.Status, src => src.Status.ToString())
            .Map(dest => dest.ProgressPercentageFormatted, src => ProgressFormatter.FormatProgress(src.ProgressPercentage));
    }
}

