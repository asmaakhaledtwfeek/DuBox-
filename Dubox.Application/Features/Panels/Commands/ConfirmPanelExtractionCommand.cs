using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Panels.Commands;

/// <summary>
/// Command to confirm and save reviewed panel extraction data
/// </summary>
public record ConfirmPanelExtractionCommand(
    Guid ProjectId,
    Guid? BoxId, // Optional - if provided, will create BoxPanels for this box
    List<ReviewPanelTypeDto> PanelTypes,
    List<ReviewBoxPanelDto> BoxPanels,
    List<BoxTagMappingDto> BoxTagMappings
) : IRequest<Result<PanelExtractionConfirmationDto>>;

public record BoxTagMappingDto(
    string BoxTag,
    Guid? BoxId, // null if creating new box
    string? NewBoxName // only if creating new box
);





