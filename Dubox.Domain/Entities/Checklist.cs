using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace Dubox.Domain.Entities;

[Table("Checklists")]
public class Checklist
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid ChecklistId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Code { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Discipline { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? SubDiscipline { get; set; }

    [Required]
    public int PageNumber { get; set; }

    // Store ReferenceDocuments as JSON string
    [Column(TypeName = "nvarchar(max)")]
    public string? ReferenceDocumentsJson { get; set; }

    // Store SignatureRoles as JSON string
    [Column(TypeName = "nvarchar(max)")]
    public string? SignatureRolesJson { get; set; }
    [MaxLength(50)]
    public string? WIRCode { get; set; }
    public bool IsActive { get; set; } = true;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public ICollection<ChecklistSection> Sections { get; set; } = new List<ChecklistSection>();

    // NotMapped properties for easy access to List<string>
    [NotMapped]
    public List<string> ReferenceDocuments
    {
        get => string.IsNullOrEmpty(ReferenceDocumentsJson)
            ? new List<string>()
            : JsonSerializer.Deserialize<List<string>>(ReferenceDocumentsJson) ?? new List<string>();
        set => ReferenceDocumentsJson = value == null || value.Count == 0
            ? null
            : JsonSerializer.Serialize(value);
    }

    [NotMapped]
    public List<string> SignatureRoles
    {
        get => string.IsNullOrEmpty(SignatureRolesJson)
            ? new List<string>()
            : JsonSerializer.Deserialize<List<string>>(SignatureRolesJson) ?? new List<string>();
        set => SignatureRolesJson = value == null || value.Count == 0
            ? null
            : JsonSerializer.Serialize(value);
    }
}

