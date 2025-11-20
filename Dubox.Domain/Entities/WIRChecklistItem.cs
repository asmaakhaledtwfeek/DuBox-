using Dubox.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities
{
    [Table("WIRChecklistItems")]
    public class WIRChecklistItem
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ChecklistItemId { get; set; }

        [Required]
        [ForeignKey(nameof(WIRCheckpoint))]
        public Guid WIRId { get; set; }
        [Required]
        [MaxLength(500)]
        public string CheckpointDescription { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? ReferenceDocument { get; set; }

        [MaxLength(20)]
        public CheckListItemStatusEnum Status { get; set; } = CheckListItemStatusEnum.Pending;// Pass, Fail, Pending

        public string? Remarks { get; set; }

        public int Sequence { get; set; }

        // Navigation properties
        public virtual WIRCheckpoint WIRCheckpoint { get; set; } = null!;
    }
}
