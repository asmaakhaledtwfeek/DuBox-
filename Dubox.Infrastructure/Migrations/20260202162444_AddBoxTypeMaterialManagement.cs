using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable


#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBoxTypeMaterialManagement : Migration

    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BoxTypeMaterials",
                columns: table => new
                {
                    BoxTypeMaterialId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectBoxTypeId = table.Column<int>(type: "int", nullable: false),
                    MaterialId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequiredBeforeDays = table.Column<int>(type: "int", nullable: false),
                    IsArrived = table.Column<bool>(type: "bit", nullable: false),
                    ArrivedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ArrivedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ArrivedQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoxTypeMaterials", x => x.BoxTypeMaterialId);
                    table.ForeignKey(
                        name: "FK_BoxTypeMaterials_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "MaterialId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BoxTypeMaterials_ProjectBoxTypes_ProjectBoxTypeId",
                        column: x => x.ProjectBoxTypeId,
                        principalTable: "ProjectBoxTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BoxTypeMaterials_Users_ArrivedBy",
                        column: x => x.ArrivedBy,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "PermissionId", "Action", "Category", "CreatedDate", "Description", "DisplayName", "DisplayOrder", "IsActive", "Module", "PermissionKey" },
                values: new object[,]
                {
                    { new Guid("150c9a72-2751-4402-a9ec-34918d631576"), "Delete", "Materials", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Delete material templates", "Delete Material Templates", 50, true, "MaterialTemplates", "materialtemplates.delete" },
                    { new Guid("1f7179dc-dab9-42a9-80c3-4f88bff776dc"), "Edit", "Materials", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Edit existing material templates", "Edit Material Templates", 49, true, "MaterialTemplates", "materialtemplates.edit" },
                    { new Guid("65f1415f-4fe7-4386-b99d-eba8b2585057"), "Manage", "Materials", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Full management of material templates", "Manage Material Templates", 51, true, "MaterialTemplates", "materialtemplates.manage" },
                    { new Guid("74a8cd42-32b1-434d-891c-dfe5e0a3fd62"), "Create", "Materials", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Create new material templates", "Create Material Templates", 48, true, "MaterialTemplates", "materialtemplates.create" },
                    { new Guid("a833003b-b0cd-41a9-934e-86e9fe604890"), "View", "Materials", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "View material template list and details", "View Material Templates", 47, true, "MaterialTemplates", "materialtemplates.view" }
                });

            
          
            migrationBuilder.CreateIndex(
                name: "IX_BoxTypeMaterials_ArrivedBy",
                table: "BoxTypeMaterials",
                column: "ArrivedBy");

            migrationBuilder.CreateIndex(
                name: "IX_BoxTypeMaterials_IsArrived",
                table: "BoxTypeMaterials",
                column: "IsArrived");

            migrationBuilder.CreateIndex(
                name: "IX_BoxTypeMaterials_MaterialId",
                table: "BoxTypeMaterials",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_BoxTypeMaterials_ProjectBoxTypeId",
                table: "BoxTypeMaterials",
                column: "ProjectBoxTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BoxTypeMaterials");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: new Guid("150c9a72-2751-4402-a9ec-34918d631576"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: new Guid("1f7179dc-dab9-42a9-80c3-4f88bff776dc"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: new Guid("65f1415f-4fe7-4386-b99d-eba8b2585057"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: new Guid("74a8cd42-32b1-434d-891c-dfe5e0a3fd62"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: new Guid("a833003b-b0cd-41a9-934e-86e9fe604890"));

       
        }
    }
}
