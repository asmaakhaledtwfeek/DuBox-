using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class oneTemplateForBoxType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BoxTypeMaterialTemplates_ProjectBoxTypeId_MaterialTemplateId",
                table: "BoxTypeMaterialTemplates");

            migrationBuilder.CreateIndex(
                name: "IX_BoxTypeMaterialTemplates_ProjectBoxTypeId",
                table: "BoxTypeMaterialTemplates",
                column: "ProjectBoxTypeId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BoxTypeMaterialTemplates_ProjectBoxTypeId",
                table: "BoxTypeMaterialTemplates");

            migrationBuilder.CreateIndex(
                name: "IX_BoxTypeMaterialTemplates_ProjectBoxTypeId_MaterialTemplateId",
                table: "BoxTypeMaterialTemplates",
                columns: new[] { "ProjectBoxTypeId", "MaterialTemplateId" },
                unique: true);
        }
    }
}
