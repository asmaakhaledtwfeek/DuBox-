using Dubox.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities;

[Table("BoxActivities")]
public class BoxActivity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid BoxActivityId { get; set; }

    public Guid BoxId { get; set; }
    public Box Box { get; set; } = null!;

    public Guid ActivityMasterId { get; set; }
    public ActivityMaster ActivityMaster { get; set; } = null!;

    public int Sequence { get; set; } // Copied from ActivityMaster

    [Required]
    [MaxLength(50)]
    public BoxStatusEnum Status { get; set; } = BoxStatusEnum.NotStarted; // Not Started, In Progress, Completed, On Hold, Delayed

    public decimal ProgressPercentage { get; set; } = 0; // 0-100%

    public DateTime? PlannedStartDate { get; set; }
    public DateTime? PlannedEndDate { get; set; }
    public DateTime? ActualStartDate { get; set; }
    public DateTime? ActualEndDate { get; set; }
    public int? Duration { get; set; }


    [MaxLength(500)]
    public string? WorkDescription { get; set; }

    [MaxLength(500)]
    public string? IssuesEncountered { get; set; }

    [ForeignKey(nameof(Team))]
    public Guid? TeamId { get; set; }
    public virtual Team? Team { get; set; }

    [ForeignKey(nameof(AssignedMember))]
    public Guid? AssignedMemberId { get; set; }
    public virtual TeamMember? AssignedMember { get; set; }

    [ForeignKey(nameof(AssignedGroup))]
    public Guid? AssignedGroupId { get; set; }
    public virtual TeamGroup? AssignedGroup { get; set; }

    // Materials tracking
    public bool MaterialsAvailable { get; set; } = true;

    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public Guid? ModifiedBy { get; set; }

    // Navigation properties
    public ICollection<ProgressUpdate> ProgressUpdates { get; set; } = new List<ProgressUpdate>();
    public ICollection<WIRRecord> WIRRecords { get; set; } = new List<WIRRecord>();
    public virtual ICollection<ActivityDependency> Dependencies { get; set; } = new List<ActivityDependency>();
    public virtual ICollection<ActivityDependency> DependentActivities { get; set; } = new List<ActivityDependency>();
    public virtual ICollection<ActivityMaterial> RequiredMaterials { get; set; } = new List<ActivityMaterial>();
    [NotMapped]
    public bool IsCompleted => Status == BoxStatusEnum.Completed || ProgressPercentage >= 100;

}
