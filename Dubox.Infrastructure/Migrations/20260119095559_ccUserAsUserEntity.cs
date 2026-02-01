using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ccUserAsUserEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QualityIssues_TeamMembers_CCUserId",
                table: "QualityIssues");

            migrationBuilder.AddForeignKey(
                name: "FK_QualityIssues_Users_CCUserId",
                table: "QualityIssues",
                column: "CCUserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QualityIssues_Users_CCUserId",
                table: "QualityIssues");

            migrationBuilder.AddForeignKey(
                name: "FK_QualityIssues_TeamMembers_CCUserId",
                table: "QualityIssues",
                column: "CCUserId",
                principalTable: "TeamMembers",
                principalColumn: "TeamMemberId");
        }
    }
}
