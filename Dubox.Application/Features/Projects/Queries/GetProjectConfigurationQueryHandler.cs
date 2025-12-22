using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Projects.Queries;

public class GetProjectConfigurationQueryHandler 
    : IRequestHandler<GetProjectConfigurationQuery, Result<ProjectConfigurationDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDbContext _context;

    public GetProjectConfigurationQueryHandler(IUnitOfWork unitOfWork, IDbContext context)
    {
        _unitOfWork = unitOfWork;
        _context = context;
    }

    public async Task<Result<ProjectConfigurationDto>> Handle(
        GetProjectConfigurationQuery request, 
        CancellationToken cancellationToken)
    {
        // Verify project exists
        var project = await _unitOfWork.Repository<Project>()
            .GetByIdAsync(request.ProjectId, cancellationToken);

        if (project == null)
            return Result.Failure<ProjectConfigurationDto>("Project not found");

        // Get all configurations
        var buildings = await _unitOfWork.Repository<ProjectBuilding>()
            .FindAsync(b => b.ProjectId == request.ProjectId, cancellationToken);

        var levels = await _unitOfWork.Repository<ProjectLevel>()
            .FindAsync(l => l.ProjectId == request.ProjectId, cancellationToken);

        // Use DbContext directly to include SubTypes
        var boxTypes = await _context.ProjectBoxTypes
            .Where(t => t.ProjectId == request.ProjectId)
            .Include(t => t.SubTypes)
            .OrderBy(t => t.DisplayOrder)
            .ToListAsync(cancellationToken);

        var zones = await _unitOfWork.Repository<ProjectZone>()
            .FindAsync(z => z.ProjectId == request.ProjectId, cancellationToken);

        var functions = await _unitOfWork.Repository<ProjectBoxFunction>()
            .FindAsync(f => f.ProjectId == request.ProjectId, cancellationToken);

        var response = new ProjectConfigurationDto
        {
            ProjectId = request.ProjectId,
            Buildings = buildings.OrderBy(b => b.DisplayOrder).Adapt<List<ProjectBuildingDto>>(),
            Levels = levels.OrderBy(l => l.DisplayOrder).Adapt<List<ProjectLevelDto>>(),
            BoxTypes = boxTypes.Adapt<List<ProjectBoxTypeDto>>(),
            Zones = zones.OrderBy(z => z.DisplayOrder).Adapt<List<ProjectZoneDto>>(),
            BoxFunctions = functions.OrderBy(f => f.DisplayOrder).Adapt<List<ProjectBoxFunctionDto>>()
        };

        return Result.Success(response);
    }
}

