using Dubox.Application.DTOs;
using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications
{
    public class AuditLogSearchSpecification : Specification<AuditLog>
    {
        public AuditLogSearchSpecification(AuditLogSearchParams parameters)
        {
            if (!string.IsNullOrEmpty(parameters.TableName))
                AddCriteria(log => log.TableName == parameters.TableName);

            if (parameters.RecordId.HasValue)
                AddCriteria(log => log.RecordId == parameters.RecordId.Value);

            if (!string.IsNullOrEmpty(parameters.Action))
                AddCriteria(log => log.Action == parameters.Action);

            if (!string.IsNullOrEmpty(parameters.SearchTerm))
                AddCriteria(log =>
                    log.Description.Contains(parameters.SearchTerm) ||
                    log.OldValues.Contains(parameters.SearchTerm) ||
                    log.NewValues.Contains(parameters.SearchTerm));

            if (parameters.FromDate.HasValue)
                AddCriteria(log => log.ChangedDate >= parameters.FromDate.Value);
            if (parameters.ToDate.HasValue)
                AddCriteria(log => log.ChangedDate <= parameters.ToDate.Value);

            AddOrderByDescending(log => log.ChangedDate);
        }
    }
}
