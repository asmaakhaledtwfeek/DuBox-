using Dubox.Domain.Specification;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Infrastructure.Specification;

public static class SpecificationEvaluator<TEntity> where TEntity : class
{
    public static (IQueryable<TEntity> Data, int Count) GetQuery(
        IQueryable<TEntity> inputQuery,
        Specification<TEntity> specifications)
    {
        IQueryable<TEntity> queryable = inputQuery;
        int count = 0;

        if (specifications.IsGlobalFiltersIgnored)
            queryable = queryable.IgnoreQueryFilters();

        if (specifications.Criteria != null)
            queryable = queryable.Where(specifications.Criteria);

        if (specifications.OrderByDescendingExpression.Any())
        {
            var orderedQuery = queryable.OrderByDescending(specifications.OrderByDescendingExpression.First());

            foreach (var orderBy in specifications.OrderByDescendingExpression.Skip(1))
                orderedQuery = orderedQuery.ThenByDescending(orderBy);

            queryable = orderedQuery;
        }
        if (specifications.IsPagingEnabled &&
            !specifications.OrderByExpression.Any() &&
            !specifications.OrderByDescendingExpression.Any())
        {
            specifications.IsSplitQuery = false; 
        }

        if (specifications.OrderByExpression.Any())
        {
            var orderedQuery = queryable.OrderBy(specifications.OrderByExpression.First());

            foreach (var orderBy in specifications.OrderByExpression.Skip(1))
                orderedQuery = orderedQuery.ThenBy(orderBy);

            queryable = orderedQuery;
        }

        if (specifications.IsDistinct)
            queryable = queryable.Distinct();

        // Calculate count BEFORE pagination but on filtered query (without includes to avoid split query issues)
        if (specifications.IsTotalCountEnable)
            count = queryable.Count();

        // Apply includes AFTER count (includes are for data loading, not counting)
        if (specifications.Includes.Any())
            specifications.Includes.ForEach(x => queryable = queryable.Include(x));

        // Apply split query AFTER includes (to split the data loading queries)
        if (specifications.IsSplitQuery)
            queryable = queryable.AsSplitQuery();

        // Apply pagination AFTER includes and split query
        if (specifications.IsPagingEnabled)
            queryable = queryable.Skip(specifications.Skip).Take(specifications.Take);

        return (queryable, count);
    }
}
