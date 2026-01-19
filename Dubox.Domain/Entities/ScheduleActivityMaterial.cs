using Dubox.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities;

/// <summary>
/// Junction table: Schedule Activity to Material assignment
/// </summary>
[Table("ScheduleActivityMaterials")]
public class ScheduleActivityMaterial : IAuditableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid ScheduleActivityMaterialId { get; set; }

    [Required]
    public Guid ScheduleActivityId { get; set; }
    [ForeignKey("ScheduleActivityId")]
    public virtual ScheduleActivity ScheduleActivity { get; set; } = null!;

    // Reference to actual materials (you can link to Material entity or use free text)
    [Required]
    [MaxLength(200)]
    public string MaterialName { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? MaterialCode { get; set; }

    public decimal Quantity { get; set; }

    [MaxLength(50)]
    public string? Unit { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }

    // Audit Fields
    public DateTime CreatedDate { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public string? ModifiedBy { get; set; }
}



