using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAssignedToUserIdToQualityIssue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AssignedToUserId",
                table: "QualityIssues",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_QualityIssues_AssignedToUserId",
                table: "QualityIssues",
                column: "AssignedToUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_QualityIssues_Users_AssignedToUserId",
                table: "QualityIssues",
                column: "AssignedToUserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QualityIssues_Users_AssignedToUserId",
                table: "QualityIssues");

            migrationBuilder.DropIndex(
                name: "IX_QualityIssues_AssignedToUserId",
                table: "QualityIssues");

            migrationBuilder.DropColumn(
                name: "AssignedToUserId",
                table: "QualityIssues");
        }
    }
}
