using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities;

[Table("BoxSubTypes")]
public class BoxSubType
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int BoxSubTypeId { get; set; }

    [Required]
    [MaxLength(200)]
    public string BoxSubTypeName { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? Abbreviation { get; set; }

    public int BoxTypeId { get; set; }

    [ForeignKey(nameof(BoxTypeId))]
    public virtual BoxType BoxType { get; set; } = null!;
}

