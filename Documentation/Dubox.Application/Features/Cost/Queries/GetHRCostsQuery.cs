using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Cost.Queries;

public record HRCostDto(
    Guid HRCostRecordId,
    string? Code,
    string? Chapter,
    string? SubChapter,
    string? Classification,
    string? SubClassification,
    string Name,
    string? Units,
    string? Type,
    string? BudgetLevel,
    string? Status,
    string? Job,
    string? OfficeAccount,
    string? JobCostAccount,
    string? SpecialAccount,
    string? IDLAccount
);

public record HRCostsResponse(
    List<HRCostDto> Data,
    int TotalCount
);

public record GetHRCostsQuery : IRequest<Result<HRCostsResponse>>
{
    public string? Code { get; init; }
    public string? Chapter { get; init; }
    public string? SubChapter { get; init; }
    public string? Classification { get; init; }
    public string? SubClassification { get; init; }
    public string? Name { get; init; }
    public string? Units { get; init; }
    public string? Type { get; init; }
    public string? BudgetLevel { get; init; }
    public string? Status { get; init; }
    public string? Job { get; init; }
    public string? OfficeAccount { get; init; }
    public string? JobCostAccount { get; init; }
    public string? SpecialAccount { get; init; }
    public string? IDLAccount { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 50;
}

