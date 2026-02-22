using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.ProjectMaterials.Queries;

public record GetProjectMaterialsQuery(
    Guid ProjectId,
    bool SelectedOnly = true // If true, only return selected materials
) : IRequest<Result<List<ProjectMaterialDto>>>;

public record ProjectMaterialDto
{
    public Guid ProjectMaterialId { get; init; }
    public Guid ProjectId { get; init; }
    public Guid MaterialId { get; init; }
    public string MaterialCode { get; init; } = string.Empty;
    public string MaterialName { get; init; } = string.Empty;
    public string? MaterialCategory { get; init; }
    public int DefaultRequiredBeforeDays { get; init; }
    public bool IsSelected { get; init; }
}

