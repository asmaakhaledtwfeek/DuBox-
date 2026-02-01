using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities;

[Table("PanelScanLogs")]
public class PanelScanLog
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid ScanLogId { get; set; }

    [Required]
    public Guid BoxPanelId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Barcode { get; set; } = string.Empty;

    // ScanType: Dispatch, FactoryArrival, Installation, Inspection
    [Required]
    [MaxLength(50)]
    public string ScanType { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? ScanLocation { get; set; }

    public Guid? ScannedBy { get; set; }

    [Required]
    public DateTime ScannedDate { get; set; } = DateTime.UtcNow;

    // GPS Coordinates
    [Column(TypeName = "decimal(10,8)")]
    public decimal? Latitude { get; set; }

    [Column(TypeName = "decimal(11,8)")]
    public decimal? Longitude { get; set; }

    public string? Notes { get; set; }

    // Navigation properties
    public virtual BoxPanel BoxPanel { get; set; } = null!;
}

