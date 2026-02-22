using Dubox.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities;

/// <summary>
/// Activity Template Activity - Custom activities within templates
/// All activities are custom - Activity Master is used only as a template source (no FK relationship)
/// </summary>
[Table("ActivityTemplateActivities")]
public class ActivityTemplateActivity : IAuditableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid ActivityTemplateActivityId { get; set; }

    [Required]
    public Guid ActivityTemplateId { get; set; }
    [ForeignKey("ActivityTemplateId")]
    public virtual ActivityTemplate ActivityTemplate { get; set; } = null!;

    /// <summary>
    /// Activity Master ID used as template source (for display/tracking only, no FK constraint)
    /// If set, activity is marked as MASTER; if null and IsCustomActivity=true, marked as CUSTOM
    /// </summary>
    public Guid? SourceActivityMasterId { get; set; }

    /// <summary>
    /// Flag to indicate if this is a custom activity (true) or from Activity Master template (false)
    /// Used for display badges: MASTER vs CUSTOM
    /// </summary>
    public bool IsCustomActivity { get; set; } = true;

    // Activity fields (may be copied from Activity Master as template)
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
 
    public Guid? AssignedTeamId { get; set; }

    // Navigation properties
    public virtual ICollection<ActivityCheckListItem> ChecklistItems { get; set; } = new List<ActivityCheckListItem>();
    [ForeignKey(nameof(AssignedTeamId))]
    public virtual Team? AssignedTeam { get; set; } = null;

    // Audit Fields
    public DateTime CreatedDate { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public string? ModifiedBy { get; set; }

}
