using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.AuditLogs.Queries
{
    public class GetAuditLogsQuery : IRequest<Result<PaginatedAuditLogsResponseDto>>
    {
        public string? TableName { get; set; }
        public Guid? RecordId { get; set; }
        public string? Action { get; set; }
        public string? SearchTerm { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public Guid? ChangedBy { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 25;

    }
}
