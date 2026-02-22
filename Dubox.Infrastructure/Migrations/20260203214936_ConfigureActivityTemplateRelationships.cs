using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureActivityTemplateRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityTemplateActivities_ActivityMaster_SourceActivityMasterId",
                table: "ActivityTemplateActivities");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityTemplates_IsActive",
                table: "ActivityTemplates",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityTemplates_TemplateName",
                table: "ActivityTemplates",
                column: "TemplateName");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityTemplateActivities_ActivityTemplateId_OverallSequence",
                table: "ActivityTemplateActivities",
                columns: new[] { "ActivityTemplateId", "OverallSequence" });

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityTemplateActivities_ActivityMaster_SourceActivityMasterId",
                table: "ActivityTemplateActivities",
                column: "SourceActivityMasterId",
                principalTable: "ActivityMaster",
                principalColumn: "ActivityMasterId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityTemplateActivities_ActivityMaster_SourceActivityMasterId",
                table: "ActivityTemplateActivities");

            migrationBuilder.DropIndex(
                name: "IX_ActivityTemplates_IsActive",
                table: "ActivityTemplates");

            migrationBuilder.DropIndex(
                name: "IX_ActivityTemplates_TemplateName",
                table: "ActivityTemplates");

            migrationBuilder.DropIndex(
                name: "IX_ActivityTemplateActivities_ActivityTemplateId_OverallSequence",
                table: "ActivityTemplateActivities");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityTemplateActivities_ActivityMaster_SourceActivityMasterId",
                table: "ActivityTemplateActivities",
                column: "SourceActivityMasterId",
                principalTable: "ActivityMaster",
                principalColumn: "ActivityMasterId");
        }
    }
}
