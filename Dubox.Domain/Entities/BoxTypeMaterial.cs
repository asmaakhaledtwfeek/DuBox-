using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities;

/// <summary>
/// Tracks materials required for box types with arrival status
/// This replaces individual box-level material tracking for better efficiency
/// </summary>
[Table("BoxTypeMaterials")]
[Index(nameof(ProjectBoxTypeId))]
[Index(nameof(MaterialId))]
[Index(nameof(IsArrived))]
public class BoxTypeMaterial
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid BoxTypeMaterialId { get; set; }

    [Required]
    public int ProjectBoxTypeId { get; set; }
    [ForeignKey(nameof(ProjectBoxTypeId))]
    public virtual ProjectBoxType ProjectBoxType { get; set; } = null!;

    [Required]
    public Guid MaterialId { get; set; }
    [ForeignKey(nameof(MaterialId))]
    public virtual Material Material { get; set; } = null!;

    /// <summary>
    /// Number of days before box planned start date that material must arrive
    /// Typically 7 (one week) or 30 (one month)
    /// </summary>
    [Required]
    public int RequiredBeforeDays { get; set; }

    /// <summary>
    /// Delivery progress 0-100. When 100, material is considered delivered (IsArrived = true).
    /// Once delivered, progress cannot be reverted to pending.
    /// </summary>
    public int DeliveryProgress { get; set; } = 0;

    /// <summary>
    /// Whether the material has arrived for this box type (set when DeliveryProgress reaches 100).
    /// When marked as arrived, all boxes of this type will inherit this status.
    /// Once true, cannot be reverted to pending.
    /// </summary>
    public bool IsArrived { get; set; } = false;

    /// <summary>
    /// Date when material was marked as arrived
    /// </summary>
    public DateTime? ArrivedDate { get; set; }

    /// <summary>
    /// User who marked the material as arrived
    /// </summary>
    [ForeignKey(nameof(ArrivedByUser))]
    public Guid? ArrivedBy { get; set; }

    /// <summary>
    /// Quantity of material that has arrived (optional)
    
    
    public int QuantityPerBox { get; set; } = 1;
    
    /// <summary>
    /// Delivered quantity entered by user when marking as delivered
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal? DeliveredQuantity { get; set; }
    /// <summary>
    /// Notes about the material arrival
    /// </summary>
    [MaxLength(500)]
    public string? Notes { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? ModifiedDate { get; set; }

    // Navigation properties
    public virtual User? ArrivedByUser { get; set; }

    /// <summary>
    /// Calculated: Status based on arrival
    /// </summary>
    [NotMapped]
    public string Status => IsArrived ? "Arrived" : "Pending";
}






