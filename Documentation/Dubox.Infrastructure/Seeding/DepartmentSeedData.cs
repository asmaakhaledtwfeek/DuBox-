using Dubox.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Infrastructure.Seeding
{
    public static class DepartmentSeesData
    {
        public static void SeedDepartmnts(ModelBuilder modelBuilder)
        {
            var seedDate = new DateTime(2024, 11, 1, 0, 0, 0, DateTimeKind.Utc);


            var departmentData = new List<Department>
{

    new Department { DepartmentId = Guid.Parse("D1000000-0000-0000-0000-000000000001"), DepartmentName = "IT", Code = "IT", IsActive = true, CreatedDate = seedDate },
    new Department { DepartmentId = Guid.Parse("D2000000-0000-0000-0000-000000000002"), DepartmentName = "Management", Code = "MGMT", IsActive = true,  CreatedDate = seedDate },
    new Department { DepartmentId = Guid.Parse("D3000000-0000-0000-0000-000000000003"), DepartmentName = "Engineering", Code = "ENG", IsActive = true,  CreatedDate = seedDate },
    new Department { DepartmentId = Guid.Parse("D4000000-0000-0000-0000-000000000004"), DepartmentName = "Construction", Code = "CONST", IsActive = true, CreatedDate = seedDate },
    new Department { DepartmentId = Guid.Parse("D5000000-0000-0000-0000-000000000005"), DepartmentName = "Quality", Code = "QLTY", IsActive = true, CreatedDate = seedDate },
    new Department { DepartmentId = Guid.Parse("D6000000-0000-0000-0000-000000000006"), DepartmentName = "Procurement", Code = "PROC", IsActive = true, CreatedDate = seedDate },
    new Department { DepartmentId = Guid.Parse("D7000000-0000-0000-0000-000000000007"), DepartmentName = "HSE", Code = "HSE", IsActive = true, CreatedDate = seedDate },
    new Department { DepartmentId = Guid.Parse("D8000000-0000-0000-0000-000000000008"), DepartmentName = "DuBox", Code = "DBX", IsActive = true, CreatedDate = seedDate },
    new Department { DepartmentId = Guid.Parse("D9000000-0000-0000-0000-000000000009"), DepartmentName = "DuPod", Code = "DPD", IsActive = true, CreatedDate = seedDate }
};

            modelBuilder.Entity<Department>().HasData(departmentData); ;
        }
    }
}
