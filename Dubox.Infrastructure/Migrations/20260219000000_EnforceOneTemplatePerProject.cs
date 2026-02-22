using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EnforceOneTemplatePerProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop the composite unique index that allowed a project to hold multiple templates
            migrationBuilder.DropIndex(
                name: "IX_ProjectMaterialTemplates_ProjectId_MaterialTemplateId",
                table: "ProjectMaterialTemplates");

            // Add a unique index on ProjectId alone – one template per project enforced at the DB level
            migrationBuilder.CreateIndex(
                name: "IX_ProjectMaterialTemplates_ProjectId",
                table: "ProjectMaterialTemplates",
                column: "ProjectId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProjectMaterialTemplates_ProjectId",
                table: "ProjectMaterialTemplates");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMaterialTemplates_ProjectId_MaterialTemplateId",
                table: "ProjectMaterialTemplates",
                columns: new[] { "ProjectId", "MaterialTemplateId" },
                unique: true);
        }
    }
}
