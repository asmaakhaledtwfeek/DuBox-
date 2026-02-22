using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMaterialTemplates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MaterialTemplates",
                columns: table => new
                {
                    MaterialTemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TemplateName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TemplateCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialTemplates", x => x.MaterialTemplateId);
                });

            migrationBuilder.CreateTable(
                name: "BoxTypeMaterialTemplates",
                columns: table => new
                {
                    BoxTypeMaterialTemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectBoxTypeId = table.Column<int>(type: "int", nullable: false),
                    MaterialTemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssignedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AssignedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoxTypeMaterialTemplates", x => x.BoxTypeMaterialTemplateId);
                    table.ForeignKey(
                        name: "FK_BoxTypeMaterialTemplates_MaterialTemplates_MaterialTemplateId",
                        column: x => x.MaterialTemplateId,
                        principalTable: "MaterialTemplates",
                        principalColumn: "MaterialTemplateId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BoxTypeMaterialTemplates_ProjectBoxTypes_ProjectBoxTypeId",
                        column: x => x.ProjectBoxTypeId,
                        principalTable: "ProjectBoxTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MaterialTemplateItems",
                columns: table => new
                {
                    MaterialTemplateItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaterialTemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaterialId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequiredBeforeDays = table.Column<int>(type: "int", nullable: false),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialTemplateItems", x => x.MaterialTemplateItemId);
                    table.ForeignKey(
                        name: "FK_MaterialTemplateItems_MaterialTemplates_MaterialTemplateId",
                        column: x => x.MaterialTemplateId,
                        principalTable: "MaterialTemplates",
                        principalColumn: "MaterialTemplateId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MaterialTemplateItems_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "MaterialId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectMaterialTemplates",
                columns: table => new
                {
                    ProjectMaterialTemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaterialTemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssignedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AssignedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectMaterialTemplates", x => x.ProjectMaterialTemplateId);
                    table.ForeignKey(
                        name: "FK_ProjectMaterialTemplates_MaterialTemplates_MaterialTemplateId",
                        column: x => x.MaterialTemplateId,
                        principalTable: "MaterialTemplates",
                        principalColumn: "MaterialTemplateId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectMaterialTemplates_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BoxTypeMaterialTemplates_MaterialTemplateId",
                table: "BoxTypeMaterialTemplates",
                column: "MaterialTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_BoxTypeMaterialTemplates_ProjectBoxTypeId_MaterialTemplateId",
                table: "BoxTypeMaterialTemplates",
                columns: new[] { "ProjectBoxTypeId", "MaterialTemplateId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MaterialTemplateItems_MaterialId",
                table: "MaterialTemplateItems",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialTemplateItems_MaterialTemplateId_DisplayOrder",
                table: "MaterialTemplateItems",
                columns: new[] { "MaterialTemplateId", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_MaterialTemplates_IsActive",
                table: "MaterialTemplates",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialTemplates_TemplateCode",
                table: "MaterialTemplates",
                column: "TemplateCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMaterialTemplates_MaterialTemplateId",
                table: "ProjectMaterialTemplates",
                column: "MaterialTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMaterialTemplates_ProjectId_MaterialTemplateId",
                table: "ProjectMaterialTemplates",
                columns: new[] { "ProjectId", "MaterialTemplateId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BoxTypeMaterialTemplates");

            migrationBuilder.DropTable(
                name: "MaterialTemplateItems");

            migrationBuilder.DropTable(
                name: "ProjectMaterialTemplates");

            migrationBuilder.DropTable(
                name: "MaterialTemplates");
        }
    }
}
