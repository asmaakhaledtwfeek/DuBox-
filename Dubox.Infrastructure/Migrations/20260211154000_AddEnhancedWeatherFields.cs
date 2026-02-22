using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEnhancedWeatherFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add new temperature field
            migrationBuilder.AddColumn<decimal>(
                name: "CurrentTemperature",
                table: "ProjectWeatherReports",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m);

            // Add humidity field
            migrationBuilder.AddColumn<decimal>(
                name: "Humidity",
                table: "ProjectWeatherReports",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m);

            // Add wind fields
            migrationBuilder.AddColumn<decimal>(
                name: "WindSpeed",
                table: "ProjectWeatherReports",
                type: "decimal(6,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "WindDirection",
                table: "ProjectWeatherReports",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m);

            // Add pressure field
            migrationBuilder.AddColumn<decimal>(
                name: "Pressure",
                table: "ProjectWeatherReports",
                type: "decimal(7,2)",
                nullable: false,
                defaultValue: 0m);

            // Add sun and moon fields
            migrationBuilder.AddColumn<DateTime>(
                name: "Sunrise",
                table: "ProjectWeatherReports",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Sunset",
                table: "ProjectWeatherReports",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Moonrise",
                table: "ProjectWeatherReports",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Moonset",
                table: "ProjectWeatherReports",
                type: "datetime2",
                nullable: true);

            // Add location fields
            migrationBuilder.AddColumn<decimal>(
                name: "Latitude",
                table: "ProjectWeatherReports",
                type: "decimal(9,6)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Longitude",
                table: "ProjectWeatherReports",
                type: "decimal(9,6)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Elevation",
                table: "ProjectWeatherReports",
                type: "decimal(7,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentTemperature",
                table: "ProjectWeatherReports");

            migrationBuilder.DropColumn(
                name: "Humidity",
                table: "ProjectWeatherReports");

            migrationBuilder.DropColumn(
                name: "WindSpeed",
                table: "ProjectWeatherReports");

            migrationBuilder.DropColumn(
                name: "WindDirection",
                table: "ProjectWeatherReports");

            migrationBuilder.DropColumn(
                name: "Pressure",
                table: "ProjectWeatherReports");

            migrationBuilder.DropColumn(
                name: "Sunrise",
                table: "ProjectWeatherReports");

            migrationBuilder.DropColumn(
                name: "Sunset",
                table: "ProjectWeatherReports");

            migrationBuilder.DropColumn(
                name: "Moonrise",
                table: "ProjectWeatherReports");

            migrationBuilder.DropColumn(
                name: "Moonset",
                table: "ProjectWeatherReports");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "ProjectWeatherReports");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "ProjectWeatherReports");

            migrationBuilder.DropColumn(
                name: "Elevation",
                table: "ProjectWeatherReports");
        }
    }
}
