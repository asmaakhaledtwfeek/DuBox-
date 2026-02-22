using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBoxActivityRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop the existing foreign key constraint
            migrationBuilder.DropForeignKey(
                name: "FK_BoxActivities_ActivityMaster_ActivityMasterId",
                table: "BoxActivities");

            // Make ActivityMasterId nullable
            migrationBuilder.AlterColumn<Guid>(
                name: "ActivityMasterId",
                table: "BoxActivities",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            // Add new ActivityTemplateActivityId column
            migrationBuilder.AddColumn<Guid>(
                name: "ActivityTemplateActivityId",
                table: "BoxActivities",
                type: "uniqueidentifier",
                nullable: true);

            // Re-create the foreign key for ActivityMaster with nullable constraint
            migrationBuilder.AddForeignKey(
                name: "FK_BoxActivities_ActivityMaster_ActivityMasterId",
                table: "BoxActivities",
                column: "ActivityMasterId",
                principalTable: "ActivityMaster",
                principalColumn: "ActivityMasterId",
                onDelete: ReferentialAction.Restrict);

            // Create index for ActivityTemplateActivityId
            migrationBuilder.CreateIndex(
                name: "IX_BoxActivities_ActivityTemplateActivityId",
                table: "BoxActivities",
                column: "ActivityTemplateActivityId");

            // Add foreign key for ActivityTemplateActivity
            migrationBuilder.AddForeignKey(
                name: "FK_BoxActivities_ActivityTemplateActivities_ActivityTemplateActivityId",
                table: "BoxActivities",
                column: "ActivityTemplateActivityId",
                principalTable: "ActivityTemplateActivities",
                principalColumn: "ActivityTemplateActivityId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop the foreign key for ActivityTemplateActivity
            migrationBuilder.DropForeignKey(
                name: "FK_BoxActivities_ActivityTemplateActivities_ActivityTemplateActivityId",
                table: "BoxActivities");

            // Drop the foreign key for ActivityMaster
            migrationBuilder.DropForeignKey(
                name: "FK_BoxActivities_ActivityMaster_ActivityMasterId",
                table: "BoxActivities");

            // Drop index
            migrationBuilder.DropIndex(
                name: "IX_BoxActivities_ActivityTemplateActivityId",
                table: "BoxActivities");

            // Remove ActivityTemplateActivityId column
            migrationBuilder.DropColumn(
                name: "ActivityTemplateActivityId",
                table: "BoxActivities");

            // Make ActivityMasterId non-nullable again
            migrationBuilder.AlterColumn<Guid>(
                name: "ActivityMasterId",
                table: "BoxActivities",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            // Re-create the original foreign key constraint
            migrationBuilder.AddForeignKey(
                name: "FK_BoxActivities_ActivityMaster_ActivityMasterId",
                table: "BoxActivities",
                column: "ActivityMasterId",
                principalTable: "ActivityMaster",
                principalColumn: "ActivityMasterId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
