using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addTargetExchangedWithBoxId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ExchangedWithBoxId",
                table: "BoxExchanges",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SecondaryQualityIssueId",
                table: "BoxExchanges",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BoxExchanges_ExchangedWithBoxId",
                table: "BoxExchanges",
                column: "ExchangedWithBoxId");

            migrationBuilder.CreateIndex(
                name: "IX_BoxExchanges_SecondaryQualityIssueId",
                table: "BoxExchanges",
                column: "SecondaryQualityIssueId");

            migrationBuilder.AddForeignKey(
                name: "FK_BoxExchanges_Boxes_ExchangedWithBoxId",
                table: "BoxExchanges",
                column: "ExchangedWithBoxId",
                principalTable: "Boxes",
                principalColumn: "BoxId");

            migrationBuilder.AddForeignKey(
                name: "FK_BoxExchanges_QualityIssues_SecondaryQualityIssueId",
                table: "BoxExchanges",
                column: "SecondaryQualityIssueId",
                principalTable: "QualityIssues",
                principalColumn: "IssueId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BoxExchanges_Boxes_ExchangedWithBoxId",
                table: "BoxExchanges");

            migrationBuilder.DropForeignKey(
                name: "FK_BoxExchanges_QualityIssues_SecondaryQualityIssueId",
                table: "BoxExchanges");

            migrationBuilder.DropIndex(
                name: "IX_BoxExchanges_ExchangedWithBoxId",
                table: "BoxExchanges");

            migrationBuilder.DropIndex(
                name: "IX_BoxExchanges_SecondaryQualityIssueId",
                table: "BoxExchanges");

            migrationBuilder.DropColumn(
                name: "ExchangedWithBoxId",
                table: "BoxExchanges");

            migrationBuilder.DropColumn(
                name: "SecondaryQualityIssueId",
                table: "BoxExchanges");
        }
    }
}
