using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Dubox.Domain.Helpers;
using Mapster;
using MediatR;
using System.Linq;

namespace Dubox.Application.Features.Projects.Commands;

public class SaveProjectConfigurationCommandHandler 
    : IRequestHandler<SaveProjectConfigurationCommand, Result<ProjectConfigurationDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public SaveProjectConfigurationCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ProjectConfigurationDto>> Handle(SaveProjectConfigurationCommand request, CancellationToken cancellationToken)
    {
        // Verify project exists
        var project = await _unitOfWork.Repository<Project>()
            .GetByIdAsync(request.ProjectId, cancellationToken);

        if (project == null)
            return Result.Failure<ProjectConfigurationDto>("Project not found");

        // Get all boxes for this project to check for usage
        var projectBoxes = await _unitOfWork.Repository<Box>()
            .FindAsync(b => b.ProjectId == request.ProjectId, cancellationToken);

        var cannotDeleteItems = new List<string>();

        // Check Buildings
        var existingBuildings = await _unitOfWork.Repository<ProjectBuilding>()
            .FindAsync(b => b.ProjectId == request.ProjectId, cancellationToken);
        
        var buildingsToDelete = existingBuildings
            .Where(eb => !request.Buildings.Any(rb => rb.BuildingCode == eb.BuildingCode))
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
                _unitOfWork.Repository<ProjectBuilding>().Delete(building);
            }
        }

        // Check Levels
        var existingLevels = await _unitOfWork.Repository<ProjectLevel>()
            .FindAsync(l => l.ProjectId == request.ProjectId, cancellationToken);
        
        var levelsToDelete = existingLevels
            .Where(el => !request.Levels.Any(rl => rl.LevelCode == el.LevelCode))
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
                _unitOfWork.Repository<ProjectLevel>().Delete(level);
            }
        }

        // Check Box Types and SubTypes
        var existingBoxTypes = await _unitOfWork.Repository<ProjectBoxType>()
            .FindAsync(t => t.ProjectId == request.ProjectId, cancellationToken);
        
        var boxTypesToDelete = existingBoxTypes
            .Where(et => !request.BoxTypes.Any(rt => rt.TypeName == et.TypeName))
            .ToList();

        foreach (var boxType in boxTypesToDelete)
        {
            var isUsed = projectBoxes.Any(box => box.ProjectBoxTypeId == boxType.Id);
            if (isUsed)
            {
                cannotDeleteItems.Add($"Box Type '{boxType.TypeName}' (used by {projectBoxes.Count(b => b.BoxType.TypeName == boxType.TypeName)} box(es))");
            }
            else
            {
                _unitOfWork.Repository<ProjectBoxType>().Delete(boxType);
            }
        }

        // Check SubTypes for remaining box types
        foreach (var existingType in existingBoxTypes.Where(t => !boxTypesToDelete.Contains(t)))
        {
            var requestType = request.BoxTypes.FirstOrDefault(rt => rt.TypeName == existingType.TypeName);
            if (requestType != null && existingType.SubTypes != null)
            {
                var subTypesToDelete = existingType.SubTypes
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
                        _unitOfWork.Repository<ProjectBoxSubType>().Delete(subType);
                    }
                }
            }
        }

        // Check Zones
        var existingZones = await _unitOfWork.Repository<ProjectZone>()
            .FindAsync(z => z.ProjectId == request.ProjectId, cancellationToken);
        
        var zonesToDelete = existingZones
            .Where(ez => !request.Zones.Any(rz => rz.ZoneCode == ez.ZoneCode))
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
                _unitOfWork.Repository<ProjectZone>().Delete(zone);
            }
        }

        // Check Box Functions
        var existingFunctions = await _unitOfWork.Repository<ProjectBoxFunction>()
            .FindAsync(f => f.ProjectId == request.ProjectId, cancellationToken);
        
        var functionsToDelete = existingFunctions
            .Where(ef => !request.BoxFunctions.Any(rf => rf.FunctionName == ef.FunctionName))
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
                _unitOfWork.Repository<ProjectBoxFunction>().Delete(function);
            }
        }

        // If there are items that cannot be deleted, return error
        if (cannotDeleteItems.Any())
        {
            var errorMessage = "The following configuration items cannot be removed because they are currently in use by boxes:\n\n" +
                string.Join("\n", cannotDeleteItems) +
                "\n\nPlease update or remove the associated boxes before removing these configuration items.";
            
            return Result.Failure<ProjectConfigurationDto>(errorMessage);
        }

        // Update or Add Buildings
        foreach (var (dto, index) in request.Buildings.Select((dto, index) => (dto, index)))
        {
            var existing = existingBuildings.FirstOrDefault(b => b.BuildingCode == dto.BuildingCode);
            if (existing != null)
            {
                existing.BuildingName = TextTransformHelper.ToUpperCase(dto.BuildingName);
                existing.DisplayOrder = index;
                existing.IsActive = true;
                _unitOfWork.Repository<ProjectBuilding>().Update(existing);
            }
            else
            {
                var newBuilding = new ProjectBuilding
                {
                    ProjectId = request.ProjectId,
                    BuildingCode = TextTransformHelper.ToUpperCase(dto.BuildingCode),
                    BuildingName = TextTransformHelper.ToUpperCase(dto.BuildingName),
                    DisplayOrder = index,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                };
                await _unitOfWork.Repository<ProjectBuilding>().AddAsync(newBuilding, cancellationToken);
            }
        }

        // Update or Add Levels
        foreach (var (dto, index) in request.Levels.Select((dto, index) => (dto, index)))
        {
            var existing = existingLevels.FirstOrDefault(l => l.LevelCode == dto.LevelCode);
            if (existing != null)
            {
                existing.LevelName = TextTransformHelper.ToUpperCase(dto.LevelName);
                existing.DisplayOrder = index;
                existing.IsActive = true;
                _unitOfWork.Repository<ProjectLevel>().Update(existing);
            }
            else
            {
                var newLevel = new ProjectLevel
                {
                    ProjectId = request.ProjectId,
                    LevelCode = TextTransformHelper.ToUpperCase(dto.LevelCode),
                    LevelName = TextTransformHelper.ToUpperCase(dto.LevelName),
                    DisplayOrder = index,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                };
                await _unitOfWork.Repository<ProjectLevel>().AddAsync(newLevel, cancellationToken);
            }
        }

        // Update or Add Box Types
        foreach (var (dto, index) in request.BoxTypes.Select((dto, index) => (dto, index)))
        {
            var existing = existingBoxTypes.FirstOrDefault(t => t.TypeName == dto.TypeName);
            if (existing != null)
            {
                existing.Abbreviation = TextTransformHelper.ToUpperCase(dto.Abbreviation);
                existing.HasSubTypes = dto.HasSubTypes;
                existing.DisplayOrder = index;
                existing.IsActive = true;
                _unitOfWork.Repository<ProjectBoxType>().Update(existing);

                // Handle SubTypes for existing type
                if (dto.SubTypes.Any())
                {
                    var existingSubTypes = await _unitOfWork.Repository<ProjectBoxSubType>()
                        .FindAsync(st => st.ProjectBoxTypeId == existing.Id, cancellationToken);

                    foreach (var (subDto, subIndex) in dto.SubTypes.Select((subDto, subIndex) => (subDto, subIndex)))
                    {
                        var existingSub = existingSubTypes.FirstOrDefault(s => s.SubTypeName == subDto.SubTypeName);
                        if (existingSub != null)
                        {
                            existingSub.Abbreviation = TextTransformHelper.ToUpperCase(subDto.Abbreviation);
                            existingSub.DisplayOrder = subIndex;
                            existingSub.IsActive = true;
                            _unitOfWork.Repository<ProjectBoxSubType>().Update(existingSub);
                        }
                        else
                        {
                            var newSubType = new ProjectBoxSubType
                            {
                                ProjectBoxTypeId = existing.Id,
                                SubTypeName = TextTransformHelper.ToUpperCase(subDto.SubTypeName),
                                Abbreviation = TextTransformHelper.ToUpperCase(subDto.Abbreviation),
                                DisplayOrder = subIndex,
                                IsActive = true,
                                CreatedDate = DateTime.UtcNow
                            };
                            await _unitOfWork.Repository<ProjectBoxSubType>().AddAsync(newSubType, cancellationToken);
                        }
                    }
                }
            }
            else
            {
                var newBoxType = new ProjectBoxType
                {
                    ProjectId = request.ProjectId,
                    TypeName = TextTransformHelper.ToUpperCase(dto.TypeName),
                    Abbreviation = TextTransformHelper.ToUpperCase(dto.Abbreviation),
                    HasSubTypes = dto.HasSubTypes,
                    DisplayOrder = index,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                };
                await _unitOfWork.Repository<ProjectBoxType>().AddAsync(newBoxType, cancellationToken);
                await _unitOfWork.CompleteAsync(cancellationToken); // Save to get BoxTypeId

                // Add SubTypes for new type
                if (dto.SubTypes.Any())
                {
                    foreach (var (subDto, subIndex) in dto.SubTypes.Select((subDto, subIndex) => (subDto, subIndex)))
                    {
                        var newSubType = new ProjectBoxSubType
                        {
                            ProjectBoxTypeId = newBoxType.Id,
                            SubTypeName = TextTransformHelper.ToUpperCase(subDto.SubTypeName),
                            Abbreviation = TextTransformHelper.ToUpperCase(subDto.Abbreviation),
                            DisplayOrder = subIndex,
                            IsActive = true,
                            CreatedDate = DateTime.UtcNow
                        };
                        await _unitOfWork.Repository<ProjectBoxSubType>().AddAsync(newSubType, cancellationToken);
                    }
                }
            }
        }

        // Update or Add Zones
        foreach (var (dto, index) in request.Zones.Select((dto, index) => (dto, index)))
        {
            var existing = existingZones.FirstOrDefault(z => z.ZoneCode == dto.ZoneCode);
            if (existing != null)
            {
                existing.ZoneName = TextTransformHelper.ToTitleCase(dto.ZoneName);
                existing.DisplayOrder = index;
                existing.IsActive = true;
                _unitOfWork.Repository<ProjectZone>().Update(existing);
            }
            else
            {
                var newZone = new ProjectZone
                {
                    ProjectId = request.ProjectId,
                    ZoneCode = TextTransformHelper.ToTitleCase(dto.ZoneCode),
                    ZoneName = TextTransformHelper.ToTitleCase(dto.ZoneName),
                    DisplayOrder = index,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                };
                await _unitOfWork.Repository<ProjectZone>().AddAsync(newZone, cancellationToken);
            }
        }

        // Update or Add Box Functions
        foreach (var (dto, index) in request.BoxFunctions.Select((dto, index) => (dto, index)))
        {
            var existing = existingFunctions.FirstOrDefault(f => f.FunctionName == dto.FunctionName);
            if (existing != null)
            {
                existing.Description = TextTransformHelper.ToTitleCase(dto.Description);
                existing.DisplayOrder = index;
                existing.IsActive = true;
                _unitOfWork.Repository<ProjectBoxFunction>().Update(existing);
            }
            else
            {
                var newFunction = new ProjectBoxFunction
                {
                    ProjectId = request.ProjectId,
                    FunctionName = TextTransformHelper.ToTitleCase(dto.FunctionName),
                    Description = TextTransformHelper.ToTitleCase(dto.Description),
                    DisplayOrder = index,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                };
                await _unitOfWork.Repository<ProjectBoxFunction>().AddAsync(newFunction, cancellationToken);
            }
        }

        await _unitOfWork.CompleteAsync(cancellationToken);

        // Fetch updated configurations for response
        var updatedBuildings = await _unitOfWork.Repository<ProjectBuilding>()
            .FindAsync(b => b.ProjectId == request.ProjectId && b.IsActive, cancellationToken);
        
        var updatedLevels = await _unitOfWork.Repository<ProjectLevel>()
            .FindAsync(l => l.ProjectId == request.ProjectId && l.IsActive, cancellationToken);
        
        var updatedBoxTypes = await _unitOfWork.Repository<ProjectBoxType>()
            .FindAsync(t => t.ProjectId == request.ProjectId && t.IsActive, cancellationToken);
        
        var updatedZones = await _unitOfWork.Repository<ProjectZone>()
            .FindAsync(z => z.ProjectId == request.ProjectId && z.IsActive, cancellationToken);
        
        var updatedFunctions = await _unitOfWork.Repository<ProjectBoxFunction>()
            .FindAsync(f => f.ProjectId == request.ProjectId && f.IsActive, cancellationToken);

        // Prepare response
        var response = new ProjectConfigurationDto
        {
            ProjectId = request.ProjectId,
            Buildings = updatedBuildings.OrderBy(b => b.DisplayOrder).ToList().Adapt<List<ProjectBuildingDto>>(),
            Levels = updatedLevels.OrderBy(l => l.DisplayOrder).ToList().Adapt<List<ProjectLevelDto>>(),
            BoxTypes = updatedBoxTypes.OrderBy(t => t.DisplayOrder).ToList().Adapt<List<ProjectBoxTypeDto>>(),
            Zones = updatedZones.OrderBy(z => z.DisplayOrder).ToList().Adapt<List<ProjectZoneDto>>(),
            BoxFunctions = updatedFunctions.OrderBy(f => f.DisplayOrder).ToList().Adapt<List<ProjectBoxFunctionDto>>()
        };

        return Result.Success(response);
    }
}

