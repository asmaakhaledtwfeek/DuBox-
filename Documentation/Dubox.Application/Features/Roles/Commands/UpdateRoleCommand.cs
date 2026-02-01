using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Roles.Commands;

public record UpdateRoleCommand(Guid RoleId, string RoleName, string? Description, bool IsActive)
    : IRequest<Result<RoleDto>>;


