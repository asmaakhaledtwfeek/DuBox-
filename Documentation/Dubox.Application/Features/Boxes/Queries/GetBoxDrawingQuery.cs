using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Boxes.Queries
{
    public record GetBoxDrawingQuery(Guid boxId) : IRequest<Result<List<ProgressUpdateImageDto>>>
    {
    }
}
