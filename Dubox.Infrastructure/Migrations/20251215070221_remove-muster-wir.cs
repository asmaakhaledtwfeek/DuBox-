using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removemusterwir : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PredefinedChecklistItems_Categories_CategoryId",
                table: "PredefinedChecklistItems");

            migrationBuilder.DropForeignKey(
                name: "FK_PredefinedChecklistItems_References_ReferenceId",
                table: "PredefinedChecklistItems");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "References");

            migrationBuilder.DropTable(
                name: "WIRMasters");

            migrationBuilder.DropIndex(
                name: "IX_PredefinedChecklistItems_CategoryId",
                table: "PredefinedChecklistItems");

            migrationBuilder.DropIndex(
                name: "IX_PredefinedChecklistItems_ReferenceId",
                table: "PredefinedChecklistItems");

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0001-000000000001"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0001-000000000002"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0001-000000000003"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0001-000000000004"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0001-000000000005"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0001-000000000006"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0001-000000000007"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0001-000000000008"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0001-000000000009"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0001-000000000010"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0001-000000000011"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0001-000000000012"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0001-000000000013"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0001-000000000014"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0001-000000000101"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0001-000000000102"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0001-000000000103"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0001-000000000104"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0001-000000000105"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0001-000000000106"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0001-000000000107"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0001-000000000108"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0001-000000000109"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0001-000000000110"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0001-000000000111"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0001-000000000112"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0001-000000000113"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0001-000000000114"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0001-000000000115"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0001-000000000116"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0004-000000000001"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0004-000000000002"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0004-000000000003"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0004-000000000004"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0004-000000000005"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0004-000000000006"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0004-000000000007"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0004-000000000008"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0004-000000000009"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0004-000000000010"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0004-000000000011"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0004-000000000012"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0004-000000000013"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0004-000000000014"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0004-000000000015"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0004-000000000016"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0004-000000000017"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0004-000000000018"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0004-000000000019"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0004-000000000020"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0004-000000000021"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0004-000000000022"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0004-000000000023"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0004-000000000024"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0004-000000000025"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0004-000000000026"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0004-000000000027"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0004-000000000028"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000001"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000002"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000003"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000004"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000005"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000006"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000007"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000008"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000009"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000010"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000011"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000012"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000013"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000014"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000015"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000016"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000017"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000018"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000019"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000020"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000021"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000022"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000023"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000024"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000025"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000026"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000027"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000028"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000029"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000030"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000031"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000032"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000033"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000034"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000035"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000036"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000037"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000038"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000001"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000002"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000003"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000004"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000005"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000006"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000007"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000008"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000009"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000010"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000011"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000012"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000013"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000014"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000015"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000016"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000017"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000018"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000019"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000020"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000021"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000022"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000023"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000024"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000025"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000026"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000027"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000028"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000029"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000030"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000031"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000032"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000033"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000034"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000035"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000036"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000037"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000038"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000039"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000040"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000041"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000042"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000043"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000044"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000045"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000046"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000047"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000048"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000049"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000050"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000051"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000052"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000053"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000054"));

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "PredefinedChecklistItems");

            migrationBuilder.DropColumn(
                name: "ReferenceId",
                table: "PredefinedChecklistItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                table: "PredefinedChecklistItems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ReferenceId",
                table: "PredefinedChecklistItems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "References",
                columns: table => new
                {
                    ReferenceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReferenceName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_References", x => x.ReferenceId);
                });

            migrationBuilder.CreateTable(
                name: "WIRMasters",
                columns: table => new
                {
                    WIRMasterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Discipline = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Phase = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Sequence = table.Column<int>(type: "int", nullable: false),
                    WIRName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    WIRNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WIRMasters", x => x.WIRMasterId);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "CategoryName", "CreatedDate" },
                values: new object[,]
                {
                    { new Guid("40000001-0000-0000-0000-000000000001"), "Installation of HVAC Duct", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000001-0000-0000-0000-000000000002"), "General", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000001-0000-0000-0000-000000000003"), "Installation of Above Ground Drainage Pipes", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000001-0000-0000-0000-000000000004"), "Leak Test of Above Ground Drainage Pipes", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000001-0000-0000-0000-000000000005"), "Installation of Above ground Water Supply pipes and fittings", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000001-0000-0000-0000-000000000006"), "Testing of Above ground Water Supply pipes and fittings", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000001-0000-0000-0000-000000000007"), "Installation of Above Ground Fire Fighting pipes system", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000001-0000-0000-0000-000000000008"), "Testing of Above Ground Fire Fighting pipes and fittings", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000001-0000-0000-0000-000000000009"), "Installation of Refrigerant Pipe", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000001-0000-0000-0000-000000000010"), "Pressure Testing of Refrigerant Pipe", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000001-0000-0000-0000-000000000011"), "Installation of LV Cables & Wires", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000001-0000-0000-0000-000000000012"), "Testing of LV Cables & Wires", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000001-0000-0000-0000-000000000013"), "Installation of LV Panels", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000001-0000-0000-0000-000000000014"), "Installation of Conduits & accessories", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000001-0000-0000-0000-000000000015"), "WIR-1: Material Verification Inspection", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000001-0000-0000-0000-000000000016"), "WIR-1: Material Receiving Inspection - MEP", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000001-0000-0000-0000-000000000017"), "WIR-4: General Requirements", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000001-0000-0000-0000-000000000018"), "WIR-4: Preparation & Setting Out", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000001-0000-0000-0000-000000000019"), "WIR-4: External Walls Erection", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000001-0000-0000-0000-000000000020"), "WIR-4: Floor Slab Installation", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000001-0000-0000-0000-000000000021"), "WIR-4: Internal Partition Installation", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000001-0000-0000-0000-000000000022"), "WIR-4: Top Slab Installation", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000001-0000-0000-0000-000000000023"), "WIR-4: Final Verification", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000001-0000-0000-0000-000000000024"), "WIR-5: Painting - General", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000001-0000-0000-0000-000000000025"), "WIR-5: Painting - Surface Preparation", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000001-0000-0000-0000-000000000026"), "WIR-5: Painting - Internal Application", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000001-0000-0000-0000-000000000027"), "WIR-5: Painting - External Application", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000001-0000-0000-0000-000000000028"), "WIR-5: Ceramic Tiling", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000001-0000-0000-0000-000000000029"), "WIR-5: Gypsum Partition Installation", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000001-0000-0000-0000-000000000030"), "WIR-5: False Ceiling Installation", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000001-0000-0000-0000-000000000031"), "WIR-5: Wet Area Waterproofing", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000001-0000-0000-0000-000000000032"), "WIR-5: Doors & Windows Installation", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000001-0000-0000-0000-000000000033"), "WIR-6: General & Structural Verification", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000001-0000-0000-0000-000000000034"), "WIR-6: Painting Inspection", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000001-0000-0000-0000-000000000035"), "WIR-6: Floor & Wall Tiling", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000001-0000-0000-0000-000000000036"), "WIR-6: Dry Wall Inspection", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000001-0000-0000-0000-000000000037"), "WIR-6: False Ceiling Inspection", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000001-0000-0000-0000-000000000038"), "WIR-6: Aluminium & Glazing Inspection", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000001-0000-0000-0000-000000000039"), "WIR-6: Doors & Windows Inspection", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000001-0000-0000-0000-000000000040"), "WIR-6: Wood Works Inspection", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000001-0000-0000-0000-000000000041"), "WIR-6: Other Finishes Inspection", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000001-0000-0000-0000-000000000042"), "WIR-6: MEP Systems Inspection", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000001"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000002"), new Guid("30000001-0000-0000-0000-000000000003") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000002"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000002"), new Guid("30000001-0000-0000-0000-000000000002") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000003"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000001"), new Guid("30000001-0000-0000-0000-000000000004") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000004"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000001"), new Guid("30000001-0000-0000-0000-000000000004") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000005"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000001"), new Guid("30000001-0000-0000-0000-000000000004") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000006"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000001"), new Guid("30000001-0000-0000-0000-000000000002") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000007"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000001"), new Guid("30000001-0000-0000-0000-000000000002") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000008"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000001"), new Guid("30000001-0000-0000-0000-000000000002") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000009"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000001"), new Guid("30000001-0000-0000-0000-000000000002") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000010"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000001"), new Guid("30000001-0000-0000-0000-000000000002") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000011"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000003"), new Guid("30000001-0000-0000-0000-000000000005") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000012"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000003"), new Guid("30000001-0000-0000-0000-000000000004") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000013"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000003"), new Guid("30000001-0000-0000-0000-000000000002") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000014"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000003"), new Guid("30000001-0000-0000-0000-000000000002") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000015"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000003"), new Guid("30000001-0000-0000-0000-000000000002") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000016"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000003"), new Guid("30000001-0000-0000-0000-000000000007") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000017"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000003"), new Guid("30000001-0000-0000-0000-000000000002") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000018"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000003"), new Guid("30000001-0000-0000-0000-000000000002") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000019"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000003"), new Guid("30000001-0000-0000-0000-000000000002") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000020"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000003"), new Guid("30000001-0000-0000-0000-000000000004") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000021"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000003"), new Guid("30000001-0000-0000-0000-000000000002") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000022"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000003"), new Guid("30000001-0000-0000-0000-000000000004") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000023"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000004"), new Guid("30000001-0000-0000-0000-000000000002") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000024"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000004"), new Guid("30000001-0000-0000-0000-000000000004") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000025"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000004"), new Guid("30000001-0000-0000-0000-000000000008") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000026"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000005"), new Guid("30000001-0000-0000-0000-000000000005") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000027"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000005"), new Guid("30000001-0000-0000-0000-000000000004") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000028"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000005"), new Guid("30000001-0000-0000-0000-000000000002") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000029"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000005"), new Guid("30000001-0000-0000-0000-000000000002") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000030"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000005"), new Guid("30000001-0000-0000-0000-000000000007") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000031"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000005"), new Guid("30000001-0000-0000-0000-000000000004") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000032"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000005"), new Guid("30000001-0000-0000-0000-000000000002") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000033"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000005"), new Guid("30000001-0000-0000-0000-000000000004") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000034"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000005"), new Guid("30000001-0000-0000-0000-000000000004") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000035"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000005"), new Guid("30000001-0000-0000-0000-000000000004") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000036"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000005"), new Guid("30000001-0000-0000-0000-000000000004") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000037"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000006"), new Guid("30000001-0000-0000-0000-000000000002") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000038"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000006"), new Guid("30000001-0000-0000-0000-000000000010") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000039"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000006"), new Guid("30000001-0000-0000-0000-000000000011") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000040"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000007"), new Guid("30000001-0000-0000-0000-000000000005") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000041"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000007"), new Guid("30000001-0000-0000-0000-000000000002") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000042"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000007"), new Guid("30000001-0000-0000-0000-000000000005") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000043"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000007"), new Guid("30000001-0000-0000-0000-000000000014") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000044"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000007"), new Guid("30000001-0000-0000-0000-000000000002") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000045"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000007"), new Guid("30000001-0000-0000-0000-000000000013") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000046"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000007"), new Guid("30000001-0000-0000-0000-000000000002") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000047"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000007"), new Guid("30000001-0000-0000-0000-000000000002") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000048"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000007"), new Guid("30000001-0000-0000-0000-000000000002") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000049"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000007"), new Guid("30000001-0000-0000-0000-000000000002") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000050"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000007"), new Guid("30000001-0000-0000-0000-000000000002") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000051"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000008"), new Guid("30000001-0000-0000-0000-000000000002") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000052"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000008"), new Guid("30000001-0000-0000-0000-000000000010") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000053"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000008"), new Guid("30000001-0000-0000-0000-000000000015") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000054"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000009"), new Guid("30000001-0000-0000-0000-000000000003") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000055"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000009"), new Guid("30000001-0000-0000-0000-000000000004") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000056"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000009"), new Guid("30000001-0000-0000-0000-000000000002") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000057"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000009"), new Guid("30000001-0000-0000-0000-000000000002") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000058"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000009"), new Guid("30000001-0000-0000-0000-000000000007") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000059"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000009"), new Guid("30000001-0000-0000-0000-000000000002") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000060"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000009"), new Guid("30000001-0000-0000-0000-000000000002") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000061"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000009"), new Guid("30000001-0000-0000-0000-000000000002") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000062"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000009"), new Guid("30000001-0000-0000-0000-000000000004") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000063"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000009"), new Guid("30000001-0000-0000-0000-000000000002") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000064"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000009"), new Guid("30000001-0000-0000-0000-000000000002") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000065"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000009"), new Guid("30000001-0000-0000-0000-000000000004") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000066"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000010"), new Guid("30000001-0000-0000-0000-000000000018") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000067"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000010"), new Guid("30000001-0000-0000-0000-000000000017") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000068"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000010"), new Guid("30000001-0000-0000-0000-000000000017") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000069"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000010"), new Guid("30000001-0000-0000-0000-000000000017") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000070"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000011"), new Guid("30000001-0000-0000-0000-000000000019") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000071"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000011"), new Guid("30000001-0000-0000-0000-000000000002") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000072"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000011"), new Guid("30000001-0000-0000-0000-000000000004") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000073"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000011"), new Guid("30000001-0000-0000-0000-000000000004") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000074"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000011"), new Guid("30000001-0000-0000-0000-000000000005") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000075"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000011"), new Guid("30000001-0000-0000-0000-000000000002") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000076"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000011"), new Guid("30000001-0000-0000-0000-000000000002") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000077"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000011"), new Guid("30000001-0000-0000-0000-000000000002") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000078"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000011"), new Guid("30000001-0000-0000-0000-000000000002") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000079"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000011"), new Guid("30000001-0000-0000-0000-000000000002") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000080"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000011"), new Guid("30000001-0000-0000-0000-000000000002") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000081"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000011"), new Guid("30000001-0000-0000-0000-000000000004") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000082"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000011"), new Guid("30000001-0000-0000-0000-000000000004") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000083"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000011"), new Guid("30000001-0000-0000-0000-000000000004") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000084"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000011"), new Guid("30000001-0000-0000-0000-000000000004") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000085"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000012"), new Guid("30000001-0000-0000-0000-000000000020") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000086"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000012"), new Guid("30000001-0000-0000-0000-000000000020") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000087"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000012"), new Guid("30000001-0000-0000-0000-000000000021") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000088"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000012"), new Guid("30000001-0000-0000-0000-000000000021") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000089"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000013"), new Guid("30000001-0000-0000-0000-000000000019") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000090"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000013"), new Guid("30000001-0000-0000-0000-000000000023") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000091"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000013"), new Guid("30000001-0000-0000-0000-000000000004") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000092"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000013"), new Guid("30000001-0000-0000-0000-000000000023") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000093"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000013"), new Guid("30000001-0000-0000-0000-000000000005") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000094"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000013"), new Guid("30000001-0000-0000-0000-000000000004") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000095"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000013"), new Guid("30000001-0000-0000-0000-000000000004") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000096"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000013"), new Guid("30000001-0000-0000-0000-000000000004") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000097"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000013"), new Guid("30000001-0000-0000-0000-000000000004") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000098"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000013"), new Guid("30000001-0000-0000-0000-000000000023") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000099"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000013"), new Guid("30000001-0000-0000-0000-000000000004") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000100"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000013"), new Guid("30000001-0000-0000-0000-000000000023") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000101"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000013"), new Guid("30000001-0000-0000-0000-000000000004") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000102"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000013"), new Guid("30000001-0000-0000-0000-000000000004") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000103"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000013"), new Guid("30000001-0000-0000-0000-000000000023") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000104"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000014"), new Guid("30000001-0000-0000-0000-000000000003") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000105"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000014"), new Guid("30000001-0000-0000-0000-000000000023") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000106"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000014"), new Guid("30000001-0000-0000-0000-000000000005") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000107"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000014"), new Guid("30000001-0000-0000-0000-000000000004") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000108"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000014"), new Guid("30000001-0000-0000-0000-000000000004") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000109"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000014"), new Guid("30000001-0000-0000-0000-000000000023") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000110"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000014"), new Guid("30000001-0000-0000-0000-000000000023") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000111"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000014"), new Guid("30000001-0000-0000-0000-000000000023") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000112"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000014"), new Guid("30000001-0000-0000-0000-000000000004") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000113"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000014"), new Guid("30000001-0000-0000-0000-000000000023") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000114"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000011"), new Guid("30000001-0000-0000-0000-000000000019") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000115"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000011"), new Guid("30000001-0000-0000-0000-000000000002") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000116"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000011"), new Guid("30000001-0000-0000-0000-000000000004") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000117"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000011"), new Guid("30000001-0000-0000-0000-000000000004") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000118"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000011"), new Guid("30000001-0000-0000-0000-000000000005") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000119"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000011"), new Guid("30000001-0000-0000-0000-000000000002") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000120"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000011"), new Guid("30000001-0000-0000-0000-000000000002") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000121"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000011"), new Guid("30000001-0000-0000-0000-000000000002") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000122"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000011"), new Guid("30000001-0000-0000-0000-000000000002") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000123"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000011"), new Guid("30000001-0000-0000-0000-000000000002") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000124"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000011"), new Guid("30000001-0000-0000-0000-000000000004") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000125"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000011"), new Guid("30000001-0000-0000-0000-000000000002") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000126"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000011"), new Guid("30000001-0000-0000-0000-000000000004") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000127"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000011"), new Guid("30000001-0000-0000-0000-000000000004") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000128"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000011"), new Guid("30000001-0000-0000-0000-000000000004") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000129"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000012"), new Guid("30000001-0000-0000-0000-000000000020") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000130"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000012"), new Guid("30000001-0000-0000-0000-000000000020") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000131"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000012"), new Guid("30000001-0000-0000-0000-000000000021") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000132"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000012"), new Guid("30000001-0000-0000-0000-000000000021") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000133"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000013"), new Guid("30000001-0000-0000-0000-000000000019") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000134"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000013"), new Guid("30000001-0000-0000-0000-000000000023") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000135"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000013"), new Guid("30000001-0000-0000-0000-000000000004") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000136"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000013"), new Guid("30000001-0000-0000-0000-000000000023") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000137"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000013"), new Guid("30000001-0000-0000-0000-000000000005") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000138"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000013"), new Guid("30000001-0000-0000-0000-000000000004") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000139"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000013"), new Guid("30000001-0000-0000-0000-000000000004") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000140"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000013"), new Guid("30000001-0000-0000-0000-000000000004") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000141"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000013"), new Guid("30000001-0000-0000-0000-000000000004") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000142"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000013"), new Guid("30000001-0000-0000-0000-000000000023") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000143"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000013"), new Guid("30000001-0000-0000-0000-000000000004") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000144"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000013"), new Guid("30000001-0000-0000-0000-000000000023") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000145"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000013"), new Guid("30000001-0000-0000-0000-000000000004") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000146"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000013"), new Guid("30000001-0000-0000-0000-000000000004") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000147"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000013"), new Guid("30000001-0000-0000-0000-000000000023") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000148"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000014"), new Guid("30000001-0000-0000-0000-000000000003") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000149"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000014"), new Guid("30000001-0000-0000-0000-000000000023") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000150"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000014"), new Guid("30000001-0000-0000-0000-000000000005") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000151"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000014"), new Guid("30000001-0000-0000-0000-000000000004") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000152"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000014"), new Guid("30000001-0000-0000-0000-000000000004") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000153"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000014"), new Guid("30000001-0000-0000-0000-000000000023") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000154"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000014"), new Guid("30000001-0000-0000-0000-000000000023") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000155"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000014"), new Guid("30000001-0000-0000-0000-000000000004") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000156"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000014"), new Guid("30000001-0000-0000-0000-000000000023") });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000157"),
                columns: new[] { "CategoryId", "ReferenceId" },
                values: new object[] { new Guid("40000001-0000-0000-0000-000000000014"), new Guid("30000001-0000-0000-0000-000000000023") });

            migrationBuilder.InsertData(
                table: "References",
                columns: new[] { "ReferenceId", "CreatedDate", "ReferenceName" },
                values: new object[,]
                {
                    { new Guid("30000001-0000-0000-0000-000000000001"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Project Specifications Section- 233113 & 230713" },
                    { new Guid("30000001-0000-0000-0000-000000000002"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Approved Drawing" },
                    { new Guid("30000001-0000-0000-0000-000000000003"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "MA" },
                    { new Guid("30000001-0000-0000-0000-000000000004"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "General" },
                    { new Guid("30000001-0000-0000-0000-000000000005"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "MIR" },
                    { new Guid("30000001-0000-0000-0000-000000000006"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Project Specification: Section- 221300" },
                    { new Guid("30000001-0000-0000-0000-000000000007"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Approved MTS" },
                    { new Guid("30000001-0000-0000-0000-000000000008"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Project specification" },
                    { new Guid("30000001-0000-0000-0000-000000000009"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Project Specification: Section- 221116" },
                    { new Guid("30000001-0000-0000-0000-000000000010"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Calibration Certificate" },
                    { new Guid("30000001-0000-0000-0000-000000000011"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Project Spec.-221116" },
                    { new Guid("30000001-0000-0000-0000-000000000012"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Project Specification: Section- 211100" },
                    { new Guid("30000001-0000-0000-0000-000000000013"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Project Specification & Approved MTS" },
                    { new Guid("30000001-0000-0000-0000-000000000014"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Approved Drawing & MTS" },
                    { new Guid("30000001-0000-0000-0000-000000000015"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Project Spec" },
                    { new Guid("30000001-0000-0000-0000-000000000016"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Project Specifications Section- 232300" },
                    { new Guid("30000001-0000-0000-0000-000000000017"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Project Spec. - 232300" },
                    { new Guid("30000001-0000-0000-0000-000000000018"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Project Spec. - 237400 & 238129" },
                    { new Guid("30000001-0000-0000-0000-000000000019"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "MAR" },
                    { new Guid("30000001-0000-0000-0000-000000000020"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Project Spec.-260513" },
                    { new Guid("30000001-0000-0000-0000-000000000021"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Project Spec.-260513-1.7 C" },
                    { new Guid("30000001-0000-0000-0000-000000000022"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Project Specification: Section- 262416" },
                    { new Guid("30000001-0000-0000-0000-000000000023"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Approved Shop Drawing" },
                    { new Guid("30000001-0000-0000-0000-000000000024"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Project Specification: Section- 260533" },
                    { new Guid("30000001-0000-0000-0000-000000000025"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "MSDS (Material Safety Data Sheet)" },
                    { new Guid("30000001-0000-0000-0000-000000000026"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Manufacturer Recommendations" },
                    { new Guid("30000001-0000-0000-0000-000000000027"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Mill Test Certificate" },
                    { new Guid("30000001-0000-0000-0000-000000000028"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "NFPA 13 (Fire Protection Standard)" },
                    { new Guid("30000001-0000-0000-0000-000000000029"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Finishing Schedule" },
                    { new Guid("30000001-0000-0000-0000-000000000030"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Color Schedule" },
                    { new Guid("30000001-0000-0000-0000-000000000031"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Door Schedule" },
                    { new Guid("30000001-0000-0000-0000-000000000032"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Window Schedule" },
                    { new Guid("30000001-0000-0000-0000-000000000033"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "DCL Product Conformity Certificate" },
                    { new Guid("30000001-0000-0000-0000-000000000034"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Fire Rating Certificate" },
                    { new Guid("30000001-0000-0000-0000-000000000035"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Mockup Approval" },
                    { new Guid("30000001-0000-0000-0000-000000000036"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Sample Approval" }
                });

            migrationBuilder.InsertData(
                table: "WIRMasters",
                columns: new[] { "WIRMasterId", "CreatedDate", "Description", "Discipline", "IsActive", "Phase", "Sequence", "WIRName", "WIRNumber" },
                values: new object[,]
                {
                    { new Guid("10000001-0000-0000-0000-000000000001"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Verify all materials meet specifications and are properly documented before use in production", "Both", true, "Material", 1, "Material Receiving & Verification", "WIR-1" },
                    { new Guid("10000001-0000-0000-0000-000000000002"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Install and test all MEP systems including HVAC, plumbing, fire fighting, and refrigerant piping", "MEP", true, "Installation", 2, "MEP Installation & Testing", "WIR-2" },
                    { new Guid("10000001-0000-0000-0000-000000000003"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Install and test all electrical systems including cables, wires, panels, and conduits", "Electrical", true, "Installation", 3, "Electrical Installation & Testing", "WIR-3" },
                    { new Guid("10000001-0000-0000-0000-000000000004"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Assemble and erect precast concrete modular elements including walls, slabs, and partitions", "Civil", true, "Assembly", 4, "Structural Assembly & Erection", "WIR-4" },
                    { new Guid("10000001-0000-0000-0000-000000000005"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Apply all interior and exterior finishes including painting, tiling, ceilings, doors, windows, and woodwork", "Civil", true, "Finishing", 5, "Finishing Works", "WIR-5" },
                    { new Guid("10000001-0000-0000-0000-000000000006"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Comprehensive final inspection of completed modular before loading and transportation to site", "Both", true, "Final", 6, "Final Pre-Loading Inspection", "WIR-6" }
                });

            migrationBuilder.InsertData(
                table: "PredefinedChecklistItems",
                columns: new[] { "PredefinedItemId", "CategoryId", "CheckpointDescription", "CreatedDate", "IsActive", "ItemNumber", "ReferenceId", "Sequence", "WIRNumber" },
                values: new object[,]
                {
                    { new Guid("20000001-0000-0000-0001-000000000001"), new Guid("40000001-0000-0000-0000-000000000015"), "Is there material approval for received item?", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "1", new Guid("30000001-0000-0000-0000-000000000005"), 1, "WIR-1" },
                    { new Guid("20000001-0000-0000-0001-000000000002"), new Guid("40000001-0000-0000-0000-000000000015"), "Is Manufacturer Name as per material approval?", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "2", new Guid("30000001-0000-0000-0000-000000000003"), 2, "WIR-1" },
                    { new Guid("20000001-0000-0000-0001-000000000003"), new Guid("40000001-0000-0000-0000-000000000015"), "Is Supplier Name as per material approval?", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "3", new Guid("30000001-0000-0000-0000-000000000003"), 3, "WIR-1" },
                    { new Guid("20000001-0000-0000-0001-000000000004"), new Guid("40000001-0000-0000-0000-000000000015"), "Is received material matching with approved sample?", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "4", new Guid("30000001-0000-0000-0000-000000000036"), 4, "WIR-1" },
                    { new Guid("20000001-0000-0000-0001-000000000005"), new Guid("40000001-0000-0000-0000-000000000015"), "Related mill test certificate (or) test reports?", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "5", new Guid("30000001-0000-0000-0000-000000000027"), 5, "WIR-1" },
                    { new Guid("20000001-0000-0000-0001-000000000006"), new Guid("40000001-0000-0000-0000-000000000015"), "Check for any defects", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "6", new Guid("30000001-0000-0000-0000-000000000004"), 6, "WIR-1" },
                    { new Guid("20000001-0000-0000-0001-000000000007"), new Guid("40000001-0000-0000-0000-000000000015"), "Check Expiry date", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "7", new Guid("30000001-0000-0000-0000-000000000004"), 7, "WIR-1" },
                    { new Guid("20000001-0000-0000-0001-000000000008"), new Guid("40000001-0000-0000-0000-000000000015"), "Check Item / product description", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "8", new Guid("30000001-0000-0000-0000-000000000004"), 8, "WIR-1" },
                    { new Guid("20000001-0000-0000-0001-000000000009"), new Guid("40000001-0000-0000-0000-000000000015"), "Check Item / product code", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "9", new Guid("30000001-0000-0000-0000-000000000004"), 9, "WIR-1" },
                    { new Guid("20000001-0000-0000-0001-000000000010"), new Guid("40000001-0000-0000-0000-000000000015"), "Check Dimensions (length, width, thickness etc.)", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "10", new Guid("30000001-0000-0000-0000-000000000002"), 10, "WIR-1" },
                    { new Guid("20000001-0000-0000-0001-000000000011"), new Guid("40000001-0000-0000-0000-000000000015"), "Check Colour", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "11", new Guid("30000001-0000-0000-0000-000000000030"), 11, "WIR-1" },
                    { new Guid("20000001-0000-0000-0001-000000000012"), new Guid("40000001-0000-0000-0000-000000000015"), "Check for Packaging Conditions", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "12", new Guid("30000001-0000-0000-0000-000000000004"), 12, "WIR-1" },
                    { new Guid("20000001-0000-0000-0001-000000000013"), new Guid("40000001-0000-0000-0000-000000000015"), "Check for received Quantity (approx) as per DO", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "13", new Guid("30000001-0000-0000-0000-000000000004"), 13, "WIR-1" },
                    { new Guid("20000001-0000-0000-0001-000000000014"), new Guid("40000001-0000-0000-0000-000000000015"), "Check the area of storage as per MSDS", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "14", new Guid("30000001-0000-0000-0000-000000000025"), 14, "WIR-1" },
                    { new Guid("20000001-0000-0000-0001-000000000101"), new Guid("40000001-0000-0000-0000-000000000016"), "Review documents for received materials", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "1", new Guid("30000001-0000-0000-0000-000000000004"), 101, "WIR-1" },
                    { new Guid("20000001-0000-0000-0001-000000000102"), new Guid("40000001-0000-0000-0000-000000000016"), "Materials outside visual checking", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "2", new Guid("30000001-0000-0000-0000-000000000004"), 102, "WIR-1" },
                    { new Guid("20000001-0000-0000-0001-000000000103"), new Guid("40000001-0000-0000-0000-000000000016"), "Check for any damages (General & Visual)", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "3", new Guid("30000001-0000-0000-0000-000000000004"), 103, "WIR-1" },
                    { new Guid("20000001-0000-0000-0001-000000000104"), new Guid("40000001-0000-0000-0000-000000000016"), "Verify original bill of landing / Delivery Note", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "4", new Guid("30000001-0000-0000-0000-000000000004"), 104, "WIR-1" },
                    { new Guid("20000001-0000-0000-0001-000000000105"), new Guid("40000001-0000-0000-0000-000000000016"), "Supplier Certificate / Warranty letter", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "5", new Guid("30000001-0000-0000-0000-000000000026"), 105, "WIR-1" },
                    { new Guid("20000001-0000-0000-0001-000000000106"), new Guid("40000001-0000-0000-0000-000000000016"), "Check and Verify the material as per delivery list / details", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "6", new Guid("30000001-0000-0000-0000-000000000004"), 106, "WIR-1" },
                    { new Guid("20000001-0000-0000-0001-000000000107"), new Guid("40000001-0000-0000-0000-000000000016"), "Check the accessories", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "7", new Guid("30000001-0000-0000-0000-000000000004"), 107, "WIR-1" },
                    { new Guid("20000001-0000-0000-0001-000000000108"), new Guid("40000001-0000-0000-0000-000000000016"), "Check the Name Plate", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "8", new Guid("30000001-0000-0000-0000-000000000002"), 108, "WIR-1" },
                    { new Guid("20000001-0000-0000-0001-000000000109"), new Guid("40000001-0000-0000-0000-000000000016"), "Materials Storage and preservation as per manufacturer", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "9", new Guid("30000001-0000-0000-0000-000000000026"), 109, "WIR-1" },
                    { new Guid("20000001-0000-0000-0001-000000000110"), new Guid("40000001-0000-0000-0000-000000000016"), "Check the identification of components", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "10", new Guid("30000001-0000-0000-0000-000000000004"), 110, "WIR-1" },
                    { new Guid("20000001-0000-0000-0001-000000000111"), new Guid("40000001-0000-0000-0000-000000000016"), "Check the rating as per approved drawing", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "11", new Guid("30000001-0000-0000-0000-000000000002"), 111, "WIR-1" },
                    { new Guid("20000001-0000-0000-0001-000000000112"), new Guid("40000001-0000-0000-0000-000000000016"), "Check the loose part", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "12", new Guid("30000001-0000-0000-0000-000000000004"), 112, "WIR-1" },
                    { new Guid("20000001-0000-0000-0001-000000000113"), new Guid("40000001-0000-0000-0000-000000000016"), "Check the dimension of delivered equipment as per approved drawing", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "13", new Guid("30000001-0000-0000-0000-000000000002"), 113, "WIR-1" },
                    { new Guid("20000001-0000-0000-0001-000000000114"), new Guid("40000001-0000-0000-0000-000000000016"), "Check the availability of spare breakers / relays/ terminals", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "14", new Guid("30000001-0000-0000-0000-000000000002"), 114, "WIR-1" },
                    { new Guid("20000001-0000-0000-0001-000000000115"), new Guid("40000001-0000-0000-0000-000000000016"), "Delivered material photos", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "15", new Guid("30000001-0000-0000-0000-000000000004"), 115, "WIR-1" },
                    { new Guid("20000001-0000-0000-0001-000000000116"), new Guid("40000001-0000-0000-0000-000000000016"), "Material Site test", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "16", new Guid("30000001-0000-0000-0000-000000000008"), 116, "WIR-1" },
                    { new Guid("20000001-0000-0000-0004-000000000001"), new Guid("40000001-0000-0000-0000-000000000017"), "Ensure method statement, materials and shop drawings are approved", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A1", new Guid("30000001-0000-0000-0000-000000000007"), 1, "WIR-4" },
                    { new Guid("20000001-0000-0000-0004-000000000002"), new Guid("40000001-0000-0000-0000-000000000017"), "Ensure materials are stored under dry, clean, shaded area, away from sunlight and other sources of heat", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A2", new Guid("30000001-0000-0000-0000-000000000004"), 2, "WIR-4" },
                    { new Guid("20000001-0000-0000-0004-000000000003"), new Guid("40000001-0000-0000-0000-000000000017"), "Check the expiry date of the material prior to applications", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A3", new Guid("30000001-0000-0000-0000-000000000004"), 3, "WIR-4" },
                    { new Guid("20000001-0000-0000-0004-000000000004"), new Guid("40000001-0000-0000-0000-000000000018"), "Drawing Stamp & Signature", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "B1", new Guid("30000001-0000-0000-0000-000000000002"), 4, "WIR-4" },
                    { new Guid("20000001-0000-0000-0004-000000000005"), new Guid("40000001-0000-0000-0000-000000000018"), "Element Tag, QC Approval for Element", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "B2", new Guid("30000001-0000-0000-0000-000000000004"), 5, "WIR-4" },
                    { new Guid("20000001-0000-0000-0004-000000000006"), new Guid("40000001-0000-0000-0000-000000000018"), "Floor Setting Out", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "B3", new Guid("30000001-0000-0000-0000-000000000002"), 6, "WIR-4" },
                    { new Guid("20000001-0000-0000-0004-000000000007"), new Guid("40000001-0000-0000-0000-000000000019"), "Erection with temporary support", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "C1", new Guid("30000001-0000-0000-0000-000000000002"), 7, "WIR-4" },
                    { new Guid("20000001-0000-0000-0004-000000000008"), new Guid("40000001-0000-0000-0000-000000000019"), "Panel to panel connections", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "C2", new Guid("30000001-0000-0000-0000-000000000002"), 8, "WIR-4" },
                    { new Guid("20000001-0000-0000-0004-000000000009"), new Guid("40000001-0000-0000-0000-000000000019"), "Dimensions (outer, inner and diagonal), line and level", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "C3", new Guid("30000001-0000-0000-0000-000000000002"), 9, "WIR-4" },
                    { new Guid("20000001-0000-0000-0004-000000000010"), new Guid("40000001-0000-0000-0000-000000000020"), "Backer Rod / Shim Pad", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "D1", new Guid("30000001-0000-0000-0000-000000000002"), 10, "WIR-4" },
                    { new Guid("20000001-0000-0000-0004-000000000011"), new Guid("40000001-0000-0000-0000-000000000020"), "Slab Position / Level", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "D2", new Guid("30000001-0000-0000-0000-000000000002"), 11, "WIR-4" },
                    { new Guid("20000001-0000-0000-0004-000000000012"), new Guid("40000001-0000-0000-0000-000000000020"), "Panel to Slab Connections", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "D3", new Guid("30000001-0000-0000-0000-000000000002"), 12, "WIR-4" },
                    { new Guid("20000001-0000-0000-0004-000000000013"), new Guid("40000001-0000-0000-0000-000000000020"), "1000mm FFL to be marked clearly", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "D4", new Guid("30000001-0000-0000-0000-000000000002"), 13, "WIR-4" },
                    { new Guid("20000001-0000-0000-0004-000000000014"), new Guid("40000001-0000-0000-0000-000000000020"), "MEP Clearance", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "D5", new Guid("30000001-0000-0000-0000-000000000004"), 14, "WIR-4" },
                    { new Guid("20000001-0000-0000-0004-000000000015"), new Guid("40000001-0000-0000-0000-000000000020"), "Check the Slab level (shall read 1016mm at 1000m FFL line)", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "D6", new Guid("30000001-0000-0000-0000-000000000002"), 15, "WIR-4" },
                    { new Guid("20000001-0000-0000-0004-000000000016"), new Guid("40000001-0000-0000-0000-000000000020"), "Grouting", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "D7", new Guid("30000001-0000-0000-0000-000000000008"), 16, "WIR-4" },
                    { new Guid("20000001-0000-0000-0004-000000000017"), new Guid("40000001-0000-0000-0000-000000000021"), "Erection with Temporary Support", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "E1", new Guid("30000001-0000-0000-0000-000000000002"), 17, "WIR-4" },
                    { new Guid("20000001-0000-0000-0004-000000000018"), new Guid("40000001-0000-0000-0000-000000000021"), "Panel to Panel Connections", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "E2", new Guid("30000001-0000-0000-0000-000000000002"), 18, "WIR-4" },
                    { new Guid("20000001-0000-0000-0004-000000000019"), new Guid("40000001-0000-0000-0000-000000000021"), "Dimensions (outer, inner and diagonal), Line and Level", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "E3", new Guid("30000001-0000-0000-0000-000000000002"), 19, "WIR-4" },
                    { new Guid("20000001-0000-0000-0004-000000000020"), new Guid("40000001-0000-0000-0000-000000000021"), "Grouting", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "E4", new Guid("30000001-0000-0000-0000-000000000008"), 20, "WIR-4" },
                    { new Guid("20000001-0000-0000-0004-000000000021"), new Guid("40000001-0000-0000-0000-000000000022"), "Backer Rod / Shim Pad", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "F1", new Guid("30000001-0000-0000-0000-000000000002"), 21, "WIR-4" },
                    { new Guid("20000001-0000-0000-0004-000000000022"), new Guid("40000001-0000-0000-0000-000000000022"), "Slab Position / Top Height", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "F2", new Guid("30000001-0000-0000-0000-000000000002"), 22, "WIR-4" },
                    { new Guid("20000001-0000-0000-0004-000000000023"), new Guid("40000001-0000-0000-0000-000000000022"), "Panel to Slab Connections", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "F3", new Guid("30000001-0000-0000-0000-000000000002"), 23, "WIR-4" },
                    { new Guid("20000001-0000-0000-0004-000000000024"), new Guid("40000001-0000-0000-0000-000000000022"), "MEP Clearance", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "F4", new Guid("30000001-0000-0000-0000-000000000004"), 24, "WIR-4" },
                    { new Guid("20000001-0000-0000-0004-000000000025"), new Guid("40000001-0000-0000-0000-000000000022"), "Grouting", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "F5", new Guid("30000001-0000-0000-0000-000000000008"), 25, "WIR-4" },
                    { new Guid("20000001-0000-0000-0004-000000000026"), new Guid("40000001-0000-0000-0000-000000000022"), "Curing", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "F6", new Guid("30000001-0000-0000-0000-000000000008"), 26, "WIR-4" },
                    { new Guid("20000001-0000-0000-0004-000000000027"), new Guid("40000001-0000-0000-0000-000000000023"), "Internal and External Dimension of Box", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "G1", new Guid("30000001-0000-0000-0000-000000000002"), 27, "WIR-4" },
                    { new Guid("20000001-0000-0000-0004-000000000028"), new Guid("40000001-0000-0000-0000-000000000023"), "Check for edges + Angles + grooves + chamfer + Pin holes + Cracks before moving to finishing area", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "G2", new Guid("30000001-0000-0000-0000-000000000004"), 28, "WIR-4" },
                    { new Guid("20000001-0000-0000-0005-000000000001"), new Guid("40000001-0000-0000-0000-000000000024"), "Ensure method statement, materials and drawings (finishing schedule) are approved", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A1", new Guid("30000001-0000-0000-0000-000000000007"), 1, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000002"), new Guid("40000001-0000-0000-0000-000000000024"), "Ensure materials are stored as per manufacturers recommendations", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A2", new Guid("30000001-0000-0000-0000-000000000026"), 2, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000003"), new Guid("40000001-0000-0000-0000-000000000024"), "Verify the expiry date and number of coats of the material prior to applications", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A3", new Guid("30000001-0000-0000-0000-000000000004"), 3, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000004"), new Guid("40000001-0000-0000-0000-000000000025"), "Check substrate is clean, free from contaminants like dust, traces of curing compound, oil and grease", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "B1", new Guid("30000001-0000-0000-0000-000000000004"), 4, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000005"), new Guid("40000001-0000-0000-0000-000000000025"), "Check for repair of surface imperfection and protrusions (if any)", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "B2", new Guid("30000001-0000-0000-0000-000000000004"), 5, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000006"), new Guid("40000001-0000-0000-0000-000000000025"), "Moisture content for the substrate and environmental conditions as per manufacturer recommendations", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "B3", new Guid("30000001-0000-0000-0000-000000000026"), 6, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000007"), new Guid("40000001-0000-0000-0000-000000000025"), "Check the MEP clearance prior to start Painting works", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "B4", new Guid("30000001-0000-0000-0000-000000000004"), 7, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000008"), new Guid("40000001-0000-0000-0000-000000000026"), "Ensure application of Primer as per manufacturers recommendation", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "C1", new Guid("30000001-0000-0000-0000-000000000026"), 8, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000009"), new Guid("40000001-0000-0000-0000-000000000026"), "Ensure application of Stucco as per manufacturers recommendation", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "C2", new Guid("30000001-0000-0000-0000-000000000026"), 9, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000010"), new Guid("40000001-0000-0000-0000-000000000026"), "Touchup, grinding, undulations, corner repairs and pinholes are filled properly", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "C3", new Guid("30000001-0000-0000-0000-000000000004"), 10, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000011"), new Guid("40000001-0000-0000-0000-000000000026"), "Application of first coat of Paint as per manufacturers recommendation", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "C4", new Guid("30000001-0000-0000-0000-000000000026"), 11, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000012"), new Guid("40000001-0000-0000-0000-000000000026"), "Line between two color shades is straight, no Brush marks should be visible", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "C5", new Guid("30000001-0000-0000-0000-000000000004"), 12, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000013"), new Guid("40000001-0000-0000-0000-000000000027"), "Ensure application of Primer as per manufacturers recommendation", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "D1", new Guid("30000001-0000-0000-0000-000000000026"), 13, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000014"), new Guid("40000001-0000-0000-0000-000000000027"), "Ensure application of Filler Coats as per manufacturers recommendation", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "D2", new Guid("30000001-0000-0000-0000-000000000026"), 14, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000015"), new Guid("40000001-0000-0000-0000-000000000027"), "Touch up, grinding, undulations, corner repairs and pinholes are filled properly", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "D3", new Guid("30000001-0000-0000-0000-000000000004"), 15, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000016"), new Guid("40000001-0000-0000-0000-000000000027"), "Application of final coat of Texture Paint as per manufacturers recommendation", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "D4", new Guid("30000001-0000-0000-0000-000000000026"), 16, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000017"), new Guid("40000001-0000-0000-0000-000000000028"), "Ensure method statement, materials and drawings (finishing schedule) are approved", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "E1", new Guid("30000001-0000-0000-0000-000000000007"), 17, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000018"), new Guid("40000001-0000-0000-0000-000000000028"), "Check the Location, Colour & Type of tile as per the approved shop drawings / material submittal", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "E2", new Guid("30000001-0000-0000-0000-000000000023"), 18, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000019"), new Guid("40000001-0000-0000-0000-000000000028"), "Check the setting out / pattern of wall and floor tiles as per the approved drawings", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "E3", new Guid("30000001-0000-0000-0000-000000000002"), 19, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000020"), new Guid("40000001-0000-0000-0000-000000000028"), "Verify the slope and level of the tiles as per the approved drawings", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "E4", new Guid("30000001-0000-0000-0000-000000000002"), 20, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000021"), new Guid("40000001-0000-0000-0000-000000000028"), "Check the application of tile grout as per the manufacturer recommendations", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "E5", new Guid("30000001-0000-0000-0000-000000000026"), 21, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000022"), new Guid("40000001-0000-0000-0000-000000000029"), "Ensure method statement, material submittal and drawings are approved", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "F1", new Guid("30000001-0000-0000-0000-000000000007"), 22, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000023"), new Guid("40000001-0000-0000-0000-000000000029"), "Verify the marking and setting out of the partition walls as per the approved drawings", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "F2", new Guid("30000001-0000-0000-0000-000000000002"), 23, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000024"), new Guid("40000001-0000-0000-0000-000000000029"), "Verify the location, spacing and fixation of the supporting grid as per the approved drawings", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "F3", new Guid("30000001-0000-0000-0000-000000000002"), 24, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000025"), new Guid("40000001-0000-0000-0000-000000000029"), "Verify the fixation of the board as per the approved drawings", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "F4", new Guid("30000001-0000-0000-0000-000000000002"), 25, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000026"), new Guid("40000001-0000-0000-0000-000000000030"), "Ensure method statement, material submittal and drawings are approved", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "G1", new Guid("30000001-0000-0000-0000-000000000007"), 26, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000027"), new Guid("40000001-0000-0000-0000-000000000030"), "Verify the marking of the false ceiling level on the walls as per the approved drawings", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "G2", new Guid("30000001-0000-0000-0000-000000000002"), 27, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000028"), new Guid("40000001-0000-0000-0000-000000000030"), "Verify the location, spacing and fixation of the grid as per the approved drawings", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "G3", new Guid("30000001-0000-0000-0000-000000000002"), 28, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000029"), new Guid("40000001-0000-0000-0000-000000000030"), "Verify the type, fixation, level and alignment of the false ceiling board / tiles", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "G4", new Guid("30000001-0000-0000-0000-000000000002"), 29, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000030"), new Guid("40000001-0000-0000-0000-000000000031"), "Ensure method statement and materials are approved", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "H1", new Guid("30000001-0000-0000-0000-000000000007"), 30, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000031"), new Guid("40000001-0000-0000-0000-000000000031"), "Check substrate is clean, free from contaminants", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "H2", new Guid("30000001-0000-0000-0000-000000000004"), 31, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000032"), new Guid("40000001-0000-0000-0000-000000000031"), "Check the application of coats as per project requirements / manufacturer recommendations", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "H3", new Guid("30000001-0000-0000-0000-000000000026"), 32, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000033"), new Guid("40000001-0000-0000-0000-000000000031"), "Check for any water seepage / leakage after 24 hours or as per the project requirements", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "H4", new Guid("30000001-0000-0000-0000-000000000008"), 33, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000034"), new Guid("40000001-0000-0000-0000-000000000032"), "Ensure method statement, material submittal and drawings are approved", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "I1", new Guid("30000001-0000-0000-0000-000000000007"), 34, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000035"), new Guid("40000001-0000-0000-0000-000000000032"), "Check the color, type, material, coating of door and window materials as per approval", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "I2", new Guid("30000001-0000-0000-0000-000000000003"), 35, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000036"), new Guid("40000001-0000-0000-0000-000000000032"), "Verify the location and clear opening of doors / windows as per the approved drawings", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "I3", new Guid("30000001-0000-0000-0000-000000000002"), 36, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000037"), new Guid("40000001-0000-0000-0000-000000000032"), "Ensure the location and No. of Door hinges provided as per the approved drawings", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "I4", new Guid("30000001-0000-0000-0000-000000000031"), 37, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000038"), new Guid("40000001-0000-0000-0000-000000000032"), "Ensure required Iron mongery sets are provided as per the door schedule drawings", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "I5", new Guid("30000001-0000-0000-0000-000000000031"), 38, "WIR-5" },
                    { new Guid("20000001-0000-0000-0006-000000000001"), new Guid("40000001-0000-0000-0000-000000000033"), "Ensure method statement, ITP, materials and shop drawings are approved", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "1.1", new Guid("30000001-0000-0000-0000-000000000007"), 1, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000002"), new Guid("40000001-0000-0000-0000-000000000033"), "Check identification tag of the modular", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "1.2", new Guid("30000001-0000-0000-0000-000000000004"), 2, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000003"), new Guid("40000001-0000-0000-0000-000000000033"), "Visually inspect the modular for any defects or damages", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "1.3", new Guid("30000001-0000-0000-0000-000000000004"), 3, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000004"), new Guid("40000001-0000-0000-0000-000000000033"), "Verify the method of loading as per the project / design requirements", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "1.4", new Guid("30000001-0000-0000-0000-000000000002"), 4, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000005"), new Guid("40000001-0000-0000-0000-000000000033"), "Internal and External Dimensions of the modular", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "2.1", new Guid("30000001-0000-0000-0000-000000000002"), 5, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000006"), new Guid("40000001-0000-0000-0000-000000000034"), "Location and color of Painting as per the App Drawing", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "3.1", new Guid("30000001-0000-0000-0000-000000000002"), 6, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000007"), new Guid("40000001-0000-0000-0000-000000000034"), "Internal Paint (Application of Primer, Stucco and 1st Coat of Paint)", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "3.2", new Guid("30000001-0000-0000-0000-000000000029"), 7, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000008"), new Guid("40000001-0000-0000-0000-000000000034"), "External Paint(Application of Primer, Filler and Final Coat Texture Paint)", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "3.3", new Guid("30000001-0000-0000-0000-000000000029"), 8, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000009"), new Guid("40000001-0000-0000-0000-000000000034"), "Ensure Paint touch ups are completed around installed items", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "3.4", new Guid("30000001-0000-0000-0000-000000000004"), 9, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000010"), new Guid("40000001-0000-0000-0000-000000000034"), "Bitumen Applied at required Areas", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "3.5", new Guid("30000001-0000-0000-0000-000000000002"), 10, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000011"), new Guid("40000001-0000-0000-0000-000000000034"), "Damages, If any", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "3.6", new Guid("30000001-0000-0000-0000-000000000004"), 11, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000012"), new Guid("40000001-0000-0000-0000-000000000035"), "Layout and Fixing of Tiles as per App Drawing", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "4.1", new Guid("30000001-0000-0000-0000-000000000002"), 12, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000013"), new Guid("40000001-0000-0000-0000-000000000035"), "Line, Level and Spacer for the Installed Tiles", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "4.2", new Guid("30000001-0000-0000-0000-000000000004"), 13, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000014"), new Guid("40000001-0000-0000-0000-000000000035"), "Skirting is installed/fixed properly and truly vertical", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "4.3", new Guid("30000001-0000-0000-0000-000000000002"), 14, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000015"), new Guid("40000001-0000-0000-0000-000000000035"), "Grouting of all Joints is done properly", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "4.4", new Guid("30000001-0000-0000-0000-000000000004"), 15, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000016"), new Guid("40000001-0000-0000-0000-000000000035"), "Elastomeric sealant under skirting is provided properly", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "4.5", new Guid("30000001-0000-0000-0000-000000000026"), 16, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000017"), new Guid("40000001-0000-0000-0000-000000000035"), "Ensure Drain holes are free from any debris and properly closed (if applicable)", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "4.6", new Guid("30000001-0000-0000-0000-000000000004"), 17, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000018"), new Guid("40000001-0000-0000-0000-000000000035"), "Damages, if any", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "4.7", new Guid("30000001-0000-0000-0000-000000000004"), 18, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000019"), new Guid("40000001-0000-0000-0000-000000000036"), "Layout, location and position of dry wall is as per App Drawing", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "5.1", new Guid("30000001-0000-0000-0000-000000000002"), 19, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000020"), new Guid("40000001-0000-0000-0000-000000000036"), "Thickness of Dry wall is as per App Drawing", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "5.2", new Guid("30000001-0000-0000-0000-000000000002"), 20, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000021"), new Guid("40000001-0000-0000-0000-000000000036"), "Opening for MEP services are cut properly", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "5.3", new Guid("30000001-0000-0000-0000-000000000002"), 21, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000022"), new Guid("40000001-0000-0000-0000-000000000036"), "Ensure Gypsum surface are Crack free at joints", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "5.4", new Guid("30000001-0000-0000-0000-000000000001"), 22, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000023"), new Guid("40000001-0000-0000-0000-000000000036"), "Damages, if any", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "5.5", new Guid("30000001-0000-0000-0000-000000000001"), 23, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000024"), new Guid("40000001-0000-0000-0000-000000000037"), "Layout of False Ceiling tiles and bulk head as per App Drawing", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "6.1", new Guid("30000001-0000-0000-0000-000000000002"), 24, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000025"), new Guid("40000001-0000-0000-0000-000000000037"), "Height of the False Ceiling as per App Drawing", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "6.2", new Guid("30000001-0000-0000-0000-000000000002"), 25, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000026"), new Guid("40000001-0000-0000-0000-000000000037"), "Access panels/ Ceiling tiles are Fixed Properly", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "6.3", new Guid("30000001-0000-0000-0000-000000000001"), 26, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000027"), new Guid("40000001-0000-0000-0000-000000000037"), "Ensure Gypsum surface are Crack free at joints", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "6.4", new Guid("30000001-0000-0000-0000-000000000001"), 27, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000028"), new Guid("40000001-0000-0000-0000-000000000038"), "Location of Window as per App Drawing", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "7.1", new Guid("30000001-0000-0000-0000-000000000002"), 28, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000029"), new Guid("40000001-0000-0000-0000-000000000038"), "Fixing of Glass/panels", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "7.2", new Guid("30000001-0000-0000-0000-000000000001"), 29, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000030"), new Guid("40000001-0000-0000-0000-000000000038"), "Fixing of Iron-Mongery and Accessories", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "7.3", new Guid("30000001-0000-0000-0000-000000000001"), 30, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000031"), new Guid("40000001-0000-0000-0000-000000000038"), "Fixing of Silicone Sealant", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "7.4", new Guid("30000001-0000-0000-0000-000000000015"), 31, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000032"), new Guid("40000001-0000-0000-0000-000000000038"), "Water leak test performed and passed", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "7.5", new Guid("30000001-0000-0000-0000-000000000005"), 32, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000033"), new Guid("40000001-0000-0000-0000-000000000038"), "Paint touch completed around the frame", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "7.6", new Guid("30000001-0000-0000-0000-000000000001"), 33, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000034"), new Guid("40000001-0000-0000-0000-000000000039"), "Location of Doors as per App Drawing", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "8.1", new Guid("30000001-0000-0000-0000-000000000002"), 34, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000035"), new Guid("40000001-0000-0000-0000-000000000039"), "Direction of doors swing as per App Drawing", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "8.2", new Guid("30000001-0000-0000-0000-000000000031"), 35, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000036"), new Guid("40000001-0000-0000-0000-000000000039"), "Main entrance door as per App Drawing", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "8.3", new Guid("30000001-0000-0000-0000-000000000031"), 36, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000037"), new Guid("40000001-0000-0000-0000-000000000039"), "Lock of Main entrance door is installed", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "8.4", new Guid("30000001-0000-0000-0000-000000000001"), 37, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000038"), new Guid("40000001-0000-0000-0000-000000000040"), "Kitchen cabinets, counter top and accessories installed as per app drawing", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "8.10", new Guid("30000001-0000-0000-0000-000000000002"), 38, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000039"), new Guid("40000001-0000-0000-0000-000000000040"), "Kitchen sink and sink mixer installed", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "8.11", new Guid("30000001-0000-0000-0000-000000000001"), 39, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000040"), new Guid("40000001-0000-0000-0000-000000000040"), "Wardrobe installed as per approved drawings", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "8.12", new Guid("30000001-0000-0000-0000-000000000002"), 40, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000041"), new Guid("40000001-0000-0000-0000-000000000040"), "Wardrobe doors and drawers functioning smoothly and free from scratches", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "8.13", new Guid("30000001-0000-0000-0000-000000000001"), 41, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000042"), new Guid("40000001-0000-0000-0000-000000000041"), "Mirror installed and free from damage", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "9.1", new Guid("30000001-0000-0000-0000-000000000001"), 42, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000043"), new Guid("40000001-0000-0000-0000-000000000041"), "Threshold installed and free from damage", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "9.2", new Guid("30000001-0000-0000-0000-000000000001"), 43, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000044"), new Guid("40000001-0000-0000-0000-000000000041"), "Glass Partition installed and free from damage", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "9.3", new Guid("30000001-0000-0000-0000-000000000001"), 44, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000045"), new Guid("40000001-0000-0000-0000-000000000041"), "Floor drain and covers installed and free from damages", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "9.4", new Guid("30000001-0000-0000-0000-000000000001"), 45, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000046"), new Guid("40000001-0000-0000-0000-000000000041"), "Vanity installed and free from damage", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "9.5", new Guid("30000001-0000-0000-0000-000000000001"), 46, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000047"), new Guid("40000001-0000-0000-0000-000000000041"), "WC and cover installed and free from damage", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "9.6", new Guid("30000001-0000-0000-0000-000000000001"), 47, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000048"), new Guid("40000001-0000-0000-0000-000000000041"), "Shower installed and free from damage", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "9.7", new Guid("30000001-0000-0000-0000-000000000001"), 48, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000049"), new Guid("40000001-0000-0000-0000-000000000041"), "Toilet accessories installed and free from damage", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "9.8", new Guid("30000001-0000-0000-0000-000000000001"), 49, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000050"), new Guid("40000001-0000-0000-0000-000000000041"), "Firestop sealant, fire rated sealant & General sealant applied around penetration pipes", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "9.9", new Guid("30000001-0000-0000-0000-000000000034"), 50, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000051"), new Guid("40000001-0000-0000-0000-000000000041"), "Painted walls are clean and free from stains", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "9.10", new Guid("30000001-0000-0000-0000-000000000001"), 51, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000052"), new Guid("40000001-0000-0000-0000-000000000041"), "Tiles are fixed with grouting properly and free from damage", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "9.11", new Guid("30000001-0000-0000-0000-000000000001"), 52, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000053"), new Guid("40000001-0000-0000-0000-000000000042"), "Check Final Condition of outside of the room and ensure its damage free", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "10.1", new Guid("30000001-0000-0000-0000-000000000001"), 53, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000054"), new Guid("40000001-0000-0000-0000-000000000042"), "Sign the delivery note for accepting the loading of precast modular in good condition", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "10.2", new Guid("30000001-0000-0000-0000-000000000001"), 54, "WIR-6" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PredefinedChecklistItems_CategoryId",
                table: "PredefinedChecklistItems",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PredefinedChecklistItems_ReferenceId",
                table: "PredefinedChecklistItems",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_CategoryName",
                table: "Categories",
                column: "CategoryName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_References_ReferenceName",
                table: "References",
                column: "ReferenceName",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PredefinedChecklistItems_Categories_CategoryId",
                table: "PredefinedChecklistItems",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_PredefinedChecklistItems_References_ReferenceId",
                table: "PredefinedChecklistItems",
                column: "ReferenceId",
                principalTable: "References",
                principalColumn: "ReferenceId",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
