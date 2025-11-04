using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities
{
    [Table("BoxActivities")]
    [Index(nameof(BoxId))]
    [Index(nameof(Status))]
    public class BoxActivity
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BoxActivityId { get; set; }

        [Required]
        [ForeignKey(nameof(Box))]
        public Guid BoxId { get; set; }

        [Required]
        [ForeignKey(nameof(ActivityMaster))]
        public int ActivityMasterId { get; set; }

        public int ActivitySequence { get; set; }

        public DateTime? PlannedStartDate { get; set; }

        public DateTime? PlannedEndDate { get; set; }

        public int? PlannedDuration { get; set; }

        public DateTime? ActualStartDate { get; set; }

        public DateTime? ActualEndDate { get; set; }

        public int? ActualDuration { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal ProgressPercentage { get; set; } = 0;

        [MaxLength(50)]
        public string Status { get; set; } = "Not Started"; // Not Started, In Progress, Completed, On Hold, Delayed

        [ForeignKey(nameof(AssignedTeam))]
        public int? AssignedTeamId { get; set; }

        [MaxLength(200)]
        public string? AssignedTo { get; set; } // Foreman name

        public string? Comments { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime? ModifiedDate { get; set; }

        public DateTime? CompletedDate { get; set; }

        // Navigation properties
        public virtual Box Box { get; set; } = null!;
        public virtual ActivityMaster ActivityMaster { get; set; } = null!;
        public virtual Team? AssignedTeam { get; set; }
        public virtual ICollection<ProgressUpdate> ProgressUpdates { get; set; } = new List<ProgressUpdate>();
        public virtual ICollection<ActivityDependency> Dependencies { get; set; } = new List<ActivityDependency>();
        public virtual ICollection<ActivityDependency> DependentActivities { get; set; } = new List<ActivityDependency>();

        // Calculated properties
        [NotMapped]
        public bool IsCompleted => Status == "Completed" || ProgressPercentage >= 100;

        [NotMapped]
        public bool IsDelayed => PlannedEndDate.HasValue &&
                                 !ActualEndDate.HasValue &&
                                 PlannedEndDate < DateTime.Today;

        [NotMapped]
        public int DelayDays => IsDelayed ?
            (DateTime.Today - PlannedEndDate!.Value).Days : 0;
    }
}
