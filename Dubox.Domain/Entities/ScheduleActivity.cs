using Dubox.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities;

/// <summary>
/// Schedule Activity - Planned activities with team and material assignments
/// Contains cloned/snapshot data from ActivityMaster or custom activity data
/// </summary>
[Table("ScheduleActivities")]
public class ScheduleActivity : IAuditableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid ScheduleActivityId { get; set; }

    // If cloned from ActivityMaster, store the original ID for reference (optional)
    public Guid? SourceActivityMasterId { get; set; }
    [ForeignKey("SourceActivityMasterId")]
    public virtual ActivityMaster? SourceActivityMaster { get; set; }

    // Flag to indicate if this is a custom activity (not from ActivityMaster)
    public bool IsCustomActivity { get; set; } = false;

    // Cloned/Snapshot fields from ActivityMaster (or custom values)
    [Required]
    [MaxLength(100)]
    public string ActivityCode { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string ActivityName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Stage { get; set; } = string.Empty;

    public int StageNumber { get; set; }

    public int SequenceInStage { get; set; }

    public int OverallSequence { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    public int EstimatedDurationDays { get; set; } = 1;

    public bool IsWIRCheckpoint { get; set; } = false;

    [MaxLength(50)]
    public string? WIRCode { get; set; }

    [MaxLength(500)]
    public string? ApplicableBoxTypes { get; set; }

    [MaxLength(500)]
    public string? DependsOnActivities { get; set; }

    // Schedule-specific fields
    public DateTime PlannedStartDate { get; set; }
    
    public DateTime PlannedFinishDate { get; set; }

    public DateTime? ActualStartDate { get; set; }
    
    public DateTime? ActualFinishDate { get; set; }

    [MaxLength(50)]
    public string Status { get; set; } = "Planned"; // Planned, In Progress, Completed, On Hold

    public decimal PercentComplete { get; set; } = 0;

    /// <summary>
    /// Weight for weighted progress calculation (default 1.0 = equal weight).
    /// Used when calculating parent progress as weighted average of children.
    /// Example: If one child has Weight=0.7 and another Weight=0.3, parent progress = (child1 * 0.7) + (child2 * 0.3)
    /// </summary>
    [Column(TypeName = "decimal(5,2)")]
    public decimal Weight { get; set; } = 1.0m;

    // Optional: Link to Project
    public Guid? ProjectId { get; set; }
    [ForeignKey("ProjectId")]
    public virtual Project? Project { get; set; }

    // Hierarchical Structure (Parent/Child relationship)
    public Guid? ParentActivityId { get; set; }
    [ForeignKey("ParentActivityId")]
    public virtual ScheduleActivity? ParentActivity { get; set; }

    // Audit Fields
    public DateTime CreatedDate { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public string? ModifiedBy { get; set; }

    // Navigation Properties
    public virtual ICollection<ScheduleActivityTeam> AssignedTeams { get; set; } = new List<ScheduleActivityTeam>();
    public virtual ICollection<ScheduleActivityMaterial> AssignedMaterials { get; set; } = new List<ScheduleActivityMaterial>();
    public virtual ICollection<ScheduleActivity> ChildActivities { get; set; } = new List<ScheduleActivity>();
}










