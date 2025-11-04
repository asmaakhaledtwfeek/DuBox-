using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities
{
    [Table("WIRChecklistItems")]
    public class WIRChecklistItem
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ChecklistItemId { get; set; }

        [Required]
        [ForeignKey(nameof(WIRCheckpoint))]
        public int WIRId { get; set; }

        [MaxLength(500)]
        public string? CheckpointDescription { get; set; }

        [MaxLength(200)]
        public string? ReferenceDocument { get; set; }

        [MaxLength(20)]
        public string? Status { get; set; } // Pass, Fail, N/A

        public string? Remarks { get; set; }

        public int Sequence { get; set; }

        // Navigation properties
        public virtual WIRCheckpoint WIRCheckpoint { get; set; } = null!;
    }
}
