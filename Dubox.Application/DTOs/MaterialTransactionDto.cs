namespace Dubox.Application.DTOs
{
    public class MaterialTransactionDto
    {
        public Guid TransactionId { get; set; }
        public decimal? Quantity { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? TransactionType { get; set; }
        public string? Reference { get; set; }
        public string? Remarks { get; set; }
        public Guid MaterialId { get; set; }
        public string MaterialCode { get; set; } = string.Empty;
        public string MaterialName { get; set; } = string.Empty;
        public Guid? PerformedById { get; set; }
        public string PerformedByUserName { get; set; } = string.Empty;
        public Guid? BoxId { get; set; }
        public string? BoxCode { get; set; }
        public Guid? BoxActivityId { get; set; }
        public string? ActivityCode { get; set; }
        public string? ActivityName { get; set; }
    }
}
