using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class factorySectionInWIR : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FactorySectionId",
                table: "WIRRecords",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WIRRecords_FactorySectionId",
                table: "WIRRecords",
                column: "FactorySectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_WIRRecords_FactorySections_FactorySectionId",
                table: "WIRRecords",
                column: "FactorySectionId",
                principalTable: "FactorySections",
                principalColumn: "SectionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WIRRecords_FactorySections_FactorySectionId",
                table: "WIRRecords");

            migrationBuilder.DropIndex(
                name: "IX_WIRRecords_FactorySectionId",
                table: "WIRRecords");

            migrationBuilder.DropColumn(
                name: "FactorySectionId",
                table: "WIRRecords");
        }
    }
}
