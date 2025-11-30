using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Roles.Commands;

public record DeleteRoleCommand(Guid RoleId) : IRequest<Result>;


