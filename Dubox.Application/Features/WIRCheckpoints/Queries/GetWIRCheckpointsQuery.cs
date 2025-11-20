using Dubox.Application.DTOs;
using Dubox.Domain.Enums;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.WIRCheckpoints.Queries
{
    public record GetWIRCheckpointsQuery(
     Guid? ProjectId,
     Guid? BoxId,
     WIRCheckpointStatusEnum? Status,
     string? WIRNumber,
     DateTime? From,
     DateTime? To
 ) : IRequest<Result<List<WIRCheckpointDto>>>;

}
