using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Teams.Queries;

public class GetAllTeamGroupsQueryHandler : IRequestHandler<GetAllTeamGroupsQuery, Result<PaginatedTeamGroupsResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllTeamGroupsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PaginatedTeamGroupsResponseDto>> Handle(GetAllTeamGroupsQuery request, CancellationToken cancellationToken)
    {
        // Normalize pagination parameters
        var (page, pageSize) = new PaginatedRequest
        {
            Page = request.Page,
            PageSize = request.PageSize
        }.GetNormalizedPagination();

        // Create specification with filters and pagination
        var specification = new GetTeamGroupWithIncludesSpecification(
            request.Search,
            request.TeamId,
            request.IsActive,
            pageSize,
            page
        );

        // Get team groups with specification
        var teamGroupsResult = _unitOfWork.Repository<TeamGroup>()
            .GetWithSpec(specification);

        var teamGroups = await teamGroupsResult.Data.ToListAsync(cancellationToken);
        var totalCount = teamGroupsResult.Count;

        // Map to DTOs
        var teamGroupDtos = teamGroups.Adapt<List<TeamGroupDto>>();

        // Calculate total pages
        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        // Create paginated response
        var response = new PaginatedTeamGroupsResponseDto
        {
            Items = teamGroupDtos,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages
        };

        return Result.Success(response);
    }
}

