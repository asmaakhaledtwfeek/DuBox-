using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPanelStageCompletionFlags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "MoldPreparationComplete",
                table: "BoxPanels",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ReinforcementSetupComplete",
                table: "BoxPanels",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ConcreteCastingComplete",
                table: "BoxPanels",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CuringAndDemoldingComplete",
                table: "BoxPanels",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MoldPreparationComplete",
                table: "BoxPanels");

            migrationBuilder.DropColumn(
                name: "ReinforcementSetupComplete",
                table: "BoxPanels");

            migrationBuilder.DropColumn(
                name: "ConcreteCastingComplete",
                table: "BoxPanels");

            migrationBuilder.DropColumn(
                name: "CuringAndDemoldingComplete",
                table: "BoxPanels");
        }
    }
}
