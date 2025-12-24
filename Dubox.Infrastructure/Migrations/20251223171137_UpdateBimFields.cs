using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBimFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BIMModelReference",
                table: "Boxes");

            migrationBuilder.AddColumn<string>(
                name: "BimLink",
                table: "Projects",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BimLink",
                table: "Projects");

            migrationBuilder.AddColumn<string>(
                name: "BIMModelReference",
                table: "Boxes",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }
    }
}
