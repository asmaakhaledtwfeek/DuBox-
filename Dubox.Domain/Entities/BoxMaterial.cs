using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities
{
    [Table("BoxMaterials")]
    public class BoxMaterial
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BoxMaterialId { get; set; }

        [Required]
        [ForeignKey(nameof(Box))]
        public Guid BoxId { get; set; }

        [Required]
        [ForeignKey(nameof(Material))]
        public int MaterialId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? RequiredQuantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? AllocatedQuantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? ConsumedQuantity { get; set; }

        [MaxLength(50)]
        public string Status { get; set; } = "Pending"; // Pending, Allocated, Consumed, Short

        public DateTime? AllocatedDate { get; set; }

        public DateTime? ConsumedDate { get; set; }

        // Navigation properties
        public virtual Box Box { get; set; } = null!;
        public virtual Material Material { get; set; } = null!;

        // Calculated properties
        [NotMapped]
        public decimal? RemainingQuantity => AllocatedQuantity - ConsumedQuantity;

        [NotMapped]
        public bool IsShort => RequiredQuantity.HasValue &&
                               AllocatedQuantity.HasValue &&
                               AllocatedQuantity < RequiredQuantity;
    }
}
