using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities
{
    [Table("DailyProductionLog")]
    public class DailyProductionLog
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid LogId { get; set; }

        [Required]
        public DateTime LogDate { get; set; }

        [Required]
        [ForeignKey(nameof(Box))]
        public Guid BoxId { get; set; }

        [ForeignKey(nameof(Team))]
        public Guid? TeamId { get; set; }

        [ForeignKey(nameof(ActivityMaster))]
        public Guid? ActivityMasterId { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? ManHours { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal? ProgressAchieved { get; set; }

        public string? Remarks { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [MaxLength(100)]
        public string? CreatedBy { get; set; }

        // Navigation properties
        public virtual Box Box { get; set; } = null!;
        public virtual Team? Team { get; set; }
        public virtual ActivityMaster? ActivityMaster { get; set; }
    }
}
