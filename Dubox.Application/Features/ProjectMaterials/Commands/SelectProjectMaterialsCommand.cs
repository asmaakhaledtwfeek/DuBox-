using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.ProjectMaterials.Commands;

public record SelectProjectMaterialsCommand(
    Guid ProjectId,
    List<Guid> MaterialIds, // List of material IDs to select (empty list means deselect all)
    bool SelectAll = false // If true, select all available materials
) : IRequest<Result<List<MaterialDto>>>;






