using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities
{
    [Table("BoxAssets")]
    public class BoxAsset
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AssetId { get; set; }

        [Required]
        [ForeignKey(nameof(Box))]
        public Guid BoxId { get; set; }

        [MaxLength(100)]
        public string? AssetType { get; set; } // Precast Wall, Slab, MEP Cage, POD, etc.

        [MaxLength(100)]
        public string? AssetCode { get; set; }

        [MaxLength(500)]
        public string? AssetDescription { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? Quantity { get; set; }

        [MaxLength(50)]
        public string? Unit { get; set; }

        [MaxLength(50)]
        public string? Status { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual Box Box { get; set; } = null!;
    }
}
