using Dubox.Domain.Enums;
using Dubox.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities;

/// <summary>
/// Activity Checklist Item - Links checklist items to activities
/// Can be linked to either ActivityMaster or ActivityTemplateActivity
/// </summary>
[Table("ActivityCheckListItems")]
public class ActivityCheckListItem : IAuditableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid ActivityCheckListItemId { get; set; }

    /// <summary>
    /// Reference to ActivityMaster (optional - used when checklist is for master activities)
    /// </summary>
    public Guid? ActivityMasterId { get; set; }
    [ForeignKey(nameof(ActivityMasterId))]
    public virtual ActivityMaster? ActivityMaster { get; set; }

    /// <summary>
    /// Reference to ActivityTemplateActivity (optional - used when checklist is for template activities)
    /// </summary>
    public Guid? ActivityTemplateActivityId { get; set; }
    [ForeignKey(nameof(ActivityTemplateActivityId))]
    public virtual ActivityTemplateActivity? ActivityTemplateActivity { get; set; }

    /// <summary>
    /// Reference to PredefinedChecklistItem
    /// </summary>
    [Required]
    public Guid PredefinedChecklistItemId { get; set; }
    [ForeignKey(nameof(PredefinedChecklistItemId))]
    public virtual PredefinedChecklistItem PredefinedChecklistItem { get; set; } = null!;

    /// <summary>
    /// Sequence order of this checklist item within the activity
    /// </summary>
    [Required]
    public int Sequence { get; set; }

    /// <summary>
    /// Whether this checklist item is mandatory
    /// </summary>
    public bool IsMandatory { get; set; } = true;

    /// <summary>
    /// Whether this checklist item is active
    /// </summary>
    public bool IsActive { get; set; } = true;

    // Audit Fields
    public DateTime CreatedDate { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public string? ModifiedBy { get; set; }
}
