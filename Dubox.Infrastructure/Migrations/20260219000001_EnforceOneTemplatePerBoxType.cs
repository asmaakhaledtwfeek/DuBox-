using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EnforceOneTemplatePerBoxType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Remove any stale duplicate rows before applying the unique constraint.
            // Keep the most-recently assigned row for each box type; delete the rest.
            migrationBuilder.Sql(@"
                DELETE FROM BoxTypeMaterialTemplates
                WHERE BoxTypeMaterialTemplateId NOT IN (
                    SELECT TOP 1 WITH TIES BoxTypeMaterialTemplateId
                    FROM BoxTypeMaterialTemplates
                    ORDER BY ROW_NUMBER() OVER (
                        PARTITION BY ProjectBoxTypeId
                        ORDER BY AssignedDate DESC
                    )
                );
            ");

            // Drop the old composite unique index (ProjectBoxTypeId + MaterialTemplateId)
            migrationBuilder.DropIndex(
                name: "IX_BoxTypeMaterialTemplates_ProjectBoxTypeId_MaterialTemplateId",
                table: "BoxTypeMaterialTemplates");

            // Add a unique index on ProjectBoxTypeId alone – one template per box type
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
