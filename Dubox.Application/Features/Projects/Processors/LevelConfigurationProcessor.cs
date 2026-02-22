using Dubox.Application.DTOs;
using Dubox.Application.Infrastructure;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Helpers;
using Microsoft.Extensions.Logging;

namespace Dubox.Application.Features.Projects.Processors;

/// <summary>
/// Processor for ProjectLevel configuration
/// </summary>
public class LevelConfigurationProcessor : ConfigurationProcessorBase<ProjectLevel, ProjectLevelDto>
{
    public LevelConfigurationProcessor(
        IUnitOfWork unitOfWork, 
        ILogger<LevelConfigurationProcessor> logger) 
        : base(unitOfWork, logger)
    {
    }

    public override async Task<List<string>> ValidateAndDeleteAsync(
        Guid projectId,
        List<ProjectLevelDto> requestDtos,
        IEnumerable<Box> projectBoxes,
        CancellationToken cancellationToken)
    {
        var cannotDeleteItems = new List<string>();

        try
        {
            var existingLevels = await UnitOfWork.Repository<ProjectLevel>()
                .FindAsync(l => l.ProjectId == projectId, cancellationToken);

            var levelsToDelete = existingLevels
                .Where(el => !requestDtos.Any(rl => rl.LevelCode == el.LevelCode))
                .ToList();

            foreach (var level in levelsToDelete)
            {
                var isUsed = projectBoxes.Any(box => box.Floor == level.LevelCode);
                if (isUsed)
                {
                    cannotDeleteItems.Add($"Level/Floor '{level.LevelCode} - {level.LevelName}' (used by {projectBoxes.Count(b => b.Floor == level.LevelCode)} box(es))");
                }
                else
                {
                    UnitOfWork.Repository<ProjectLevel>().Delete(level);
                }
            }
        }
        catch (Exception ex)
        {
            LogError($"Error validating and deleting levels for project {projectId}", ex);
            throw;
        }

        return cannotDeleteItems;
    }

    public override async Task<List<int>> UpdateOrAddAsync(
        Guid projectId,
        List<ProjectLevelDto> requestDtos,
        CancellationToken cancellationToken)
    {
        try
        {
            var existingLevels = await UnitOfWork.Repository<ProjectLevel>()
                .FindAsync(l => l.ProjectId == projectId, cancellationToken);

            foreach (var (dto, index) in requestDtos.Select((dto, index) => (dto, index)))
            {
                var existing = existingLevels.FirstOrDefault(l => l.LevelCode == dto.LevelCode);
                if (existing != null)
                {
                    existing.LevelName = TextTransformHelper.ToUpperCase(dto.LevelName);
                    existing.DisplayOrder = index;
                    existing.IsActive = true;
                    UnitOfWork.Repository<ProjectLevel>().Update(existing);
                }
                else
                {
                    var newLevel = new ProjectLevel
                    {
                        ProjectId = projectId,
                        LevelCode = TextTransformHelper.ToUpperCase(dto.LevelCode),
                        LevelName = TextTransformHelper.ToUpperCase(dto.LevelName),
                        DisplayOrder = index,
                        IsActive = true,
                        CreatedDate = DateTime.UtcNow
                    };
                    await UnitOfWork.Repository<ProjectLevel>().AddAsync(newLevel, cancellationToken);
                }
            }

            return new List<int>(); // Levels don't need to return IDs
        }
        catch (Exception ex)
        {
            LogError($"Error updating or adding levels for project {projectId}", ex);
            throw;
        }
    }
}
