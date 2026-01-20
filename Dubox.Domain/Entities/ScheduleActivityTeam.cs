using Dubox.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities;

/// <summary>
/// Junction table: Schedule Activity to Team assignment
/// </summary>
[Table("ScheduleActivityTeams")]
public class ScheduleActivityTeam : IAuditableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid ScheduleActivityTeamId { get; set; }

    [Required]
    public Guid ScheduleActivityId { get; set; }
    [ForeignKey("ScheduleActivityId")]
    public virtual ScheduleActivity ScheduleActivity { get; set; } = null!;

    [Required]
    public Guid TeamId { get; set; }
    [ForeignKey("TeamId")]
    public virtual Team Team { get; set; } = null!;

    public DateTime AssignedDate { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }

    // Audit Fields
    public DateTime CreatedDate { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public string? ModifiedBy { get; set; }
}




