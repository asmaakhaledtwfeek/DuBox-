using MediatR;
using Dubox.Domain.Shared;

namespace Dubox.Application.Features.Cost.Commands;

public class CreateCostCodeCommand : IRequest<Result<CreateCostCodeResponse>>
{
    public string Code { get; set; } = string.Empty;
    public string? CostCodeLevel1 { get; set; }
    public string? Level1Description { get; set; }
    public string? CostCodeLevel2 { get; set; }
    public string? Level2Description { get; set; }
    public string? CostCodeLevel3 { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? Level3DescriptionAbbrev { get; set; }
    public string? Level3DescriptionAmana { get; set; }
    public string? Category { get; set; }
    public string? UnitOfMeasure { get; set; }
    public decimal? UnitRate { get; set; }
    public string Currency { get; set; } = "SAR";
    public bool IsActive { get; set; } = true;
}

public record CreateCostCodeResponse(
    Guid CostCodeId,
    string Code,
    string Description
);

