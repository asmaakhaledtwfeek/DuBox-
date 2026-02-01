using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.PanelTypes.Commands;

public record DeletePanelTypeCommand(Guid PanelTypeId) : IRequest<Result<bool>>;

