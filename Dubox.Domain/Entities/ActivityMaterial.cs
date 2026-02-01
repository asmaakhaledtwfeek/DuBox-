using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities
{
    [Table("ActivityMaterials")]
    public class ActivityMaterial
    {
        [Key]
        public Guid ActivityMaterialId { get; set; }

        [Required]
        public Guid MaterialId { get; set; }
        [Required]
        public Guid BoxActivityId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal QuantityNeeded { get; set; }

        [MaxLength(100)]
        public string? Unit { get; set; }

        [ForeignKey(nameof(MaterialId))]
        public virtual Material Material { get; set; } = null!;
        [ForeignKey(nameof(BoxActivityId))]
        public virtual BoxActivity BoxActivity { get; set; } = null!;
    }
}
