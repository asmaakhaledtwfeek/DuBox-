using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameCostCodeToCostCodeMaster : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectCostItems_CostCodes_CostCodeId",
                table: "ProjectCostItems");

            migrationBuilder.DropTable(
                name: "CostCodes");

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectMangerId",
                table: "Projects",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CostCodesMaster",
                columns: table => new
                {
                    CostCodeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CostCodeLevel1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Level1Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CostCodeLevel2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Level2Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CostCodePerCSI = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Level1DescriptionAbbrev = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Level0DescriptionAmana = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SubCategory = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UnitOfMeasure = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UnitRate = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CostCodesMaster", x => x.CostCodeId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ProjectMangerId",
                table: "Projects",
                column: "ProjectMangerId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectCostItems_CostCodesMaster_CostCodeId",
                table: "ProjectCostItems",
                column: "CostCodeId",
                principalTable: "CostCodesMaster",
                principalColumn: "CostCodeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Users_ProjectMangerId",
                table: "Projects",
                column: "ProjectMangerId",
                principalTable: "Users",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectCostItems_CostCodesMaster_CostCodeId",
                table: "ProjectCostItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Users_ProjectMangerId",
                table: "Projects");

            migrationBuilder.DropTable(
                name: "CostCodesMaster");

            migrationBuilder.DropIndex(
                name: "IX_Projects_ProjectMangerId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ProjectMangerId",
                table: "Projects");

            migrationBuilder.CreateTable(
                name: "CostCodes",
                columns: table => new
                {
                    CostCodeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    SubCategory = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UnitOfMeasure = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UnitRate = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CostCodes", x => x.CostCodeId);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectCostItems_CostCodes_CostCodeId",
                table: "ProjectCostItems",
                column: "CostCodeId",
                principalTable: "CostCodes",
                principalColumn: "CostCodeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
