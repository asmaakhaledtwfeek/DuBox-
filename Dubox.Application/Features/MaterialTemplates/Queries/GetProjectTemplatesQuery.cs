using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.MaterialTemplates.Queries;

public record GetProjectTemplatesQuery(Guid ProjectId) : IRequest<Result<List<ProjectMaterialTemplateDto>>>;

public record ProjectMaterialTemplateDto
{
    public Guid ProjectMaterialTemplateId { get; init; }
    public Guid ProjectId { get; init; }
    public Guid MaterialTemplateId { get; init; }
    public string TemplateName { get; init; } = string.Empty;
    public string TemplateCode { get; init; } = string.Empty;
    public string? Category { get; init; }
    public int ItemCount { get; init; }
    public DateTime AssignedDate { get; init; }
    public string? AssignedBy { get; init; }
}






