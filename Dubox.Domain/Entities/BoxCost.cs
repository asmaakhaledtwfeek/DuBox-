using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities
{
    [Table("BoxCosts")]
    public class BoxCost
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CostId { get; set; }

        [Required]
        [ForeignKey(nameof(Box))]
        public Guid BoxId { get; set; }

        [Required]
        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }

        [MaxLength(50)]
        public string? CostType { get; set; } // Labor, Material, Equipment, Subcontract, Overhead

        [Column(TypeName = "decimal(18,2)")]
        public decimal? BudgetedCost { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? ActualCost { get; set; }

        public DateTime? CostDate { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [MaxLength(200)]
        public string? Reference { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [MaxLength(100)]
        public string? CreatedBy { get; set; }

        // Navigation properties
        public virtual Box Box { get; set; } = null!;
        public virtual CostCategory Category { get; set; } = null!;

        [NotMapped]
        public decimal? Variance => BudgetedCost.HasValue && ActualCost.HasValue
            ? ActualCost - BudgetedCost
            : null;

        [NotMapped]
        public decimal? VariancePercentage => BudgetedCost.HasValue &&
                                               BudgetedCost > 0 &&
                                               ActualCost.HasValue
            ? (Variance / BudgetedCost) * 100
            : null;
    }
}
