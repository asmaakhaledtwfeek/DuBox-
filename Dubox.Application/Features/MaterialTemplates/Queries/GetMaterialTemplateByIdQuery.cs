using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.MaterialTemplates.Queries;

public record GetMaterialTemplateByIdQuery(Guid MaterialTemplateId) : IRequest<Result<MaterialTemplateDetailDto>>;

public record MaterialTemplateDetailDto
{
    public Guid MaterialTemplateId { get; init; }
    public string TemplateName { get; init; } = string.Empty;
    public string TemplateCode { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string? Category { get; init; }
    public bool IsActive { get; init; }
    public List<MaterialTemplateItemDetailDto> Items { get; init; } = new();
    public DateTime CreatedDate { get; init; }
    public string? CreatedBy { get; init; }
    public DateTime? ModifiedDate { get; init; }
    public string? ModifiedBy { get; init; }
}

public record MaterialTemplateItemDetailDto
{
    public Guid MaterialTemplateItemId { get; init; }
    public Guid MaterialId { get; init; }
    public string MaterialCode { get; init; } = string.Empty;
    public string MaterialName { get; init; } = string.Empty;
    public string? MaterialCategory { get; init; }
    public int RequiredBeforeDays { get; init; }
    public bool IsRequired { get; init; }
    public string? Notes { get; init; }
    public int DisplayOrder { get; init; }
    public string Unit { get; init; } = string.Empty;
    public int QuantityPerBox {  get; init; }
}






