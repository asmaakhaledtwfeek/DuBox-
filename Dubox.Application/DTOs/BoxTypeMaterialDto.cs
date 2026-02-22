namespace Dubox.Application.DTOs;

public class BoxTypeMaterialDto
{
    public Guid BoxTypeMaterialId { get; set; }
    public int ProjectBoxTypeId { get; set; }
    public string BoxTypeName { get; set; } = string.Empty;
    public int BoxCount { get; set; } // Number of boxes under this box type
    public Guid MaterialId { get; set; }
    public string MaterialCode { get; set; } = string.Empty;
    public string MaterialName { get; set; } = string.Empty;
    public string? MaterialCategory { get; set; }
    public string? Unit { get; set; }
    public int QuantityPerBox { get; set; }
    public int RequiredBeforeDays { get; set; }
    public int DeliveryProgress { get; set; }
    public bool IsArrived { get; set; }
    public DateTime? ArrivedDate { get; set; }
    public Guid? ArrivedBy { get; set; }
    public string? ArrivedByName { get; set; }
    public decimal? ArrivedQuantity { get; set; }
    public decimal? DeliveredQuantity { get; set; }
    public decimal TotalQuantity => BoxCount * QuantityPerBox; // Calculated field
    public string? Notes { get; set; }
    public string Status { get; set; } = "Pending";
    public DateTime CreatedDate { get; set; }
}






