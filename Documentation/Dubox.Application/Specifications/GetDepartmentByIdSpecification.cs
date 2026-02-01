using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications
{
    public class GetDepartmentByIdSpecification : Specification<Department>
    {
        public GetDepartmentByIdSpecification(Guid departmentId)
        {
            AddCriteria(dept => dept.DepartmentId == departmentId);

            AddInclude(nameof(Department.Manager));
        }
    }
}
