using Dubox.Application.DTOs;
using Dubox.Application.Services;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Boxes.Queries;

public class GetBoxesByProjectQueryHandler : IRequestHandler<GetBoxesByProjectQuery, Result<PaginatedBoxesResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProjectTeamVisibilityService _visibilityService;
    private readonly IBoxMapper _boxMapper;

    public GetBoxesByProjectQueryHandler(IUnitOfWork unitOfWork, IProjectTeamVisibilityService visibilityService, IBoxMapper boxMapper)
    {
        _unitOfWork = unitOfWork;
        _visibilityService = visibilityService;
        _boxMapper = boxMapper;
    }

    public async Task<Result<PaginatedBoxesResponseDto>> Handle(GetBoxesByProjectQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Verify user has access to the requested project
            var canAccessProject = await _visibilityService.CanAccessProjectAsync(request.ProjectId, cancellationToken);
            if (!canAccessProject)
            {
                return Result.Failure<PaginatedBoxesResponseDto>("Access denied. You do not have permission to view boxes for this project.");
            }

            // Validate pagination parameters
            if (request.Page < 1)
            {
                return Result.Failure<PaginatedBoxesResponseDto>("Page number must be greater than 0.");
            }

            if (request.PageSize < 1 || request.PageSize > 100)
            {
                return Result.Failure<PaginatedBoxesResponseDto>("Page size must be between 1 and 100.");
            }

            var hasFilters = (request.Statuses != null && request.Statuses.Count > 0)
                || !string.IsNullOrWhiteSpace(request.BoxType)
                || !string.IsNullOrWhiteSpace(request.BoxSubType)
                || !string.IsNullOrWhiteSpace(request.BuildingNumber)
                || !string.IsNullOrWhiteSpace(request.Floor)
                || !string.IsNullOrWhiteSpace(request.Zone)
                || !string.IsNullOrWhiteSpace(request.Search);

            // Get total count first (more efficient than loading all data)
            var allBoxesQuery = hasFilters
                ? _unitOfWork.Repository<Box>().GetWithSpec(new GetBoxesByProjectWithFiltersSpecification(
                    request.ProjectId,
                    request.Statuses,
                    request.BoxType,
                    request.BoxSubType,
                    request.BuildingNumber,
                    request.Floor,
                    request.Zone,
                    request.Search)).Data
                : _unitOfWork.Repository<Box>().GetWithSpec(new GetBoxesByProjectIdSpecification(request.ProjectId)).Data;

            var totalCount = allBoxesQuery.Count();

            // Calculate status counts
            var statusCounts = new BoxStatusCounts
            {
                NotStarted = allBoxesQuery.Count(b => b.Status == BoxStatusEnum.NotStarted),
                ReadyToStart = allBoxesQuery.Count(b => b.Status == BoxStatusEnum.ReadyToStart),
                InProgress = allBoxesQuery.Count(b => b.Status == BoxStatusEnum.InProgress),
                Completed = allBoxesQuery.Count(b => b.Status == BoxStatusEnum.Completed),
                OnHold = allBoxesQuery.Count(b => b.Status == BoxStatusEnum.OnHold),
                Delayed = allBoxesQuery.Count(b => b.Status == BoxStatusEnum.Delayed),
                Dispatched = allBoxesQuery.Count(b => b.Status == BoxStatusEnum.Dispatched)
            };

            // Calculate total pages
            var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

            // Return empty result if no boxes found
            if (totalCount == 0)
            {
                return Result.Success(new PaginatedBoxesResponseDto
                {
                    Items = new List<BoxDto>(),
                    TotalCount = 0,
                    Page = request.Page,
                    PageSize = request.PageSize,
                    TotalPages = 0,
                    HasPreviousPage = false,
                    HasNextPage = false,
                    StatusCounts = statusCounts
                });
            }

            // If countOnly is true, return just the count and status counts without loading boxes
            if (request.CountOnly)
            {
                return Result.Success(new PaginatedBoxesResponseDto
                {
                    Items = new List<BoxDto>(),
                    TotalCount = totalCount,
                    Page = request.Page,
                    PageSize = request.PageSize,
                    TotalPages = totalPages,
                    HasPreviousPage = request.Page > 1,
                    HasNextPage = request.Page < totalPages,
                    StatusCounts = statusCounts
                });
            }

            // Get paginated boxes with Skip and Take
            var boxes = hasFilters
                ? _unitOfWork.Repository<Box>().GetWithSpec(new GetBoxesByProjectWithFiltersSpecification(
                    request.ProjectId,
                    request.Statuses,
                    request.BoxType,
                    request.BoxSubType,
                    request.BuildingNumber,
                    request.Floor,
                    request.Zone,
                    request.Search))
                    .Data
                    .OrderBy(b => b.CreatedDate) // Required for Skip/Take with split query
                    .ThenBy(b => b.BoxId)
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToList()
                : _unitOfWork.Repository<Box>().GetWithSpec(new GetBoxesByProjectIdSpecification(request.ProjectId))
                    .Data
                    .OrderBy(b => b.CreatedDate) // Required for Skip/Take with split query
                    .ThenBy(b => b.BoxId)
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToList();

            // Performance optimization: Pre-load all box types and subtypes to avoid N+1 queries
            var boxTypeIds = boxes.Where(b => b.ProjectBoxTypeId.HasValue)
                .Select(b => b.ProjectBoxTypeId!.Value)
                .Distinct()
                .ToList();

            var boxSubTypeIds = boxes.Where(b => b.ProjectBoxSubTypeId.HasValue)
                .Select(b => b.ProjectBoxSubTypeId!.Value)
                .Distinct()
                .ToList();

            var boxTypesDictionary = boxTypeIds.Any()
                ? _unitOfWork.Repository<ProjectBoxType>()
                    .Get()
                    .Where(pbt => boxTypeIds.Contains(pbt.Id) && pbt.ProjectId == request.ProjectId)
                    .ToDictionary(pbt => pbt.Id, pbt => pbt.TypeName)
                : new Dictionary<int, string>();

            var boxSubTypesDictionary = boxSubTypeIds.Any()
                ? _unitOfWork.Repository<ProjectBoxSubType>()
                    .Get()
                    .Where(pbst => boxSubTypeIds.Contains(pbst.Id))
                    .ToDictionary(pbst => pbst.Id, pbst => pbst.SubTypeName)
                : new Dictionary<int, string>();

            var boxDtos = new List<BoxDto>();

            foreach (var box in boxes)
            {
                try
                {
                    var dto = _boxMapper.Map(box, boxTypesDictionary, boxSubTypesDictionary);
                    dto.DrawingsCount = box.BoxDrawings.Count;
                    boxDtos.Add(dto);
                }
                catch (Exception ex)
                {
                    // Log the error for this specific box
                    return Result.Failure<PaginatedBoxesResponseDto>($"Error mapping box {box.BoxId}: {ex.Message}. Inner exception: {ex.InnerException?.Message}. Stack trace: {ex.StackTrace}");
                }
            }

            var response = new PaginatedBoxesResponseDto
            {
                Items = boxDtos,
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize,
                TotalPages = totalPages,
                HasPreviousPage = request.Page > 1,
                HasNextPage = request.Page < totalPages,
                StatusCounts = statusCounts
            };

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            return Result.Failure<PaginatedBoxesResponseDto>($"Error in GetBoxesByProjectQueryHandler: {ex.Message}. Inner exception: {ex.InnerException?.Message}. Stack trace: {ex.StackTrace}");
        }
    }


}

