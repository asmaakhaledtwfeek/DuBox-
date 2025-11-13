using Dubox.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities;

[Table("WIRRecords")]
public class WIRRecord
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid WIRRecordId { get; set; }

    public Guid BoxActivityId { get; set; }
    public BoxActivity BoxActivity { get; set; } = null!;

    [Required]
    [MaxLength(50)]
    public string WIRCode { get; set; } = string.Empty; // WIR-1, WIR-2, etc.

    [Required]
    public WIRRecordStatusEnum Status { get; set; } = WIRRecordStatusEnum.Pending; // Pending, Approved, Rejected

    public DateTime RequestedDate { get; set; }

    public Guid RequestedBy { get; set; }
    public User RequestedByUser { get; set; } = null!;

    public Guid? InspectedBy { get; set; }
    public User? InspectedByUser { get; set; }

    public DateTime? InspectionDate { get; set; }

    [MaxLength(1000)]
    public string? InspectionNotes { get; set; }

    [MaxLength(2000)]
    public string? PhotoUrls { get; set; }

    [MaxLength(1000)]
    public string? RejectionReason { get; set; }

    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
}

