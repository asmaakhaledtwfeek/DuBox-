using Dubox.Application.Abstractions;
using Dubox.Application.Common;
using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;
using System.Data.Common;

namespace Dubox.Application.Features.ActivityCheckListItems.Queries;

/// <summary>
/// Handles the paginated Activity Reviews query using optimised raw SQL.
///
/// Why raw SQL instead of LINQ/EF?
/// ─────────────────────────────────────────────────────────────────────────
/// The ReviewStatus column is a computed value (derived from review counts vs
/// checklist item counts).  The previous LINQ approach was forced to:
///   1. Load ALL matching rows into application memory before paginating, and
///   2. Run one COUNT sub-query per row for checklist items, and
///   3. Run a second COUNT sub-query per row for reviews  (N+1 pattern).
///
/// On a production dataset of 20 k+ rows this caused query timeouts.
///
/// The SQL in GetActivityReviews.sql resolves all aggregations once via
/// GROUP BY CTEs, joins them in a single pass, applies the ReviewStatus
/// filter in SQL, and uses OFFSET/FETCH for true DB-level pagination.
/// COUNT(*) OVER() returns the total count in the same round-trip.
/// </summary>
public class GetActivityReviewsQueryHandler
    : IRequestHandler<GetActivityReviewsQuery, Result<PaginatedActivityReviewsDto>>
{
    private readonly IDbQueryExecutor _db;

    // Load the SQL once per application lifetime (it's an embedded resource).
    private static readonly string _sql =
        SqlLoader.LoadFor<GetActivityReviewsQueryHandler>();

    public GetActivityReviewsQueryHandler(IDbQueryExecutor db)
    {
        _db = db;
    }

    public async Task<Result<PaginatedActivityReviewsDto>> Handle(
        GetActivityReviewsQuery request,
        CancellationToken cancellationToken)
    {
        var skip = Math.Max(0, (request.Page - 1) * request.PageSize);

        // Build the parameter list. Pass DBNull for every optional filter
        // that is not specified; the SQL handles NULL as "no filter".
        var parameters = BuildParameters(request, skip);

        // ── Execute the single SQL batch ─────────────────────────────────────
        // The map lambda is invoked for each page row. All rows carry the same
        // TotalCount (COUNT(*) OVER()), so capturing from the last row is fine.
        var totalCount = 0;

        var items = await _db.QueryListAsync(
            _sql,
            reader =>
            {
                // Ordinal 16 is TotalCount (COUNT(*) OVER() — same for all rows)
                totalCount = reader.GetInt32(16);
                return MapRow(reader);
            },
            parameters,
            cancellationToken);

        // ── Derive pagination metadata ────────────────────────────────────────
        // When skip >= totalCount (page beyond end), 0 rows are returned and
        // totalCount stays 0, which is the correct "no results" signal.
        var totalPages = totalCount > 0
            ? (int)Math.Ceiling(totalCount / (double)request.PageSize)
            : 0;

        var page = Math.Max(1, Math.Min(request.Page, totalPages > 0 ? totalPages : 1));

        return Result.Success(
            new PaginatedActivityReviewsDto(items, totalCount, page, request.PageSize, totalPages));
    }

    // ── Parameter builder ────────────────────────────────────────────────────

    private static IEnumerable<(string Name, object? Value)> BuildParameters(
        GetActivityReviewsQuery req, int skip)
    {
        yield return ("@ProjectId",      ToParam(req.ProjectId));
        yield return ("@BoxId",          ToParam(req.BoxId));
        yield return ("@BuildingNumber", ToParam(req.BuildingNumber));
        yield return ("@Floor",          ToParam(req.Floor));
        yield return ("@BoxTypeId",      ToParam(req.BoxTypeId));
        yield return ("@ReviewStatus",   ToParam(req.ReviewStatus));
        yield return ("@SearchTerm",     ToParam(req.SearchTerm));
        yield return ("@Skip",           skip);
        yield return ("@PageSize",       req.PageSize);
    }

    /// <summary>
    /// Returns DBNull.Value for null/empty strings and null value types,
    /// so the SQL "<c>@param IS NULL</c>" predicates work correctly.
    /// </summary>
    private static object? ToParam(Guid? value)
        => value.HasValue ? value.Value : DBNull.Value;

    private static object? ToParam(int? value)
        => value.HasValue ? value.Value : DBNull.Value;

    private static object? ToParam(string? value)
        => string.IsNullOrWhiteSpace(value) ? DBNull.Value : value;

    // ── Row mapper ───────────────────────────────────────────────────────────
    // Column ordinals must match the SELECT list in GetActivityReviews.sql.

    private static ActivityReviewItemDto MapRow(DbDataReader r)
    {
        var reviewedItems = r.GetInt32(10);
        var totalItems    = r.GetInt32(9);

        // Derive ReviewStatus from ordinal 15 (already computed by SQL CASE)
        var reviewStatus = r.GetString(15);

        return new ActivityReviewItemDto(
            BoxActivityId:      r.GetGuid(0),
            ActivityName:       r.GetString(1),
            ActivityCode:       r.GetString(2),
            BoxId:              r.GetGuid(3),
            BoxTag:             r.GetString(4),
            ProjectId:          r.GetGuid(5),
            ProjectCode:        r.GetString(6),
            ProjectName:        r.GetString(7),
            TotalItems:         totalItems,
            ReviewedItems:      reviewedItems,
            PendingItems:       totalItems - reviewedItems,
            PassedItems:        r.GetInt32(11),
            FailedItems:        r.GetInt32(12),
            ReviewStatus:       reviewStatus,
            LastReviewedDate:   r.IsDBNull(13) ? null : r.GetDateTime(13),
            LastReviewedBy:     r.IsDBNull(14) ? null : r.GetString(14),
            ProgressPercentage: r.GetDecimal(8)
        );
    }
}
