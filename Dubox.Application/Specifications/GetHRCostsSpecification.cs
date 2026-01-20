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

            // Apply Chapter filter
            if (!string.IsNullOrWhiteSpace(query.Chapter))
            {
                var chapterLower = query.Chapter.ToLower().Trim();
                AddCriteria(h => h.Chapter != null && h.Chapter.ToLower().Contains(chapterLower));
            }

            // Apply SubChapter filter
            if (!string.IsNullOrWhiteSpace(query.SubChapter))
            {
                var subChapterLower = query.SubChapter.ToLower().Trim();
                AddCriteria(h => h.SubChapter != null && h.SubChapter.ToLower().Contains(subChapterLower));
            }

            // Apply Classification filter
            if (!string.IsNullOrWhiteSpace(query.Classification))
            {
                var classificationLower = query.Classification.ToLower().Trim();
                AddCriteria(h => h.Classification != null && h.Classification.ToLower().Contains(classificationLower));
            }

            // Apply SubClassification filter
            if (!string.IsNullOrWhiteSpace(query.SubClassification))
            {
                var subClassificationLower = query.SubClassification.ToLower().Trim();
                AddCriteria(h => h.SubClassification != null && h.SubClassification.ToLower().Contains(subClassificationLower));
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

            // Apply Type filter
            if (!string.IsNullOrWhiteSpace(query.Type))
            {
                var typeLower = query.Type.ToLower().Trim();
                AddCriteria(h => h.Type != null && h.Type.ToLower().Contains(typeLower));
            }

            // Apply BudgetLevel filter
            if (!string.IsNullOrWhiteSpace(query.BudgetLevel))
            {
                var budgetLevelLower = query.BudgetLevel.ToLower().Trim();
                AddCriteria(h => h.BudgetLevel != null && h.BudgetLevel.ToLower().Contains(budgetLevelLower));
            }

            // Apply Status filter (exact match, not Contains)
            if (!string.IsNullOrWhiteSpace(query.Status))
            {
                var statusLower = query.Status.ToLower().Trim();
                AddCriteria(h => h.Status != null && h.Status.ToLower() == statusLower);
            }

            // Apply Job filter
            if (!string.IsNullOrWhiteSpace(query.Job))
            {
                var jobLower = query.Job.ToLower().Trim();
                AddCriteria(h => h.Job != null && h.Job.ToLower().Contains(jobLower));
            }

            // Apply OfficeAccount filter
            if (!string.IsNullOrWhiteSpace(query.OfficeAccount))
            {
                var officeAccountLower = query.OfficeAccount.ToLower().Trim();
                AddCriteria(h => h.OfficeAccount != null && h.OfficeAccount.ToLower().Contains(officeAccountLower));
            }

            // Apply JobCostAccount filter
            if (!string.IsNullOrWhiteSpace(query.JobCostAccount))
            {
                var jobCostAccountLower = query.JobCostAccount.ToLower().Trim();
                AddCriteria(h => h.JobCostAccount != null && h.JobCostAccount.ToLower().Contains(jobCostAccountLower));
            }

            // Apply SpecialAccount filter
            if (!string.IsNullOrWhiteSpace(query.SpecialAccount))
            {
                var specialAccountLower = query.SpecialAccount.ToLower().Trim();
                AddCriteria(h => h.SpecialAccount != null && h.SpecialAccount.ToLower().Contains(specialAccountLower));
            }

            // Apply IDLAccount filter
            if (!string.IsNullOrWhiteSpace(query.IDLAccount))
            {
                var idlAccountLower = query.IDLAccount.ToLower().Trim();
                AddCriteria(h => h.IDLAccount != null && h.IDLAccount.ToLower().Contains(idlAccountLower));
            }

            // Apply ordering
            AddOrderBy(h => h.Chapter);
            AddOrderBy(h => h.Code);
            AddOrderBy(h => h.Name);

            // Apply pagination
            if (query.PageSize > 0 && query.PageNumber > 0)
            {
                ApplyPaging(query.PageSize, query.PageNumber);
            }
        }
    }
}




