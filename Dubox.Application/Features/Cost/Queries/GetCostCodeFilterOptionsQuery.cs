using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Cost.Queries;

public record CostCodeFilterOptionsDto(
    List<string> Level1Options,
    List<string> Level2Options,
    List<string> Level3Options
);

public record GetCostCodeFilterOptionsQuery : IRequest<Result<CostCodeFilterOptionsDto>>
{
    public string? Level1 { get; init; }
    public string? Level2 { get; init; }
}

