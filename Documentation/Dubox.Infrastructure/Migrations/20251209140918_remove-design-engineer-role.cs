using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removedesignengineerrole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "GroupRoles",
                keyColumn: "GroupRoleId",
                keyValue: new Guid("b0000001-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "GroupRoles",
                keyColumn: "GroupRoleId",
                keyValue: new Guid("b4444444-4444-4444-4444-444444444444"));

            migrationBuilder.DeleteData(
                table: "GroupRoles",
                keyColumn: "GroupRoleId",
                keyValue: new Guid("beeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RoleId", "CreatedDate", "Description", "IsActive", "RoleName" },
                values: new object[] { new Guid("88888888-8888-8888-8888-888888888888"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Design and BIM modeling", true, "DesignEngineer" });

            migrationBuilder.InsertData(
                table: "GroupRoles",
                columns: new[] { "GroupRoleId", "AssignedDate", "GroupId", "RoleId" },
                values: new object[,]
                {
                    { new Guid("b0000001-0000-0000-0000-000000000001"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("a8888888-8888-8888-8888-888888888888"), new Guid("88888888-8888-8888-8888-888888888888") },
                    { new Guid("b4444444-4444-4444-4444-444444444444"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("a2222222-2222-2222-2222-222222222222"), new Guid("88888888-8888-8888-8888-888888888888") },
                    { new Guid("beeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("a7777777-7777-7777-7777-777777777777"), new Guid("88888888-8888-8888-8888-888888888888") }
                });
        }
    }
}
