using LinqKit;
using System.Linq.Expressions;


namespace Dubox.Domain.Specification
{
    public abstract class Specification<TEntity>
      where TEntity : class
    {
        public Expression<Func<TEntity, bool>>? Criteria { get; private set; }
        public List<string> Includes { get; private set; } = new();
        public List<Expression<Func<TEntity, object>>> OrderByExpression { get; private set; } = new();
        public List<Expression<Func<TEntity, object>>> OrderByDescendingExpression { get; private set; } = new();
        public int Take { get; private set; }
        public int Skip { get; private set; }
        public bool IsPagingEnabled { get; private set; }
        public bool IsTotalCountEnable { get; private set; }
        public bool IsDistinct { get; private set; }
        public bool IsGlobalFiltersIgnored { get; private set; }
        public bool IsSplitQuery { get;  set; }

        protected void AddInclude(string includeExpression)
            => Includes.Add(includeExpression);
        protected void AddInclude(List<string> includeExpression)
            => Includes.AddRange(includeExpression);
        protected void AddCriteria(Expression<Func<TEntity, bool>> criteriaExpression)
        {
            if (Criteria == null)
                Criteria = criteriaExpression!;
            else
                Criteria = Criteria.And(criteriaExpression);
        }
        protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression)
            => OrderByExpression.Add(orderByExpression);
        protected void AddOrderByDescending(Expression<Func<TEntity, object>> orderByDescendingExpression)
            => OrderByDescendingExpression.Add(orderByDescendingExpression);
        protected void ApplyPaging(int PageSize, int PageIndex)
        {
            Skip = PageSize * (PageIndex - 1);
            Take = PageSize;
            IsPagingEnabled = true;
            EnableTotalCount();
        }

        protected void EnableTotalCount()
            => IsTotalCountEnable = true;

        protected void EnableDistinct()
            => IsDistinct = true;

        protected void IgnoreGlobalFilters()
            => IsGlobalFiltersIgnored = true;

        protected void EnableSplitQuery()
            => IsSplitQuery = true;
    }
}
