using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Boxes.Commands;

public record DeleteBoxCommand(Guid BoxId) : IRequest<Result<bool>>;

