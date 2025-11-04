using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities
{
    [Table("MaterialTransactions")]
    public class MaterialTransaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionId { get; set; }

        [Required]
        [ForeignKey(nameof(Material))]
        public int MaterialId { get; set; }

        [ForeignKey(nameof(Box))]
        public Guid? BoxId { get; set; }

        [MaxLength(50)]
        public string? TransactionType { get; set; } // Receipt, Issue, Return, Adjustment

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Quantity { get; set; }

        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

        [MaxLength(200)]
        public string? Reference { get; set; }

        [MaxLength(200)]
        public string? PerformedBy { get; set; }

        public string? Remarks { get; set; }

        // Navigation properties
        public virtual Material Material { get; set; } = null!;
        public virtual Box? Box { get; set; }
    }
}
