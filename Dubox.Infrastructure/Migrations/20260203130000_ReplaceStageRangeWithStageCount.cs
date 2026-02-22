using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceStageRangeWithStageCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StageCount",
                table: "ActivityTemplates",
                type: "int",
                nullable: false,
                defaultValue: 1);

            // Migrate existing data: StageCount = StageRangeEnd - StageRangeStart + 1
            migrationBuilder.Sql(@"
                UPDATE ActivityTemplates 
                SET StageCount = CASE 
                    WHEN StageRangeEnd >= StageRangeStart 
                    THEN StageRangeEnd - StageRangeStart + 1 
                    ELSE 1 
                END
            ");

            migrationBuilder.DropColumn(
                name: "StageRangeStart",
                table: "ActivityTemplates");

            migrationBuilder.DropColumn(
                name: "StageRangeEnd",
                table: "ActivityTemplates");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StageRangeStart",
                table: "ActivityTemplates",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "StageRangeEnd",
                table: "ActivityTemplates",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.Sql(@"
                UPDATE ActivityTemplates 
                SET StageRangeStart = 1, StageRangeEnd = StageCount
            ");

            migrationBuilder.DropColumn(
                name: "StageCount",
                table: "ActivityTemplates");
        }
    }
}
