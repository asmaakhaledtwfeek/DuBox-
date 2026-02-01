using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Boxes.Queries;

public record GetBoxesByFactoryQuery(
    Guid FactoryId, 
    bool IncludeDispatched = false,
    int Page = 1,
    int PageSize = 50
) : IRequest<Result<PaginatedBoxesByFactoryResponseDto>>;

