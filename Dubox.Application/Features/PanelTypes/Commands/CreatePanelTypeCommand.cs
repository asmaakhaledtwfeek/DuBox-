using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.PanelTypes.Commands;

public record CreatePanelTypeCommand(
    Guid ProjectId,
    string PanelTypeName,
    string PanelTypeCode,
    string? Description,
    int DisplayOrder = 0
) : IRequest<Result<PanelTypeDto>>;

