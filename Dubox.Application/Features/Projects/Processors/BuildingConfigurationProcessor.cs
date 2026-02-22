using Dubox.Application.DTOs;
using Dubox.Application.Infrastructure;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Helpers;
using Microsoft.Extensions.Logging;

namespace Dubox.Application.Features.Projects.Processors;

/// <summary>
/// Processor for ProjectBuilding configuration
/// </summary>
public class BuildingConfigurationProcessor : ConfigurationProcessorBase<ProjectBuilding, ProjectBuildingDto>
{
    public BuildingConfigurationProcessor(
        IUnitOfWork unitOfWork, 
        ILogger<BuildingConfigurationProcessor> logger) 
        : base(unitOfWork, logger)
    {
    }

    public override async Task<List<string>> ValidateAndDeleteAsync(
        Guid projectId,
        List<ProjectBuildingDto> requestDtos,
        IEnumerable<Box> projectBoxes,
        CancellationToken cancellationToken)
    {
        var cannotDeleteItems = new List<string>();

        try
        {
            var existingBuildings = await UnitOfWork.Repository<ProjectBuilding>()
                .FindAsync(b => b.ProjectId == projectId, cancellationToken);

            var buildingsToDelete = existingBuildings
                .Where(eb => !requestDtos.Any(rb => rb.BuildingCode == eb.BuildingCode))
                .ToList();

            foreach (var building in buildingsToDelete)
            {
                var isUsed = projectBoxes.Any(box => box.BuildingNumber == building.BuildingCode);
                if (isUsed)
                {
                    cannotDeleteItems.Add($"Building '{building.BuildingCode} - {building.BuildingName}' (used by {projectBoxes.Count(b => b.BuildingNumber == building.BuildingCode)} box(es))");
                }
                else
                {
                    UnitOfWork.Repository<ProjectBuilding>().Delete(building);
                }
            }
        }
        catch (Exception ex)
        {
            LogError($"Error validating and deleting buildings for project {projectId}", ex);
            throw;
        }

        return cannotDeleteItems;
    }

    public override async Task<List<int>> UpdateOrAddAsync(
        Guid projectId,
        List<ProjectBuildingDto> requestDtos,
        CancellationToken cancellationToken)
    {
        try
        {
            var existingBuildings = await UnitOfWork.Repository<ProjectBuilding>()
                .FindAsync(b => b.ProjectId == projectId, cancellationToken);

            foreach (var (dto, index) in requestDtos.Select((dto, index) => (dto, index)))
            {
                var existing = existingBuildings.FirstOrDefault(b => b.BuildingCode == dto.BuildingCode);
                if (existing != null)
                {
                    existing.BuildingName = TextTransformHelper.ToUpperCase(dto.BuildingName);
                    existing.DisplayOrder = index;
                    existing.IsActive = true;
                    UnitOfWork.Repository<ProjectBuilding>().Update(existing);
                }
                else
                {
                    var newBuilding = new ProjectBuilding
                    {
                        ProjectId = projectId,
                        BuildingCode = TextTransformHelper.ToUpperCase(dto.BuildingCode),
                        BuildingName = TextTransformHelper.ToUpperCase(dto.BuildingName),
                        DisplayOrder = index,
                        IsActive = true,
                        CreatedDate = DateTime.UtcNow
                    };
                    await UnitOfWork.Repository<ProjectBuilding>().AddAsync(newBuilding, cancellationToken);
                }
            }

            return new List<int>(); // Buildings don't need to return IDs
        }
        catch (Exception ex)
        {
            LogError($"Error updating or adding buildings for project {projectId}", ex);
            throw;
        }
    }
}
