using Dubox.Application.Features.Reports.Queries;
using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications;


public class BoxesSummaryReportSpecification : Specification<Box>
{
    public BoxesSummaryReportSpecification(GetBoxesSummaryReportQuery query, List<Guid>? accessibleProjectIds, bool enablePaging = true)
    {
        AddInclude(nameof(Box.Project));
        AddInclude(nameof(Box.CurrentLocation));
        AddInclude(nameof(Box.Factory));

        // Apply visibility filtering
        // If accessibleProjectIds is null, user can access all projects (SystemAdmin)
        // If list is provided, filter to only those projects
        if (accessibleProjectIds != null)
        {
            AddCriteria(b => accessibleProjectIds.Contains(b.ProjectId));
        }

        // Filter out boxes from closed projects
        AddCriteria(b => b.Project.Status != Domain.Enums.ProjectStatusEnum.Closed);

        if (query.ProjectId.HasValue && query.ProjectId.Value != Guid.Empty)
            AddCriteria(b => b.ProjectId == query.ProjectId.Value);

        //if (query.BoxType != null && query.BoxType.Any())
        //    AddCriteria(b => query.BoxType.Contains(b.BoxType));

        if (!string.IsNullOrWhiteSpace(query.Floor))
            AddCriteria(b => b.Floor == query.Floor);

        if (!string.IsNullOrWhiteSpace(query.BuildingNumber))
            AddCriteria(b => b.BuildingNumber == query.BuildingNumber);

        if (!string.IsNullOrEmpty(query.Zone))
            AddCriteria(b => b.Zone == query.Zone);

        if (query.Status != null && query.Status.Any())
            AddCriteria(b => query.Status.Contains((int)b.Status));

        if (query.ProgressMin.HasValue)
            AddCriteria(b => b.ProgressPercentage >= query.ProgressMin.Value);

        if (query.ProgressMax.HasValue)
            AddCriteria(b => b.ProgressPercentage <= query.ProgressMax.Value);

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var searchTerm = query.Search.Trim().ToLower();
            AddCriteria(b =>
                b.BoxTag.ToLower().Contains(searchTerm) ||
                (b.SerialNumber != null && b.SerialNumber.ToLower().Contains(searchTerm)) ||
                (b.BoxName != null && b.BoxName.ToLower().Contains(searchTerm)));
        }

        if (query.DateFrom.HasValue || query.DateTo.HasValue)
        {
            var dateFilterType = query.DateFilterType ?? "LastUpdate";

            if (dateFilterType == "PlannedStart")
            {
                if (query.DateFrom.HasValue)
                {
                    AddCriteria(b => b.PlannedStartDate.HasValue && b.PlannedStartDate.Value >= query.DateFrom.Value);
                }
                if (query.DateTo.HasValue)
                {
                    var toDate = query.DateTo.Value.Date.AddDays(1).AddTicks(-1);
                    AddCriteria(b => b.PlannedStartDate.HasValue && b.PlannedStartDate.Value <= toDate);
                }
            }
            else // LastUpdate (default)
            {
                // Use navigation property ProgressUpdates - EF Core will translate this to SQL efficiently
                if (query.DateFrom.HasValue)
                {
                    var fromDate = query.DateFrom.Value;
                    AddCriteria(b => 
                        b.ProgressUpdates.Any(pu => pu.UpdateDate >= fromDate) ||
                        (b.ModifiedDate.HasValue && b.ModifiedDate.Value >= fromDate));
                }
                if (query.DateTo.HasValue)
                {
                    var toDate = query.DateTo.Value.Date.AddDays(1).AddTicks(-1);
                    AddCriteria(b =>
                        b.ProgressUpdates.Any(pu => pu.UpdateDate <= toDate) ||
                        (b.ModifiedDate.HasValue && b.ModifiedDate.Value <= toDate));
                }
            }
        }

        var sortBy = query.SortBy?.ToLower() ?? "boxtag";
        var sortDir = query.SortDir?.ToLower() ?? "asc";
        var isAscending = sortDir == "asc";

        switch (sortBy)
        {
            case "boxtag":
                if (isAscending)
                    AddOrderBy(b => b.BoxTag);
                else
                    AddOrderByDescending(b => b.BoxTag);
                break;
            case "serialnumber":
                if (isAscending)
                    AddOrderBy(b => b.SerialNumber ?? string.Empty);
                else
                    AddOrderByDescending(b => b.SerialNumber ?? string.Empty);
                break;
            case "project":
                if (isAscending)
                    AddOrderBy(b => b.Project != null ? b.Project.ProjectName : string.Empty);
                else
                    AddOrderByDescending(b => b.Project != null ? b.Project.ProjectName : string.Empty);
                break;
            case "boxtype":
                if (isAscending)
                    AddOrderBy(b => b.ProjectBoxTypeId ?? 0);
                else
                    AddOrderByDescending(b => b.ProjectBoxTypeId ?? 0);
                break;
            case "floor":
                if (isAscending)
                    AddOrderBy(b => b.Floor ?? string.Empty);
                else
                    AddOrderByDescending(b => b.Floor ?? string.Empty);
                break;
            case "building":
                if (isAscending)
                    AddOrderBy(b => b.BuildingNumber ?? string.Empty);
                else
                    AddOrderByDescending(b => b.BuildingNumber ?? string.Empty);
                break;
            case "zone":
                if (isAscending)
                    AddOrderBy(b => b.Zone );
                else
                    AddOrderByDescending(b => b.Zone);
                break;
            case "progress":
                if (isAscending)
                    AddOrderBy(b => b.ProgressPercentage);
                else
                    AddOrderByDescending(b => b.ProgressPercentage);
                break;
            case "status":
                if (isAscending)
                    AddOrderBy(b => b.Status);
                else
                    AddOrderByDescending(b => b.Status);
                break;
            case "location":
                if (isAscending)
                    AddOrderBy(b => b.CurrentLocation != null ? b.CurrentLocation.LocationName : string.Empty);
                else
                    AddOrderByDescending(b => b.CurrentLocation != null ? b.CurrentLocation.LocationName : string.Empty);
                break;
            case "plannedstart":
                if (isAscending)
                    AddOrderBy(b => b.PlannedStartDate ?? DateTime.MaxValue);
                else
                    AddOrderByDescending(b => b.PlannedStartDate ?? DateTime.MinValue);
                break;
            case "plannedend":
                if (isAscending)
                    AddOrderBy(b => b.PlannedEndDate ?? DateTime.MaxValue);
                else
                    AddOrderByDescending(b => b.PlannedEndDate ?? DateTime.MinValue);
                break;
            case "lastupdate":
                if (isAscending)
                    AddOrderBy(b => b.ModifiedDate ?? b.CreatedDate);
                else
                    AddOrderByDescending(b => b.ModifiedDate ?? b.CreatedDate);
                break;
            default:
                AddOrderBy(b => b.BoxTag);
                break;
        }

        if (enablePaging)
        {
            var pageNumber = query.PageNumber < 1 ? 1 : query.PageNumber;
            var pageSize = query.PageSize < 1 ? 25 : (query.PageSize > 100 ? 100 : query.PageSize);
            ApplyPaging(pageSize, pageNumber);
        }
    }
}

