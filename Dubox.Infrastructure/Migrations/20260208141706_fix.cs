using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QuantityBerBox",
                table: "Materials",
                newName: "QuantityPerBox");

            migrationBuilder.RenameColumn(
                name: "QuantityBerBox",
                table: "BoxTypeMaterials",
                newName: "QuantityPerBox");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QuantityPerBox",
                table: "Materials",
                newName: "QuantityBerBox");

            migrationBuilder.RenameColumn(
                name: "QuantityPerBox",
                table: "BoxTypeMaterials",
                newName: "QuantityBerBox");
        }
    }
}
