using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.MaterialTemplates.Commands;

public record UpdateMaterialTemplateCommand(
    Guid MaterialTemplateId,
    string TemplateName,
    string? Description,
    string? Category,
    List<MaterialTemplateItemDto> Items
) : IRequest<Result<bool>>;






