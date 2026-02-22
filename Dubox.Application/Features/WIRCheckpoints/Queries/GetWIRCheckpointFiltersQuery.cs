using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.WIRCheckpoints.Queries
{
    public record GetWIRCheckpointFiltersQuery() : IRequest<Result<WIRCheckpointFiltersDto>>;
}
