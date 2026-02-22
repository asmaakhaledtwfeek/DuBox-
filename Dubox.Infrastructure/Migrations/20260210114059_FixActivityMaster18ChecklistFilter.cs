using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixActivityMaster18ChecklistFilter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ActivityCheckListItems",
                columns: new[] { "ActivityCheckListItemId", "ActivityMasterId", "ActivityTemplateActivityId", "CreatedBy", "CreatedDate", "IsActive", "IsMandatory", "ModifiedBy", "ModifiedDate", "PredefinedChecklistItemId", "Sequence" },
                values: new object[,]
                {
                    { new Guid("f5b00000-0000-0000-0000-000000000001"), new Guid("10000005-0000-0000-0000-000000000001"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("15c7f5fd-b05c-49a0-a35f-a548d9f86126"), 1 },
                    { new Guid("f5b00000-0000-0000-0000-000000000002"), new Guid("10000005-0000-0000-0000-000000000001"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("a9b4eb9a-5062-4a6b-a3ea-98bb4db4dfdb"), 2 },
                    { new Guid("f5b00000-0000-0000-0000-000000000003"), new Guid("10000005-0000-0000-0000-000000000001"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("dc1347fa-997e-4a1f-ab5e-ba8c1c286b3f"), 3 },
                    { new Guid("f5b00000-0000-0000-0000-000000000004"), new Guid("10000005-0000-0000-0000-000000000001"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("ddf6192a-38b4-4eab-b81b-ac7178214cc1"), 4 },
                    { new Guid("f5b00000-0000-0000-0000-000000000005"), new Guid("10000005-0000-0000-0000-000000000001"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("6f868e6e-7a11-4c65-9cef-f2aeae768964"), 5 },
                    { new Guid("f5b00000-0000-0000-0000-000000000006"), new Guid("10000005-0000-0000-0000-000000000001"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("09749601-1157-43d5-bd9b-43ae7994d520"), 6 },
                    { new Guid("f5b00000-0000-0000-0000-000000000007"), new Guid("10000005-0000-0000-0000-000000000001"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("e66380f3-38a2-4392-b82b-fced24df44d9"), 7 },
                    { new Guid("f5b00000-0000-0000-0000-000000000008"), new Guid("10000005-0000-0000-0000-000000000001"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("acfc8c2c-7b88-4a40-a78e-4a545e855ab0"), 8 },
                    { new Guid("f5b00000-0000-0000-0000-000000000009"), new Guid("10000005-0000-0000-0000-000000000001"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("de5ae193-0f9a-4a87-800b-186dd1ad03e2"), 9 },
                    { new Guid("f5b00000-0000-0000-0000-000000000010"), new Guid("10000005-0000-0000-0000-000000000001"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("de8bc96d-aaa0-4176-8415-1f5c222116c6"), 10 },
                    { new Guid("f5b00000-0000-0000-0000-000000000011"), new Guid("10000005-0000-0000-0000-000000000001"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("0cd699e5-ca4d-4797-93a1-029308ade190"), 11 },
                    { new Guid("f5b00000-0000-0000-0000-000000000012"), new Guid("10000005-0000-0000-0000-000000000001"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("83770710-147d-46d3-8b71-414f5c09b677"), 12 },
                    { new Guid("f5b00000-0000-0000-0000-000000000013"), new Guid("10000005-0000-0000-0000-000000000001"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("113778e7-dca1-43e7-9539-22c3caf843dc"), 13 },
                    { new Guid("f5b00000-0000-0000-0000-000000000014"), new Guid("10000005-0000-0000-0000-000000000001"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("387e21fd-ad8c-432a-b08e-059a790522a4"), 14 },
                    { new Guid("f5b00000-0000-0000-0000-000000000015"), new Guid("10000005-0000-0000-0000-000000000001"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("ba041522-26d3-48cd-acae-729cb0b667ab"), 15 },
                    { new Guid("f5b00000-0000-0000-0000-000000000016"), new Guid("10000005-0000-0000-0000-000000000001"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("9c4da2e4-6734-4e94-9037-d2e8f88f931f"), 16 },
                    { new Guid("f5b00000-0000-0000-0000-000000000017"), new Guid("10000005-0000-0000-0000-000000000001"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("54c1ece9-f4f7-4b9a-a76f-a6c22b4729ec"), 17 },
                    { new Guid("f5b00000-0000-0000-0000-000000000018"), new Guid("10000005-0000-0000-0000-000000000001"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("6deff94b-68f3-4827-8af9-06b37affbaec"), 18 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("f5b00000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("f5b00000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("f5b00000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("f5b00000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("f5b00000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("f5b00000-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("f5b00000-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("f5b00000-0000-0000-0000-000000000008"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("f5b00000-0000-0000-0000-000000000009"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("f5b00000-0000-0000-0000-000000000010"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("f5b00000-0000-0000-0000-000000000011"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("f5b00000-0000-0000-0000-000000000012"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("f5b00000-0000-0000-0000-000000000013"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("f5b00000-0000-0000-0000-000000000014"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("f5b00000-0000-0000-0000-000000000015"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("f5b00000-0000-0000-0000-000000000016"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("f5b00000-0000-0000-0000-000000000017"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("f5b00000-0000-0000-0000-000000000018"));
        }
    }
}
