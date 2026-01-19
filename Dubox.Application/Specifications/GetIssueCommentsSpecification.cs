using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications
{
    public class GetIssueCommentsSpecification : Specification<IssueComment>
    {
        public GetIssueCommentsSpecification(Guid issueId, bool includeDeleted = false)
        {
            AddCriteria(c => c.IssueId == issueId);
            
            if (!includeDeleted)
            {
                AddCriteria(c => !c.IsDeleted);
            }

            AddInclude(nameof(IssueComment.Author));
            AddInclude(nameof(IssueComment.UpdatedByUser));
            AddOrderBy(c => c.CreatedDate);
        }
    }
}

