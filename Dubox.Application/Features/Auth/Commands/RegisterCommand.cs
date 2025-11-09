using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Auth.Commands;

public record RegisterCommand(string Email, string Password, string FullName, Guid? DepartmentId)
    : IRequest<Result<UserDto>>;

