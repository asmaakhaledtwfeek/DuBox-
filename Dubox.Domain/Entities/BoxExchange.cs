using Dubox.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities;

[Table("BoxExchanges")]
public class BoxExchange
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid BoxExchangeId { get; set; }

    [Required]
    [ForeignKey(nameof(Box))]
    public Guid BoxId { get; set; }
    public Box Box { get; set; } = null!;

    [ForeignKey(nameof(QualityIssue))]
    public Guid QualityIssueId { get; set; }
    public QualityIssue QualityIssue { get; set; } = null!;

    // Target box for exchange (the box to switch with)
    [ForeignKey(nameof(ExchangedWithBox))]
    public Guid? ExchangedWithBoxId { get; set; }
    public Box? ExchangedWithBox { get; set; }

    // Secondary quality issue for target box (read-only)
    [ForeignKey(nameof(SecondaryQualityIssue))]
    public Guid? SecondaryQualityIssueId { get; set; }
    public QualityIssue? SecondaryQualityIssue { get; set; }

    // Original values
    [MaxLength(100)]
    public string? OldBuildingNumber { get; set; }

    [MaxLength(50)]
    public string? OldFloor { get; set; }

    [MaxLength(100)]
    public string? OldBoxTag { get; set; }

    // Requested new values
    [MaxLength(100)]
    public string? NewBuildingNumber { get; set; }

    [MaxLength(50)]
    public string? NewFloor { get; set; }

    [MaxLength(100)]
    public string? NewBoxTag { get; set; }

    // Exchange request details
    [MaxLength(1000)]
    public string? RequestReason { get; set; }

    public ExchangeRequestStatusEnum Status { get; set; } = ExchangeRequestStatusEnum.Pending;

    public DateTime RequestedDate { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(RequestedByUser))]
    public Guid RequestedBy { get; set; }
    public User RequestedByUser { get; set; } = null!;

    public DateTime? ApprovedDate { get; set; }

    [ForeignKey(nameof(ApprovedByUser))]
    public Guid? ApprovedBy { get; set; }
    public User? ApprovedByUser { get; set; }

    public DateTime? RejectedDate { get; set; }

    [ForeignKey(nameof(RejectedByUser))]
    public Guid? RejectedBy { get; set; }
    public User? RejectedByUser { get; set; }

    [MaxLength(1000)]
    public string? RejectionReason { get; set; }

    public DateTime? AppliedDate { get; set; }

    [ForeignKey(nameof(AppliedByUser))]
    public Guid? AppliedBy { get; set; }
    public User? AppliedByUser { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public Guid? CreatedBy { get; set; }
}
