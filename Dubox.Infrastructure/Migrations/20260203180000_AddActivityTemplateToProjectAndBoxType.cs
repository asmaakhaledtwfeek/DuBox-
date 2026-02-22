using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddActivityTemplateToProjectAndBoxType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ActivityTemplateId",
                table: "Projects",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ActivityTemplateId",
                table: "ProjectBoxTypes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ActivityTemplateId",
                table: "Projects",
                column: "ActivityTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectBoxTypes_ActivityTemplateId",
                table: "ProjectBoxTypes",
                column: "ActivityTemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_ActivityTemplates_ActivityTemplateId",
                table: "Projects",
                column: "ActivityTemplateId",
                principalTable: "ActivityTemplates",
                principalColumn: "ActivityTemplateId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectBoxTypes_ActivityTemplates_ActivityTemplateId",
                table: "ProjectBoxTypes",
                column: "ActivityTemplateId",
                principalTable: "ActivityTemplates",
                principalColumn: "ActivityTemplateId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_ActivityTemplates_ActivityTemplateId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectBoxTypes_ActivityTemplates_ActivityTemplateId",
                table: "ProjectBoxTypes");

            migrationBuilder.DropIndex(
                name: "IX_Projects_ActivityTemplateId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_ProjectBoxTypes_ActivityTemplateId",
                table: "ProjectBoxTypes");

            migrationBuilder.DropColumn(
                name: "ActivityTemplateId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ActivityTemplateId",
                table: "ProjectBoxTypes");
        }
    }
}
