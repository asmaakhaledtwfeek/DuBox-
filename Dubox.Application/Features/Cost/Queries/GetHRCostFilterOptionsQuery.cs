using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Cost.Queries;

public record HRCostFilterOptionsDto(
    List<string> Codes,
    List<string> Chapters,
    List<string> SubChapters,
    List<string> Classifications,
    List<string> SubClassifications,
    List<string> Units,
    List<string> Types,
    List<string> Statuses
);

public record GetHRCostFilterOptionsQuery : IRequest<Result<HRCostFilterOptionsDto>>
{
    public string? Code { get; init; }
    public string? Chapter { get; init; }
    public string? SubChapter { get; init; }
    public string? Classification { get; init; }
    public string? SubClassification { get; init; }
    public string? Units { get; init; }
    public string? Type { get; init; }
    public string? Status { get; init; }
}

