using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities;

/// <summary>
/// Material Template Item - Individual materials within a template
/// </summary>
[Table("MaterialTemplateItems")]
public class MaterialTemplateItem
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid MaterialTemplateItemId { get; set; }

    [Required]
    public Guid MaterialTemplateId { get; set; }
    [ForeignKey("MaterialTemplateId")]
    public virtual MaterialTemplate MaterialTemplate { get; set; } = null!;

    [Required]
    public Guid MaterialId { get; set; }
    [ForeignKey("MaterialId")]
    public virtual Material Material { get; set; } = null!;

    /// <summary>
    /// Number of days before box start date that this material must arrive
    /// </summary>
    [Required]
    public int RequiredBeforeDays { get; set; }

    public bool IsRequired { get; set; } = true;

    [MaxLength(500)]
    public string? Notes { get; set; }

    public int DisplayOrder { get; set; }
}






