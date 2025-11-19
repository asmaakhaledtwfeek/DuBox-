namespace Dubox.Application.DTOs
{
    public class AuditLogDto
    {
        public Guid AuditLogId { get; set; }
        public string TableName { get; set; }
        public Guid RecordId { get; set; }
        public string Action { get; set; }
        public string Description { get; set; }
        public DateTime Timestamp { get; set; }
        public string OldValues { get; set; }

        public string NewValues { get; set; }
        public Guid? ChangedBy { get; set; }
        public string ChangedByUsername { get; set; }
    }
}
