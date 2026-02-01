using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class assignedActivityToGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BoxActivities_TeamGroups_AssignedGroupId",
                table: "BoxActivities");

            migrationBuilder.DropIndex(
                name: "IX_BoxActivities_AssignedGroupId",
                table: "BoxActivities");

            migrationBuilder.DropColumn(
                name: "AssignedGroupId",
                table: "BoxActivities");
        }
    }
}
