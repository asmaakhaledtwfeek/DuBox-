using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Users.Commands;

public record UpdateUserCommand(Guid UserId, string Email, string? FullName, string? Department, bool IsActive) 
    : IRequest<Result<UserDto>>;

