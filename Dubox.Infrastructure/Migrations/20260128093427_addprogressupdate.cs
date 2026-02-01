using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addprogressupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PreCastProgress",
                table: "BoxPanels",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "QualityIssueId",
                table: "BoxPanels",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkflowStatus",
                table: "BoxPanels",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "WorkflowStatusBy",
                table: "BoxPanels",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "WorkflowStatusDate",
                table: "BoxPanels",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkflowStatusNotes",
                table: "BoxPanels",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreCastProgress",
                table: "BoxPanels");

            migrationBuilder.DropColumn(
                name: "QualityIssueId",
                table: "BoxPanels");

            migrationBuilder.DropColumn(
                name: "WorkflowStatus",
                table: "BoxPanels");

            migrationBuilder.DropColumn(
                name: "WorkflowStatusBy",
                table: "BoxPanels");

            migrationBuilder.DropColumn(
                name: "WorkflowStatusDate",
                table: "BoxPanels");

            migrationBuilder.DropColumn(
                name: "WorkflowStatusNotes",
                table: "BoxPanels");
        }
    }
}
