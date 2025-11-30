using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Groups.Commands;

public record DeleteGroupCommand(Guid GroupId) : IRequest<Result>;


