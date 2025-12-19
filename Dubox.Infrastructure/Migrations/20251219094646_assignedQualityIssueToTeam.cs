using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class assignedQualityIssueToTeam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "AssignedTo",
                table: "QualityIssues",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedBy",
                table: "QualityIssues",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_QualityIssues_AssignedTo",
                table: "QualityIssues",
                column: "AssignedTo");

            migrationBuilder.AddForeignKey(
                name: "FK_QualityIssues_Teams_AssignedTo",
                table: "QualityIssues",
                column: "AssignedTo",
                principalTable: "Teams",
                principalColumn: "TeamId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QualityIssues_Teams_AssignedTo",
                table: "QualityIssues");

            migrationBuilder.DropIndex(
                name: "IX_QualityIssues_AssignedTo",
                table: "QualityIssues");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "QualityIssues");

            migrationBuilder.AlterColumn<string>(
                name: "AssignedTo",
                table: "QualityIssues",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);
        }
    }
}
