using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Users.Commands;

public record CreateUserCommand(
    string Email, 
    string Password, 
    string FullName, 
    Guid DepartmentId, 
    bool IsActive = true)
    : IRequest<Result<UserDto>>;
