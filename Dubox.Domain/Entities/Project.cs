using Dubox.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities;

[Table("Projects")]
public class Project
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid ProjectId { get; set; }

    [Required]
    [MaxLength(50)]
    public string ProjectCode { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string ProjectName { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? ClientName { get; set; }

    [Required]
    public ProjectLocationEnum Location { get; set; }
    [Required]
    public int CategoryId { get; set; }

    [ForeignKey(nameof(CategoryId))]
    public ProjectTypeCategory? Category { get; set; }
    public DateTime? PlannedStartDate { get; set; }
    public DateTime? ActualStartDate { get; set; }
    public int? Duration { get; set; }
    public DateTime? PlannedEndDate { get; set; }
    public DateTime? ActualEndDate { get; set; }
    public DateTime? CompressionStartDate { get; set; }

    [Required]
    public ProjectStatusEnum Status { get; set; } = ProjectStatusEnum.Active;

    [MaxLength(500)]
    public string? Description { get; set; }

    [MaxLength(500)]
    public string? BimLink { get; set; }

    public int TotalBoxes { get; set; }
    public decimal ProgressPercentage { get; set; } = 0;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public string? CreatedBy { get; set; }
    public string? ModifiedBy { get; set; }

    // Navigation properties
    public ICollection<Box> Boxes { get; set; } = new List<Box>();
    
    // Project Configuration Collections
    public ICollection<ProjectBuilding> ProjectBuildings { get; set; } = new List<ProjectBuilding>();
    public ICollection<ProjectLevel> ProjectLevels { get; set; } = new List<ProjectLevel>();
    public ICollection<ProjectBoxType> ProjectBoxTypes { get; set; } = new List<ProjectBoxType>();
    public ICollection<ProjectZone> ProjectZones { get; set; } = new List<ProjectZone>();
    public ICollection<ProjectBoxFunction> ProjectBoxFunctions { get; set; } = new List<ProjectBoxFunction>();
}
