using Dubox.Application.Features.Cost.Queries;
using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications
{
    public class GetCostCodesSpecification : Specification<CostCodeMaster>
    {
        public GetCostCodesSpecification(GetCostCodesQuery query)
        {
            // Apply Code filter
            if (!string.IsNullOrWhiteSpace(query.Code))
            {
                var codeLower = query.Code.ToLower().Trim();
                AddCriteria(c => c.Code != null && c.Code.ToLower().Contains(codeLower));
            }

            // Apply Cost Code Level 1 filter
            if (!string.IsNullOrWhiteSpace(query.CostCodeLevel1))
            {
                var level1 = query.CostCodeLevel1.Trim();
                AddCriteria(c => c.CostCodeLevel1 == level1);
            }

            // Apply Cost Code Level 2 filter
            if (!string.IsNullOrWhiteSpace(query.CostCodeLevel2))
            {
                var level2 = query.CostCodeLevel2.Trim();
                AddCriteria(c => c.CostCodeLevel2 == level2);
            }

            // Apply Cost Code Level 3 filter
            if (!string.IsNullOrWhiteSpace(query.CostCodeLevel3))
            {
                var level3 = query.CostCodeLevel3.Trim();
                AddCriteria(c => c.CostCodeLevel3 == level3);
            }

            // Apply Level 1 Description filter
            if (!string.IsNullOrWhiteSpace(query.Level1Description))
            {
                var level1DescLower = query.Level1Description.ToLower().Trim();
                AddCriteria(c => c.Level1Description != null && c.Level1Description.ToLower().Contains(level1DescLower));
            }

            // Apply Level 2 Description filter
            if (!string.IsNullOrWhiteSpace(query.Level2Description))
            {
                var level2DescLower = query.Level2Description.ToLower().Trim();
                AddCriteria(c => c.Level2Description != null && c.Level2Description.ToLower().Contains(level2DescLower));
            }

            // Apply Level 3 Description filter
            if (!string.IsNullOrWhiteSpace(query.Level3Description))
            {
                var level3DescLower = query.Level3Description.ToLower().Trim();
                AddCriteria(c => 
                    (c.Description != null && c.Description.ToLower().Contains(level3DescLower)) ||
                    (c.Level3DescriptionAbbrev != null && c.Level3DescriptionAbbrev.ToLower().Contains(level3DescLower)) ||
                    (c.Level3DescriptionAmana != null && c.Level3DescriptionAmana.ToLower().Contains(level3DescLower))
                );
            }

            // Apply IsActive filter
            if (query.IsActive.HasValue)
            {
                AddCriteria(c => c.IsActive == query.IsActive.Value);
            }

            // Apply ordering
            AddOrderBy(c => c.DisplayOrder);
            AddOrderBy(c => c.Code);

            // Apply pagination
            if (query.PageSize > 0 && query.PageNumber > 0)
            {
                ApplyPaging(query.PageSize, query.PageNumber);
            }
        }
    }
}

