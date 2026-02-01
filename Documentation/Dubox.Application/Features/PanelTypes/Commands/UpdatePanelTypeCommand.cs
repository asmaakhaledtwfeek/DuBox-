using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.PanelTypes.Commands;

public record UpdatePanelTypeCommand(
    Guid PanelTypeId,
    string PanelTypeName,
    string PanelTypeCode,
    string? Description,
    bool IsActive,
    int DisplayOrder
) : IRequest<Result<PanelTypeDto>>;

