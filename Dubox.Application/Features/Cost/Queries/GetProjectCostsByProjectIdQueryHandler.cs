using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Cost.Queries;

public class GetProjectCostsByProjectIdQueryHandler : IRequestHandler<GetProjectCostsByProjectIdQuery, Result<List<ProjectCostDto>>>
{
    private readonly IDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public GetProjectCostsByProjectIdQueryHandler(IDbContext context, IUnitOfWork unitOfWork)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<ProjectCostDto>>> Handle(GetProjectCostsByProjectIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Verify project exists
            var projectExists = await _context.Projects
                .AnyAsync(p => p.ProjectId == request.ProjectId, cancellationToken);

            if (!projectExists)
                return Result.Failure<List<ProjectCostDto>>("Project not found.");

            var specification = new ProjectCostSpecification(request);

            var projectCosts= _unitOfWork.Repository<ProjectCost>().GetWithSpec(specification).Data.ToList();
            List<ProjectCostDto> dtos = new List<ProjectCostDto>();
            foreach (var projectCostDto in projectCosts)
            {
                var dto = projectCostDto.Adapt<ProjectCostDto>() with
                {
                    BoxTag= projectCostDto.Box.BoxTag,
                    BoxSerialNumber= projectCostDto.Box.SerialNumber,
                    HRCostCode = projectCostDto.HRCost?.Code,
                    HRCostName = projectCostDto.HRCost?.Name
                };

                dtos.Add(dto);  
            }

            return Result.Success(dtos);
        }
        catch (Exception ex)
        {
            return Result.Failure<List<ProjectCostDto>>(new Error("QueryFailed", $"Failed to retrieve project costs: {ex.Message}"));
        }
    }
}



