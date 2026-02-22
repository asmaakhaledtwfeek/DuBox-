using Dubox.Application.DTOs;
using Dubox.Application.Infrastructure;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Helpers;
using Microsoft.Extensions.Logging;

namespace Dubox.Application.Features.Projects.Processors;

/// <summary>
/// Processor for ProjectZone configuration
/// </summary>
public class ZoneConfigurationProcessor : ConfigurationProcessorBase<ProjectZone, ProjectZoneDto>
{
    public ZoneConfigurationProcessor(
        IUnitOfWork unitOfWork, 
        ILogger<ZoneConfigurationProcessor> logger) 
        : base(unitOfWork, logger)
    {
    }

    public override async Task<List<string>> ValidateAndDeleteAsync(
        Guid projectId,
        List<ProjectZoneDto> requestDtos,
        IEnumerable<Box> projectBoxes,
        CancellationToken cancellationToken)
    {
        var cannotDeleteItems = new List<string>();

        try
        {
            var existingZones = await UnitOfWork.Repository<ProjectZone>()
                .FindAsync(z => z.ProjectId == projectId, cancellationToken);

            var zonesToDelete = existingZones
                .Where(ez => !requestDtos.Any(rz => rz.ZoneCode == ez.ZoneCode))
                .ToList();

            foreach (var zone in zonesToDelete)
            {
                var isUsed = projectBoxes.Any(box => box.Zone == zone.ZoneCode);
                if (isUsed)
                {
                    cannotDeleteItems.Add($"Zone '{zone.ZoneCode} - {zone.ZoneName}' (used by {projectBoxes.Count(b => b.Zone == zone.ZoneCode)} box(es))");
                }
                else
                {
                    UnitOfWork.Repository<ProjectZone>().Delete(zone);
                }
            }
        }
        catch (Exception ex)
        {
            LogError($"Error validating and deleting zones for project {projectId}", ex);
            throw;
        }

        return cannotDeleteItems;
    }

    public override async Task<List<int>> UpdateOrAddAsync(
        Guid projectId,
        List<ProjectZoneDto> requestDtos,
        CancellationToken cancellationToken)
    {
        try
        {
            var existingZones = await UnitOfWork.Repository<ProjectZone>()
                .FindAsync(z => z.ProjectId == projectId, cancellationToken);

            foreach (var (dto, index) in requestDtos.Select((dto, index) => (dto, index)))
            {
                var existing = existingZones.FirstOrDefault(z => z.ZoneCode == dto.ZoneCode);
                if (existing != null)
                {
                    existing.ZoneName = TextTransformHelper.ToTitleCase(dto.ZoneName);
                    existing.DisplayOrder = index;
                    existing.IsActive = true;
                    UnitOfWork.Repository<ProjectZone>().Update(existing);
                }
                else
                {
                    var newZone = new ProjectZone
                    {
                        ProjectId = projectId,
                        ZoneCode = TextTransformHelper.ToTitleCase(dto.ZoneCode),
                        ZoneName = TextTransformHelper.ToTitleCase(dto.ZoneName),
                        DisplayOrder = index,
                        IsActive = true,
                        CreatedDate = DateTime.UtcNow
                    };
                    await UnitOfWork.Repository<ProjectZone>().AddAsync(newZone, cancellationToken);
                }
            }

            return new List<int>(); // Zones don't need to return IDs
        }
        catch (Exception ex)
        {
            LogError($"Error updating or adding zones for project {projectId}", ex);
            throw;
        }
    }
}
