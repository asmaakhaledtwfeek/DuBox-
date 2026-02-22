using Dubox.Domain.Enums;

namespace Dubox.Application.DTOs
{
    public class BoxExchangeDto
    {
        public Guid BoxExchangeId { get; set; }
        public Guid BoxId { get; set; }
        public string? BoxTag { get; set; }
        public string? BoxName { get; set; }
        public Guid QualityIssueId { get; set; }
        public string? IssueNumber { get; set; }
        
        // Exchanged with box (target box for exchange)
        public Guid? ExchangedWithBoxId { get; set; }
        public Guid? ExchangedWithBoxProjectId { get; set; }
        public string? ExchangedWithBoxTag { get; set; }
        public string? ExchangedWithBoxName { get; set; }
        
        // Original values
        public string? OldBuildingNumber { get; set; }
        public string? OldFloor { get; set; }
        public string? OldBoxTag { get; set; }
        
        // Requested new values
        public string? NewBuildingNumber { get; set; }
        public string? NewFloor { get; set; }
        public string? NewBoxTag { get; set; }
        
        // Exchange request details
        public string? RequestReason { get; set; }
        public ExchangeRequestStatusEnum Status { get; set; }
        public DateTime RequestedDate { get; set; }
        public Guid RequestedBy { get; set; }
        public string? RequestedByName { get; set; }
        
        public DateTime? ApprovedDate { get; set; }
        public Guid? ApprovedBy { get; set; }
        public string? ApprovedByName { get; set; }
        
        public DateTime? RejectedDate { get; set; }
        public Guid? RejectedBy { get; set; }
        public string? RejectedByName { get; set; }
        public string? RejectionReason { get; set; }
        
        public DateTime? AppliedDate { get; set; }
        public Guid? AppliedBy { get; set; }
        public string? AppliedByName { get; set; }
        
        public DateTime CreatedDate { get; set; }
    }
}
