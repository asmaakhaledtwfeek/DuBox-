using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities;

/// <summary>
/// Box Type Material Template Assignment - Links templates to box types
/// </summary>
[Table("BoxTypeMaterialTemplates")]
public class BoxTypeMaterialTemplate
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid BoxTypeMaterialTemplateId { get; set; }

    [Required]
    public int ProjectBoxTypeId { get; set; }
    [ForeignKey("ProjectBoxTypeId")]
    public virtual ProjectBoxType ProjectBoxType { get; set; } = null!;

    [Required]
    public Guid MaterialTemplateId { get; set; }
    [ForeignKey("MaterialTemplateId")]
    public virtual MaterialTemplate MaterialTemplate { get; set; } = null!;

    public DateTime AssignedDate { get; set; }

    [MaxLength(255)]
    public string? AssignedBy { get; set; }
}






