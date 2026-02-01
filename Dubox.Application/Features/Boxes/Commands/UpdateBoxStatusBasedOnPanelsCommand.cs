using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Boxes.Commands;

public record UpdateBoxStatusBasedOnPanelsCommand(Guid BoxId) : IRequest<Result<string>>;
