using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications
{
    public class GetBoxExchangesByBoxIdSpecification : Specification<BoxExchange>
    {
        public GetBoxExchangesByBoxIdSpecification(Guid boxId)
        {
            // Filter by BoxId
            AddCriteria(be => be.BoxId == boxId);

            // Include related entities
            AddInclude(nameof(BoxExchange.Box));
            AddInclude(nameof(BoxExchange.ExchangedWithBox));
            AddInclude(nameof(BoxExchange.QualityIssue));
            AddInclude(nameof(BoxExchange.RequestedByUser));
            AddInclude(nameof(BoxExchange.ApprovedByUser));
            AddInclude(nameof(BoxExchange.RejectedByUser));
            AddInclude(nameof(BoxExchange.AppliedByUser));

            // Order by RequestedDate descending (most recent first)
            AddOrderByDescending(be => be.RequestedDate);

            // Enable split query to avoid Cartesian explosion with multiple includes
            EnableSplitQuery();
        }
    }
}
