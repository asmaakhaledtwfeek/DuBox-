using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.MaterialTemplates.Queries;

public record GetAllMaterialTemplatesQuery(
    bool ActiveOnly = true,
    string? Category = null,
    string? SearchTerm = null
) : IRequest<Result<List<MaterialTemplateListDto>>>;

public record MaterialTemplateListDto
{
    public Guid MaterialTemplateId { get; init; }
    public string TemplateName { get; init; } = string.Empty;
    public string TemplateCode { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string? Category { get; init; }
    public bool IsActive { get; init; }
    public int ItemCount { get; init; }
    public DateTime CreatedDate { get; init; }
    public string? CreatedBy { get; init; }
}






