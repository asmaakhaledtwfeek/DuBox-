using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedHrcCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Currency",
                table: "HRCostRecords");

            migrationBuilder.DropColumn(
                name: "DailyRate",
                table: "HRCostRecords");

            migrationBuilder.DropColumn(
                name: "HourlyRate",
                table: "HRCostRecords");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "HRCostRecords");

            migrationBuilder.DropColumn(
                name: "MonthlyRate",
                table: "HRCostRecords");

            migrationBuilder.DropColumn(
                name: "OvertimeRate",
                table: "HRCostRecords");

            migrationBuilder.RenameColumn(
                name: "Trade",
                table: "HRCostRecords",
                newName: "SubClassification");

            migrationBuilder.RenameColumn(
                name: "Position",
                table: "HRCostRecords",
                newName: "SubChapter");

            migrationBuilder.RenameColumn(
                name: "CostType",
                table: "HRCostRecords",
                newName: "Type");

            migrationBuilder.AddColumn<string>(
                name: "Chapter",
                table: "ProjectCosts",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Classification",
                table: "ProjectCosts",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CostCodeId",
                table: "ProjectCosts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CostCodeLevel1",
                table: "ProjectCosts",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CostCodeLevel2",
                table: "ProjectCosts",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CostCodeLevel3",
                table: "ProjectCosts",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubChapter",
                table: "ProjectCosts",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubClassification",
                table: "ProjectCosts",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "ProjectCosts",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Units",
                table: "ProjectCosts",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BudgetLevel",
                table: "HRCostRecords",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Chapter",
                table: "HRCostRecords",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Classification",
                table: "HRCostRecords",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IDLAccount",
                table: "HRCostRecords",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Job",
                table: "HRCostRecords",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JobCostAccount",
                table: "HRCostRecords",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OfficeAccount",
                table: "HRCostRecords",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SpecialAccount",
                table: "HRCostRecords",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "HRCostRecords",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectCosts_CostCodeId",
                table: "ProjectCosts",
                column: "CostCodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectCosts_CostCodesMaster_CostCodeId",
                table: "ProjectCosts",
                column: "CostCodeId",
                principalTable: "CostCodesMaster",
                principalColumn: "CostCodeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectCosts_CostCodesMaster_CostCodeId",
                table: "ProjectCosts");

            migrationBuilder.DropIndex(
                name: "IX_ProjectCosts_CostCodeId",
                table: "ProjectCosts");

            migrationBuilder.DropColumn(
                name: "Chapter",
                table: "ProjectCosts");

            migrationBuilder.DropColumn(
                name: "Classification",
                table: "ProjectCosts");

            migrationBuilder.DropColumn(
                name: "CostCodeId",
                table: "ProjectCosts");

            migrationBuilder.DropColumn(
                name: "CostCodeLevel1",
                table: "ProjectCosts");

            migrationBuilder.DropColumn(
                name: "CostCodeLevel2",
                table: "ProjectCosts");

            migrationBuilder.DropColumn(
                name: "CostCodeLevel3",
                table: "ProjectCosts");

            migrationBuilder.DropColumn(
                name: "SubChapter",
                table: "ProjectCosts");

            migrationBuilder.DropColumn(
                name: "SubClassification",
                table: "ProjectCosts");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "ProjectCosts");

            migrationBuilder.DropColumn(
                name: "Units",
                table: "ProjectCosts");

            migrationBuilder.DropColumn(
                name: "BudgetLevel",
                table: "HRCostRecords");

            migrationBuilder.DropColumn(
                name: "Chapter",
                table: "HRCostRecords");

            migrationBuilder.DropColumn(
                name: "Classification",
                table: "HRCostRecords");

            migrationBuilder.DropColumn(
                name: "IDLAccount",
                table: "HRCostRecords");

            migrationBuilder.DropColumn(
                name: "Job",
                table: "HRCostRecords");

            migrationBuilder.DropColumn(
                name: "JobCostAccount",
                table: "HRCostRecords");

            migrationBuilder.DropColumn(
                name: "OfficeAccount",
                table: "HRCostRecords");

            migrationBuilder.DropColumn(
                name: "SpecialAccount",
                table: "HRCostRecords");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "HRCostRecords");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "HRCostRecords",
                newName: "CostType");

            migrationBuilder.RenameColumn(
                name: "SubClassification",
                table: "HRCostRecords",
                newName: "Trade");

            migrationBuilder.RenameColumn(
                name: "SubChapter",
                table: "HRCostRecords",
                newName: "Position");

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "HRCostRecords",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "DailyRate",
                table: "HRCostRecords",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "HourlyRate",
                table: "HRCostRecords",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "HRCostRecords",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "MonthlyRate",
                table: "HRCostRecords",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "OvertimeRate",
                table: "HRCostRecords",
                type: "decimal(18,2)",
                nullable: true);
        }
    }
}
