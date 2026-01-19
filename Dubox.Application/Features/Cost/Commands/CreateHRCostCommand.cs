using MediatR;
using Dubox.Domain.Shared;

namespace Dubox.Application.Features.Cost.Commands;

public class CreateHRCostCommand : IRequest<Result<CreateHRCostResponse>>
{
    public string? Code { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Units { get; set; }
    public string? CostType { get; set; }
    public string? Trade { get; set; }
    public string? Position { get; set; }
    public decimal? HourlyRate { get; set; }
    public decimal? DailyRate { get; set; }
    public decimal? MonthlyRate { get; set; }
    public decimal? OvertimeRate { get; set; }
    public string Currency { get; set; } = "SAR";
    public bool IsActive { get; set; } = true;
}

public record CreateHRCostResponse(
    Guid HRCostRecordId,
    string? Code,
    string Name
);

