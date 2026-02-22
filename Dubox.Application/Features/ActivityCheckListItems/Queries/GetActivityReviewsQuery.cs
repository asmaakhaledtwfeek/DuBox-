using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.ActivityCheckListItems.Queries;

/// <summary>
/// Query to get paginated list of activity reviews
/// </summary>
public record GetActivityReviewsQuery(
    int Page,
    int PageSize,
    Guid? ProjectId,
    Guid? BoxId,
    string? BuildingNumber,
    string? Floor,
    int? BoxTypeId,
    string? ReviewStatus,
    string? SearchTerm
) : IRequest<Result<PaginatedActivityReviewsDto>>;





