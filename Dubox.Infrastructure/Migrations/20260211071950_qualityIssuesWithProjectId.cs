using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class qualityIssuesWithProjectId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QualityIssues_Boxes_BoxId",
                table: "QualityIssues");

            migrationBuilder.AlterColumn<Guid>(
                name: "BoxId",
                table: "QualityIssues",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "QualityIssues",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_QualityIssues_ProjectId",
                table: "QualityIssues",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_QualityIssues_Boxes_BoxId",
                table: "QualityIssues",
                column: "BoxId",
                principalTable: "Boxes",
                principalColumn: "BoxId");

            migrationBuilder.AddForeignKey(
                name: "FK_QualityIssues_Projects_ProjectId",
                table: "QualityIssues",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "ProjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QualityIssues_Boxes_BoxId",
                table: "QualityIssues");

            migrationBuilder.DropForeignKey(
                name: "FK_QualityIssues_Projects_ProjectId",
                table: "QualityIssues");

            migrationBuilder.DropIndex(
                name: "IX_QualityIssues_ProjectId",
                table: "QualityIssues");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "QualityIssues");

            migrationBuilder.AlterColumn<Guid>(
                name: "BoxId",
                table: "QualityIssues",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_QualityIssues_Boxes_BoxId",
                table: "QualityIssues",
                column: "BoxId",
                principalTable: "Boxes",
                principalColumn: "BoxId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
