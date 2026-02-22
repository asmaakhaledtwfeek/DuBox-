using Dubox.Application.Abstractions;
using Dubox.Application.DTOs;
using Dubox.Application.Features.Projects.Processors;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dubox.Application.Features.Projects.Commands;

public class SaveProjectConfigurationCommandHandler 
    : IRequestHandler<SaveProjectConfigurationCommand, Result<ProjectConfigurationDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfigurationProcessor<ProjectBuilding, ProjectBuildingDto> _buildingProcessor;
    private readonly IConfigurationProcessor<ProjectLevel, ProjectLevelDto> _levelProcessor;
    private readonly IConfigurationProcessor<ProjectBoxType, ProjectBoxTypeDto> _boxTypeProcessor;
    private readonly IConfigurationProcessor<ProjectZone, ProjectZoneDto> _zoneProcessor;
    private readonly IConfigurationProcessor<ProjectBoxFunction, ProjectBoxFunctionDto> _boxFunctionProcessor;
    private readonly ILogger<SaveProjectConfigurationCommandHandler> _logger;

    public SaveProjectConfigurationCommandHandler(
        IUnitOfWork unitOfWork,
        IConfigurationProcessor<ProjectBuilding, ProjectBuildingDto> buildingProcessor,
        IConfigurationProcessor<ProjectLevel, ProjectLevelDto> levelProcessor,
        IConfigurationProcessor<ProjectBoxType, ProjectBoxTypeDto> boxTypeProcessor,
        IConfigurationProcessor<ProjectZone, ProjectZoneDto> zoneProcessor,
        IConfigurationProcessor<ProjectBoxFunction, ProjectBoxFunctionDto> boxFunctionProcessor,
        ILogger<SaveProjectConfigurationCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _buildingProcessor = buildingProcessor;
        _levelProcessor = levelProcessor;
        _boxTypeProcessor = boxTypeProcessor;
        _zoneProcessor = zoneProcessor;
        _boxFunctionProcessor = boxFunctionProcessor;
        _logger = logger;
    }

    public async Task<Result<ProjectConfigurationDto>> Handle(SaveProjectConfigurationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Verify project exists
            var project = await _unitOfWork.Repository<Project>()
                .GetByIdAsync(request.ProjectId, cancellationToken);

            if (project == null)
                return Result.Failure<ProjectConfigurationDto>("Project not found");

            // Begin transaction - all operations must succeed or all rollback
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                // Get all boxes for this project to check for usage
                var projectBoxes = await _unitOfWork.Repository<Box>()
                    .FindAsync(b => b.ProjectId == request.ProjectId, cancellationToken);

                // Phase 1: Validate and Delete (check usage before making changes)
                var cannotDeleteItems = new List<string>();

                var buildingErrors = await _buildingProcessor.ValidateAndDeleteAsync(
                    request.ProjectId, request.Buildings, projectBoxes, cancellationToken);
                cannotDeleteItems.AddRange(buildingErrors);

                var levelErrors = await _levelProcessor.ValidateAndDeleteAsync(
                    request.ProjectId, request.Levels, projectBoxes, cancellationToken);
                cannotDeleteItems.AddRange(levelErrors);

                var boxTypeErrors = await _boxTypeProcessor.ValidateAndDeleteAsync(
                    request.ProjectId, request.BoxTypes, projectBoxes, cancellationToken);
                cannotDeleteItems.AddRange(boxTypeErrors);

                var zoneErrors = await _zoneProcessor.ValidateAndDeleteAsync(
                    request.ProjectId, request.Zones, projectBoxes, cancellationToken);
                cannotDeleteItems.AddRange(zoneErrors);

                var functionErrors = await _boxFunctionProcessor.ValidateAndDeleteAsync(
                    request.ProjectId, request.BoxFunctions, projectBoxes, cancellationToken);
                cannotDeleteItems.AddRange(functionErrors);

                // If there are items that cannot be deleted, rollback and return error
                if (cannotDeleteItems.Any())
                {
                    await _unitOfWork.RollbackTransactionAsync(cancellationToken);

                    var errorMessage = "The following configuration items cannot be removed because they are currently in use by boxes:\n\n" +
                        string.Join("\n", cannotDeleteItems) +
                        "\n\nPlease update or remove the associated boxes before removing these configuration items.";

                    return Result.Failure<ProjectConfigurationDto>(errorMessage);
                }

                // Phase 2: Update or Add (all validations passed)
                await _buildingProcessor.UpdateOrAddAsync(
                    request.ProjectId, request.Buildings, cancellationToken);

                await _levelProcessor.UpdateOrAddAsync(
                    request.ProjectId, request.Levels, cancellationToken);

                // BoxTypeProcessor handles SubTypes and Loose Element creation internally
                await _boxTypeProcessor.UpdateOrAddAsync(
                    request.ProjectId, request.BoxTypes, cancellationToken);

                await _zoneProcessor.UpdateOrAddAsync(
                    request.ProjectId, request.Zones, cancellationToken);

                await _boxFunctionProcessor.UpdateOrAddAsync(
                    request.ProjectId, request.BoxFunctions, cancellationToken);

                // Save all changes
                await _unitOfWork.CompleteAsync(cancellationToken);

                // Commit transaction
                await _unitOfWork.CommitTransactionAsync(cancellationToken);

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
            catch (Exception)
            {
                // Rollback transaction on any error
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving project configuration for project {ProjectId}", request.ProjectId);
            return Result.Failure<ProjectConfigurationDto>($"An error occurred while saving project configuration: {ex.Message}");
        }
    }
}

