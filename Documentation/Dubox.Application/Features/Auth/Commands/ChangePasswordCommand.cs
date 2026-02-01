using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Auth.Commands;

public record ChangePasswordCommand(Guid UserId, string CurrentPassword, string NewPassword) 
    : IRequest<Result>;

