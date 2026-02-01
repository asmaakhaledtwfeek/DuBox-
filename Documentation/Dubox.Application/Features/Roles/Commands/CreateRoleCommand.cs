using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Roles.Commands;

public record CreateRoleCommand(string RoleName, string? Description) : IRequest<Result<RoleDto>>;

