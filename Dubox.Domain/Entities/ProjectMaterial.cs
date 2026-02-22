using Dubox.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities
{
    [Table("ProjectMaterials")]
    public class ProjectMaterial
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ProjectMaterialId { get; set; }

        [Required]
        [ForeignKey(nameof(Project))]
        public Guid ProjectId { get; set; }

        [Required]
        [ForeignKey(nameof(Material))]
        public Guid MaterialId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? RequiredQuantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? AllocatedQuantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? ConsumedQuantity { get; set; }
        public DateTime? AllocatedDate { get; set; }

        public DateTime? ConsumedDate { get; set; }

        /// <summary>
        /// Indicates whether this material is selected/required for this project
        /// Used for project-level material selection
        /// </summary>
        public bool IsSelected { get; set; } = false;

        // Navigation properties
        public virtual Project Project { get; set; } = null!;
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
