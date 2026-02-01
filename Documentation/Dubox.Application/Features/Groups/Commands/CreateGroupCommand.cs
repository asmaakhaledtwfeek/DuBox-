using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Groups.Commands;

public record CreateGroupCommand(string GroupName, string? Description) : IRequest<Result<GroupDto>>;

