using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities;

[Table("WIRMasters")]
public class WIRMaster
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid WIRMasterId { get; set; }

    [Required]
    [MaxLength(20)]
    public string WIRNumber { get; set; } = string.Empty; // WIR-1, WIR-2, WIR-3, WIR-4, WIR-5, WIR-6

    [Required]
    [MaxLength(200)]
    public string WIRName { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    [Required]
    public int Sequence { get; set; } // Display order: 1, 2, 3, 4, 5, 6

    [MaxLength(50)]
    public string Discipline { get; set; } = string.Empty; // "Civil", "MEP", "Electrical", "Both"

    [MaxLength(50)]
    public string Phase { get; set; } = string.Empty; // "Material", "Installation", "Finishing", "Final"

    public bool IsActive { get; set; } = true;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}
