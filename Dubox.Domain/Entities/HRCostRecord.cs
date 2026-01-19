using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dubox.Domain.Interfaces;

namespace Dubox.Domain.Entities;

/// <summary>
/// HR Cost/Resource Cost Record - for labor/personnel costs
/// </summary>
[Table("HRCostRecords")]
public class HRCostRecord : IAuditableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid HRCostRecordId { get; set; }

    /// <summary>
    /// Cost Code
    /// </summary>
    [MaxLength(50)]
    public string? Code { get; set; }

    /// <summary>
    /// Chapter
    /// </summary>
    [MaxLength(100)]
    public string? Chapter { get; set; }

    /// <summary>
    /// Sub Chapter
    /// </summary>
    [MaxLength(100)]
    public string? SubChapter { get; set; }

    /// <summary>
    /// Classification
    /// </summary>
    [MaxLength(100)]
    public string? Classification { get; set; }

    /// <summary>
    /// Sub Classification
    /// </summary>
    [MaxLength(100)]
    public string? SubClassification { get; set; }

    /// <summary>
    /// Name/Description
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Units (e.g., "LS", "HR", "M2", "M3", "EA")
    /// </summary>
    [MaxLength(20)]
    public string? Units { get; set; }

    /// <summary>
    /// Cost Type (e.g., "Direct", "Indirect", "Overhead")
    /// </summary>
    [MaxLength(50)]
    public string? Type { get; set; }

    /// <summary>
    /// Budget Level
    /// </summary>
    [MaxLength(50)]
    public string? BudgetLevel { get; set; }

    /// <summary>
    /// Status (e.g., "Active", "Inactive", "Archived")
    /// </summary>
    [MaxLength(20)]
    public string? Status { get; set; }

    /// <summary>
    /// Job
    /// </summary>
    [MaxLength(100)]
    public string? Job { get; set; }

    /// <summary>
    /// Office Account
    /// </summary>
    [MaxLength(100)]
    public string? OfficeAccount { get; set; }

    /// <summary>
    /// Job Cost Account
    /// </summary>
    [MaxLength(100)]
    public string? JobCostAccount { get; set; }

    /// <summary>
    /// Special Account
    /// </summary>
    [MaxLength(100)]
    public string? SpecialAccount { get; set; }

    /// <summary>
    /// IDL Account
    /// </summary>
    [MaxLength(100)]
    public string? IDLAccount { get; set; }

    /// <summary>
    /// Additional notes or description
    /// </summary>
    [MaxLength(500)]
    public string? Notes { get; set; }

    // Audit fields
    public DateTime CreatedDate { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public string? ModifiedBy { get; set; }
}



