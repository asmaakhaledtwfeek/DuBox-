using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.Projects.Commands;

public class SaveProjectConfigurationCommandHandler 
    : IRequestHandler<SaveProjectConfigurationCommand, Result<ProjectConfigurationDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public SaveProjectConfigurationCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ProjectConfigurationDto>> Handle(
        SaveProjectConfigurationCommand request, 
        CancellationToken cancellationToken)
    {
        // Verify project exists
        var project = await _unitOfWork.Repository<Project>()
            .GetByIdAsync(request.ProjectId, cancellationToken);

        if (project == null)
            return Result.Failure<ProjectConfigurationDto>("Project not found");

        // Clear existing configurations
        var existingBuildings = await _unitOfWork.Repository<ProjectBuilding>()
            .FindAsync(b => b.ProjectId == request.ProjectId, cancellationToken);
        foreach (var building in existingBuildings)
            _unitOfWork.Repository<ProjectBuilding>().Delete(building);

        var existingLevels = await _unitOfWork.Repository<ProjectLevel>()
            .FindAsync(l => l.ProjectId == request.ProjectId, cancellationToken);
        foreach (var level in existingLevels)
            _unitOfWork.Repository<ProjectLevel>().Delete(level);

        var existingBoxTypes = await _unitOfWork.Repository<ProjectBoxType>()
            .FindAsync(t => t.ProjectId == request.ProjectId, cancellationToken);
        foreach (var type in existingBoxTypes)
            _unitOfWork.Repository<ProjectBoxType>().Delete(type);

        var existingZones = await _unitOfWork.Repository<ProjectZone>()
            .FindAsync(z => z.ProjectId == request.ProjectId, cancellationToken);
        foreach (var zone in existingZones)
            _unitOfWork.Repository<ProjectZone>().Delete(zone);

        var existingFunctions = await _unitOfWork.Repository<ProjectBoxFunction>()
            .FindAsync(f => f.ProjectId == request.ProjectId, cancellationToken);
        foreach (var function in existingFunctions)
            _unitOfWork.Repository<ProjectBoxFunction>().Delete(function);

        // Add new configurations
        var newBuildings = request.Buildings.Select((dto, index) => new ProjectBuilding
        {
            ProjectId = request.ProjectId,
            BuildingCode = dto.BuildingCode,
            BuildingName = dto.BuildingName,
            DisplayOrder = index,
            IsActive = true,
            CreatedDate = DateTime.UtcNow
        }).ToList();

        var newLevels = request.Levels.Select((dto, index) => new ProjectLevel
        {
            ProjectId = request.ProjectId,
            LevelCode = dto.LevelCode,
            LevelName = dto.LevelName,
            DisplayOrder = index,
            IsActive = true,
            CreatedDate = DateTime.UtcNow
        }).ToList();

        var newBoxTypes = request.BoxTypes.Select((dto, index) => new ProjectBoxType
        {
            ProjectId = request.ProjectId,
            TypeName = dto.TypeName,
            Abbreviation = dto.Abbreviation,
            HasSubTypes = dto.HasSubTypes,
            DisplayOrder = index,
            IsActive = true,
            CreatedDate = DateTime.UtcNow
        }).ToList();

        // Handle sub-types
        var subTypesList = new List<ProjectBoxSubType>();
        foreach (var typeDto in request.BoxTypes)
        {
            var parentType = newBoxTypes.FirstOrDefault(t => t.TypeName == typeDto.TypeName);
            if (parentType != null && typeDto.SubTypes.Any())
            {
                var subTypes = typeDto.SubTypes.Select((subDto, index) => new ProjectBoxSubType
                {
                    SubTypeName = subDto.SubTypeName,
                    Abbreviation = subDto.Abbreviation,
                    DisplayOrder = index,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                }).ToList();

                parentType.SubTypes = subTypes;
                subTypesList.AddRange(subTypes);
            }
        }

        var newZones = request.Zones.Select((dto, index) => new ProjectZone
        {
            ProjectId = request.ProjectId,
            ZoneCode = dto.ZoneCode,
            ZoneName = dto.ZoneName,
            DisplayOrder = index,
            IsActive = true,
            CreatedDate = DateTime.UtcNow
        }).ToList();

        var newFunctions = request.BoxFunctions.Select((dto, index) => new ProjectBoxFunction
        {
            ProjectId = request.ProjectId,
            FunctionName = dto.FunctionName,
            Description = dto.Description,
            DisplayOrder = index,
            IsActive = true,
            CreatedDate = DateTime.UtcNow
        }).ToList();

        // Add to repository
        foreach (var building in newBuildings)
            await _unitOfWork.Repository<ProjectBuilding>().AddAsync(building, cancellationToken);

        foreach (var level in newLevels)
            await _unitOfWork.Repository<ProjectLevel>().AddAsync(level, cancellationToken);

        foreach (var type in newBoxTypes)
            await _unitOfWork.Repository<ProjectBoxType>().AddAsync(type, cancellationToken);

        foreach (var zone in newZones)
            await _unitOfWork.Repository<ProjectZone>().AddAsync(zone, cancellationToken);

        foreach (var function in newFunctions)
            await _unitOfWork.Repository<ProjectBoxFunction>().AddAsync(function, cancellationToken);

        await _unitOfWork.CompleteAsync(cancellationToken);

        // Prepare response
        var response = new ProjectConfigurationDto
        {
            ProjectId = request.ProjectId,
            Buildings = newBuildings.Adapt<List<ProjectBuildingDto>>(),
            Levels = newLevels.Adapt<List<ProjectLevelDto>>(),
            BoxTypes = newBoxTypes.Adapt<List<ProjectBoxTypeDto>>(),
            Zones = newZones.Adapt<List<ProjectZoneDto>>(),
            BoxFunctions = newFunctions.Adapt<List<ProjectBoxFunctionDto>>()
        };

        return Result.Success(response);
    }
}

