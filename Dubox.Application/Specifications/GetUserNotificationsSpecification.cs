using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications
{
    public class GetUserNotificationsSpecification : Specification<Notification>
    {
        public GetUserNotificationsSpecification(Guid userId, bool unreadOnly = false)
        {
            AddCriteria(n => n.RecipientUserId == userId && !n.IsExpired);

            if (unreadOnly)
            {
                AddCriteria(n => !n.IsRead);
            }

            AddInclude(nameof(Notification.RelatedIssue));
            AddInclude($"{nameof(Notification.RelatedComment)}.{nameof(IssueComment.Author)}");
            AddInclude(nameof(Notification.RecipientUser));
            AddOrderByDescending(n => n.CreatedDate);
        }
    }
}

