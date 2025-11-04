using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities
{
    [Table("AuditLog")]
    public class AuditLog
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AuditId { get; set; }

        [MaxLength(100)]
        public string? TableName { get; set; }

        public int? RecordId { get; set; }

        [MaxLength(50)]
        public string? Action { get; set; } // INSERT, UPDATE, DELETE

        public string? OldValues { get; set; }

        public string? NewValues { get; set; }

        [MaxLength(100)]
        public string? ChangedBy { get; set; }

        public DateTime ChangedDate { get; set; } = DateTime.UtcNow;

        [MaxLength(50)]
        public string? IPAddress { get; set; }

        [MaxLength(200)]
        public string? DeviceInfo { get; set; }
    }
}
