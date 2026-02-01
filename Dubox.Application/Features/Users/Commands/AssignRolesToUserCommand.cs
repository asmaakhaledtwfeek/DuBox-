using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Users.Commands;

public record AssignRolesToUserCommand(Guid UserId, List<Guid> RoleIds) : IRequest<Result>;

