using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Cost.Queries;

public record HRCostDto(
    Guid HRCostRecordId,
    string? Code,
    string Name,
    string? Units,
    string? CostType,
    string? Trade,
    string? Position,
    decimal? HourlyRate,
    decimal? DailyRate,
    decimal? MonthlyRate,
    decimal? OvertimeRate,
    string Currency,
    bool IsActive
);

public record HRCostsResponse(
    List<HRCostDto> Data,
    int TotalCount
);

public record GetHRCostsQuery : IRequest<Result<HRCostsResponse>>
{
    public string? Code { get; init; }
    public string? Name { get; init; }
    public string? Units { get; init; }
    public string? CostType { get; init; }
    public bool? IsActive { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 50;
}

