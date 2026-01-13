using Dubox.Domain.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities;

[Table("WIRCheckpointImages")]
public class WIRCheckpointImage : BaseImageEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid WIRCheckpointImageId { get; set; }

    [Required]
    [ForeignKey(nameof(WIRCheckpoint))]
    public Guid WIRId { get; set; }

    public virtual WIRCheckpoint WIRCheckpoint { get; set; } = null!;


}

