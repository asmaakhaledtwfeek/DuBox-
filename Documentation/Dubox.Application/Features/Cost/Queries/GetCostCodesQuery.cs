using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Cost.Queries;

public record CostCodesResponse(
    List<CostCodeListDto> Data,
    int TotalCount
);

public record GetCostCodesQuery : IRequest<Result<CostCodesResponse>>
{
    public string? Code  { get; init; }
    public string? CostCodeLevel1 { get; init; }
    public string? CostCodeLevel2 { get; init; }
    public string? CostCodeLevel3 { get; init; }
    public string? Level1Description { get; init; }
    public string? Level2Description { get; init; }
    public string? Level3Description { get; init; }
    public bool? IsActive { get; init; }

    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 50;
}


