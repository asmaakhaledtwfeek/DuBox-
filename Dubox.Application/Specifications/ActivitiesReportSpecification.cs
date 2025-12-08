using Dubox.Application.DTOs;
using Dubox.Application.Features.Reports.Queries;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications;

public class ActivitiesReportSpecification : Specification<BoxActivity>
{
    public ActivitiesReportSpecification(GetActivitiesReportQuery query, bool enablePaging = true)
        : this(query.ProjectId, query.TeamId, query.Status,
               query.PlannedStartDateFrom, query.PlannedStartDateTo,
               query.PlannedEndDateFrom, query.PlannedEndDateTo,
               query.Search, enablePaging, query.Page, query.PageSize, onlyWithTeamId: false)
    {
    }

    public ActivitiesReportSpecification(GetActivitiesSummaryQuery query)
        : this(query.ProjectId, query.TeamId, query.Status,
               query.PlannedStartDateFrom, query.PlannedStartDateTo,
               query.PlannedEndDateFrom, query.PlannedEndDateTo,
               query.Search, enablePaging: false, page: 1, pageSize: 1, onlyWithTeamId: false)
    {
    }

    public ActivitiesReportSpecification(ExportActivitiesReportQuery query)
        : this(query.ProjectId, query.TeamId, query.Status,
               query.PlannedStartDateFrom, query.PlannedStartDateTo,
               query.PlannedEndDateFrom, query.PlannedEndDateTo,
               query.Search, enablePaging: false, page: 1, pageSize: 1, onlyWithTeamId: false)
    {
    }


    public ActivitiesReportSpecification(
        Guid? projectId,
        int? status,
        bool enablePaging = false,
        bool onlyWithTeamId = false)
        : this(projectId, teamId: null, status,
               plannedStartDateFrom: null, plannedStartDateTo: null,
               plannedEndDateFrom: null, plannedEndDateTo: null,
               search: null, enablePaging, page: 1, pageSize: int.MaxValue, onlyWithTeamId)
    {
    }

    private ActivitiesReportSpecification(
        Guid? projectId,
        Guid? teamId,
        int? status,
        DateTime? plannedStartDateFrom,
        DateTime? plannedStartDateTo,
        DateTime? plannedEndDateFrom,
        DateTime? plannedEndDateTo,
        string? search,
        bool enablePaging,
        int page = 1,
        int pageSize = 1,
        bool onlyWithTeamId = false)
    {
        AddInclude(nameof(BoxActivity.Box));
        AddInclude($"{nameof(BoxActivity.Box)}.{nameof(Box.Project)}");
        AddInclude(nameof(BoxActivity.ActivityMaster));
        AddInclude(nameof(BoxActivity.Team));
        
        // Enable split query to avoid Cartesian explosion with multiple includes
        EnableSplitQuery();

        AddCriteria(ba => ba.IsActive);

        if (projectId.HasValue && projectId.Value != Guid.Empty)
            AddCriteria(ba => ba.Box.ProjectId == projectId.Value);

        if (teamId.HasValue && teamId.Value != Guid.Empty)
            AddCriteria(ba => ba.TeamId == teamId.Value);

        // Filter only activities with TeamId assigned (for Teams Performance Report)
        if (onlyWithTeamId)
            AddCriteria(ba => ba.TeamId.HasValue);

        if (status.HasValue)
        {
            var statusEnum = (BoxStatusEnum)status.Value;
            AddCriteria(ba => ba.Status == statusEnum);
        }

        if (plannedStartDateFrom.HasValue)
            AddCriteria(ba => ba.PlannedStartDate.HasValue &&
                ba.PlannedStartDate >= plannedStartDateFrom.Value);

        if (plannedStartDateTo.HasValue)
            AddCriteria(ba => ba.PlannedStartDate.HasValue &&
                ba.PlannedStartDate <= plannedStartDateTo.Value);

        if (plannedEndDateFrom.HasValue)
            AddCriteria(ba => ba.PlannedEndDate.HasValue &&
                ba.PlannedEndDate >= plannedEndDateFrom.Value);

        if (plannedEndDateTo.HasValue)
            AddCriteria(ba => ba.PlannedEndDate.HasValue &&
                ba.PlannedEndDate <= plannedEndDateTo.Value);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchTerm = search.Trim().ToLowerInvariant();
            AddCriteria(ba =>
                ba.ActivityMaster.ActivityName.ToLower().Contains(searchTerm) ||
                ba.Box.BoxTag.ToLower().Contains(searchTerm) ||
                ba.Box.Project.ProjectCode.ToLower().Contains(searchTerm));
        }

        AddOrderByDescending(ba => ba.BoxActivityId);

        if (enablePaging)
        {
            var (normalizedPage, normalizedPageSize) = new PaginatedRequest
            {
                Page = page,
                PageSize = pageSize
            }.GetNormalizedPagination();

            ApplyPaging(normalizedPageSize, normalizedPage);
        }
    }
}

