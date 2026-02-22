using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.MaterialTemplates.Commands;

public record RemoveTemplateFromBoxTypeCommand(
    int ProjectBoxTypeId,
    Guid MaterialTemplateId
) : IRequest<Result<bool>>;






