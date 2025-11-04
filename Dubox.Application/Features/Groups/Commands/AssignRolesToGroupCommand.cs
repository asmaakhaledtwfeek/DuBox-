using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Groups.Commands;

public record AssignRolesToGroupCommand(Guid GroupId, List<Guid> RoleIds) : IRequest<Result>;

