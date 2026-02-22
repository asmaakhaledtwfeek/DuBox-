using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities;

[Table("PanelTypes")]
public class PanelType
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid PanelTypeId { get; set; }

    [Required]
    public Guid ProjectId { get; set; }

    [Required]
    [MaxLength(200)]
    public string PanelTypeName { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string PanelTypeCode { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    public int DisplayOrder { get; set; } = 0;

    // Technical Specifications (extracted from PDF)
    [Column(TypeName = "decimal(10,2)")]
    public decimal? VolumeM3 { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal? WeightTon { get; set; }

    [MaxLength(50)]
    public string? ConcreteGrade { get; set; }

    public int? CoverMm { get; set; }

    public string? EmbedsJson { get; set; } // JSON array: [{"symbol": "F50", "description": "...", "qty": 2}]

    // Audit fields
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public Guid? CreatedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public Guid? ModifiedBy { get; set; }

    // Navigation properties
    public virtual Project Project { get; set; } = null!;
    public virtual ICollection<BoxPanel> BoxPanels { get; set; } = new List<BoxPanel>();
}

