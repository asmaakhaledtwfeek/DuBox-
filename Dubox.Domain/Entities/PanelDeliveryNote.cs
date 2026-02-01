using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities;

[Table("PanelDeliveryNotes")]
public class PanelDeliveryNote
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid DeliveryNoteId { get; set; }

    [Required]
    [MaxLength(100)]
    public string DeliveryNoteNumber { get; set; } = string.Empty;

    [Required]
    public Guid ProjectId { get; set; }

    public Guid? FactoryId { get; set; }

    // Delivery Information
    [MaxLength(200)]
    public string? SupplierName { get; set; }

    [MaxLength(200)]
    public string? DriverName { get; set; }

    [MaxLength(50)]
    public string? VehicleNumber { get; set; }

    [Required]
    public DateTime DeliveryDate { get; set; }

    // Documents
    [MaxLength(500)]
    public string? QRCodeUrl { get; set; }

    [MaxLength(500)]
    public string? DocumentUrl { get; set; }

    // Status: Draft, InTransit, Delivered, Completed
    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = "Draft";

    public string? Notes { get; set; }

    // Audit fields
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public Guid? CreatedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public Guid? ModifiedBy { get; set; }

    // Navigation properties
    public virtual Project Project { get; set; } = null!;
    public virtual Factory? Factory { get; set; }
    public virtual ICollection<BoxPanel> Panels { get; set; } = new List<BoxPanel>();
}

