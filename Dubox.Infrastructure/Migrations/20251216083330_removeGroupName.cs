using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removeGroupName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TeamGroups_TeamId_GroupName",
                table: "TeamGroups");

            migrationBuilder.DropColumn(
                name: "GroupName",
                table: "TeamGroups");

            migrationBuilder.CreateIndex(
                name: "IX_TeamGroups_TeamId_GroupTag",
                table: "TeamGroups",
                columns: new[] { "TeamId", "GroupTag" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TeamGroups_TeamId_GroupTag",
                table: "TeamGroups");

            migrationBuilder.AddColumn<string>(
                name: "GroupName",
                table: "TeamGroups",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_TeamGroups_TeamId_GroupName",
                table: "TeamGroups",
                columns: new[] { "TeamId", "GroupName" },
                unique: true);
        }
    }
}
