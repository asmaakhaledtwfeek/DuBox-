using Dubox.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities;

/// <summary>
/// Material Template - Reusable sets of materials with lead times
/// </summary>
[Table("MaterialTemplates")]
public class MaterialTemplate : IAuditableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid MaterialTemplateId { get; set; }

    [Required]
    [MaxLength(200)]
    public string TemplateName { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string TemplateCode { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    [MaxLength(100)]
    public string? Category { get; set; }

    public bool IsActive { get; set; } = true;

    // Audit Fields
    public DateTime CreatedDate { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public string? ModifiedBy { get; set; }

    // Navigation Properties
    public virtual ICollection<MaterialTemplateItem> Items { get; set; } = new List<MaterialTemplateItem>();
    public virtual ICollection<ProjectMaterialTemplate> ProjectAssignments { get; set; } = new List<ProjectMaterialTemplate>();
    public virtual ICollection<BoxTypeMaterialTemplate> BoxTypeAssignments { get; set; } = new List<BoxTypeMaterialTemplate>();
}






