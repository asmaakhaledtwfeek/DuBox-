namespace Dubox.Application.DTOs;

public record ProjectCostDto
{
    public Guid ProjectCostId { get; init; }
    public Guid? BoxId { get; init; }
    public Guid? CostCodeId { get; init; }
    public Guid? HRCostRecordId { get; init; }
    public decimal Cost { get; init; }
    public string CostType { get; init; } = string.Empty;
    
    // Cost Code Master fields
    public string? CostCodeLevel1 { get; init; }
    public string? CostCodeLevel2 { get; init; }
    public string? CostCodeLevel3 { get; init; }
    
    // HRC Code fields
    public string? Chapter { get; init; }
    public string? SubChapter { get; init; }
    public string? Classification { get; init; }
    public string? SubClassification { get; init; }
    public string? Units { get; init; }
    public string? Type { get; init; }
    
    public DateTime CreatedDate { get; init; }
    public Guid? CreatedBy { get; init; }
    
    // Optional navigation data
    public string? BoxTag { get; init; }
    public string? BoxSerialNumber { get; init; }
    public string? HRCostCode { get; init; }
    public string? HRCostName { get; init; }
}

public record CreateProjectCostDto
{
    public Guid BoxId { get; init; }
    public decimal Cost { get; init; }
    public string CostType { get; init; } = string.Empty;
}



