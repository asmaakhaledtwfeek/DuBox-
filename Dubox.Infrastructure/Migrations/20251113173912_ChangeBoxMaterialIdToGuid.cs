using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeBoxMaterialIdToGuid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
              name: "FK_BoxMaterials_Materials_MaterialId",
              table: "BoxMaterials");

            migrationBuilder.DropIndex(
                name: "IX_BoxMaterials_MaterialId",
                table: "BoxMaterials");

            migrationBuilder.DropColumn(
              name: "MaterialId",
              table: "BoxMaterials");

            migrationBuilder.AddColumn<Guid>(
                name: "MaterialId",
                table: "BoxMaterials",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: Guid.Empty);

            migrationBuilder.CreateIndex(
                name: "IX_BoxMaterials_MaterialId",
                table: "BoxMaterials",
                column: "MaterialId");

            migrationBuilder.AddForeignKey(
                name: "FK_BoxMaterials_Materials_MaterialId",
                table: "BoxMaterials",
                column: "MaterialId",
                principalTable: "Materials",
                principalColumn: "MaterialId",
                onDelete: ReferentialAction.Cascade);
        }


        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BoxMaterials_Materials_MaterialId",
                table: "BoxMaterials");

            migrationBuilder.DropIndex(
                name: "IX_BoxMaterials_MaterialId",
                table: "BoxMaterials");

            migrationBuilder.DropColumn(
           name: "MaterialId",
           table: "BoxMaterials");

            migrationBuilder.AddColumn<int>(
                name: "MaterialId",
                table: "BoxMaterials",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BoxMaterials_MaterialId",
                table: "BoxMaterials",
                column: "MaterialId");

            migrationBuilder.AddForeignKey(
                name: "FK_BoxMaterials_Materials_MaterialId",
                table: "BoxMaterials",
                column: "MaterialId",
                principalTable: "Materials",
                principalColumn: "MaterialId",
                onDelete: ReferentialAction.Cascade);
        }


    }
}
