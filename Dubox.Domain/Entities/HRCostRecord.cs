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
    /// Resource Code or Employee Code
    /// </summary>
    [MaxLength(50)]
    public string? Code { get; set; }

    /// <summary>
    /// Resource/Position Name (e.g., "Civil Engineer", "Foreman", "Labor")
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Trade or Discipline (e.g., "Civil", "MEP", "Structural")
    /// </summary>
    [MaxLength(100)]
    public string? Trade { get; set; }

    /// <summary>
    /// Position/Role
    /// </summary>
    [MaxLength(100)]
    public string? Position { get; set; }

    /// <summary>
    /// Hourly Rate
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal? HourlyRate { get; set; }

    /// <summary>
    /// Daily Rate
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal? DailyRate { get; set; }

    /// <summary>
    /// Monthly Rate
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal? MonthlyRate { get; set; }

    /// <summary>
    /// Currency
    /// </summary>
    [MaxLength(10)]
    public string Currency { get; set; } = "SAR";

    /// <summary>
    /// Overtime Rate
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal? OvertimeRate { get; set; }

    /// <summary>
    /// Units (e.g., "Ls" for Lump Sum, "Hr" for Hourly, "Day", "Month")
    /// </summary>
    [MaxLength(20)]
    public string? Units { get; set; }

    /// <summary>
    /// Cost Type/Manpower Type (e.g., "Manpower", "Direct Labor", "Indirect", "Overhead")
    /// </summary>
    [MaxLength(50)]
    public string? CostType { get; set; }

    /// <summary>
    /// Additional notes or description
    /// </summary>
    [MaxLength(500)]
    public string? Notes { get; set; }

    /// <summary>
    /// Whether this record is active
    /// </summary>
    public bool IsActive { get; set; } = true;

    // Audit fields
    public DateTime CreatedDate { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public string? ModifiedBy { get; set; }
}



