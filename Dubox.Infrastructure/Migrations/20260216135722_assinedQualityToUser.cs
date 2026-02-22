using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class assinedQualityToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AssignedUserId",
                table: "QualityIssues",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_QualityIssues_AssignedUserId",
                table: "QualityIssues",
                column: "AssignedUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_QualityIssues_Users_AssignedUserId",
                table: "QualityIssues",
                column: "AssignedUserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QualityIssues_Users_AssignedUserId",
                table: "QualityIssues");

            migrationBuilder.DropIndex(
                name: "IX_QualityIssues_AssignedUserId",
                table: "QualityIssues");

            migrationBuilder.DropColumn(
                name: "AssignedUserId",
                table: "QualityIssues");
        }
    }
}
