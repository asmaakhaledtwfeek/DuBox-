using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddScheduleActivityTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ScheduleActivities",
                columns: table => new
                {
                    ScheduleActivityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActivityName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ActivityCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PlannedStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PlannedFinishDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActualStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActualFinishDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PercentComplete = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleActivities", x => x.ScheduleActivityId);
                    table.ForeignKey(
                        name: "FK_ScheduleActivities_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId");
                });

            migrationBuilder.CreateTable(
                name: "ScheduleActivityMaterials",
                columns: table => new
                {
                    ScheduleActivityMaterialId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScheduleActivityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaterialName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    MaterialCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleActivityMaterials", x => x.ScheduleActivityMaterialId);
                    table.ForeignKey(
                        name: "FK_ScheduleActivityMaterials_ScheduleActivities_ScheduleActivityId",
                        column: x => x.ScheduleActivityId,
                        principalTable: "ScheduleActivities",
                        principalColumn: "ScheduleActivityId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleActivityTeams",
                columns: table => new
                {
                    ScheduleActivityTeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScheduleActivityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssignedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleActivityTeams", x => x.ScheduleActivityTeamId);
                    table.ForeignKey(
                        name: "FK_ScheduleActivityTeams_ScheduleActivities_ScheduleActivityId",
                        column: x => x.ScheduleActivityId,
                        principalTable: "ScheduleActivities",
                        principalColumn: "ScheduleActivityId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScheduleActivityTeams_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleActivities_ProjectId",
                table: "ScheduleActivities",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleActivityMaterials_ScheduleActivityId",
                table: "ScheduleActivityMaterials",
                column: "ScheduleActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleActivityTeams_ScheduleActivityId",
                table: "ScheduleActivityTeams",
                column: "ScheduleActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleActivityTeams_TeamId",
                table: "ScheduleActivityTeams",
                column: "TeamId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScheduleActivityMaterials");

            migrationBuilder.DropTable(
                name: "ScheduleActivityTeams");

            migrationBuilder.DropTable(
                name: "ScheduleActivities");
        }
    }
}
