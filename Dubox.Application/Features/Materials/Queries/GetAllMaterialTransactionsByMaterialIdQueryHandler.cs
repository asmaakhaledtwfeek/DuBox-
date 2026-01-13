using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.Materials.Queries
{
    public class GetAllMaterialTransactionsByMaterialIdQueryHandler :
        IRequestHandler<GetAllMaterialTransactionsByMaterialIdQuery, Result<GetAllMaterialTransactionsDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllMaterialTransactionsByMaterialIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<GetAllMaterialTransactionsDto>> Handle(GetAllMaterialTransactionsByMaterialIdQuery request, CancellationToken cancellationToken)
        {
            var material = await _unitOfWork.Repository<Material>().GetByIdAsync(request.materialId);
            if (material == null)
                return Result.Failure<GetAllMaterialTransactionsDto>("Material not found.");
            var materialTransactions = _unitOfWork.Repository<MaterialTransaction>()
                .GetWithSpec(new GetAllMaterialTransactionsByMaterialIdSpecification(request.materialId)).Data.ToList();
            if (!materialTransactions.Any())
                return Result.Failure<GetAllMaterialTransactionsDto>("Not fount any transaction for this material");
            var result = new GetAllMaterialTransactionsDto
            {
                MaterialId = material.MaterialId,
                MaterialName = material.MaterialName,
                MaterialCode = material.MaterialCode,
                CurrentStock = material.CurrentStock,
                Transactions = materialTransactions.Adapt<List<MaterialTransactionDto>>()
            };
            return Result.Success(result);
        }
    }
}
