using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Materials.Queries
{
    public record GetAllMaterialTransactionsByMaterialIdQuery(int materialId) : IRequest<Result<GetAllMaterialTransactionsDto>>;

}
