using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UsersAndRolesSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Groups",
                columns: new[] { "GroupId", "CreatedDate", "Description", "GroupName", "IsActive" },
                values: new object[,]
                {
                    { new Guid("a1111111-1111-1111-1111-111111111111"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Senior management and administrators", "Management", true },
                    { new Guid("a2222222-2222-2222-2222-222222222222"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Engineering department - design and site", "Engineering", true },
                    { new Guid("a3333333-3333-3333-3333-333333333333"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Site construction team", "Construction", true },
                    { new Guid("a4444444-4444-4444-4444-444444444444"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Quality control and inspection team", "QualityControl", true },
                    { new Guid("a5555555-5555-5555-5555-555555555555"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Procurement and logistics team", "Procurement", true },
                    { new Guid("a6666666-6666-6666-6666-666666666666"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Health, Safety, and Environment team", "HSE", true },
                    { new Guid("a7777777-7777-7777-7777-777777777777"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Modular construction team - DuBox", "DuBoxTeam", true },
                    { new Guid("a8888888-8888-8888-8888-888888888888"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Plug-and-play modular solutions - DuPod", "DuPodTeam", true }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RoleId", "CreatedDate", "Description", "IsActive", "RoleName" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Full system administration access", true, "SystemAdmin" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Manage projects and teams", true, "ProjectManager" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Oversee construction site activities", true, "SiteEngineer" },
                    { new Guid("44444444-4444-4444-4444-444444444444"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Supervise construction workers", true, "Foreman" },
                    { new Guid("55555555-5555-5555-5555-555555555555"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Quality control and inspection", true, "QCInspector" },
                    { new Guid("66666666-6666-6666-6666-666666666666"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Handle material procurement", true, "ProcurementOfficer" },
                    { new Guid("77777777-7777-7777-7777-777777777777"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Health, Safety, and Environment oversight", true, "HSEOfficer" },
                    { new Guid("88888888-8888-8888-8888-888888888888"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Design and BIM modeling", true, "DesignEngineer" },
                    { new Guid("99999999-9999-9999-9999-999999999999"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Cost estimation and budgeting", true, "CostEstimator" },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Read-only access to projects", true, "Viewer" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CreatedDate", "DepartmentId", "Email", "FullName", "IsActive", "LastLoginDate", "PasswordHash" },
                values: new object[,]
                {
                    { new Guid("f0000001-0000-0000-0000-000000000001"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("d4000000-0000-0000-0000-000000000004"), "rania.khalifa@groupamana.com", "Rania Khalifa", true, null, "AQIDBAUGBwgJCgsMDQ4PEL47IPnruFDHjvgQn4gt+aHjj9Wvhi+9Lw6p4tvk8d7H" },
                    { new Guid("f0000002-0000-0000-0000-000000000002"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("d3000000-0000-0000-0000-000000000003"), "salim.rashid@groupamana.com", "Salim Rashid", true, null, "AQIDBAUGBwgJCgsMDQ4PEL47IPnruFDHjvgQn4gt+aHjj9Wvhi+9Lw6p4tvk8d7H" },
                    { new Guid("f0000003-0000-0000-0000-000000000003"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("d2000000-0000-0000-0000-000000000002"), "huda.almarri@groupamana.com", "Huda Al Marri", true, null, "AQIDBAUGBwgJCgsMDQ4PEL47IPnruFDHjvgQn4gt+aHjj9Wvhi+9Lw6p4tvk8d7H" },
                    { new Guid("f0000004-0000-0000-0000-000000000004"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("d8000000-0000-0000-0000-000000000008"), "faisal.sultan@groupamana.com", "Faisal Sultan", true, null, "AQIDBAUGBwgJCgsMDQ4PEL47IPnruFDHjvgQn4gt+aHjj9Wvhi+9Lw6p4tvk8d7H" },
                    { new Guid("f1111111-1111-1111-1111-111111111111"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("d1000000-0000-0000-0000-000000000001"), "admin@groupamana.com", "System Administrator", true, null, "AQIDBAUGBwgJCgsMDQ4PEL47IPnruFDHjvgQn4gt+aHjj9Wvhi+9Lw6p4tvk8d7H" },
                    { new Guid("f2222222-2222-2222-2222-222222222222"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("d2000000-0000-0000-0000-000000000002"), "ahmed.almazrouei@groupamana.com", "Ahmed Al Mazrouei", true, null, "AQIDBAUGBwgJCgsMDQ4PEL47IPnruFDHjvgQn4gt+aHjj9Wvhi+9Lw6p4tvk8d7H" },
                    { new Guid("f3333333-3333-3333-3333-333333333333"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("d3000000-0000-0000-0000-000000000003"), "sara.alkhan@groupamana.com", "Sara Al Khan", true, null, "AQIDBAUGBwgJCgsMDQ4PEL47IPnruFDHjvgQn4gt+aHjj9Wvhi+9Lw6p4tvk8d7H" },
                    { new Guid("f4444444-4444-4444-4444-444444444444"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("d4000000-0000-0000-0000-000000000004"), "mohammed.hassan@groupamana.com", "Mohammed Hassan", true, null, "AQIDBAUGBwgJCgsMDQ4PEL47IPnruFDHjvgQn4gt+aHjj9Wvhi+9Lw6p4tvk8d7H" },
                    { new Guid("f5555555-5555-5555-5555-555555555555"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("d5000000-0000-0000-0000-000000000005"), "fatima.alali@groupamana.com", "Fatima Al Ali", true, null, "AQIDBAUGBwgJCgsMDQ4PEL47IPnruFDHjvgQn4gt+aHjj9Wvhi+9Lw6p4tvk8d7H" },
                    { new Guid("f6666666-6666-6666-6666-666666666666"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("d1000000-0000-0000-0000-000000000001"), "khalid.omar@groupamana.com", "Khalid Omar", true, null, "AQIDBAUGBwgJCgsMDQ4PEL47IPnruFDHjvgQn4gt+aHjj9Wvhi+9Lw6p4tvk8d7H" },
                    { new Guid("f7777777-7777-7777-7777-777777777777"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("d6000000-0000-0000-0000-000000000006"), "ali.mohammed@groupamana.com", "Ali Mohammed", true, null, "AQIDBAUGBwgJCgsMDQ4PEL47IPnruFDHjvgQn4gt+aHjj9Wvhi+9Lw6p4tvk8d7H" },
                    { new Guid("f8888888-8888-8888-8888-888888888888"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("d6000000-0000-0000-0000-000000000006"), "omar.saleh@groupamana.com", "Omar Saleh", true, null, "AQIDBAUGBwgJCgsMDQ4PEL47IPnruFDHjvgQn4gt+aHjj9Wvhi+9Lw6p4tvk8d7H" },
                    { new Guid("f9999999-9999-9999-9999-999999999999"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("d7000000-0000-0000-0000-000000000007"), "youssef.ahmed@groupamana.com", "Youssef Ahmed", true, null, "AQIDBAUGBwgJCgsMDQ4PEL47IPnruFDHjvgQn4gt+aHjj9Wvhi+9Lw6p4tvk8d7H" },
                    { new Guid("faaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("d7000000-0000-0000-0000-000000000007"), "layla.ibrahim@groupamana.com", "Layla Ibrahim", true, null, "AQIDBAUGBwgJCgsMDQ4PEL47IPnruFDHjvgQn4gt+aHjj9Wvhi+9Lw6p4tvk8d7H" },
                    { new Guid("fbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("d8000000-0000-0000-0000-000000000008"), "hamza.khalil@groupamana.com", "Hamza Khalil", true, null, "AQIDBAUGBwgJCgsMDQ4PEL47IPnruFDHjvgQn4gt+aHjj9Wvhi+9Lw6p4tvk8d7H" },
                    { new Guid("fccccccc-cccc-cccc-cccc-cccccccccccc"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("d8000000-0000-0000-0000-000000000008"), "noor.alhassan@groupamana.com", "Noor Al Hassan", true, null, "AQIDBAUGBwgJCgsMDQ4PEL47IPnruFDHjvgQn4gt+aHjj9Wvhi+9Lw6p4tvk8d7H" },
                    { new Guid("fddddddd-dddd-dddd-dddd-dddddddddddd"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("d9000000-0000-0000-0000-000000000009"), "zaid.mansour@groupamana.com", "Zaid Mansour", true, null, "AQIDBAUGBwgJCgsMDQ4PEL47IPnruFDHjvgQn4gt+aHjj9Wvhi+9Lw6p4tvk8d7H" },
                    { new Guid("feeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("d9000000-0000-0000-0000-000000000009"), "maryam.Said@groupamana.com", "Maryam Said", true, null, "AQIDBAUGBwgJCgsMDQ4PEL47IPnruFDHjvgQn4gt+aHjj9Wvhi+9Lw6p4tvk8d7H" },
                    { new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("d5000000-0000-0000-0000-000000000005"), "tariq.abdullah@groupamana.com", "Tariq Abdullah", true, null, "AQIDBAUGBwgJCgsMDQ4PEL47IPnruFDHjvgQn4gt+aHjj9Wvhi+9Lw6p4tvk8d7H" }
                });

            migrationBuilder.InsertData(
                table: "GroupRoles",
                columns: new[] { "GroupRoleId", "AssignedDate", "GroupId", "RoleId" },
                values: new object[,]
                {
                    { new Guid("b0000001-0000-0000-0000-000000000001"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("a8888888-8888-8888-8888-888888888888"), new Guid("88888888-8888-8888-8888-888888888888") },
                    { new Guid("b0000002-0000-0000-0000-000000000002"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("a8888888-8888-8888-8888-888888888888"), new Guid("22222222-2222-2222-2222-222222222222") },
                    { new Guid("b1111111-1111-1111-1111-111111111111"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("a1111111-1111-1111-1111-111111111111"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("b2222222-2222-2222-2222-222222222222"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("a1111111-1111-1111-1111-111111111111"), new Guid("22222222-2222-2222-2222-222222222222") },
                    { new Guid("b3333333-3333-3333-3333-333333333333"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("a2222222-2222-2222-2222-222222222222"), new Guid("33333333-3333-3333-3333-333333333333") },
                    { new Guid("b4444444-4444-4444-4444-444444444444"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("a2222222-2222-2222-2222-222222222222"), new Guid("88888888-8888-8888-8888-888888888888") },
                    { new Guid("b5555555-5555-5555-5555-555555555555"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("a2222222-2222-2222-2222-222222222222"), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa") },
                    { new Guid("b6666666-6666-6666-6666-666666666666"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("a3333333-3333-3333-3333-333333333333"), new Guid("44444444-4444-4444-4444-444444444444") },
                    { new Guid("b7777777-7777-7777-7777-777777777777"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("a3333333-3333-3333-3333-333333333333"), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa") },
                    { new Guid("b8888888-8888-8888-8888-888888888888"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("a4444444-4444-4444-4444-444444444444"), new Guid("55555555-5555-5555-5555-555555555555") },
                    { new Guid("b9999999-9999-9999-9999-999999999999"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("a4444444-4444-4444-4444-444444444444"), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa") },
                    { new Guid("baaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("a5555555-5555-5555-5555-555555555555"), new Guid("66666666-6666-6666-6666-666666666666") },
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("a5555555-5555-5555-5555-555555555555"), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa") },
                    { new Guid("bccccccc-cccc-cccc-cccc-cccccccccccc"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("a6666666-6666-6666-6666-666666666666"), new Guid("77777777-7777-7777-7777-777777777777") },
                    { new Guid("bddddddd-dddd-dddd-dddd-dddddddddddd"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("a6666666-6666-6666-6666-666666666666"), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa") },
                    { new Guid("beeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("a7777777-7777-7777-7777-777777777777"), new Guid("88888888-8888-8888-8888-888888888888") },
                    { new Guid("bfffffff-ffff-ffff-ffff-ffffffffffff"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("a7777777-7777-7777-7777-777777777777"), new Guid("22222222-2222-2222-2222-222222222222") }
                });

            migrationBuilder.InsertData(
                table: "UserGroups",
                columns: new[] { "UserGroupId", "GroupId", "JoinedDate", "UserId" },
                values: new object[,]
                {
                    { new Guid("c0000001-0000-0000-0000-000000000001"), new Guid("a7777777-7777-7777-7777-777777777777"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("f0000001-0000-0000-0000-000000000001") },
                    { new Guid("c0000002-0000-0000-0000-000000000002"), new Guid("a7777777-7777-7777-7777-777777777777"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("f0000002-0000-0000-0000-000000000002") },
                    { new Guid("c0000003-0000-0000-0000-000000000003"), new Guid("a8888888-8888-8888-8888-888888888888"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("f0000003-0000-0000-0000-000000000003") },
                    { new Guid("c0000004-0000-0000-0000-000000000004"), new Guid("a8888888-8888-8888-8888-888888888888"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("f0000004-0000-0000-0000-000000000004") },
                    { new Guid("c1111111-1111-1111-1111-111111111111"), new Guid("a1111111-1111-1111-1111-111111111111"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("f1111111-1111-1111-1111-111111111111") },
                    { new Guid("c2222222-2222-2222-2222-222222222222"), new Guid("a1111111-1111-1111-1111-111111111111"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("f2222222-2222-2222-2222-222222222222") },
                    { new Guid("c3333333-3333-3333-3333-333333333333"), new Guid("a1111111-1111-1111-1111-111111111111"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("f3333333-3333-3333-3333-333333333333") },
                    { new Guid("c4444444-4444-4444-4444-444444444444"), new Guid("a2222222-2222-2222-2222-222222222222"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("f4444444-4444-4444-4444-444444444444") },
                    { new Guid("c5555555-5555-5555-5555-555555555555"), new Guid("a2222222-2222-2222-2222-222222222222"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("f5555555-5555-5555-5555-555555555555") },
                    { new Guid("c6666666-6666-6666-6666-666666666666"), new Guid("a2222222-2222-2222-2222-222222222222"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("f6666666-6666-6666-6666-666666666666") },
                    { new Guid("c7777777-7777-7777-7777-777777777777"), new Guid("a3333333-3333-3333-3333-333333333333"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("f7777777-7777-7777-7777-777777777777") },
                    { new Guid("c8888888-8888-8888-8888-888888888888"), new Guid("a3333333-3333-3333-3333-333333333333"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("f8888888-8888-8888-8888-888888888888") },
                    { new Guid("c9999999-9999-9999-9999-999999999999"), new Guid("a3333333-3333-3333-3333-333333333333"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("f9999999-9999-9999-9999-999999999999") },
                    { new Guid("caaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new Guid("a4444444-4444-4444-4444-444444444444"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("faaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa") },
                    { new Guid("cbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new Guid("a4444444-4444-4444-4444-444444444444"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("fbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb") },
                    { new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), new Guid("a5555555-5555-5555-5555-555555555555"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("fccccccc-cccc-cccc-cccc-cccccccccccc") },
                    { new Guid("cddddddd-dddd-dddd-dddd-dddddddddddd"), new Guid("a5555555-5555-5555-5555-555555555555"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("fddddddd-dddd-dddd-dddd-dddddddddddd") },
                    { new Guid("ceeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), new Guid("a6666666-6666-6666-6666-666666666666"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("feeeeeee-eeee-eeee-eeee-eeeeeeeeeeee") },
                    { new Guid("cfffffff-ffff-ffff-ffff-ffffffffffff"), new Guid("a6666666-6666-6666-6666-666666666666"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff") }
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "UserRoleId", "AssignedDate", "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("d1111111-1111-1111-1111-111111111111"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999999"), new Guid("f2222222-2222-2222-2222-222222222222") },
                    { new Guid("d2222222-2222-2222-2222-222222222222"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("22222222-2222-2222-2222-222222222222"), new Guid("f4444444-4444-4444-4444-444444444444") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "GroupRoles",
                keyColumn: "GroupRoleId",
                keyValue: new Guid("b0000001-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "GroupRoles",
                keyColumn: "GroupRoleId",
                keyValue: new Guid("b0000002-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "GroupRoles",
                keyColumn: "GroupRoleId",
                keyValue: new Guid("b1111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "GroupRoles",
                keyColumn: "GroupRoleId",
                keyValue: new Guid("b2222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "GroupRoles",
                keyColumn: "GroupRoleId",
                keyValue: new Guid("b3333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "GroupRoles",
                keyColumn: "GroupRoleId",
                keyValue: new Guid("b4444444-4444-4444-4444-444444444444"));

            migrationBuilder.DeleteData(
                table: "GroupRoles",
                keyColumn: "GroupRoleId",
                keyValue: new Guid("b5555555-5555-5555-5555-555555555555"));

            migrationBuilder.DeleteData(
                table: "GroupRoles",
                keyColumn: "GroupRoleId",
                keyValue: new Guid("b6666666-6666-6666-6666-666666666666"));

            migrationBuilder.DeleteData(
                table: "GroupRoles",
                keyColumn: "GroupRoleId",
                keyValue: new Guid("b7777777-7777-7777-7777-777777777777"));

            migrationBuilder.DeleteData(
                table: "GroupRoles",
                keyColumn: "GroupRoleId",
                keyValue: new Guid("b8888888-8888-8888-8888-888888888888"));

            migrationBuilder.DeleteData(
                table: "GroupRoles",
                keyColumn: "GroupRoleId",
                keyValue: new Guid("b9999999-9999-9999-9999-999999999999"));

            migrationBuilder.DeleteData(
                table: "GroupRoles",
                keyColumn: "GroupRoleId",
                keyValue: new Guid("baaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));

            migrationBuilder.DeleteData(
                table: "GroupRoles",
                keyColumn: "GroupRoleId",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"));

            migrationBuilder.DeleteData(
                table: "GroupRoles",
                keyColumn: "GroupRoleId",
                keyValue: new Guid("bccccccc-cccc-cccc-cccc-cccccccccccc"));

            migrationBuilder.DeleteData(
                table: "GroupRoles",
                keyColumn: "GroupRoleId",
                keyValue: new Guid("bddddddd-dddd-dddd-dddd-dddddddddddd"));

            migrationBuilder.DeleteData(
                table: "GroupRoles",
                keyColumn: "GroupRoleId",
                keyValue: new Guid("beeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"));

            migrationBuilder.DeleteData(
                table: "GroupRoles",
                keyColumn: "GroupRoleId",
                keyValue: new Guid("bfffffff-ffff-ffff-ffff-ffffffffffff"));

            migrationBuilder.DeleteData(
                table: "UserGroups",
                keyColumn: "UserGroupId",
                keyValue: new Guid("c0000001-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "UserGroups",
                keyColumn: "UserGroupId",
                keyValue: new Guid("c0000002-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "UserGroups",
                keyColumn: "UserGroupId",
                keyValue: new Guid("c0000003-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "UserGroups",
                keyColumn: "UserGroupId",
                keyValue: new Guid("c0000004-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "UserGroups",
                keyColumn: "UserGroupId",
                keyValue: new Guid("c1111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "UserGroups",
                keyColumn: "UserGroupId",
                keyValue: new Guid("c2222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "UserGroups",
                keyColumn: "UserGroupId",
                keyValue: new Guid("c3333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "UserGroups",
                keyColumn: "UserGroupId",
                keyValue: new Guid("c4444444-4444-4444-4444-444444444444"));

            migrationBuilder.DeleteData(
                table: "UserGroups",
                keyColumn: "UserGroupId",
                keyValue: new Guid("c5555555-5555-5555-5555-555555555555"));

            migrationBuilder.DeleteData(
                table: "UserGroups",
                keyColumn: "UserGroupId",
                keyValue: new Guid("c6666666-6666-6666-6666-666666666666"));

            migrationBuilder.DeleteData(
                table: "UserGroups",
                keyColumn: "UserGroupId",
                keyValue: new Guid("c7777777-7777-7777-7777-777777777777"));

            migrationBuilder.DeleteData(
                table: "UserGroups",
                keyColumn: "UserGroupId",
                keyValue: new Guid("c8888888-8888-8888-8888-888888888888"));

            migrationBuilder.DeleteData(
                table: "UserGroups",
                keyColumn: "UserGroupId",
                keyValue: new Guid("c9999999-9999-9999-9999-999999999999"));

            migrationBuilder.DeleteData(
                table: "UserGroups",
                keyColumn: "UserGroupId",
                keyValue: new Guid("caaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));

            migrationBuilder.DeleteData(
                table: "UserGroups",
                keyColumn: "UserGroupId",
                keyValue: new Guid("cbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"));

            migrationBuilder.DeleteData(
                table: "UserGroups",
                keyColumn: "UserGroupId",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"));

            migrationBuilder.DeleteData(
                table: "UserGroups",
                keyColumn: "UserGroupId",
                keyValue: new Guid("cddddddd-dddd-dddd-dddd-dddddddddddd"));

            migrationBuilder.DeleteData(
                table: "UserGroups",
                keyColumn: "UserGroupId",
                keyValue: new Guid("ceeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"));

            migrationBuilder.DeleteData(
                table: "UserGroups",
                keyColumn: "UserGroupId",
                keyValue: new Guid("cfffffff-ffff-ffff-ffff-ffffffffffff"));

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumn: "UserRoleId",
                keyValue: new Guid("d1111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumn: "UserRoleId",
                keyValue: new Guid("d2222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "Groups",
                keyColumn: "GroupId",
                keyValue: new Guid("a1111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Groups",
                keyColumn: "GroupId",
                keyValue: new Guid("a2222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "Groups",
                keyColumn: "GroupId",
                keyValue: new Guid("a3333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "Groups",
                keyColumn: "GroupId",
                keyValue: new Guid("a4444444-4444-4444-4444-444444444444"));

            migrationBuilder.DeleteData(
                table: "Groups",
                keyColumn: "GroupId",
                keyValue: new Guid("a5555555-5555-5555-5555-555555555555"));

            migrationBuilder.DeleteData(
                table: "Groups",
                keyColumn: "GroupId",
                keyValue: new Guid("a6666666-6666-6666-6666-666666666666"));

            migrationBuilder.DeleteData(
                table: "Groups",
                keyColumn: "GroupId",
                keyValue: new Guid("a7777777-7777-7777-7777-777777777777"));

            migrationBuilder.DeleteData(
                table: "Groups",
                keyColumn: "GroupId",
                keyValue: new Guid("a8888888-8888-8888-8888-888888888888"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("f0000001-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("f0000002-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("f0000003-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("f0000004-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("f1111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("f2222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("f3333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("f4444444-4444-4444-4444-444444444444"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("f5555555-5555-5555-5555-555555555555"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("f6666666-6666-6666-6666-666666666666"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("f7777777-7777-7777-7777-777777777777"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("f8888888-8888-8888-8888-888888888888"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("f9999999-9999-9999-9999-999999999999"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("faaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("fbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("fccccccc-cccc-cccc-cccc-cccccccccccc"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("fddddddd-dddd-dddd-dddd-dddddddddddd"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("feeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"));
        }
    }
}
