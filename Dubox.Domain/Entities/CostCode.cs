using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dubox.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Domain.Entities;

[Table("CostCodesMaster")]
[Index(nameof(Code), IsUnique = true)]
[Index(nameof(CostCodeLevel1))]
[Index(nameof(CostCodeLevel2))]
[Index(nameof(CostCodeLevel3))]
[Index(nameof(IsActive))]
[Index(nameof(CostCodeLevel1), nameof(CostCodeLevel2))]
[Index(nameof(CostCodeLevel1), nameof(CostCodeLevel2), nameof(CostCodeLevel3))]
public class CostCodeMaster : IAuditableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid CostCodeId { get; set; }

    /// <summary>
    /// Cost Code (Main identifier)
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Cost Code Level 1
    /// </summary>
    [MaxLength(50)]
    public string? CostCodeLevel1 { get; set; }

    /// <summary>
    /// Level 1 Description
    /// </summary>
    [MaxLength(500)]
    public string? Level1Description { get; set; }

    /// <summary>
    /// Cost Code Level 2
    /// </summary>
    [MaxLength(50)]
    public string? CostCodeLevel2 { get; set; }

    /// <summary>
    /// Level 2 Description
    /// </summary>
    [MaxLength(500)]
    public string? Level2Description { get; set; }

    /// <summary>
    /// Cost Code Level 3
    /// </summary>
    [MaxLength(50)]
    public string? CostCodeLevel3 { get; set; }

    /// <summary>
    /// Level 3 Description – as per CSI
    /// </summary>
    [Required]
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Level 3 Description – as per CSI (abbreviation less than 50 characters)
    /// </summary>
    [MaxLength(500)]
    public string? Level3DescriptionAbbrev { get; set; }

    /// <summary>
    /// Level 3 Description – AMANA
    /// </summary>
    [MaxLength(500)]
    public string? Level3DescriptionAmana { get; set; }

    /// <summary>
    /// Category (e.g., "Civil", "MEP", "Structural")
    /// </summary>
    [MaxLength(100)]
    public string? Category { get; set; }

    /// <summary>
    /// Sub-category for grouping
    /// </summary>
    [MaxLength(100)]
    public string? SubCategory { get; set; }

    /// <summary>
    /// Unit of measure (e.g., "m3", "m2", "ton", "lump sum")
    /// </summary>
    [MaxLength(50)]
    public string? UnitOfMeasure { get; set; }

    /// <summary>
    /// Unit rate/cost
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal? UnitRate { get; set; }

    /// <summary>
    /// Currency (e.g., "SAR", "USD")
    /// </summary>
    [MaxLength(10)]
    public string Currency { get; set; } = "SAR";

    /// <summary>
    /// Notes or additional information
    /// </summary>
    [MaxLength(1000)]
    public string? Notes { get; set; }

    /// <summary>
    /// Whether this cost code is active
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Display order for sorting
    /// </summary>
    public int DisplayOrder { get; set; } = 0;

    // Audit fields
    public DateTime CreatedDate { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public string? ModifiedBy { get; set; }

    // Navigation properties
    public virtual ICollection<ProjectCostItem> ProjectCostItems { get; set; } = new List<ProjectCostItem>();
}




