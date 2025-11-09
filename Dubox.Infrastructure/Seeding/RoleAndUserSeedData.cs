using Dubox.Domain.Entities;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Infrastructure.Seeding;

public static class RoleAndUserSeedData
{
    public static void SeedRolesGroupsAndUsers(ModelBuilder modelBuilder)
    {
        var roles = CreateRoles();
        var groups = CreateGroups();
        var users = CreateUsers();
        var groupRoles = CreateGroupRoles();
        var userGroups = CreateUserGroups();
        var userRoles = CreateUserRoles();

        modelBuilder.Entity<Role>().HasData(roles);
        modelBuilder.Entity<Group>().HasData(groups);
        modelBuilder.Entity<User>().HasData(users);
        modelBuilder.Entity<GroupRole>().HasData(groupRoles);
        modelBuilder.Entity<UserGroup>().HasData(userGroups);
        modelBuilder.Entity<UserRole>().HasData(userRoles);
    }

    private static string HashPassword(string password)
    {
        byte[] salt = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10 };

        byte[] hash = KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 32);

        byte[] hashBytes = new byte[16 + 32];
        Array.Copy(salt, 0, hashBytes, 0, 16);
        Array.Copy(hash, 0, hashBytes, 16, 32);

        return Convert.ToBase64String(hashBytes);
    }

    private static List<Role> CreateRoles()
    {
        var seedDate = new DateTime(2024, 11, 1, 0, 0, 0, DateTimeKind.Utc);

        return new List<Role>
        {
            new Role { RoleId = Guid.Parse("11111111-1111-1111-1111-111111111111"), RoleName = "SystemAdmin", Description = "Full system administration access", IsActive = true, CreatedDate = seedDate },
            new Role { RoleId = Guid.Parse("22222222-2222-2222-2222-222222222222"), RoleName = "ProjectManager", Description = "Manage projects and teams", IsActive = true, CreatedDate = seedDate },
            new Role { RoleId = Guid.Parse("33333333-3333-3333-3333-333333333333"), RoleName = "SiteEngineer", Description = "Oversee construction site activities", IsActive = true, CreatedDate = seedDate },
            new Role { RoleId = Guid.Parse("44444444-4444-4444-4444-444444444444"), RoleName = "Foreman", Description = "Supervise construction workers", IsActive = true, CreatedDate = seedDate },
            new Role { RoleId = Guid.Parse("55555555-5555-5555-5555-555555555555"), RoleName = "QCInspector", Description = "Quality control and inspection", IsActive = true, CreatedDate = seedDate },
            new Role { RoleId = Guid.Parse("66666666-6666-6666-6666-666666666666"), RoleName = "ProcurementOfficer", Description = "Handle material procurement", IsActive = true, CreatedDate = seedDate },
            new Role { RoleId = Guid.Parse("77777777-7777-7777-7777-777777777777"), RoleName = "HSEOfficer", Description = "Health, Safety, and Environment oversight", IsActive = true, CreatedDate = seedDate },
            new Role { RoleId = Guid.Parse("88888888-8888-8888-8888-888888888888"), RoleName = "DesignEngineer", Description = "Design and BIM modeling", IsActive = true, CreatedDate = seedDate },
            new Role { RoleId = Guid.Parse("99999999-9999-9999-9999-999999999999"), RoleName = "CostEstimator", Description = "Cost estimation and budgeting", IsActive = true, CreatedDate = seedDate },
            new Role { RoleId = Guid.Parse("AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA"), RoleName = "Viewer", Description = "Read-only access to projects", IsActive = true, CreatedDate = seedDate }
        };
    }

    private static List<Group> CreateGroups()
    {
        var seedDate = new DateTime(2024, 11, 1, 0, 0, 0, DateTimeKind.Utc);

        return new List<Group>
        {
            new Group { GroupId = Guid.Parse("A1111111-1111-1111-1111-111111111111"), GroupName = "Management", Description = "Senior management and administrators", IsActive = true, CreatedDate = seedDate },
            new Group { GroupId = Guid.Parse("A2222222-2222-2222-2222-222222222222"), GroupName = "Engineering", Description = "Engineering department - design and site", IsActive = true, CreatedDate = seedDate },
            new Group { GroupId = Guid.Parse("A3333333-3333-3333-3333-333333333333"), GroupName = "Construction", Description = "Site construction team", IsActive = true, CreatedDate = seedDate },
            new Group { GroupId = Guid.Parse("A4444444-4444-4444-4444-444444444444"), GroupName = "QualityControl", Description = "Quality control and inspection team", IsActive = true, CreatedDate = seedDate },
            new Group { GroupId = Guid.Parse("A5555555-5555-5555-5555-555555555555"), GroupName = "Procurement", Description = "Procurement and logistics team", IsActive = true, CreatedDate = seedDate },
            new Group { GroupId = Guid.Parse("A6666666-6666-6666-6666-666666666666"), GroupName = "HSE", Description = "Health, Safety, and Environment team", IsActive = true, CreatedDate = seedDate },
            new Group { GroupId = Guid.Parse("A7777777-7777-7777-7777-777777777777"), GroupName = "DuBoxTeam", Description = "Modular construction team - DuBox", IsActive = true, CreatedDate = seedDate },
            new Group { GroupId = Guid.Parse("A8888888-8888-8888-8888-888888888888"), GroupName = "DuPodTeam", Description = "Plug-and-play modular solutions - DuPod", IsActive = true, CreatedDate = seedDate }
        };
    }

    private static List<User> CreateUsers()
    {
        var defaultPassword = HashPassword("AMANA@2024");
        var seedDate = new DateTime(2024, 11, 1, 0, 0, 0, DateTimeKind.Utc);

        return new List<User>
        {
            new User { UserId = Guid.Parse("F1111111-1111-1111-1111-111111111111"), Email = "admin@groupamana.com", PasswordHash = defaultPassword, FullName = "System Administrator", DepartmentId = Guid.Parse("D1000000-0000-0000-0000-000000000001"), IsActive = true, CreatedDate = seedDate },

            new User { UserId = Guid.Parse("F2222222-2222-2222-2222-222222222222"), Email = "ahmed.almazrouei@groupamana.com", PasswordHash = defaultPassword, FullName = "Ahmed Al Mazrouei", DepartmentId = Guid.Parse("D2000000-0000-0000-0000-000000000002"), IsActive = true, CreatedDate = seedDate },
            new User { UserId = Guid.Parse("F3333333-3333-3333-3333-333333333333"), Email = "sara.alkhan@groupamana.com", PasswordHash = defaultPassword, FullName = "Sara Al Khan", DepartmentId = Guid.Parse("D3000000-0000-0000-0000-000000000003"), IsActive = true, CreatedDate = seedDate },

            new User { UserId = Guid.Parse("F4444444-4444-4444-4444-444444444444"), Email = "mohammed.hassan@groupamana.com", PasswordHash = defaultPassword, FullName = "Mohammed Hassan", DepartmentId = Guid.Parse("D4000000-0000-0000-0000-000000000004"), IsActive = true, CreatedDate = seedDate },
            new User { UserId = Guid.Parse("F5555555-5555-5555-5555-555555555555"), Email = "fatima.alali@groupamana.com", PasswordHash = defaultPassword, FullName = "Fatima Al Ali", DepartmentId = Guid.Parse("D5000000-0000-0000-0000-000000000005"), IsActive = true, CreatedDate = seedDate },
            new User { UserId = Guid.Parse("F6666666-6666-6666-6666-666666666666"), Email = "khalid.omar@groupamana.com", PasswordHash = defaultPassword, FullName = "Khalid Omar", DepartmentId = Guid.Parse("D1000000-0000-0000-0000-000000000001"), IsActive = true, CreatedDate = seedDate },

            new User { UserId = Guid.Parse("F7777777-7777-7777-7777-777777777777"), Email = "ali.mohammed@groupamana.com", PasswordHash = defaultPassword, FullName = "Ali Mohammed", DepartmentId = Guid.Parse("D6000000-0000-0000-0000-000000000006"), IsActive = true, CreatedDate = seedDate },
            new User { UserId = Guid.Parse("F8888888-8888-8888-8888-888888888888"), Email = "omar.saleh@groupamana.com", PasswordHash = defaultPassword, FullName = "Omar Saleh", DepartmentId = Guid.Parse("D6000000-0000-0000-0000-000000000006"), IsActive = true, CreatedDate = seedDate },
            new User { UserId = Guid.Parse("F9999999-9999-9999-9999-999999999999"), Email = "youssef.ahmed@groupamana.com", PasswordHash = defaultPassword, FullName = "Youssef Ahmed", DepartmentId = Guid.Parse("D7000000-0000-0000-0000-000000000007"), IsActive = true, CreatedDate = seedDate },

            new User { UserId = Guid.Parse("FAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA"), Email = "layla.ibrahim@groupamana.com", PasswordHash = defaultPassword, FullName = "Layla Ibrahim", DepartmentId = Guid.Parse("D7000000-0000-0000-0000-000000000007"), IsActive = true, CreatedDate = seedDate },
            new User { UserId = Guid.Parse("FBBBBBBB-BBBB-BBBB-BBBB-BBBBBBBBBBBB"), Email = "hamza.khalil@groupamana.com", PasswordHash = defaultPassword, FullName = "Hamza Khalil",DepartmentId = Guid.Parse("D8000000-0000-0000-0000-000000000008"), IsActive = true, CreatedDate = seedDate },

            new User { UserId = Guid.Parse("FCCCCCCC-CCCC-CCCC-CCCC-CCCCCCCCCCCC"), Email = "noor.alhassan@groupamana.com", PasswordHash = defaultPassword, FullName = "Noor Al Hassan", DepartmentId = Guid.Parse("D8000000-0000-0000-0000-000000000008"), IsActive = true, CreatedDate = seedDate },
            new User { UserId = Guid.Parse("FDDDDDDD-DDDD-DDDD-DDDD-DDDDDDDDDDDD"), Email = "zaid.mansour@groupamana.com", PasswordHash = defaultPassword, FullName = "Zaid Mansour", DepartmentId = Guid.Parse("D9000000-0000-0000-0000-000000000009"), IsActive = true, CreatedDate = seedDate },

            new User { UserId = Guid.Parse("FEEEEEEE-EEEE-EEEE-EEEE-EEEEEEEEEEEE"), Email = "maryam.Said@groupamana.com", PasswordHash = defaultPassword, FullName = "Maryam Said", DepartmentId = Guid.Parse("D9000000-0000-0000-0000-000000000009"), IsActive = true, CreatedDate = seedDate },
            new User { UserId = Guid.Parse("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF"), Email = "tariq.abdullah@groupamana.com", PasswordHash = defaultPassword, FullName = "Tariq Abdullah", DepartmentId = Guid.Parse("D5000000-0000-0000-0000-000000000005"), IsActive = true, CreatedDate = seedDate },

            new User { UserId = Guid.Parse("F0000001-0000-0000-0000-000000000001"), Email = "rania.khalifa@groupamana.com", PasswordHash = defaultPassword, FullName = "Rania Khalifa", DepartmentId = Guid.Parse("D4000000-0000-0000-0000-000000000004"), IsActive = true, CreatedDate = seedDate },
            new User { UserId = Guid.Parse("F0000002-0000-0000-0000-000000000002"), Email = "salim.rashid@groupamana.com", PasswordHash = defaultPassword, FullName = "Salim Rashid", DepartmentId = Guid.Parse("D3000000-0000-0000-0000-000000000003"), IsActive = true, CreatedDate = seedDate },

            new User { UserId = Guid.Parse("F0000003-0000-0000-0000-000000000003"), Email = "huda.almarri@groupamana.com", PasswordHash = defaultPassword, FullName = "Huda Al Marri", DepartmentId = Guid.Parse("D2000000-0000-0000-0000-000000000002"), IsActive = true, CreatedDate = seedDate },
            new User { UserId = Guid.Parse("F0000004-0000-0000-0000-000000000004"), Email = "faisal.sultan@groupamana.com", PasswordHash = defaultPassword, FullName = "Faisal Sultan", DepartmentId = Guid.Parse("D8000000-0000-0000-0000-000000000008"), IsActive = true, CreatedDate = seedDate }
        };
    }

    private static List<GroupRole> CreateGroupRoles()
    {
        var seedDate = new DateTime(2024, 11, 1, 0, 0, 0, DateTimeKind.Utc);

        return new List<GroupRole>
        {
            new GroupRole { GroupRoleId = Guid.Parse("B1111111-1111-1111-1111-111111111111"), GroupId = Guid.Parse("A1111111-1111-1111-1111-111111111111"), RoleId = Guid.Parse("11111111-1111-1111-1111-111111111111"), AssignedDate = seedDate },
            new GroupRole { GroupRoleId = Guid.Parse("B2222222-2222-2222-2222-222222222222"), GroupId = Guid.Parse("A1111111-1111-1111-1111-111111111111"), RoleId = Guid.Parse("22222222-2222-2222-2222-222222222222"), AssignedDate = seedDate },

            new GroupRole { GroupRoleId = Guid.Parse("B3333333-3333-3333-3333-333333333333"), GroupId = Guid.Parse("A2222222-2222-2222-2222-222222222222"), RoleId = Guid.Parse("33333333-3333-3333-3333-333333333333"), AssignedDate = seedDate },
            new GroupRole { GroupRoleId = Guid.Parse("B4444444-4444-4444-4444-444444444444"), GroupId = Guid.Parse("A2222222-2222-2222-2222-222222222222"), RoleId = Guid.Parse("88888888-8888-8888-8888-888888888888"), AssignedDate = seedDate },
            new GroupRole { GroupRoleId = Guid.Parse("B5555555-5555-5555-5555-555555555555"), GroupId = Guid.Parse("A2222222-2222-2222-2222-222222222222"), RoleId = Guid.Parse("AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA"), AssignedDate = seedDate },

            new GroupRole { GroupRoleId = Guid.Parse("B6666666-6666-6666-6666-666666666666"), GroupId = Guid.Parse("A3333333-3333-3333-3333-333333333333"), RoleId = Guid.Parse("44444444-4444-4444-4444-444444444444"), AssignedDate = seedDate },
            new GroupRole { GroupRoleId = Guid.Parse("B7777777-7777-7777-7777-777777777777"), GroupId = Guid.Parse("A3333333-3333-3333-3333-333333333333"), RoleId = Guid.Parse("AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA"), AssignedDate = seedDate },

            new GroupRole { GroupRoleId = Guid.Parse("B8888888-8888-8888-8888-888888888888"), GroupId = Guid.Parse("A4444444-4444-4444-4444-444444444444"), RoleId = Guid.Parse("55555555-5555-5555-5555-555555555555"), AssignedDate = seedDate },
            new GroupRole { GroupRoleId = Guid.Parse("B9999999-9999-9999-9999-999999999999"), GroupId = Guid.Parse("A4444444-4444-4444-4444-444444444444"), RoleId = Guid.Parse("AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA"), AssignedDate = seedDate },

            new GroupRole { GroupRoleId = Guid.Parse("BAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA"), GroupId = Guid.Parse("A5555555-5555-5555-5555-555555555555"), RoleId = Guid.Parse("66666666-6666-6666-6666-666666666666"), AssignedDate = seedDate },
            new GroupRole { GroupRoleId = Guid.Parse("BBBBBBBB-BBBB-BBBB-BBBB-BBBBBBBBBBBB"), GroupId = Guid.Parse("A5555555-5555-5555-5555-555555555555"), RoleId = Guid.Parse("AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA"), AssignedDate = seedDate },

            new GroupRole { GroupRoleId = Guid.Parse("BCCCCCCC-CCCC-CCCC-CCCC-CCCCCCCCCCCC"), GroupId = Guid.Parse("A6666666-6666-6666-6666-666666666666"), RoleId = Guid.Parse("77777777-7777-7777-7777-777777777777"), AssignedDate = seedDate },
            new GroupRole { GroupRoleId = Guid.Parse("BDDDDDDD-DDDD-DDDD-DDDD-DDDDDDDDDDDD"), GroupId = Guid.Parse("A6666666-6666-6666-6666-666666666666"), RoleId = Guid.Parse("AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA"), AssignedDate = seedDate },

            new GroupRole { GroupRoleId = Guid.Parse("BEEEEEEE-EEEE-EEEE-EEEE-EEEEEEEEEEEE"), GroupId = Guid.Parse("A7777777-7777-7777-7777-777777777777"), RoleId = Guid.Parse("88888888-8888-8888-8888-888888888888"), AssignedDate = seedDate },
            new GroupRole { GroupRoleId = Guid.Parse("BFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF"), GroupId = Guid.Parse("A7777777-7777-7777-7777-777777777777"), RoleId = Guid.Parse("22222222-2222-2222-2222-222222222222"), AssignedDate = seedDate },

            new GroupRole { GroupRoleId = Guid.Parse("B0000001-0000-0000-0000-000000000001"), GroupId = Guid.Parse("A8888888-8888-8888-8888-888888888888"), RoleId = Guid.Parse("88888888-8888-8888-8888-888888888888"), AssignedDate = seedDate },
            new GroupRole { GroupRoleId = Guid.Parse("B0000002-0000-0000-0000-000000000002"), GroupId = Guid.Parse("A8888888-8888-8888-8888-888888888888"), RoleId = Guid.Parse("22222222-2222-2222-2222-222222222222"), AssignedDate = seedDate }
        };
    }

    private static List<UserGroup> CreateUserGroups()
    {
        var seedDate = new DateTime(2024, 11, 1, 0, 0, 0, DateTimeKind.Utc);

        return new List<UserGroup>
        {
            new UserGroup { UserGroupId = Guid.Parse("C1111111-1111-1111-1111-111111111111"), UserId = Guid.Parse("F1111111-1111-1111-1111-111111111111"), GroupId = Guid.Parse("A1111111-1111-1111-1111-111111111111"), JoinedDate = seedDate },

            new UserGroup { UserGroupId = Guid.Parse("C2222222-2222-2222-2222-222222222222"), UserId = Guid.Parse("F2222222-2222-2222-2222-222222222222"), GroupId = Guid.Parse("A1111111-1111-1111-1111-111111111111"), JoinedDate = seedDate },
            new UserGroup { UserGroupId = Guid.Parse("C3333333-3333-3333-3333-333333333333"), UserId = Guid.Parse("F3333333-3333-3333-3333-333333333333"), GroupId = Guid.Parse("A1111111-1111-1111-1111-111111111111"), JoinedDate = seedDate },

            new UserGroup { UserGroupId = Guid.Parse("C4444444-4444-4444-4444-444444444444"), UserId = Guid.Parse("F4444444-4444-4444-4444-444444444444"), GroupId = Guid.Parse("A2222222-2222-2222-2222-222222222222"), JoinedDate = seedDate },
            new UserGroup { UserGroupId = Guid.Parse("C5555555-5555-5555-5555-555555555555"), UserId = Guid.Parse("F5555555-5555-5555-5555-555555555555"), GroupId = Guid.Parse("A2222222-2222-2222-2222-222222222222"), JoinedDate = seedDate },
            new UserGroup { UserGroupId = Guid.Parse("C6666666-6666-6666-6666-666666666666"), UserId = Guid.Parse("F6666666-6666-6666-6666-666666666666"), GroupId = Guid.Parse("A2222222-2222-2222-2222-222222222222"), JoinedDate = seedDate },

            new UserGroup { UserGroupId = Guid.Parse("C7777777-7777-7777-7777-777777777777"), UserId = Guid.Parse("F7777777-7777-7777-7777-777777777777"), GroupId = Guid.Parse("A3333333-3333-3333-3333-333333333333"), JoinedDate = seedDate },
            new UserGroup { UserGroupId = Guid.Parse("C8888888-8888-8888-8888-888888888888"), UserId = Guid.Parse("F8888888-8888-8888-8888-888888888888"), GroupId = Guid.Parse("A3333333-3333-3333-3333-333333333333"), JoinedDate = seedDate },
            new UserGroup { UserGroupId = Guid.Parse("C9999999-9999-9999-9999-999999999999"), UserId = Guid.Parse("F9999999-9999-9999-9999-999999999999"), GroupId = Guid.Parse("A3333333-3333-3333-3333-333333333333"), JoinedDate = seedDate },

            new UserGroup { UserGroupId = Guid.Parse("CAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA"), UserId = Guid.Parse("FAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA"), GroupId = Guid.Parse("A4444444-4444-4444-4444-444444444444"), JoinedDate = seedDate },
            new UserGroup { UserGroupId = Guid.Parse("CBBBBBBB-BBBB-BBBB-BBBB-BBBBBBBBBBBB"), UserId = Guid.Parse("FBBBBBBB-BBBB-BBBB-BBBB-BBBBBBBBBBBB"), GroupId = Guid.Parse("A4444444-4444-4444-4444-444444444444"), JoinedDate = seedDate },

            new UserGroup { UserGroupId = Guid.Parse("CCCCCCCC-CCCC-CCCC-CCCC-CCCCCCCCCCCC"), UserId = Guid.Parse("FCCCCCCC-CCCC-CCCC-CCCC-CCCCCCCCCCCC"), GroupId = Guid.Parse("A5555555-5555-5555-5555-555555555555"), JoinedDate = seedDate },
            new UserGroup { UserGroupId = Guid.Parse("CDDDDDDD-DDDD-DDDD-DDDD-DDDDDDDDDDDD"), UserId = Guid.Parse("FDDDDDDD-DDDD-DDDD-DDDD-DDDDDDDDDDDD"), GroupId = Guid.Parse("A5555555-5555-5555-5555-555555555555"), JoinedDate = seedDate },

            new UserGroup { UserGroupId = Guid.Parse("CEEEEEEE-EEEE-EEEE-EEEE-EEEEEEEEEEEE"), UserId = Guid.Parse("FEEEEEEE-EEEE-EEEE-EEEE-EEEEEEEEEEEE"), GroupId = Guid.Parse("A6666666-6666-6666-6666-666666666666"), JoinedDate = seedDate },
            new UserGroup { UserGroupId = Guid.Parse("CFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF"), UserId = Guid.Parse("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF"), GroupId = Guid.Parse("A6666666-6666-6666-6666-666666666666"), JoinedDate = seedDate },

            new UserGroup { UserGroupId = Guid.Parse("C0000001-0000-0000-0000-000000000001"), UserId = Guid.Parse("F0000001-0000-0000-0000-000000000001"), GroupId = Guid.Parse("A7777777-7777-7777-7777-777777777777"), JoinedDate = seedDate },
            new UserGroup { UserGroupId = Guid.Parse("C0000002-0000-0000-0000-000000000002"), UserId = Guid.Parse("F0000002-0000-0000-0000-000000000002"), GroupId = Guid.Parse("A7777777-7777-7777-7777-777777777777"), JoinedDate = seedDate },

            new UserGroup { UserGroupId = Guid.Parse("C0000003-0000-0000-0000-000000000003"), UserId = Guid.Parse("F0000003-0000-0000-0000-000000000003"), GroupId = Guid.Parse("A8888888-8888-8888-8888-888888888888"), JoinedDate = seedDate },
            new UserGroup { UserGroupId = Guid.Parse("C0000004-0000-0000-0000-000000000004"), UserId = Guid.Parse("F0000004-0000-0000-0000-000000000004"), GroupId = Guid.Parse("A8888888-8888-8888-8888-888888888888"), JoinedDate = seedDate }
        };
    }

    private static List<UserRole> CreateUserRoles()
    {
        var seedDate = new DateTime(2024, 11, 1, 0, 0, 0, DateTimeKind.Utc);

        return new List<UserRole>
        {
            new UserRole { UserRoleId = Guid.Parse("D1111111-1111-1111-1111-111111111111"), UserId = Guid.Parse("F2222222-2222-2222-2222-222222222222"), RoleId = Guid.Parse("99999999-9999-9999-9999-999999999999"), AssignedDate = seedDate },
            new UserRole { UserRoleId = Guid.Parse("D2222222-2222-2222-2222-222222222222"), UserId = Guid.Parse("F4444444-4444-4444-4444-444444444444"), RoleId = Guid.Parse("22222222-2222-2222-2222-222222222222"), AssignedDate = seedDate }
        };
    }
}

