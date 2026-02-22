using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Projects.Commands;

public record UpdateProjectBoxTypeCommand(
    Guid ProjectId,
    int BoxTypeId,
    Guid? ActivityTemplateId,
    Guid? MaterialTemplateId
) : IRequest<Result<bool>>;
