using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications
{
    public class GetMaterialTransactionWithIncludesSpecification : Specification<MaterialTransaction>
    {
        public GetMaterialTransactionWithIncludesSpecification(Guid transactionId)
        {
            AddCriteria(t => t.TransactionId == transactionId);
            AddInclude(nameof(MaterialTransaction.Material));
            AddInclude(nameof(MaterialTransaction.Box));
            AddInclude(nameof(MaterialTransaction.BoxActivity));
            AddInclude($"{nameof(MaterialTransaction.BoxActivity)}.{nameof(MaterialTransaction.BoxActivity.ActivityMaster)}");


        }
    }
}
