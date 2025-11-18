using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.AuditLogs
{
    public record GetAuditLogsQuery(
     string TableName,
     Guid? RecordId,
     string Action,
     string SearchTerm,
     DateTime? FromDate,
     DateTime? ToDate
        ) : IRequest<Result<List<AuditLogDto>>>;

}
