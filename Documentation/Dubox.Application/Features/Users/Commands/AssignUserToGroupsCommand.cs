using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Users.Commands;

public record AssignUserToGroupsCommand(Guid UserId, List<Guid> GroupIds) : IRequest<Result>;

