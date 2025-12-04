using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.ProgressUpdates.Queries;

public class GetProgressUpdatesByBoxQueryHandler : IRequestHandler<GetProgressUpdatesByBoxQuery, Result<PaginatedProgressUpdatesResponseDto>>
{
    private readonly IDbContext _dbContext;
    private readonly IUnitOfWork _unitOfWork;

    public GetProgressUpdatesByBoxQueryHandler(IDbContext dbContext, IUnitOfWork unitOfWork)
    {
        _dbContext = dbContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PaginatedProgressUpdatesResponseDto>> Handle(GetProgressUpdatesByBoxQuery request, CancellationToken cancellationToken)
    {
        // Validate pagination parameters
        var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
        var pageSize = request.PageSize < 1 ? 10 : (request.PageSize > 100 ? 100 : request.PageSize);

        // Create query with validated pagination
        var query = request with
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var specification = new GetProgressUpdatesByBoxSpecification(query);
        var updatesResult = _unitOfWork.Repository<ProgressUpdate>().GetWithSpec(specification);
        var updates = updatesResult.Data.ToList();
        var totalCount = updatesResult.Count;

        var updateDtos = updates.Select(u =>
        {
            var dto = u.Adapt<ProgressUpdateDto>();
            return dto with
            {
                BoxTag = u.Box.BoxTag,
                ActivityName = u.BoxActivity.ActivityMaster.ActivityName,
                UpdatedByName = u.UpdatedByUser.FullName ?? u.UpdatedByUser.Email,
                Photo = u.Photo // Explicitly map Photo to PhotoUrls
            };
        }).ToList();

        // Calculate total pages
        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        var response = new PaginatedProgressUpdatesResponseDto
        {
            Items = updateDtos,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPages = totalPages
        };

        return Result.Success(response);
    }
}

