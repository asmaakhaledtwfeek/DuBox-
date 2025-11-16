using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBoxMaterialIdToGuid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BoxMaterials",
                table: "BoxMaterials");

            migrationBuilder.DropColumn(
                name: "BoxMaterialId",
                table: "BoxMaterials");

            migrationBuilder.AddColumn<Guid>(
                name: "BoxMaterialId",
                table: "BoxMaterials",
                nullable: false,
                defaultValueSql: "NEWSEQUENTIALID()");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BoxMaterials",
                table: "BoxMaterials",
                column: "BoxMaterialId");
        }



        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BoxMaterials",
                table: "BoxMaterials");

            migrationBuilder.DropColumn(
                name: "BoxMaterialId",
                table: "BoxMaterials");

            migrationBuilder.AddColumn<int>(
                name: "BoxMaterialId",
                table: "BoxMaterials",
                type: "int",
                nullable: false)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BoxMaterials",
                table: "BoxMaterials",
                column: "BoxMaterialId");
        }
    }
}
