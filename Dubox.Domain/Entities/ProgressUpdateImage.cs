using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities;

[Table("ProgressUpdateImages")]
public class ProgressUpdateImage
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid ProgressUpdateImageId { get; set; }

    public Guid ProgressUpdateId { get; set; }
    public ProgressUpdate ProgressUpdate { get; set; } = null!;

    [Required]
    public string ImageData { get; set; } = string.Empty;

    [Required]
    [MaxLength(10)]
    public string ImageType { get; set; } = "file";


    [MaxLength(500)]
    public string? OriginalName { get; set; }

    public long? FileSize { get; set; }

    public int Sequence { get; set; } = 0;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}

