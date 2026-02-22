using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.ActivityCheckListItems.Queries;

/// <summary>
/// Query to get summary/count of activity checklist reviews
/// </summary>
public record GetActivityReviewsSummaryQuery() 
    : IRequest<Result<ActivityReviewsSummaryDto>>;





