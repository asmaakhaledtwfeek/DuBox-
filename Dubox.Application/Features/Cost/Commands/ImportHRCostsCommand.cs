using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Cost.Commands;

public record ImportHRCostsCommand : IRequest<Result<ImportCostCodesResult>>
{
    public string FilePath { get; init; } = string.Empty;
    public bool ClearExisting { get; init; } = false;
}



