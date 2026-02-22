using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ProjectWatherReport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectWeatherReports",
                columns: table => new
                {
                    ReportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReportDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MinTemperature = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    MaxTemperature = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    PrecipitationProbability = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    PrecipitationAmount = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    WindGust = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsFavorable = table.Column<bool>(type: "bit", nullable: false),
                    AlertMessage = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    QualityIssueCreated = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectWeatherReports");
        }
    }
}
