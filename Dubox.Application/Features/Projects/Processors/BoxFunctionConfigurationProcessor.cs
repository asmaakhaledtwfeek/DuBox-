using Dubox.Application.DTOs;
using Dubox.Application.Infrastructure;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Helpers;
using Microsoft.Extensions.Logging;

namespace Dubox.Application.Features.Projects.Processors;

/// <summary>
/// Processor for ProjectBoxFunction configuration
/// </summary>
public class BoxFunctionConfigurationProcessor : ConfigurationProcessorBase<ProjectBoxFunction, ProjectBoxFunctionDto>
{
    public BoxFunctionConfigurationProcessor(
        IUnitOfWork unitOfWork, 
        ILogger<BoxFunctionConfigurationProcessor> logger) 
        : base(unitOfWork, logger)
    {
    }

    public override async Task<List<string>> ValidateAndDeleteAsync(
        Guid projectId,
        List<ProjectBoxFunctionDto> requestDtos,
        IEnumerable<Box> projectBoxes,
        CancellationToken cancellationToken)
    {
        var cannotDeleteItems = new List<string>();

        try
        {
            var existingFunctions = await UnitOfWork.Repository<ProjectBoxFunction>()
                .FindAsync(f => f.ProjectId == projectId, cancellationToken);

            var functionsToDelete = existingFunctions
                .Where(ef => !requestDtos.Any(rf => rf.FunctionName == ef.FunctionName))
                .ToList();

            foreach (var function in functionsToDelete)
            {
                var isUsed = projectBoxes.Any(box => box.BoxFunction == function.FunctionName);
                if (isUsed)
                {
                    cannotDeleteItems.Add($"Box Function '{function.FunctionName}' (used by {projectBoxes.Count(b => b.BoxFunction == function.FunctionName)} box(es))");
                }
                else
                {
                    UnitOfWork.Repository<ProjectBoxFunction>().Delete(function);
                }
            }
        }
        catch (Exception ex)
        {
            LogError($"Error validating and deleting box functions for project {projectId}", ex);
            throw;
        }

        return cannotDeleteItems;
    }

    public override async Task<List<int>> UpdateOrAddAsync(
        Guid projectId,
        List<ProjectBoxFunctionDto> requestDtos,
        CancellationToken cancellationToken)
    {
        try
        {
            var existingFunctions = await UnitOfWork.Repository<ProjectBoxFunction>()
                .FindAsync(f => f.ProjectId == projectId, cancellationToken);

            foreach (var (dto, index) in requestDtos.Select((dto, index) => (dto, index)))
            {
                var existing = existingFunctions.FirstOrDefault(f => f.FunctionName == dto.FunctionName);
                if (existing != null)
                {
                    existing.Description = TextTransformHelper.ToTitleCase(dto.Description);
                    existing.DisplayOrder = index;
                    existing.IsActive = true;
                    UnitOfWork.Repository<ProjectBoxFunction>().Update(existing);
                }
                else
                {
                    var newFunction = new ProjectBoxFunction
                    {
                        ProjectId = projectId,
                        FunctionName = TextTransformHelper.ToTitleCase(dto.FunctionName),
                        Description = TextTransformHelper.ToTitleCase(dto.Description),
                        DisplayOrder = index,
                        IsActive = true,
                        CreatedDate = DateTime.UtcNow
                    };
                    await UnitOfWork.Repository<ProjectBoxFunction>().AddAsync(newFunction, cancellationToken);
                }
            }

            return new List<int>(); // Functions don't need to return IDs
        }
        catch (Exception ex)
        {
            LogError($"Error updating or adding box functions for project {projectId}", ex);
            throw;
        }
    }
}
