using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Cost.Commands;

public record ImportCostCodesCommand : IRequest<Result<ImportCostCodesResult>>
{
    public string FilePath { get; init; } = string.Empty;
    public bool ClearExisting { get; init; } = false;
}

public record ImportCostCodesResult
{
    public int TotalRecords { get; init; }
    public int SuccessCount { get; init; }
    public int SkippedCount { get; init; }
    public int ErrorCount { get; init; }
    public List<string> Errors { get; init; } = new();
}



