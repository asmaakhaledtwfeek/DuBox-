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
    public string Status { get; set; } = "Not Started"; // Not Started, In Progress, Completed, On Hold, Delayed

    public decimal ProgressPercentage { get; set; } = 0; // 0-100%

    public DateTime? PlannedStartDate { get; set; }
    public DateTime? PlannedEndDate { get; set; }
    public DateTime? ActualStartDate { get; set; }
    public DateTime? ActualEndDate { get; set; }

    [MaxLength(500)]
    public string? WorkDescription { get; set; }

    [MaxLength(500)]
    public string? IssuesEncountered { get; set; }

    [ForeignKey(nameof(Team))]
    public int? TeamId { get; set; }
    public virtual Team? Team { get; set; }

    [ForeignKey(nameof(AssignedMember))]
    public Guid? AssignedMemberId { get; set; }
    public virtual TeamMember? AssignedMember { get; set; }

    //// Team assignment
    //[MaxLength(100)]
    //public string? AssignedTeam { get; set; }

    //public Guid? AssignedUserId { get; set; }
    //public User? AssignedUser { get; set; }

    // Materials tracking
    public bool MaterialsAvailable { get; set; } = true;

    [MaxLength(500)]
    public string? MaterialsNeeded { get; set; }

    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public string? ModifiedBy { get; set; }

    // Navigation properties
    public ICollection<ProgressUpdate> ProgressUpdates { get; set; } = new List<ProgressUpdate>();
    public ICollection<WIRRecord> WIRRecords { get; set; } = new List<WIRRecord>();
}
