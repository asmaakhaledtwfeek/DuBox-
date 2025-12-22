using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceBoxLetterWithBoxFunction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BoxLetter",
                table: "Boxes");

            migrationBuilder.AddColumn<string>(
                name: "BoxFunction",
                table: "Boxes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BoxFunction",
                table: "Boxes");

            migrationBuilder.AddColumn<string>(
                name: "BoxLetter",
                table: "Boxes",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);
        }
    }
}
