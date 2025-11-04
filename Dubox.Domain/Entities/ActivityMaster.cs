using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities
{
    [Table("ActivityMaster")]
    [Index(nameof(ActivityCode), IsUnique = true)]
    public class ActivityMaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ActivityMasterId { get; set; }

        [Required]
        [MaxLength(50)]
        public string ActivityCode { get; set; } = string.Empty; // ACT-001, ACT-002, etc.

        [Required]
        [MaxLength(200)]
        public string ActivityName { get; set; } = string.Empty; // "Assembly & joints", "PODS Installation", etc.

        [MaxLength(500)]
        public string? ActivityDescription { get; set; }

        [MaxLength(100)]
        public string? Department { get; set; } // Civil, MEP, QC

        [MaxLength(100)]
        public string? Trade { get; set; } // Assembly, Mechanical, Electrical, Finishing

        public int StandardDuration { get; set; }

        public int Sequence { get; set; }

        public bool IsWIRCheckpoint { get; set; } = false;

        [MaxLength(20)]
        public string? WIRNumber { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; }

        // Navigation properties
        public virtual ICollection<BoxActivity> BoxActivities { get; set; } = new List<BoxActivity>();
    }
}
