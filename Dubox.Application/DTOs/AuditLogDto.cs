namespace Dubox.Application.DTOs
{
    public class AuditLogDto
    {
        public Guid AuditLogId { get; set; }
        public string TableName { get; set; }
        public Guid RecordId { get; set; }
        public string? EntityDisplayName { get; set; }
        public string Action { get; set; }
        public string Description { get; set; }
        public DateTime Timestamp { get; set; }
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
        public List<FieldChangeDto> Changes { get; set; } = new List<FieldChangeDto>();
        public Guid? ChangedBy { get; set; }
        public string? ChangedByUsername { get; set; }
        public string? ChangedByFullName { get; set; }
    }

    public class FieldChangeDto
    {
        public string Field { get; set; } = string.Empty;
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
    }
}
