using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities
{
    [Table("ProjectCosts")]
    public class ProjectCost
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ProjectCostId { get; set; }

        [Required]
        [ForeignKey(nameof(Box))]
        public Guid BoxId { get; set; }

        [Required]
        [ForeignKey(nameof(Project))]
        public Guid ProjectId { get; set; }
        
        [ForeignKey(nameof(HRCost))]
        public Guid? HRCostRecordId { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Cost { get; set; }

        [Required]
        [MaxLength(100)]
        public string CostType { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [MaxLength(100)]
        public Guid? CreatedBy { get; set; }

        // Navigation properties
        public virtual Box Box { get; set; } = null!;
        public virtual Project Project { get; set; } = null!;
        public virtual HRCostRecord? HRCost { get; set; }

    }
}


