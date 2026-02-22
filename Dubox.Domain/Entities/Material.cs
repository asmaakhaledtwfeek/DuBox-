using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities
{
    [Table("Materials")]
    [Index(nameof(MaterialCode), IsUnique = true)]
    public class Material
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid MaterialId { get; set; }

        [Required]
        [MaxLength(50)]
        public string MaterialCode { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string MaterialName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? MaterialCategory { get; set; } // Precast, MEP, Finishing, etc.
        [Required]
        [MaxLength(50)]
        public string Unit { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal? UnitCost { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? CurrentStock { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? AllocatedStock { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? MinimumStock { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? ReorderLevel { get; set; }

        [MaxLength(200)]
        public string? SupplierName { get; set; }

        /// <summary>
        /// Default number of days before box start date that this material must arrive
        /// Common values: 7 (one week) or 30 (one month)
        /// </summary>
        public int DefaultRequiredBeforeDays { get; set; } = 7;

        public bool IsActive { get; set; } = true;
        public int QuantityPerBox { get; set; } = 1;
        // Navigation properties
        public virtual ICollection<Project>? Projects { get; set; }
        public virtual ICollection<MaterialTransaction> Transactions { get; set; } = new List<MaterialTransaction>();
        public virtual ICollection<ActivityMaterial>? ActivityMaterials { get; set; }

        // Calculated properties
        [NotMapped]
        public bool IsLowStock => CurrentStock.HasValue &&
                                  MinimumStock.HasValue &&
                                  CurrentStock <= MinimumStock;

        [NotMapped]
        public bool NeedsReorder => CurrentStock.HasValue &&
                                    ReorderLevel.HasValue &&
                                    CurrentStock <= ReorderLevel;
    }
}
