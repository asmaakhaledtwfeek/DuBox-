using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedActivityMaster2Checklist : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ActivityCheckListItems",
                columns: new[] { "ActivityCheckListItemId", "ActivityMasterId", "ActivityTemplateActivityId", "CreatedBy", "CreatedDate", "IsActive", "IsMandatory", "ModifiedBy", "ModifiedDate", "PredefinedChecklistItemId", "Sequence" },
                values: new object[,]
                {
                    { new Guid("f2a00000-0000-0000-0000-000000000001"), new Guid("10000001-0000-0000-0000-000000000002"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("c76908fd-b7f1-4ace-b87c-ea30bafda313"), 1 },
                    { new Guid("f2a00000-0000-0000-0000-000000000002"), new Guid("10000001-0000-0000-0000-000000000002"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("cd97222f-0ae5-4822-84a9-398a52188da1"), 2 },
                    { new Guid("f2a00000-0000-0000-0000-000000000003"), new Guid("10000001-0000-0000-0000-000000000002"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("1416a5de-152c-4c95-bcc9-f983c1e4ad88"), 3 },
                    { new Guid("f2a00000-0000-0000-0000-000000000004"), new Guid("10000001-0000-0000-0000-000000000002"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("0dcd763b-9cbf-43c8-93a3-49b24a0592b5"), 4 },
                    { new Guid("f2a00000-0000-0000-0000-000000000005"), new Guid("10000001-0000-0000-0000-000000000002"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("6d78ce26-de03-4b4e-9198-dedd505a35c4"), 5 },
                    { new Guid("f2a00000-0000-0000-0000-000000000006"), new Guid("10000001-0000-0000-0000-000000000002"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("681d9573-580c-47ad-a080-6b4e544c8ec4"), 6 },
                    { new Guid("f2a00000-0000-0000-0000-000000000007"), new Guid("10000001-0000-0000-0000-000000000002"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("0c083254-bbf4-4496-85c5-173dec86ddef"), 7 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("f2a00000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("f2a00000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("f2a00000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("f2a00000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("f2a00000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("f2a00000-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("f2a00000-0000-0000-0000-000000000007"));
        }
    }
}
