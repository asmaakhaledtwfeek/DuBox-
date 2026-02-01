using Dubox.Application.DTOs;
using Dubox.Domain.Enums;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.BoxPanels.Commands;

public record UpdateBoxPanelStatusCommand(
    Guid BoxPanelId,
    PanelStatusEnum PanelStatus
) : IRequest<Result<BoxPanelDto>>;
