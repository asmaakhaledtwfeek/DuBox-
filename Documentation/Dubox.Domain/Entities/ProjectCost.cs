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

        [ForeignKey(nameof(Box))]
        public Guid? BoxId { get; set; }

        [Required]
        [ForeignKey(nameof(Project))]
        public Guid ProjectId { get; set; }
        
        // Optional references to master data
        [ForeignKey(nameof(CostCodeMaster))]
        public Guid? CostCodeId { get; set; }
        
        [ForeignKey(nameof(HRCost))]
        public Guid? HRCostRecordId { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Cost { get; set; }

        // Cost Code Master fields (denormalized for reporting)
        [MaxLength(50)]
        public string? CostCodeLevel1 { get; set; }

        [MaxLength(50)]
        public string? CostCodeLevel2 { get; set; }

        [MaxLength(50)]
        public string? CostCodeLevel3 { get; set; }

        // HRC Code fields (denormalized for reporting)
        [MaxLength(100)]
        public string? Chapter { get; set; }

        [MaxLength(100)]
        public string? SubChapter { get; set; }

        [MaxLength(100)]
        public string? Classification { get; set; }

        [MaxLength(100)]
        public string? SubClassification { get; set; }

        [MaxLength(20)]
        public string? Units { get; set; }

        [MaxLength(50)]
        public string? Type { get; set; }

        [Required]
        [MaxLength(100)]
        public string CostType { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [MaxLength(100)]
        public Guid? CreatedBy { get; set; }

        // Navigation properties
        public virtual Box? Box { get; set; }
        public virtual Project Project { get; set; } = null!;
        public virtual CostCodeMaster? CostCodeMaster { get; set; }
        public virtual HRCostRecord? HRCost { get; set; }

    }
}


