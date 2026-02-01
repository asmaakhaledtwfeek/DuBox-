using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addReferenceAndCategoryTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReferenceDocument",
                table: "PredefinedChecklistItems");

            migrationBuilder.RenameColumn(
                name: "Category",
                table: "PredefinedChecklistItems",
                newName: "ItemNumber");

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
                    ReferenceName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_References", x => x.ReferenceId);
                });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000001"),
                columns: new[] { "CategoryId", "ItemNumber", "ReferenceId" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000002"),
                columns: new[] { "CategoryId", "ItemNumber", "ReferenceId" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000003"),
                columns: new[] { "CategoryId", "ItemNumber", "ReferenceId" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000004"),
                columns: new[] { "CategoryId", "ItemNumber", "ReferenceId" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000005"),
                columns: new[] { "CategoryId", "ItemNumber", "ReferenceId" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000006"),
                columns: new[] { "CategoryId", "ItemNumber", "ReferenceId" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000007"),
                columns: new[] { "CategoryId", "ItemNumber", "ReferenceId" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000008"),
                columns: new[] { "CategoryId", "ItemNumber", "ReferenceId" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000009"),
                columns: new[] { "CategoryId", "ItemNumber", "ReferenceId" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000010"),
                columns: new[] { "CategoryId", "ItemNumber", "ReferenceId" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000011"),
                columns: new[] { "CategoryId", "ItemNumber", "ReferenceId" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000012"),
                columns: new[] { "CategoryId", "ItemNumber", "ReferenceId" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000013"),
                columns: new[] { "CategoryId", "ItemNumber", "ReferenceId" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000014"),
                columns: new[] { "CategoryId", "ItemNumber", "ReferenceId" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000015"),
                columns: new[] { "CategoryId", "ItemNumber", "ReferenceId" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000016"),
                columns: new[] { "CategoryId", "ItemNumber", "ReferenceId" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000017"),
                columns: new[] { "CategoryId", "ItemNumber", "ReferenceId" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000018"),
                columns: new[] { "CategoryId", "ItemNumber", "ReferenceId" },
                values: new object[] { null, null, null });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropIndex(
                name: "IX_PredefinedChecklistItems_CategoryId",
                table: "PredefinedChecklistItems");

            migrationBuilder.DropIndex(
                name: "IX_PredefinedChecklistItems_ReferenceId",
                table: "PredefinedChecklistItems");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "PredefinedChecklistItems");

            migrationBuilder.DropColumn(
                name: "ReferenceId",
                table: "PredefinedChecklistItems");

            migrationBuilder.RenameColumn(
                name: "ItemNumber",
                table: "PredefinedChecklistItems",
                newName: "Category");

            migrationBuilder.AddColumn<string>(
                name: "ReferenceDocument",
                table: "PredefinedChecklistItems",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000001"),
                columns: new[] { "Category", "ReferenceDocument" },
                values: new object[] { "General", null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000002"),
                columns: new[] { "Category", "ReferenceDocument" },
                values: new object[] { "General", null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000003"),
                columns: new[] { "Category", "ReferenceDocument" },
                values: new object[] { "General", null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000004"),
                columns: new[] { "Category", "ReferenceDocument" },
                values: new object[] { "General", null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000005"),
                columns: new[] { "Category", "ReferenceDocument" },
                values: new object[] { "Setting Out", null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000006"),
                columns: new[] { "Category", "ReferenceDocument" },
                values: new object[] { "Installation Activity", null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000007"),
                columns: new[] { "Category", "ReferenceDocument" },
                values: new object[] { "Installation Activity", null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000008"),
                columns: new[] { "Category", "ReferenceDocument" },
                values: new object[] { "Installation Activity", null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000009"),
                columns: new[] { "Category", "ReferenceDocument" },
                values: new object[] { "Installation Activity", null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000010"),
                columns: new[] { "Category", "ReferenceDocument" },
                values: new object[] { "Installation Activity", null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000011"),
                columns: new[] { "Category", "ReferenceDocument" },
                values: new object[] { "Installation Activity", null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000012"),
                columns: new[] { "Category", "ReferenceDocument" },
                values: new object[] { "Installation Activity", null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000013"),
                columns: new[] { "Category", "ReferenceDocument" },
                values: new object[] { "Installation Activity", null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000014"),
                columns: new[] { "Category", "ReferenceDocument" },
                values: new object[] { "Installation Activity", null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000015"),
                columns: new[] { "Category", "ReferenceDocument" },
                values: new object[] { "Installation Activity", null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000016"),
                columns: new[] { "Category", "ReferenceDocument" },
                values: new object[] { "Installation Activity", null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000017"),
                columns: new[] { "Category", "ReferenceDocument" },
                values: new object[] { "Installation Activity", null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000018"),
                columns: new[] { "Category", "ReferenceDocument" },
                values: new object[] { "Installation Activity", null });
        }
    }
}
