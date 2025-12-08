namespace Dubox.Application.Specifications
{
    using Dubox.Application.Features.WIRCheckpoints.Queries;
    using Dubox.Domain.Entities;
    using Dubox.Domain.Specification;

    public class GetWIRCheckpointsSpecification : Specification<WIRCheckpoint>
    {
        public GetWIRCheckpointsSpecification(GetWIRCheckpointsQuery query)
        {
            AddInclude(nameof(WIRCheckpoint.Box));
            AddInclude($"{nameof(WIRCheckpoint.Box)}.{nameof(WIRCheckpoint.Box.Project)}");
            AddInclude(nameof(WIRCheckpoint.ChecklistItems));
            AddInclude(nameof(WIRCheckpoint.QualityIssues));
            // NOTE: Don't include Images or QualityIssues.Images - base64 ImageData is too large
            // Image metadata is loaded separately with lightweight query
            
            // Enable split query to avoid Cartesian explosion with multiple collection includes
            

            if (!string.IsNullOrWhiteSpace(query.ProjectCode))
                AddCriteria(x => x.Box.Project.ProjectCode == query.ProjectCode);

            if (!string.IsNullOrWhiteSpace(query.BoxTag))
                AddCriteria(x => x.Box.BoxTag == query.BoxTag);

            if (query.Status.HasValue)
                AddCriteria(x => x.Status == query.Status.Value);

            if (!string.IsNullOrWhiteSpace(query.WIRNumber))
            {
                var wirNumberLower = query.WIRNumber.ToLower().Trim();
                AddCriteria(x => x.WIRNumber != null && x.WIRNumber.ToLower().Contains(wirNumberLower));
            }
            if (query.From.HasValue)
                AddCriteria(x => x.CreatedDate >= query.From.Value);

            if (query.To.HasValue)
                AddCriteria(x => x.CreatedDate <= query.To.Value);

            AddOrderByDescending(x => x.CreatedDate);
        }
    }

}
