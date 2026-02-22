using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.MaterialTemplates.Commands;

public record CreateMaterialTemplateCommand(
    string TemplateName,
    string TemplateCode,
    string? Description,
    string? Category,
    List<MaterialTemplateItemDto> Items
) : IRequest<Result<Guid>>;

public record MaterialTemplateItemDto
{
    public Guid MaterialId { get; init; }
    public int RequiredBeforeDays { get; init; }
    public bool IsRequired { get; init; } = true;
    public string? Notes { get; init; }
    public int DisplayOrder { get; init; }
}






