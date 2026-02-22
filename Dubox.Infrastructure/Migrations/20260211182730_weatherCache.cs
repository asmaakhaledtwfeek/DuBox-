using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class weatherCache : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectWeatherReports");

            migrationBuilder.CreateTable(
                name: "WeatherDataCaches",
                columns: table => new
                {
                    CacheId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CurrentTemperature = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    MinTemperature = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    MaxTemperature = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Humidity = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    PrecipitationProbability = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    PrecipitationAmount = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    WindSpeed = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    WindGust = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    WindDirection = table.Column<decimal>(type: "decimal(18,2)", maxLength: 50, nullable: false),
                    Pressure = table.Column<decimal>(type: "decimal(7,2)", nullable: false),
                    SolarRadiation = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    Sunrise = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Sunset = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Moonrise = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Moonset = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Latitude = table.Column<decimal>(type: "decimal(10,7)", nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(10,7)", nullable: false),
                    Elevation = table.Column<decimal>(type: "decimal(7,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherDataCaches", x => x.CacheId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeatherDataCaches");

            migrationBuilder.CreateTable(
                name: "ProjectWeatherReports",
                columns: table => new
                {
                    ReportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AlertMessage = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CurrentTemperature = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Elevation = table.Column<decimal>(type: "decimal(7,2)", nullable: false),
                    Humidity = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    IsFavorable = table.Column<bool>(type: "bit", nullable: false),
                    Latitude = table.Column<decimal>(type: "decimal(9,6)", nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(9,6)", nullable: false),
                    MaxTemperature = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    MinTemperature = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Moonrise = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Moonset = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PrecipitationAmount = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    PrecipitationProbability = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Pressure = table.Column<decimal>(type: "decimal(7,2)", nullable: false),
                    QualityIssueCreated = table.Column<bool>(type: "bit", nullable: false),
                    ReportDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SolarRadiation = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    Sunrise = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Sunset = table.Column<DateTime>(type: "datetime2", nullable: true),
                    WindDirection = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    WindGust = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    WindSpeed = table.Column<decimal>(type: "decimal(6,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectWeatherReports", x => x.ReportId);
                    table.ForeignKey(
                        name: "FK_ProjectWeatherReports_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectWeatherReports_ProjectId",
                table: "ProjectWeatherReports",
                column: "ProjectId");
        }
    }
}
