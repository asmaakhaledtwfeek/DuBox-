using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class assignedCustomActivity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedTeamIds",
                table: "ActivityTemplateActivities");

            migrationBuilder.AddColumn<Guid>(
                name: "AssignedTeamId",
                table: "ActivityTemplateActivities",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ActivityTemplateActivities_AssignedTeamId",
                table: "ActivityTemplateActivities",
                column: "AssignedTeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityTemplateActivities_Teams_AssignedTeamId",
                table: "ActivityTemplateActivities",
                column: "AssignedTeamId",
                principalTable: "Teams",
                principalColumn: "TeamId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityTemplateActivities_Teams_AssignedTeamId",
                table: "ActivityTemplateActivities");

            migrationBuilder.DropIndex(
                name: "IX_ActivityTemplateActivities_AssignedTeamId",
                table: "ActivityTemplateActivities");

            migrationBuilder.DropColumn(
                name: "AssignedTeamId",
                table: "ActivityTemplateActivities");

            migrationBuilder.AddColumn<string>(
                name: "AssignedTeamIds",
                table: "ActivityTemplateActivities",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }
    }
}
