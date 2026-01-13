using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications
{
    public class GetTeamWithIncludesSpecification : Specification<Team>
    {
        public GetTeamWithIncludesSpecification(Guid teamId)
        {
            AddCriteria(team => team.TeamId == teamId);

            AddInclude(nameof(Team.Department));
            AddInclude(nameof(Team.TeamLeader));
            AddInclude("TeamLeader.User"); // Include User for TeamLeader navigation
            AddInclude(nameof(Team.Members));
            
            // Enable split query to avoid Cartesian explosion with Members collection
            EnableSplitQuery();
        }


        public GetTeamWithIncludesSpecification(string? search, string? department, string? trade, bool? isActive, List<Guid>? accessibleTeamIds, int pageSize, int pageNumber)
        {
            AddInclude(nameof(Team.Department));
            AddInclude(nameof(Team.TeamLeader));
            AddInclude("TeamLeader.User"); 
            AddInclude(nameof(Team.Members));
            EnableSplitQuery();

            if (accessibleTeamIds != null)
                AddCriteria(team => accessibleTeamIds.Contains(team.TeamId));
            
            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchTerm = search.Trim().ToLowerInvariant();
                AddCriteria(team => 
                    team.TeamCode.ToLower().Contains(searchTerm) ||
                    team.TeamName.ToLower().Contains(searchTerm) ||
                    (team.Department != null && team.Department.DepartmentName.ToLower().Contains(searchTerm)) ||
                    (team.Trade != null && team.Trade.ToLower().Contains(searchTerm)) ||
                    (team.TeamLeader != null && team.TeamLeader.User != null && team.TeamLeader.User.FullName != null && team.TeamLeader.User.FullName.ToLower().Contains(searchTerm))
                );
            }

            if (!string.IsNullOrWhiteSpace(department))
                AddCriteria(team => team.Department != null && team.Department.DepartmentName == department);
            
            if (!string.IsNullOrWhiteSpace(trade))
                AddCriteria(team => team.Trade == trade);
            
            if (isActive.HasValue)
                AddCriteria(team => team.IsActive == isActive.Value);
            
            ApplyPaging(pageSize, pageNumber);
        }
    }
}
