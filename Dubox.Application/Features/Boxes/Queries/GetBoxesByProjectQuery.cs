using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Boxes.Queries;

public record GetBoxesByProjectQuery(
    Guid ProjectId,
    IReadOnlyList<int>? Statuses = null,
    string? BoxType = null,
    string? BoxSubType = null,
    string? BuildingNumber = null,
    string? Floor = null,
    string? Zone = null,
    string? Search = null,
    int Page = 1,
    int PageSize = 50,
    bool CountOnly = false
) : IRequest<Result<PaginatedBoxesResponseDto>>;

