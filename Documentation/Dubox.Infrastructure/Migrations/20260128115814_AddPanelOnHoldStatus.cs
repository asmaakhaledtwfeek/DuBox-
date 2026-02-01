using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPanelOnHoldStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreCastProgress",
                table: "BoxPanels");

            migrationBuilder.AddColumn<bool>(
                name: "ConcreteCastingComplete",
                table: "BoxPanels",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ConcreteCastingDate",
                table: "BoxPanels",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "CuringAndDemoldingComplete",
                table: "BoxPanels",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "CuringAndDemoldingDate",
                table: "BoxPanels",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurrentStage",
                table: "BoxPanels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "MoldPreparationComplete",
                table: "BoxPanels",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "MoldPreparationDate",
                table: "BoxPanels",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ReinforcementSetupComplete",
                table: "BoxPanels",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReinforcementSetupDate",
                table: "BoxPanels",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConcreteCastingComplete",
                table: "BoxPanels");

            migrationBuilder.DropColumn(
                name: "ConcreteCastingDate",
                table: "BoxPanels");

            migrationBuilder.DropColumn(
                name: "CuringAndDemoldingComplete",
                table: "BoxPanels");

            migrationBuilder.DropColumn(
                name: "CuringAndDemoldingDate",
                table: "BoxPanels");

            migrationBuilder.DropColumn(
                name: "CurrentStage",
                table: "BoxPanels");

            migrationBuilder.DropColumn(
                name: "MoldPreparationComplete",
                table: "BoxPanels");

            migrationBuilder.DropColumn(
                name: "MoldPreparationDate",
                table: "BoxPanels");

            migrationBuilder.DropColumn(
                name: "ReinforcementSetupComplete",
                table: "BoxPanels");

            migrationBuilder.DropColumn(
                name: "ReinforcementSetupDate",
                table: "BoxPanels");

            migrationBuilder.AddColumn<decimal>(
                name: "PreCastProgress",
                table: "BoxPanels",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
