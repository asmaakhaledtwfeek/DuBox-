using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class stagesCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StageRangeEnd",
                table: "ActivityTemplates");

            migrationBuilder.RenameColumn(
                name: "StageRangeStart",
                table: "ActivityTemplates",
                newName: "StageCount");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StageCount",
                table: "ActivityTemplates",
                newName: "StageRangeStart");

            migrationBuilder.AddColumn<int>(
                name: "StageRangeEnd",
                table: "ActivityTemplates",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
