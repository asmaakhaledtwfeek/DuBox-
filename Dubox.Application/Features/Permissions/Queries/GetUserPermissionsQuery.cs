using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Permissions.Queries;

public record GetUserPermissionsQuery(Guid UserId) : IRequest<Result<UserPermissionsDto>>;

