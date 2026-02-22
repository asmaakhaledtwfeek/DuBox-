using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FreezingCellsEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FreezingCells",
                columns: table => new
                {
                    FreezingCellId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FactorySectionPartId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RowNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FreezingCells", x => x.FreezingCellId);
                    table.ForeignKey(
                        name: "FK_FreezingCells_FactorySectionParts_FactorySectionPartId",
                        column: x => x.FactorySectionPartId,
                        principalTable: "FactorySectionParts",
                        principalColumn: "PartId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FreezingCells_FactorySectionPartId",
                table: "FreezingCells",
                column: "FactorySectionPartId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FreezingCells");
        }
    }
}
