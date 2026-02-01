using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateQualityIsuueAssigned : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BoxActivities_TeamGroups_AssignedGroupId",
                table: "BoxActivities");

            migrationBuilder.DropForeignKey(
                name: "FK_QualityIssues_Teams_AssignedTo",
                table: "QualityIssues");

            migrationBuilder.DropForeignKey(
                name: "FK_QualityIssues_Users_AssignedToUserId",
                table: "QualityIssues");

            migrationBuilder.DropIndex(
                name: "IX_BoxActivities_AssignedGroupId",
                table: "BoxActivities");

            migrationBuilder.DropColumn(
                name: "AssignedGroupId",
                table: "BoxActivities");

            migrationBuilder.RenameColumn(
                name: "AssignedToUserId",
                table: "QualityIssues",
                newName: "AssignedToTeamId");

            migrationBuilder.RenameColumn(
                name: "AssignedTo",
                table: "QualityIssues",
                newName: "AssignedToMemberId");

            migrationBuilder.RenameIndex(
                name: "IX_QualityIssues_AssignedToUserId",
                table: "QualityIssues",
                newName: "IX_QualityIssues_AssignedToTeamId");

            migrationBuilder.RenameIndex(
                name: "IX_QualityIssues_AssignedTo",
                table: "QualityIssues",
                newName: "IX_QualityIssues_AssignedToMemberId");
            migrationBuilder.Sql(@"
                  UPDATE QualityIssues
                  SET AssignedToMemberId = NULL
                    ");
            migrationBuilder.AddForeignKey(
                name: "FK_QualityIssues_TeamMembers_AssignedToMemberId",
                table: "QualityIssues",
                column: "AssignedToMemberId",
                principalTable: "TeamMembers",
                principalColumn: "TeamMemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_QualityIssues_Teams_AssignedToTeamId",
                table: "QualityIssues",
                column: "AssignedToTeamId",
                principalTable: "Teams",
                principalColumn: "TeamId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QualityIssues_TeamMembers_AssignedToMemberId",
                table: "QualityIssues");

            migrationBuilder.DropForeignKey(
                name: "FK_QualityIssues_Teams_AssignedToTeamId",
                table: "QualityIssues");

            migrationBuilder.RenameColumn(
                name: "AssignedToTeamId",
                table: "QualityIssues",
                newName: "AssignedToUserId");

            migrationBuilder.RenameColumn(
                name: "AssignedToMemberId",
                table: "QualityIssues",
                newName: "AssignedTo");

            migrationBuilder.RenameIndex(
                name: "IX_QualityIssues_AssignedToTeamId",
                table: "QualityIssues",
                newName: "IX_QualityIssues_AssignedToUserId");

            migrationBuilder.RenameIndex(
                name: "IX_QualityIssues_AssignedToMemberId",
                table: "QualityIssues",
                newName: "IX_QualityIssues_AssignedTo");

            migrationBuilder.AddColumn<Guid>(
                name: "AssignedGroupId",
                table: "BoxActivities",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BoxActivities_AssignedGroupId",
                table: "BoxActivities",
                column: "AssignedGroupId");
            
            migrationBuilder.AddForeignKey(
                name: "FK_BoxActivities_TeamGroups_AssignedGroupId",
                table: "BoxActivities",
                column: "AssignedGroupId",
                principalTable: "TeamGroups",
                principalColumn: "TeamGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_QualityIssues_Teams_AssignedTo",
                table: "QualityIssues",
                column: "AssignedTo",
                principalTable: "Teams",
                principalColumn: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_QualityIssues_Users_AssignedToUserId",
                table: "QualityIssues",
                column: "AssignedToUserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }
    }
}
