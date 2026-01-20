using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removeBoxWalls : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Wall1",
                table: "Boxes");

            migrationBuilder.DropColumn(
                name: "Wall2",
                table: "Boxes");

            migrationBuilder.DropColumn(
                name: "Wall3",
                table: "Boxes");

            migrationBuilder.DropColumn(
                name: "Wall4",
                table: "Boxes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Wall1",
                table: "Boxes",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Wall2",
                table: "Boxes",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Wall3",
                table: "Boxes",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Wall4",
                table: "Boxes",
                type: "bit",
                nullable: true);
        }
    }
}
