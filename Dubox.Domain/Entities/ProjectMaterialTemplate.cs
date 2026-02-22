using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities;

/// <summary>
/// Project Material Template Assignment - Links templates to projects
/// </summary>
[Table("ProjectMaterialTemplates")]
public class ProjectMaterialTemplate
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid ProjectMaterialTemplateId { get; set; }

    [Required]
    public Guid ProjectId { get; set; }
    [ForeignKey("ProjectId")]
    public virtual Project Project { get; set; } = null!;

    [Required]
    public Guid MaterialTemplateId { get; set; }
    [ForeignKey("MaterialTemplateId")]
    public virtual MaterialTemplate MaterialTemplate { get; set; } = null!;

    public DateTime AssignedDate { get; set; }

    [MaxLength(255)]
    public string? AssignedBy { get; set; }
}






