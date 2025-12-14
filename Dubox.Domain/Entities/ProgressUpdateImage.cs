using Dubox.Domain.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities;

[Table("ProgressUpdateImages")]
public class ProgressUpdateImage : BaseImageEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid ProgressUpdateImageId { get; set; }

    public Guid ProgressUpdateId { get; set; }
    public ProgressUpdate ProgressUpdate { get; set; } = null!;


}

