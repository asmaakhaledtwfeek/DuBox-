using Dubox.Application.DTOs;
using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications
{
    public class AuditLogSearchSpecification : Specification<AuditLog>
    {
        public AuditLogSearchSpecification(AuditLogSearchParams parameters, int? pageSize = null, int? pageNumber = null)
        {
            if (!string.IsNullOrEmpty(parameters.TableName))
                AddCriteria(log => log.TableName == parameters.TableName);

            if (parameters.RecordId.HasValue)
                AddCriteria(log => log.RecordId == parameters.RecordId.Value);

            if (!string.IsNullOrEmpty(parameters.Action))
                AddCriteria(log => log.Action == parameters.Action);

            if (parameters.FromDate.HasValue)
                AddCriteria(log => log.ChangedDate >= parameters.FromDate.Value);
            if (parameters.ToDate.HasValue)
                AddCriteria(log => log.ChangedDate <= parameters.ToDate.Value);

            if (parameters.ChangedBy.HasValue)
                AddCriteria(log => log.ChangedBy == parameters.ChangedBy.Value);

            AddOrderByDescending(log => log.ChangedDate);

            // Apply pagination if provided
            if (pageSize.HasValue && pageNumber.HasValue && pageSize.Value > 0 && pageNumber.Value > 0)
            {
                ApplyPaging(pageSize.Value, pageNumber.Value);
            }
        }
    }
}
