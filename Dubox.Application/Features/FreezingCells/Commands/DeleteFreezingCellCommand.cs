using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.FreezingCells.Commands;

public record DeleteFreezingCellCommand(Guid FreezingCellId) : IRequest<Result<bool>>;
