using Dubox.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities;

/// <summary>
/// Activity Template - Contains a collection of activities that can be reused
/// </summary>
[Table("ActivityTemplates")]
public class ActivityTemplate : IAuditableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid ActivityTemplateId { get; set; }

    [Required]
    [MaxLength(200)]
    public string TemplateName { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    /// <summary>
    /// Number of stages. Stages are created from 01 to this count (e.g., 5 = Stage 01, 02, 03, 04, 05)
    /// </summary>
    public int StageCount { get; set; } = 1;

    public bool IsActive { get; set; } = true;

    // Audit Fields
    public DateTime CreatedDate { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public string? ModifiedBy { get; set; }

    // Navigation Properties
    public virtual ICollection<ActivityTemplateActivity> TemplateActivities { get; set; } = new List<ActivityTemplateActivity>();
}
