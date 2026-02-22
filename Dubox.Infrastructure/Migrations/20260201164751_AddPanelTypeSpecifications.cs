using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPanelTypeSpecifications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConcreteGrade",
                table: "PanelTypes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CoverMm",
                table: "PanelTypes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmbedsJson",
                table: "PanelTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "VolumeM3",
                table: "PanelTypes",
                type: "decimal(10,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "WeightTon",
                table: "PanelTypes",
                type: "decimal(10,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConcreteGrade",
                table: "PanelTypes");

            migrationBuilder.DropColumn(
                name: "CoverMm",
                table: "PanelTypes");

            migrationBuilder.DropColumn(
                name: "EmbedsJson",
                table: "PanelTypes");

            migrationBuilder.DropColumn(
                name: "VolumeM3",
                table: "PanelTypes");

            migrationBuilder.DropColumn(
                name: "WeightTon",
                table: "PanelTypes");
        }
    }
}
