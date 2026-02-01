using Dubox.Domain.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities;

[Table("QualityIssueImages")]
public class QualityIssueImage : BaseImageEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid QualityIssueImageId { get; set; }

    public Guid IssueId { get; set; }
    public QualityIssue QualityIssue { get; set; } = null!;

}

