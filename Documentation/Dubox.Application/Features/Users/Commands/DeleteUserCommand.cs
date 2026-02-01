using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Users.Commands;

public record DeleteUserCommand(Guid UserId) : IRequest<Result>;


