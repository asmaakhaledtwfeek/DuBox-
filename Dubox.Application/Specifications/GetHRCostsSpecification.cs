using Dubox.Application.Features.Cost.Queries;
using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications
{
    public class GetHRCostsSpecification : Specification<HRCostRecord>
    {
        public GetHRCostsSpecification(GetHRCostsQuery query)
        {
            // Apply Code filter
            if (!string.IsNullOrWhiteSpace(query.Code))
            {
                var codeLower = query.Code.ToLower().Trim();
                AddCriteria(h => h.Code != null && h.Code.ToLower().Contains(codeLower));
            }

            // Apply Name filter
            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                var nameLower = query.Name.ToLower().Trim();
                AddCriteria(h => h.Name != null && h.Name.ToLower().Contains(nameLower));
            }

            // Apply Units filter
            if (!string.IsNullOrWhiteSpace(query.Units))
            {
                var unitsLower = query.Units.ToLower().Trim();
                AddCriteria(h => h.Units != null && h.Units.ToLower().Contains(unitsLower));
            }

            // Apply CostType filter
            if (!string.IsNullOrWhiteSpace(query.CostType))
            {
                AddCriteria(h => h.CostType == query.CostType);
            }

            // Apply IsActive filter
            if (query.IsActive.HasValue)
            {
                AddCriteria(h => h.IsActive == query.IsActive.Value);
            }

            // Apply ordering
            AddOrderBy(h => h.Trade);
            AddOrderBy(h => h.Name);

            // Apply pagination
            if (query.PageSize > 0 && query.PageNumber > 0)
            {
                ApplyPaging(query.PageSize, query.PageNumber);
            }
        }
    }
}



