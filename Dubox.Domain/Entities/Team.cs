using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities
{
    [Table("Teams")]
    [Index(nameof(TeamCode), IsUnique = true)]
    public class Team
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TeamId { get; set; }

        [Required]
        [MaxLength(50)]
        public string TeamCode { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string TeamName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Department { get; set; } // Civil, MEP, QC

        [MaxLength(100)]
        public string? Trade { get; set; } // Assembly, Mechanical, Electrical, Finishing

        [MaxLength(200)]
        public string? TeamLeaderName { get; set; }

        public int? TeamSize { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual ICollection<TeamMember> Members { get; set; } = new List<TeamMember>();
        public virtual ICollection<BoxActivity> AssignedActivities { get; set; } = new List<BoxActivity>();
        public virtual ICollection<ProgressUpdate> ProgressUpdates { get; set; } = new List<ProgressUpdate>();
        public virtual ICollection<DailyProductionLog> ProductionLogs { get; set; } = new List<DailyProductionLog>();
    }
}
