using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Groups.Commands;

public record UpdateGroupCommand(Guid GroupId, string GroupName, string? Description, bool IsActive)
    : IRequest<Result<GroupDto>>;


