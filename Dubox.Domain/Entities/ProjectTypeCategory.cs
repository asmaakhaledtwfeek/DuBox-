using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities;

[Table("ProjectTypeCategories")]
public class ProjectTypeCategory
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CategoryId { get; set; }

    [Required]
    [MaxLength(200)]
    public string CategoryName { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? Abbreviation { get; set; }

    // Navigation properties
    public ICollection<BoxType> BoxTypes { get; set; } = new List<BoxType>();
}

