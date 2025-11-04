using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Users.Queries;

public record GetAllUsersQuery : IRequest<Result<List<UserDto>>>;

