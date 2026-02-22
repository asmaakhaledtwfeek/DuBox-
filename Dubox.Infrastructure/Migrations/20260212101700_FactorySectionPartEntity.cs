using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FactorySectionPartEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FactorySectionId",
                table: "Boxes",
                type: "uniqueidentifier",
                nullable: true);


            migrationBuilder.CreateTable(
                name: "FactorySectionParts",
                columns: table => new
                {
                    PartId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PartNumber = table.Column<int>(type: "int", nullable: false),
                    PartName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MinRow = table.Column<int>(type: "int", nullable: false),
                    MaxRow = table.Column<int>(type: "int", nullable: false),
                    MinBay = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    MaxBay = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: true),
                    CurrentOccupancy = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FactorySectionParts", x => x.PartId);
                    table.ForeignKey(
                        name: "FK_FactorySectionParts_FactorySections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "FactorySections",
                        principalColumn: "SectionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Boxes_FactorySectionId",
                table: "Boxes",
                column: "FactorySectionId");

            migrationBuilder.CreateIndex(
                name: "IX_FactorySectionParts_SectionId_PartNumber",
                table: "FactorySectionParts",
                columns: new[] { "SectionId", "PartNumber" },
                unique: true);


            migrationBuilder.AddForeignKey(
                name: "FK_Boxes_FactorySections_FactorySectionId",
                table: "Boxes",
                column: "FactorySectionId",
                principalTable: "FactorySections",
                principalColumn: "SectionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Boxes_FactorySections_FactorySectionId",
                table: "Boxes");

            migrationBuilder.DropTable(
                name: "FactorySectionParts");


            migrationBuilder.DropIndex(
                name: "IX_Boxes_FactorySectionId",
                table: "Boxes");

            migrationBuilder.DropColumn(
                name: "FactorySectionId",
                table: "Boxes");
        }
    }
}
