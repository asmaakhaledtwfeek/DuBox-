using Dubox.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities;

/// <summary>
/// Schedule Activity - Planned activities with team and material assignments
/// Completely separate from Activity Master
/// </summary>
[Table("ScheduleActivities")]
public class ScheduleActivity : IAuditableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid ScheduleActivityId { get; set; }

    [Required]
    [MaxLength(100)]
    public string ActivityName { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string ActivityCode { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    public DateTime PlannedStartDate { get; set; }
    
    public DateTime PlannedFinishDate { get; set; }

    public DateTime? ActualStartDate { get; set; }
    
    public DateTime? ActualFinishDate { get; set; }

    [MaxLength(50)]
    public string Status { get; set; } = "Planned"; // Planned, In Progress, Completed, On Hold

    public decimal PercentComplete { get; set; } = 0;

    // Optional: Link to Project
    public Guid? ProjectId { get; set; }
    [ForeignKey("ProjectId")]
    public virtual Project? Project { get; set; }

    // Audit Fields
    public DateTime CreatedDate { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public string? ModifiedBy { get; set; }

    // Navigation Properties
    public virtual ICollection<ScheduleActivityTeam> AssignedTeams { get; set; } = new List<ScheduleActivityTeam>();
    public virtual ICollection<ScheduleActivityMaterial> AssignedMaterials { get; set; } = new List<ScheduleActivityMaterial>();
}




