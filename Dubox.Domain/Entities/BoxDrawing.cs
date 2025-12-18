using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities;

[Table("BoxDrawings")]
public class BoxDrawing
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid BoxDrawingId { get; set; }

    public Guid BoxId { get; set; }
    public Box Box { get; set; } = null!;

    [MaxLength(1000)]
    public string? DrawingUrl { get; set; }

    public string? FileData { get; set; } // Base64 string for PDF or DWG files

    [MaxLength(500)]
    public string? OriginalFileName { get; set; }

    [MaxLength(50)]
    public string? FileExtension { get; set; } // .pdf or .dwg

    [MaxLength(20)]
    public string? FileType { get; set; } // "url" or "file"

    public long? FileSize { get; set; }

    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? ModifiedDate { get; set; }
    public Guid? CreatedBy { get; set; }
    public Guid? ModifiedBy { get; set; }
}

