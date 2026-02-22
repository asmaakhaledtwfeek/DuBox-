using Dubox.Domain.Enums;
using Dubox.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities;

/// <summary>
/// ActivityCheckListItemReview - Stores reviews for activity checklist items
/// Reviews are performed after an activity reaches 100% progress
/// </summary>
[Table("ActivityCheckListItemReviews")]
public class ActivityCheckListItemReview : IAuditableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid ActivityCheckListItemReviewId { get; set; }

    /// <summary>
    /// Reference to the BoxActivity this review is for
    /// </summary>
    [Required]
    public Guid BoxActivityId { get; set; }
    [ForeignKey(nameof(BoxActivityId))]
    public virtual BoxActivity BoxActivity { get; set; } = null!;

    /// <summary>
    /// Reference to the ActivityCheckListItem being reviewed
    /// </summary>
    [Required]
    public Guid ActivityCheckListItemId { get; set; }
    [ForeignKey(nameof(ActivityCheckListItemId))]
    public virtual ActivityCheckListItem ActivityCheckListItem { get; set; } = null!;

    /// <summary>
    /// Review status: Pending, Pass, or Fail
    /// </summary>
    [Required]
    public CheckListItemStatusEnum Status { get; set; } = CheckListItemStatusEnum.Pending;

    /// <summary>
    /// Comments or remarks for the review
    /// </summary>
    [MaxLength(1000)]
    public string? Remarks { get; set; }

    /// <summary>
    /// User who performed the review
    /// </summary>
    public Guid? ReviewedBy { get; set; }
    [ForeignKey(nameof(ReviewedBy))]
    public virtual User? ReviewedByUser { get; set; }

    /// <summary>
    /// Date when the review was performed
    /// </summary>
    public DateTime? ReviewedDate { get; set; }

    // Audit Fields
    public DateTime CreatedDate { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public string? ModifiedBy { get; set; }
}





