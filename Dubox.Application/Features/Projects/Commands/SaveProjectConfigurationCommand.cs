using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Projects.Commands;

public record SaveProjectConfigurationCommand(
    Guid ProjectId,
    List<ProjectBuildingDto> Buildings,
    List<ProjectLevelDto> Levels,
    List<ProjectBoxTypeDto> BoxTypes,
    List<ProjectZoneDto> Zones,
    List<ProjectBoxFunctionDto> BoxFunctions
) : IRequest<Result<ProjectConfigurationDto>>;

