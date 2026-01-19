using MediatR;
using Dubox.Domain.Shared;

namespace Dubox.Application.Features.Cost.Commands;

public class CreateHRCostCommand : IRequest<Result<CreateHRCostResponse>>
{
    public string? Code { get; set; }
    public string? Chapter { get; set; }
    public string? SubChapter { get; set; }
    public string? Classification { get; set; }
    public string? SubClassification { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Units { get; set; }
    public string? Type { get; set; }
    public string? BudgetLevel { get; set; }
    public string? Status { get; set; } = "Active";
    public string? Job { get; set; }
    public string? OfficeAccount { get; set; }
    public string? JobCostAccount { get; set; }
    public string? SpecialAccount { get; set; }
    public string? IDLAccount { get; set; }
}

public record CreateHRCostResponse(
    Guid HRCostRecordId,
    string? Code,
    string Name
);

