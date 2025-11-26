using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.WIRCheckpoints.Queries
{
    public record GetWIRCheckpointsByBoxIdQuery(Guid BoxId)
    : IRequest<Result<List<WIRCheckpointDto>>>;

}
