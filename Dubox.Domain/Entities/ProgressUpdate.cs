using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities
{
    [Table("ProgressUpdates")]
    [Index(nameof(BoxId))]
    [Index(nameof(UpdateDate))]
    public class ProgressUpdate
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UpdateId { get; set; }

        [Required]
        [ForeignKey(nameof(Box))]
        public Guid BoxId { get; set; }

        [ForeignKey(nameof(BoxActivity))]
        public int? BoxActivityId { get; set; }

        public DateTime UpdateDate { get; set; } = DateTime.UtcNow;

        [MaxLength(200)]
        public string? UpdatedBy { get; set; }

        [ForeignKey(nameof(Team))]
        public int? TeamId { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal? ProgressPercentage { get; set; }

        [MaxLength(50)]
        public string? Status { get; set; }

        public string? WorkDescription { get; set; }

        public string? IssuesEncountered { get; set; }

        [MaxLength(500)]
        public string? PhotoPath { get; set; }

        [Column(TypeName = "decimal(10,8)")]
        public decimal? Latitude { get; set; }

        [Column(TypeName = "decimal(11,8)")]
        public decimal? Longitude { get; set; }

        [MaxLength(200)]
        public string? DeviceInfo { get; set; }

        // Navigation properties
        public virtual Box Box { get; set; } = null!;
        public virtual BoxActivity? BoxActivity { get; set; }
        public virtual Team? Team { get; set; }
    }
}
