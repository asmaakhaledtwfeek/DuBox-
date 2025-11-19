namespace Dubox.Application.DTOs
{
    public class AuditLogSearchParams
    {
        public string TableName { get; set; }
        public Guid? RecordId { get; set; }
        public string Action { get; set; }
        public string SearchTerm { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
