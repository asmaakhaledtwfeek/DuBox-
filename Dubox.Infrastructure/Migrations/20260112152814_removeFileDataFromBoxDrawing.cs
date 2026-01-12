using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removeFileDataFromBoxDrawing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FileData",
                table: "BoxDrawings",
                newName: "DrawingFileName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DrawingFileName",
                table: "BoxDrawings",
                newName: "FileData");
        }
    }
}
