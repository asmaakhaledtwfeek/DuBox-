using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateProjectCost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "HRCostRecordId",
                table: "ProjectCosts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectCosts_HRCostRecordId",
                table: "ProjectCosts",
                column: "HRCostRecordId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectCosts_HRCostRecords_HRCostRecordId",
                table: "ProjectCosts",
                column: "HRCostRecordId",
                principalTable: "HRCostRecords",
                principalColumn: "HRCostRecordId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectCosts_HRCostRecords_HRCostRecordId",
                table: "ProjectCosts");

            migrationBuilder.DropIndex(
                name: "IX_ProjectCosts_HRCostRecordId",
                table: "ProjectCosts");

            migrationBuilder.DropColumn(
                name: "HRCostRecordId",
                table: "ProjectCosts");
        }
    }
}
