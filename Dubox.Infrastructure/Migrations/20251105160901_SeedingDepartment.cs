using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedingDepartment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedDate",
                table: "Departments",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Departments",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "DepartmentId", "Code", "CreatedDate", "DepartmentName", "Description", "IsActive", "Location", "ManagerId" },
                values: new object[,]
                {
                    { new Guid("d1000000-0000-0000-0000-000000000001"), "IT", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "IT", null, true, null, null },
                    { new Guid("d2000000-0000-0000-0000-000000000002"), "MGMT", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Management", null, true, null, null },
                    { new Guid("d3000000-0000-0000-0000-000000000003"), "ENG", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Engineering", null, true, null, null },
                    { new Guid("d4000000-0000-0000-0000-000000000004"), "CONST", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Construction", null, true, null, null },
                    { new Guid("d5000000-0000-0000-0000-000000000005"), "QLTY", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Quality", null, true, null, null },
                    { new Guid("d6000000-0000-0000-0000-000000000006"), "PROC", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Procurement", null, true, null, null },
                    { new Guid("d7000000-0000-0000-0000-000000000007"), "HSE", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "HSE", null, true, null, null },
                    { new Guid("d8000000-0000-0000-0000-000000000008"), "DBX", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "DuBox", null, true, null, null },
                    { new Guid("d9000000-0000-0000-0000-000000000009"), "DPD", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "DuPod", null, true, null, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: new Guid("d1000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: new Guid("d2000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: new Guid("d3000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: new Guid("d4000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: new Guid("d5000000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: new Guid("d6000000-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: new Guid("d7000000-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: new Guid("d8000000-0000-0000-0000-000000000008"));

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: new Guid("d9000000-0000-0000-0000-000000000009"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedDate",
                table: "Departments",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Departments",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");
        }
    }
}
