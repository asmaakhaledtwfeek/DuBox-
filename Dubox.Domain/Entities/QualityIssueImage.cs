using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities;

[Table("QualityIssueImages")]
public class QualityIssueImage
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid QualityIssueImageId { get; set; }

    public Guid IssueId { get; set; }
    public QualityIssue QualityIssue { get; set; } = null!;

    [Required]
    public string ImageData { get; set; } = string.Empty;

    [Required]
    [MaxLength(10)]
    public string ImageType { get; set; } = "file"; // "file" or "url"

    [MaxLength(500)]
    public string? OriginalName { get; set; }

    public long? FileSize { get; set; }

    public int Sequence { get; set; } = 0;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}

