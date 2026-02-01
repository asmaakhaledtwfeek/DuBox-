using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSlabAndSoffitToBox : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Slab",
                table: "Boxes",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Soffit",
                table: "Boxes",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Slab",
                table: "Boxes");

            migrationBuilder.DropColumn(
                name: "Soffit",
                table: "Boxes");
        }
    }
}
