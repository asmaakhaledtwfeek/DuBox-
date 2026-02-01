using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Permissions.Queries;

public record GetPermissionMatrixQuery : IRequest<Result<List<RolePermissionMatrixDto>>>;

