using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class activityTemplates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ActivityName",
                table: "ScheduleActivities",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "ActivityCode",
                table: "ScheduleActivities",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "ApplicableBoxTypes",
                table: "ScheduleActivities",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DependsOnActivities",
                table: "ScheduleActivities",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EstimatedDurationDays",
                table: "ScheduleActivities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsCustomActivity",
                table: "ScheduleActivities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsWIRCheckpoint",
                table: "ScheduleActivities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "OverallSequence",
                table: "ScheduleActivities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SequenceInStage",
                table: "ScheduleActivities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "SourceActivityMasterId",
                table: "ScheduleActivities",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Stage",
                table: "ScheduleActivities",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "StageNumber",
                table: "ScheduleActivities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "WIRCode",
                table: "ScheduleActivities",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ActivityTemplates",
                columns: table => new
                {
                    ActivityTemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TemplateName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityTemplates", x => x.ActivityTemplateId);
                });

            migrationBuilder.CreateTable(
                name: "ActivityTemplateActivities",
                columns: table => new
                {
                    ActivityTemplateActivityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActivityTemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SourceActivityMasterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsCustomActivity = table.Column<bool>(type: "bit", nullable: false),
                    ActivityCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ActivityName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Stage = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StageNumber = table.Column<int>(type: "int", nullable: false),
                    SequenceInStage = table.Column<int>(type: "int", nullable: false),
                    OverallSequence = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    EstimatedDurationDays = table.Column<int>(type: "int", nullable: false),
                    IsWIRCheckpoint = table.Column<bool>(type: "bit", nullable: false),
                    WIRCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ApplicableBoxTypes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DependsOnActivities = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityTemplateActivities", x => x.ActivityTemplateActivityId);
                    table.ForeignKey(
                        name: "FK_ActivityTemplateActivities_ActivityMaster_SourceActivityMasterId",
                        column: x => x.SourceActivityMasterId,
                        principalTable: "ActivityMaster",
                        principalColumn: "ActivityMasterId");
                    table.ForeignKey(
                        name: "FK_ActivityTemplateActivities_ActivityTemplates_ActivityTemplateId",
                        column: x => x.ActivityTemplateId,
                        principalTable: "ActivityTemplates",
                        principalColumn: "ActivityTemplateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleActivities_SourceActivityMasterId",
                table: "ScheduleActivities",
                column: "SourceActivityMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityTemplateActivities_ActivityTemplateId",
                table: "ActivityTemplateActivities",
                column: "ActivityTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityTemplateActivities_SourceActivityMasterId",
                table: "ActivityTemplateActivities",
                column: "SourceActivityMasterId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduleActivities_ActivityMaster_SourceActivityMasterId",
                table: "ScheduleActivities",
                column: "SourceActivityMasterId",
                principalTable: "ActivityMaster",
                principalColumn: "ActivityMasterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScheduleActivities_ActivityMaster_SourceActivityMasterId",
                table: "ScheduleActivities");

            migrationBuilder.DropTable(
                name: "ActivityTemplateActivities");

            migrationBuilder.DropTable(
                name: "ActivityTemplates");

            migrationBuilder.DropIndex(
                name: "IX_ScheduleActivities_SourceActivityMasterId",
                table: "ScheduleActivities");

            migrationBuilder.DropColumn(
                name: "ApplicableBoxTypes",
                table: "ScheduleActivities");

            migrationBuilder.DropColumn(
                name: "DependsOnActivities",
                table: "ScheduleActivities");

            migrationBuilder.DropColumn(
                name: "EstimatedDurationDays",
                table: "ScheduleActivities");

            migrationBuilder.DropColumn(
                name: "IsCustomActivity",
                table: "ScheduleActivities");

            migrationBuilder.DropColumn(
                name: "IsWIRCheckpoint",
                table: "ScheduleActivities");

            migrationBuilder.DropColumn(
                name: "OverallSequence",
                table: "ScheduleActivities");

            migrationBuilder.DropColumn(
                name: "SequenceInStage",
                table: "ScheduleActivities");

            migrationBuilder.DropColumn(
                name: "SourceActivityMasterId",
                table: "ScheduleActivities");

            migrationBuilder.DropColumn(
                name: "Stage",
                table: "ScheduleActivities");

            migrationBuilder.DropColumn(
                name: "StageNumber",
                table: "ScheduleActivities");

            migrationBuilder.DropColumn(
                name: "WIRCode",
                table: "ScheduleActivities");

            migrationBuilder.AlterColumn<string>(
                name: "ActivityName",
                table: "ScheduleActivities",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "ActivityCode",
                table: "ScheduleActivities",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);
        }
    }
}
