using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class OneToOneProjectMaterialTemplate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProjectMaterialTemplates_ProjectId_MaterialTemplateId",
                table: "ProjectMaterialTemplates");

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
