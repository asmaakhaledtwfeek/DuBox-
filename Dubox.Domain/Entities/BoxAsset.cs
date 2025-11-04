using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities;

[Table("BoxAssets")]
public class BoxAsset
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid BoxAssetId { get; set; }

    public Guid BoxId { get; set; }
    public Box Box { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    public string AssetType { get; set; } = string.Empty; // Precast Wall, Slab, MEP Cage, etc.

    [MaxLength(100)]
    public string? AssetCode { get; set; }

    [MaxLength(200)]
    public string? AssetName { get; set; }

    public int Quantity { get; set; } = 1;

    [MaxLength(100)]
    public string? Unit { get; set; } // pcs, m2, m, kg, etc.

    [MaxLength(500)]
    public string? Specifications { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }

    public DateTime CreatedDate { get; set; }
}
