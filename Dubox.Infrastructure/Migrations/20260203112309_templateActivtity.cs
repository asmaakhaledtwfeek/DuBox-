using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class templateActivtity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "ActivityMasterId",
                table: "BoxActivities",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "ActivityTemplateActivityId",
                table: "BoxActivities",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BoxActivities_ActivityTemplateActivityId",
                table: "BoxActivities",
                column: "ActivityTemplateActivityId");

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
            migrationBuilder.DropForeignKey(
                name: "FK_BoxActivities_ActivityTemplateActivities_ActivityTemplateActivityId",
                table: "BoxActivities");

            migrationBuilder.DropIndex(
                name: "IX_BoxActivities_ActivityTemplateActivityId",
                table: "BoxActivities");

            migrationBuilder.DropColumn(
                name: "ActivityTemplateActivityId",
                table: "BoxActivities");

            migrationBuilder.AlterColumn<Guid>(
                name: "ActivityMasterId",
                table: "BoxActivities",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);
        }
    }
}
