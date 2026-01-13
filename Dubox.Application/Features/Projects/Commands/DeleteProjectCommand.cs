using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Projects.Commands;

public record DeleteProjectCommand(Guid ProjectId) : IRequest<Result<bool>>;

