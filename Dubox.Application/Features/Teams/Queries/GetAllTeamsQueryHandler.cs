using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using Dubox.Domain.Abstraction;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Teams.Queries;

public class GetAllTeamsQueryHandler : IRequestHandler<GetAllTeamsQuery, Result<PaginatedTeamsResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProjectTeamVisibilityService _visibilityService;

    public GetAllTeamsQueryHandler(IUnitOfWork unitOfWork, IProjectTeamVisibilityService visibilityService)
    {
        _unitOfWork = unitOfWork;
        _visibilityService = visibilityService;
    }

    public async Task<Result<PaginatedTeamsResponseDto>> Handle(GetAllTeamsQuery request, CancellationToken cancellationToken)
    {
        // Get accessible team IDs based on user role
        var accessibleTeamIds = await _visibilityService.GetAccessibleTeamIdsAsync(cancellationToken);

        // Normalize pagination parameters
        var (page, pageSize) = new PaginatedRequest
        {
            Page = request.Page,
            PageSize = request.PageSize
        }.GetNormalizedPagination();

        // Create specification with filters, visibility, and pagination
        var specification = new GetTeamWithIncludesSpecification(
            request.Search,
            request.Department,
            request.Trade,
            request.IsActive,
            accessibleTeamIds,
            pageSize,
            page
        );

        // Get teams with specification
        var teamsResult = _unitOfWork.Repository<Team>().GetWithSpec(specification);
        var teams = await teamsResult.Data.ToListAsync(cancellationToken);
        var totalCount = teamsResult.Count;

        // Map to DTOs
        var teamDtos = teams.Adapt<List<TeamDto>>();

        // Calculate total pages
        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        // Create paginated response
        var response = new PaginatedTeamsResponseDto
        {
            Items = teamDtos,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages
        };

        return Result.Success(response);
    }
}

