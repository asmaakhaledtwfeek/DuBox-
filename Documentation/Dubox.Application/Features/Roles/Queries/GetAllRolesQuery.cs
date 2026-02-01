using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Roles.Queries;

public record GetAllRolesQuery : IRequest<Result<List<RoleDto>>>;

