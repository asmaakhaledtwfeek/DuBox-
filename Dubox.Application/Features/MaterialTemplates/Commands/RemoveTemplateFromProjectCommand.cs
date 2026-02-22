using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.MaterialTemplates.Commands;

public record RemoveTemplateFromProjectCommand(
    Guid ProjectId,
    Guid MaterialTemplateId
) : IRequest<Result<bool>>;






