using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class boxPanelsStages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "InitialComplete",
                table: "BoxPanels",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "InitialDate",
                table: "BoxPanels",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "MEPInsertsInstallationComplete",
                table: "BoxPanels",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "MEPInsertsInstallationDate",
                table: "BoxPanels",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SurfaceFinishingComplete",
                table: "BoxPanels",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "SurfaceFinishingDate",
                table: "BoxPanels",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InitialComplete",
                table: "BoxPanels");

            migrationBuilder.DropColumn(
                name: "InitialDate",
                table: "BoxPanels");

            migrationBuilder.DropColumn(
                name: "MEPInsertsInstallationComplete",
                table: "BoxPanels");

            migrationBuilder.DropColumn(
                name: "MEPInsertsInstallationDate",
                table: "BoxPanels");

            migrationBuilder.DropColumn(
                name: "SurfaceFinishingComplete",
                table: "BoxPanels");

            migrationBuilder.DropColumn(
                name: "SurfaceFinishingDate",
                table: "BoxPanels");
        }
    }
}
