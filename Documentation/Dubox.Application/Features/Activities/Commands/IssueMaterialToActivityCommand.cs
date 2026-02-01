using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Activities.Commands
{
    public record IssueMaterialToActivityCommand(
     Guid BoxActivityId,
     Guid MaterialId,
     decimal Quantity,
     string? Reference
 ) : IRequest<Result<MaterialTransactionDto>>;
}
