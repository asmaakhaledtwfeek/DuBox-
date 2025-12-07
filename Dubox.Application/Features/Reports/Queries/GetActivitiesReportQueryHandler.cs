using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Reports.Queries;

public class GetActivitiesReportQueryHandler : IRequestHandler<GetActivitiesReportQuery, Result<PaginatedActivitiesReportResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetActivitiesReportQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PaginatedActivitiesReportResponseDto>> Handle(GetActivitiesReportQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var (page, pageSize) = new PaginatedRequest
            {
                Page = request.Page,
                PageSize = request.PageSize
            }.GetNormalizedPagination();

            var paginatedResult = _unitOfWork.Repository<Domain.Entities.BoxActivity>().GetWithSpec(new ActivitiesReportSpecification(request, enablePaging: true));

            var countSpec = new ActivitiesReportSpecification(request, enablePaging: false);
            var countResult = _unitOfWork.Repository<Domain.Entities.BoxActivity>().GetWithSpec(countSpec);
            var totalCount = countResult.Count > 0
                ? countResult.Count
                : await countResult.Data.AsNoTracking().CountAsync(cancellationToken);

            var activities = await paginatedResult.Data
                .AsNoTracking()
                .ToListAsync(cancellationToken);
            var items = activities.Adapt<List<ReportActivityDto>>();

            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            var response = new PaginatedActivitiesReportResponseDto
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages
            };

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            return Result.Failure<PaginatedActivitiesReportResponseDto>(
                $"Failed to generate activities report: {ex.Message}");
        }
    }
}

