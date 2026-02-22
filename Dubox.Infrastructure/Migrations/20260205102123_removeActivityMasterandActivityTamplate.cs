using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removeActivityMasterandActivityTamplate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityTemplateActivities_ActivityMaster_SourceActivityMasterId",
                table: "ActivityTemplateActivities");

            migrationBuilder.DropIndex(
                name: "IX_ActivityTemplateActivities_SourceActivityMasterId",
                table: "ActivityTemplateActivities");

            migrationBuilder.DropColumn(
                name: "IsCustomActivity",
                table: "ActivityTemplateActivities");

            migrationBuilder.DropColumn(
                name: "SourceActivityMasterId",
                table: "ActivityTemplateActivities");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCustomActivity",
                table: "ActivityTemplateActivities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "SourceActivityMasterId",
                table: "ActivityTemplateActivities",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ActivityTemplateActivities_SourceActivityMasterId",
                table: "ActivityTemplateActivities",
                column: "SourceActivityMasterId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityTemplateActivities_ActivityMaster_SourceActivityMasterId",
                table: "ActivityTemplateActivities",
                column: "SourceActivityMasterId",
                principalTable: "ActivityMaster",
                principalColumn: "ActivityMasterId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
