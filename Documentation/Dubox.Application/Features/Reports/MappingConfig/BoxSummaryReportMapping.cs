using Dubox.Application.DTOs;
using Dubox.Application.Utilities;
using Dubox.Domain.Entities;
using Mapster;

namespace Dubox.Application.Features.Reports.MappingConfig;

public class BoxSummaryReportMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Box, BoxSummaryReportItemDto>()
            .Map(dest => dest.ProjectCode, src => src.Project != null ? src.Project.ProjectCode : string.Empty)
            .Map(dest => dest.ProjectName, src => src.Project != null ? src.Project.ProjectName : string.Empty)
            .Map(dest => dest.ProjectStatus, src => src.Project != null ? src.Project.Status.ToString() : string.Empty)
            .Map(dest => dest.ProgressPercentageFormatted, src => ProgressFormatter.FormatProgress(src.ProgressPercentage))
            .Map(dest => dest.Status, src => src.Status.ToString())
            .Map(dest => dest.CurrentLocationName, src => src.CurrentLocation != null ? src.CurrentLocation.LocationName : null)
            .Map(dest => dest.LastUpdateDate, src => src.ModifiedDate)
            .Map(dest => dest.ActivitiesCount, src => 0)
            .Map(dest => dest.AssetsCount, src => 0);
    }
}

