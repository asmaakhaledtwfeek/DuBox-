using Dubox.Application.DTOs;
using Dubox.Application.Infrastructure;
using Dubox.Application.Services;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Helpers;
using Microsoft.Extensions.Logging;

namespace Dubox.Application.Features.Projects.Processors;

/// <summary>
/// Processor for ProjectBoxType and ProjectBoxSubType configuration
/// Includes "Loose Element" box type creation and template auto-assignment
/// </summary>
public class BoxTypeConfigurationProcessor : ConfigurationProcessorBase<ProjectBoxType, ProjectBoxTypeDto>
{
    private readonly IBoxTypeTemplateAutoAssignmentService _autoAssignmentService;
    private readonly List<int> _newBoxTypeIds = new();

    public BoxTypeConfigurationProcessor(
        IUnitOfWork unitOfWork,
        IBoxTypeTemplateAutoAssignmentService autoAssignmentService,
        ILogger<BoxTypeConfigurationProcessor> logger)
        : base(unitOfWork, logger)
    {
        _autoAssignmentService = autoAssignmentService;
    }

    public override async Task<List<string>> ValidateAndDeleteAsync(
        Guid projectId,
        List<ProjectBoxTypeDto> requestDtos,
        IEnumerable<Box> projectBoxes,
        CancellationToken cancellationToken)
    {
        var cannotDeleteItems = new List<string>();

        try
        {
            var existingBoxTypes = await UnitOfWork.Repository<ProjectBoxType>()
                .FindAsync(t => t.ProjectId == projectId, cancellationToken);

            var boxTypesToDelete = existingBoxTypes
                .Where(et => !requestDtos.Any(rt => rt.TypeName == et.TypeName))
                .ToList();

            // Check Box Types
            foreach (var boxType in boxTypesToDelete)
            {
                var isUsed = projectBoxes.Any(box => box.ProjectBoxTypeId == boxType.Id);
                if (isUsed)
                {
                    cannotDeleteItems.Add($"Box Type '{boxType.TypeName}' (used by {projectBoxes.Count(b => b.BoxType.TypeName == boxType.TypeName)} box(es))");
                }
                else
                {
                    UnitOfWork.Repository<ProjectBoxType>().Delete(boxType);
                }
            }

            // Check SubTypes for remaining box types
            foreach (var existingType in existingBoxTypes.Where(t => !boxTypesToDelete.Contains(t)))
            {
                var requestType = requestDtos.FirstOrDefault(rt => rt.TypeName == existingType.TypeName);
                if (requestType != null)
                {
                    // Explicitly fetch SubTypes for this box type (navigation property not loaded)
                    var existingSubTypes = await UnitOfWork.Repository<ProjectBoxSubType>()
                        .FindAsync(st => st.ProjectBoxTypeId == existingType.Id, cancellationToken);

                    var subTypesToDelete = existingSubTypes
                        .Where(st => !requestType.SubTypes.Any(rst => rst.SubTypeName == st.SubTypeName))
                        .ToList();

                    foreach (var subType in subTypesToDelete)
                    {
                        var isUsed = projectBoxes.Any(box =>
                            box.ProjectBoxTypeId == existingType.Id &&
                            box.ProjectBoxSubTypeId == subType.Id);

                        if (isUsed)
                        {
                            cannotDeleteItems.Add($"Box SubType '{existingType.TypeName} - {subType.SubTypeName}' (used by {projectBoxes.Count(b => b.ProjectBoxTypeId == existingType.Id && b.ProjectBoxSubTypeId == subType.Id)} box(es))");
                        }
                        else
                        {
                            UnitOfWork.Repository<ProjectBoxSubType>().Delete(subType);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            LogError($"Error validating and deleting box types for project {projectId}", ex);
            throw;
        }

        return cannotDeleteItems;
    }

    public override async Task<List<int>> UpdateOrAddAsync(
        Guid projectId,
        List<ProjectBoxTypeDto> requestDtos,
        CancellationToken cancellationToken)
    {
        _newBoxTypeIds.Clear();

        try
        {
            var existingBoxTypes = await UnitOfWork.Repository<ProjectBoxType>()
                .FindAsync(t => t.ProjectId == projectId, cancellationToken);

            // Update or Add Box Types
            foreach (var (dto, index) in requestDtos.Select((dto, index) => (dto, index)))
            {
                var existing = existingBoxTypes.FirstOrDefault(t => t.TypeName == dto.TypeName);
                if (existing != null)
                {
                    existing.Abbreviation = TextTransformHelper.ToUpperCase(dto.Abbreviation);
                    existing.HasSubTypes = dto.HasSubTypes;
                    existing.DisplayOrder = index;
                    existing.IsActive = true;
                    UnitOfWork.Repository<ProjectBoxType>().Update(existing);

                    // Handle SubTypes for existing type (always process, even if empty to handle deletions)
                    await ProcessSubTypesAsync(existing.Id, dto.SubTypes, cancellationToken);
                }
                else
                {
                    var newBoxType = new ProjectBoxType
                    {
                        ProjectId = projectId,
                        TypeName = TextTransformHelper.ToUpperCase(dto.TypeName),
                        Abbreviation = TextTransformHelper.ToUpperCase(dto.Abbreviation),
                        HasSubTypes = dto.HasSubTypes,
                        DisplayOrder = index,
                        IsActive = true,
                        CreatedDate = DateTime.UtcNow
                    };
                    await UnitOfWork.Repository<ProjectBoxType>().AddAsync(newBoxType, cancellationToken);
                    await UnitOfWork.CompleteAsync(cancellationToken); // Save to get BoxTypeId

                    // Track this new box type for template auto-assignment
                    _newBoxTypeIds.Add(newBoxType.Id);

                    // Add SubTypes for new type
                    if (dto.SubTypes.Any())
                    {
                        await ProcessSubTypesAsync(newBoxType.Id, dto.SubTypes, cancellationToken);
                    }
                }
            }

            // Create "Loose Element" box type if it doesn't exist
            await CreateLooseElementBoxTypeIfNeededAsync(projectId, existingBoxTypes, cancellationToken);

            // Auto-assign project templates to new box types
            if (_newBoxTypeIds.Any())
            {
                await _autoAssignmentService.AssignProjectTemplatesToNewBoxTypesAsync(
                    projectId,
                    _newBoxTypeIds,
                    cancellationToken);
            }

            return _newBoxTypeIds;
        }
        catch (Exception ex)
        {
            LogError($"Error updating or adding box types for project {projectId}", ex);
            throw;
        }
    }

    private async Task ProcessSubTypesAsync(
        int boxTypeId,
        List<ProjectBoxSubTypeDto> subTypeDtos,
        CancellationToken cancellationToken)
    {
        var existingSubTypes = await UnitOfWork.Repository<ProjectBoxSubType>()
            .FindAsync(st => st.ProjectBoxTypeId == boxTypeId, cancellationToken);

        foreach (var (subDto, subIndex) in subTypeDtos.Select((subDto, subIndex) => (subDto, subIndex)))
        {
            var existingSub = existingSubTypes.FirstOrDefault(s => s.SubTypeName == subDto.SubTypeName);
            if (existingSub != null)
            {
                existingSub.Abbreviation = TextTransformHelper.ToUpperCase(subDto.Abbreviation);
                existingSub.DisplayOrder = subIndex;
                existingSub.IsActive = true;
                UnitOfWork.Repository<ProjectBoxSubType>().Update(existingSub);
            }
            else
            {
                var newSubType = new ProjectBoxSubType
                {
                    ProjectBoxTypeId = boxTypeId,
                    SubTypeName = TextTransformHelper.ToUpperCase(subDto.SubTypeName),
                    Abbreviation = TextTransformHelper.ToUpperCase(subDto.Abbreviation),
                    DisplayOrder = subIndex,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                };
                await UnitOfWork.Repository<ProjectBoxSubType>().AddAsync(newSubType, cancellationToken);
            }
        }
    }

    private async Task CreateLooseElementBoxTypeIfNeededAsync(
        Guid projectId,
        IEnumerable<ProjectBoxType> existingBoxTypes,
        CancellationToken cancellationToken)
    {
        var looseElementExists = existingBoxTypes.FirstOrDefault(bt => bt.TypeName == "Loose Element");
        if (looseElementExists == null)
        {
            var looseElementBoxType = new ProjectBoxType
            {
                ProjectId = projectId,
                TypeName = "Loose Element",
                Abbreviation = "LE",
                HasSubTypes = false,
                DisplayOrder = 1,
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            };
            await UnitOfWork.Repository<ProjectBoxType>().AddAsync(looseElementBoxType, cancellationToken);
            await UnitOfWork.CompleteAsync(cancellationToken);
            _newBoxTypeIds.Add(looseElementBoxType.Id);
        }
    }
}
