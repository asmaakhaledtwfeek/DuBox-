using Dubox.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities;

/// <summary>
/// BIM Model - Stores BIM/Revit model information
/// </summary>
[Table("BIMModels")]
public class BIMModel : IAuditableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid BIMModelId { get; set; }

    [Required]
    [MaxLength(200)]
    public string ModelName { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? Category { get; set; }

    [MaxLength(200)]
    public string? RevitFamily { get; set; }

    [MaxLength(100)]
    public string? Type { get; set; }

    [MaxLength(100)]
    public string? Instance { get; set; }

    // BIM 5D - Quantity
    public decimal? Quantity { get; set; }

    [MaxLength(50)]
    public string? Unit { get; set; }

    // BIM 4D - Schedule
    public DateTime? PlannedStartDate { get; set; }
    public DateTime? PlannedFinishDate { get; set; }
    public DateTime? ActualStartDate { get; set; }
    public DateTime? ActualFinishDate { get; set; }

    // 3D Model File
    [MaxLength(500)]
    public string? ModelFilePath { get; set; }

    [MaxLength(100)]
    public string? ModelFileType { get; set; } // e.g., IFC, RVT, OBJ

    [MaxLength(1000)]
    public string? ThumbnailPath { get; set; }

    // Link to Project
    public Guid? ProjectId { get; set; }
    [ForeignKey("ProjectId")]
    public virtual Project? Project { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    // Audit Fields
    public DateTime CreatedDate { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public string? ModifiedBy { get; set; }
}



