using Dubox.Application.Features.Reports.Queries;
using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications;

/// <summary>
/// Specification for querying projects for summary report
/// </summary>
public class ProjectsSummaryReportSpecification : Specification<Project>
{
    public ProjectsSummaryReportSpecification(GetProjectsSummaryReportQuery query, bool enablePaging = true)
    {
        // IsActive filter
        if (query.IsActive.HasValue)
        {
            AddCriteria(p => p.IsActive == query.IsActive.Value);
        }

        // Status filter (multi-select)
        if (query.Status != null && query.Status.Any())
        {
            AddCriteria(p => query.Status.Contains((int)p.Status));
        }

        // Search filter (ProjectCode, ProjectName, ClientName)
        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var searchTerm = query.Search.Trim().ToLower();
            AddCriteria(p =>
                p.ProjectCode.ToLower().Contains(searchTerm) ||
                p.ProjectName.ToLower().Contains(searchTerm) ||
                (p.ClientName != null && p.ClientName.ToLower().Contains(searchTerm)));
        }

        // Ordering
        AddOrderBy(p => p.ProjectName);

        // Pagination
        if (enablePaging)
        {
            var pageNumber = query.PageNumber < 1 ? 1 : query.PageNumber;
            var pageSize = query.PageSize < 1 ? 25 : (query.PageSize > 100 ? 100 : query.PageSize);
            ApplyPaging(pageSize, pageNumber);
        }
    }
}

