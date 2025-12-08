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
            // Exclude "NotStarted" status - progress updates should never have this status
            AddCriteria(pu => pu.Status != Domain.Enums.BoxStatusEnum.NotStarted);
            AddInclude(nameof(ProgressUpdate.Box));
            AddInclude(nameof(ProgressUpdate.BoxActivity));
            AddInclude($"{nameof(ProgressUpdate.BoxActivity)}.{nameof(ProgressUpdate.BoxActivity.ActivityMaster)}");
            AddInclude(nameof(ProgressUpdate.UpdatedByUser));
            // NOTE: Don't include Images - base64 ImageData is too large
            // Image metadata is loaded separately with lightweight query

            // Search by activity name
            if (!string.IsNullOrWhiteSpace(query.ActivityName))
            {
                var activityNameLower = query.ActivityName.ToLower().Trim();
                AddCriteria(pu => pu.BoxActivity.ActivityMaster.ActivityName.ToLower().Contains(activityNameLower));
            }

            // Search by status
            if (!string.IsNullOrWhiteSpace(query.Status))
            {
                AddCriteria(pu => pu.Status.ToString().ToLower() == query.Status.ToLower().Trim());
            }

            // Search by updated by user
            if (query.UpdatedBy.HasValue)
            {
                AddCriteria(pu => pu.UpdatedBy == query.UpdatedBy.Value);
            }

            // Date range filter
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

            // General search term (searches in work description, issues, activity name, updated by name)
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
            
            // Enable split query to avoid Cartesian explosion with collection includes (Images)
            EnableSplitQuery();

            if (query.PageSize > 0 && query.PageNumber > 0)
            {
                ApplyPaging(query.PageSize, query.PageNumber);
            }
        }
    }
}
