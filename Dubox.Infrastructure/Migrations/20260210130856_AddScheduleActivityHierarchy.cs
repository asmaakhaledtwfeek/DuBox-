using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddScheduleActivityHierarchy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ParentActivityId",
                table: "ScheduleActivities",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleActivities_ParentActivityId",
                table: "ScheduleActivities",
                column: "ParentActivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduleActivities_ScheduleActivities_ParentActivityId",
                table: "ScheduleActivities",
                column: "ParentActivityId",
                principalTable: "ScheduleActivities",
                principalColumn: "ScheduleActivityId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScheduleActivities_ScheduleActivities_ParentActivityId",
                table: "ScheduleActivities");

            migrationBuilder.DropIndex(
                name: "IX_ScheduleActivities_ParentActivityId",
                table: "ScheduleActivities");

            migrationBuilder.DropColumn(
                name: "ParentActivityId",
                table: "ScheduleActivities");
        }
    }
}
