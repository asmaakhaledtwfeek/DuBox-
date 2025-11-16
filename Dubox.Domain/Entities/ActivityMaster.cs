using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities;

[Table("ActivityMaster")]
public class ActivityMaster
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid ActivityMasterId { get; set; }

    [Required]
    [MaxLength(100)]
    public string ActivityCode { get; set; } = string.Empty; // e.g., STAGE1-FAB

    [Required]
    [MaxLength(200)]
    public string ActivityName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Stage { get; set; } = string.Empty; // Stage 1, Stage 2, etc.

    public int StageNumber { get; set; } // 1-6

    public int SequenceInStage { get; set; } // Order within stage

    public int OverallSequence { get; set; } // 1-28 overall order

    [MaxLength(500)]
    public string? Description { get; set; }

    public int EstimatedDurationDays { get; set; } = 1;

    public bool IsWIRCheckpoint { get; set; } = false; // Work Inspection Request checkpoint

    [MaxLength(50)]
    public string? WIRCode { get; set; } 

    [MaxLength(500)]
    public string? ApplicableBoxTypes { get; set; } 

    // Dependencies (comma-separated ActivityMasterId GUIDs or codes)
    [MaxLength(500)]
    public string? DependsOnActivities { get; set; }

    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; }

    // Navigation properties
    public ICollection<BoxActivity> BoxActivities { get; set; } = new List<BoxActivity>();
}
