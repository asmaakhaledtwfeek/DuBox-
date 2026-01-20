using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dubox.Domain.Interfaces;

namespace Dubox.Domain.Entities;

/// <summary>
/// Represents a cost item for a specific project
/// </summary>
[Table("ProjectCostItems")]
public class ProjectCostItem : IAuditableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid ProjectCostItemId { get; set; }

    /// <summary>
    /// Reference to the project
    /// </summary>
    [Required]
    public Guid ProjectId { get; set; }
    [ForeignKey("ProjectId")]
    public virtual Project? Project { get; set; }

    /// <summary>
    /// Reference to the cost code
    /// </summary>
    [Required]
    public Guid CostCodeId { get; set; }
    [ForeignKey("CostCodeId")]
    public virtual CostCodeMaster? CostCode { get; set; }

    /// <summary>
    /// Quantity for this project
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// Unit rate (can override the cost code's default rate)
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitRate { get; set; }

    /// <summary>
    /// Total cost (Quantity * UnitRate)
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalCost { get; set; }

    /// <summary>
    /// Budgeted amount
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal? BudgetedAmount { get; set; }

    /// <summary>
    /// Actual spent amount
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal? ActualAmount { get; set; }

    /// <summary>
    /// Status (e.g., "Planned", "In Progress", "Completed")
    /// </summary>
    [MaxLength(50)]
    public string Status { get; set; } = "Planned";

    /// <summary>
    /// Notes specific to this project cost item
    /// </summary>
    [MaxLength(1000)]
    public string? Notes { get; set; }

    public bool IsActive { get; set; } = true;

    // Audit fields
    public DateTime CreatedDate { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public string? ModifiedBy { get; set; }
}




