using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addFactoryrange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MaxBay",
                table: "Factories",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxRow",
                table: "Factories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MinBay",
                table: "Factories",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MinRow",
                table: "Factories",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxBay",
                table: "Factories");

            migrationBuilder.DropColumn(
                name: "MaxRow",
                table: "Factories");

            migrationBuilder.DropColumn(
                name: "MinBay",
                table: "Factories");

            migrationBuilder.DropColumn(
                name: "MinRow",
                table: "Factories");
        }
    }
}
