using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCCUserIdAndIssueNumberToQualityIssue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IssueNumber",
                table: "QualityIssues",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "00001");

            migrationBuilder.AddColumn<Guid>(
                name: "CCUserId",
                table: "QualityIssues",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_QualityIssues_CCUserId",
                table: "QualityIssues",
                column: "CCUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_QualityIssues_TeamMembers_CCUserId",
                table: "QualityIssues",
                column: "CCUserId",
                principalTable: "TeamMembers",
                principalColumn: "TeamMemberId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QualityIssues_TeamMembers_CCUserId",
                table: "QualityIssues");

            migrationBuilder.DropIndex(
                name: "IX_QualityIssues_CCUserId",
                table: "QualityIssues");

            migrationBuilder.DropColumn(
                name: "CCUserId",
                table: "QualityIssues");

            migrationBuilder.DropColumn(
                name: "IssueNumber",
                table: "QualityIssues");
        }
    }
}

