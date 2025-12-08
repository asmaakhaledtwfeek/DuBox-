using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPredefinedChecklistItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Create PredefinedChecklistItems table
            migrationBuilder.CreateTable(
                name: "PredefinedChecklistItems",
                columns: table => new
                {
                    PredefinedItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CheckpointDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ReferenceDocument = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Sequence = table.Column<int>(type: "int", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PredefinedChecklistItems", x => x.PredefinedItemId);
                });

            // Add PredefinedItemId column to WIRChecklistItems
            migrationBuilder.AddColumn<Guid>(
                name: "PredefinedItemId",
                table: "WIRChecklistItems",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove PredefinedItemId column from WIRChecklistItems
            migrationBuilder.DropColumn(
                name: "PredefinedItemId",
                table: "WIRChecklistItems");

            // Drop PredefinedChecklistItems table
            migrationBuilder.DropTable(
                name: "PredefinedChecklistItems");
        }
    }
}

