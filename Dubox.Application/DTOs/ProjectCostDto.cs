namespace Dubox.Application.DTOs;

public record ProjectCostDto
{
    public Guid ProjectCostId { get; init; }
    public Guid BoxId { get; init; }
    public Guid? HRCostRecordId { get; init; }
    public decimal Cost { get; init; }
    public string CostType { get; init; } = string.Empty;
    public DateTime CreatedDate { get; init; }
    public Guid? CreatedBy { get; init; }
    
    // Optional navigation data
    public string? BoxTag { get; init; }
    public string ? BoxSerialNumber { get; init; }
    public string? HRCostCode { get; init; }
    public string? HRCostName { get; init; }
}

public record CreateProjectCostDto
{
    public Guid BoxId { get; init; }
    public decimal Cost { get; init; }
    public string CostType { get; init; } = string.Empty;
}



