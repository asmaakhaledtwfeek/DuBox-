using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.MaterialTemplates.Queries;

public record GetBoxTypeTemplatesQuery(Guid ProjectId) : IRequest<Result<List<BoxTypeMaterialTemplateDto>>>;

public record BoxTypeMaterialTemplateDto
{
    public Guid BoxTypeMaterialTemplateId { get; init; }
    public int ProjectBoxTypeId { get; init; }
    public string BoxTypeName { get; init; } = string.Empty;
    public Guid MaterialTemplateId { get; init; }
    public string TemplateName { get; init; } = string.Empty;
    public string TemplateCode { get; init; } = string.Empty;
    public string? Category { get; init; }
    public int ItemCount { get; init; }
    public DateTime AssignedDate { get; init; }
    public string? AssignedBy { get; init; }
}






