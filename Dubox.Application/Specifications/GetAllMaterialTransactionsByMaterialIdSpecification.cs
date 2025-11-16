using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications
{
    public class GetAllMaterialTransactionsByMaterialIdSpecification : Specification<MaterialTransaction>
    {
        public GetAllMaterialTransactionsByMaterialIdSpecification(Guid materialId)
        {
            AddCriteria(t => t.MaterialId == materialId);
            AddInclude(nameof(MaterialTransaction.PerformedBy));
            AddInclude(nameof(MaterialTransaction.Box));
            AddInclude(nameof(MaterialTransaction.BoxActivity));
            AddInclude($"{nameof(ProgressUpdate.BoxActivity)}.{nameof(ProgressUpdate.BoxActivity.ActivityMaster)}");
            AddInclude(nameof(MaterialTransaction.Material));

            AddOrderByDescending(t => t.TransactionDate);
        }
    }
}
