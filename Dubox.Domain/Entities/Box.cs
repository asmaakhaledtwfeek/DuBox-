using Dubox.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities;

[Table("Boxes")]
public class Box
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid BoxId { get; set; }

    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    public string BoxTag { get; set; } = string.Empty; // e.g., B2-FF-L3-2

    [MaxLength(50)]
    public string? SerialNumber { get; set; } // e.g., SN-2025-000123 (nullable for backward compatibility with existing boxes)

    [MaxLength(200)]
    public string? BoxName { get; set; }

    // Project configuration box type and subtype IDs
    // NOTE: These IDs now reference ProjectBoxTypes/ProjectBoxSubTypes (project-specific config)
    // The navigation properties below are for backward compatibility but may be null
    public int? BoxTypeId { get; set; }
    public int? BoxSubTypeId { get; set; }

    [Required]
    [MaxLength(50)]
    public string Floor { get; set; }

    [MaxLength(100)]
    public string? BuildingNumber { get; set; }

    [MaxLength(100)]
    public string? BoxFunction { get; set; }

    [MaxLength(100)]
    public BoxZone? Zone { get; set; }
    [ForeignKey(nameof(Factory))]
    public Guid? FactoryId { get; set; }

    public Factory? Factory { get; set; }
    // Current Location
    public Guid? CurrentLocationId { get; set; }
    public FactoryLocation? CurrentLocation { get; set; }

    // QR Code Information
    [Required]
    [MaxLength(200)]
    public string QRCodeString { get; set; } = string.Empty; // PROJECT-CODE_BOX-TAG

    [MaxLength(500)]
    public string? QRCodeImageUrl { get; set; } // Azure Blob Storage URL

    // Progress tracking
    public decimal ProgressPercentage { get; set; } = 0; // 0-100%

    [Required]
    public BoxStatusEnum Status { get; set; } = BoxStatusEnum.NotStarted; // Not Started, In Progress, Completed, On Hold, Delayed

    // Dimensions and specifications
    public decimal? Length { get; set; }
    public decimal? Width { get; set; }
    public decimal? Height { get; set; }

    [MaxLength(50)]
    public UnitOfMeasureEnum? UnitOfMeasure { get; set; } = UnitOfMeasureEnum.m;

    // BIM reference
    [MaxLength(100)]
    public string? RevitElementId { get; set; }

    // Tracking
    public int? Duration { get; set; }
    public DateTime? PlannedStartDate { get; set; }
    public DateTime? ActualStartDate { get; set; }
    public DateTime? PlannedEndDate { get; set; }
    public DateTime? ActualEndDate { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }

    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public Guid? CreatedBy { get; set; }
    public Guid? ModifiedBy { get; set; }
    public int SequentialNumber { get; set; } = 1; // For generating serial numbers
    [MaxLength(50)]
    public string? Bay { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? Row { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? Position { get; set; } = string.Empty;
    // Navigation properties
    // NOTE: These are kept for backward compatibility but may be null since we use project-specific configs
    public virtual BoxType? BoxType { get; set; }
    public virtual BoxSubType? BoxSubType { get; set; }
    public ICollection<BoxAsset> BoxAssets { get; set; } = new List<BoxAsset>();
    public ICollection<BoxActivity> BoxActivities { get; set; } = new List<BoxActivity>();
    public ICollection<ProgressUpdate> ProgressUpdates { get; set; } = new List<ProgressUpdate>();
    public ICollection<MaterialTransaction> MaterialTransactions { get; set; } = new List<MaterialTransaction>();
    public virtual ICollection<BoxLocationHistory> BoxLocationHistory { get; set; } = new List<BoxLocationHistory>();
    public ICollection<BoxDrawing> BoxDrawings { get; set; } = new List<BoxDrawing>();


}
