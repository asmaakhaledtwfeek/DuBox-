using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities;

[Table("BoxTypes")]
public class BoxType
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int BoxTypeId { get; set; }

    [Required]
    [MaxLength(200)]
    public string BoxTypeName { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? Abbreviation { get; set; }

    public int CategoryId { get; set; }

    [ForeignKey(nameof(CategoryId))]
    public ProjectTypeCategory Category { get; set; } = null!;

    // Navigation properties
    public ICollection<BoxSubType> BoxSubTypes { get; set; } = new List<BoxSubType>();
}

