using Dubox.Application.Abstractions;
using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.ActivityCheckListItems.Queries;

public class GetActivityReviewsSummaryQueryHandler
    : IRequestHandler<GetActivityReviewsSummaryQuery, Result<ActivityReviewsSummaryDto>>
{
    private readonly IDbQueryExecutor _db;

    public GetActivityReviewsSummaryQueryHandler(IDbQueryExecutor db)
    {
        _db = db;
    }

    public async Task<Result<ActivityReviewsSummaryDto>> Handle(
        GetActivityReviewsSummaryQuery request,
        CancellationToken cancellationToken)
    {
        const string sql = @"
            SELECT
                COUNT(*)                                                                              AS Total,
                SUM(CASE WHEN ReviewedItems = 0                              THEN 1 ELSE 0 END)       AS Pending,
                SUM(CASE WHEN ReviewedItems >= TotalItems AND TotalItems > 0 THEN 1 ELSE 0 END)       AS Completed,
                COUNT(*)
                    - SUM(CASE WHEN ReviewedItems = 0                              THEN 1 ELSE 0 END)
                    - SUM(CASE WHEN ReviewedItems >= TotalItems AND TotalItems > 0 THEN 1 ELSE 0 END) AS InProgress
            FROM [dbo].[vw_ActivityReviewsSummary];";

        var result = await _db.QuerySingleAsync(
            sql,
            reader => new ActivityReviewsSummaryDto(
                TotalReviews:   reader.GetInt32(0),
                PendingReviews: reader.GetInt32(1),
                PassedReviews:  reader.GetInt32(2),
                FailedReviews:  reader.GetInt32(3),
                NaReviews:      0),
            cancellationToken: cancellationToken);

        return Result.Success(result ?? new ActivityReviewsSummaryDto(0, 0, 0, 0, 0));
    }
}
