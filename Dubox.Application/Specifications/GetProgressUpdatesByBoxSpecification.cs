using Dubox.Application.Features.ProgressUpdates.Queries;
using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications
{
    public class GetProgressUpdatesByBoxSpecification : Specification<ProgressUpdate>
    {
        public GetProgressUpdatesByBoxSpecification(GetProgressUpdatesByBoxQuery query)
        {
            AddCriteria(pu => pu.BoxId == query.BoxId);
            AddInclude(nameof(ProgressUpdate.BoxActivity));
            AddInclude(nameof(ProgressUpdate.UpdatedByUser));
            AddInclude($"{nameof(ProgressUpdate.BoxActivity)}.{(nameof(BoxActivity.ActivityMaster))}");
            if (!string.IsNullOrWhiteSpace(query.ActivityName))
            {
                var activityNameLower = query.ActivityName.ToLower().Trim();
                AddCriteria(pu => pu.BoxActivity.ActivityMaster.ActivityName.ToLower().Contains(activityNameLower));
            }

            if (!string.IsNullOrWhiteSpace(query.Status))
            {
                AddCriteria(pu => pu.Status.ToString().ToLower() == query.Status.ToLower().Trim());
            }

            if (query.UpdatedBy.HasValue)
            {
                AddCriteria(pu => pu.UpdatedBy == query.UpdatedBy.Value);
            }

            if (query.FromDate.HasValue)
            {
                var fromDate = query.FromDate.Value.Date;
                AddCriteria(pu => pu.UpdateDate >= fromDate);
            }

            if (query.ToDate.HasValue)
            {
                var toDate = query.ToDate.Value.Date.AddDays(1).AddTicks(-1);
                AddCriteria(pu => pu.UpdateDate <= toDate);
            }

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                var searchTermLower = query.SearchTerm.ToLower().Trim();
                AddCriteria(pu =>
                    (pu.WorkDescription != null && pu.WorkDescription.ToLower().Contains(searchTermLower)) ||
                    (pu.IssuesEncountered != null && pu.IssuesEncountered.ToLower().Contains(searchTermLower)) ||
                    (pu.BoxActivity.ActivityMaster.ActivityName.ToLower().Contains(searchTermLower)) ||
                    (pu.UpdatedByUser.FullName != null && pu.UpdatedByUser.FullName.ToLower().Contains(searchTermLower)) ||
                    (pu.UpdatedByUser.Email != null && pu.UpdatedByUser.Email.ToLower().Contains(searchTermLower)) ||
                    (pu.LocationDescription != null && pu.LocationDescription.ToLower().Contains(searchTermLower))
                );
            }

            AddOrderByDescending(pu => pu.UpdateDate);

            EnableSplitQuery();

            if (query.PageSize > 0 && query.PageNumber > 0)
            {
                ApplyPaging(query.PageSize, query.PageNumber);
            }
        }
    }
}
