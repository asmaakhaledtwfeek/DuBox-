using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Permissions.Commands;

public record AssignPermissionsToRoleCommand(
    Guid RoleId,
    List<Guid> PermissionIds
) : IRequest<Result<bool>>;

