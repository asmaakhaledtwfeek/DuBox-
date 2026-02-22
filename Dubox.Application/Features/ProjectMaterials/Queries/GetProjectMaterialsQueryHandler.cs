using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.ProjectMaterials.Queries;

public class GetProjectMaterialsQueryHandler 
    : IRequestHandler<GetProjectMaterialsQuery, Result<List<ProjectMaterialDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetProjectMaterialsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<ProjectMaterialDto>>> Handle(
        GetProjectMaterialsQuery request, 
        CancellationToken cancellationToken)
    {
        // Get all active materials
        var allMaterials = await _unitOfWork.Repository<Material>()
            .FindAsync(m => m.IsActive, cancellationToken);

        // Get existing project materials for this project
        var projectMaterials =  _unitOfWork.Repository<ProjectMaterial>()
            .Get()
            .Where(pm => pm.ProjectId == request.ProjectId).ToList();

        var result = new List<ProjectMaterialDto>();

        foreach (var material in allMaterials)
        {
            var projectMaterial = projectMaterials.FirstOrDefault(pm => pm.MaterialId == material.MaterialId);
            bool isSelected = projectMaterial?.IsSelected ?? false;

            // If SelectedOnly is true, filter by selection status
            if (!request.SelectedOnly || isSelected)
            {
                result.Add(new ProjectMaterialDto
                {
                    ProjectMaterialId = projectMaterial?.ProjectMaterialId ?? Guid.Empty,
                    ProjectId = request.ProjectId,
                    MaterialId = material.MaterialId,
                    MaterialCode = material.MaterialCode,
                    MaterialName = material.MaterialName,
                    MaterialCategory = material.MaterialCategory,
                    DefaultRequiredBeforeDays = material.DefaultRequiredBeforeDays,
                    IsSelected = isSelected
                });
            }
        }

        return Result.Success(result.OrderBy(x => x.MaterialName).ToList());
    }
}

