using Dubox.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities
{
    [Table("MaterialTransactions")]
    public class MaterialTransaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid TransactionId { get; set; }

        [Required]
        [ForeignKey(nameof(Material))]
        public int MaterialId { get; set; }

        [ForeignKey(nameof(Box))]
        public Guid? BoxId { get; set; }
        [ForeignKey(nameof(BoxActivity))]
        public Guid? BoxActivityId { get; set; }

        [MaxLength(50)]
        public MaterialTransactionTypeEnum? TransactionType { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Quantity { get; set; }

        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

        [MaxLength(200)]
        public string? Reference { get; set; }
        public Guid? PerformedById { get; set; }

        public string? Remarks { get; set; }

        // Navigation properties
        public virtual Material Material { get; set; } = null!;
        public virtual Box? Box { get; set; }
        public virtual BoxActivity? BoxActivity { get; set; }

        [ForeignKey(nameof(PerformedById))]
        public virtual User? PerformedBy { get; set; }
    }
}
