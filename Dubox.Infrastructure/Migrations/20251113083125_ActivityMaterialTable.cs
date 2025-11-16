using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ActivityMaterialTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaterialTransactions_Materials_MaterialId",
                table: "MaterialTransactions");

            migrationBuilder.DropColumn(
                name: "MaterialsNeeded",
                table: "BoxActivities");

            migrationBuilder.AlterColumn<int>(
                name: "TransactionType",
                table: "MaterialTransactions",
                type: "int",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "BoxActivityId",
                table: "MaterialTransactions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ActivityMaterials",
                columns: table => new
                {
                    ActivityMaterialId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaterialId = table.Column<int>(type: "int", nullable: false),
                    BoxActivityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuantityNeeded = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityMaterials", x => x.ActivityMaterialId);
                    table.ForeignKey(
                        name: "FK_ActivityMaterials_BoxActivities_BoxActivityId",
                        column: x => x.BoxActivityId,
                        principalTable: "BoxActivities",
                        principalColumn: "BoxActivityId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActivityMaterials_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "MaterialId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MaterialTransactions_BoxActivityId",
                table: "MaterialTransactions",
                column: "BoxActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityMaterials_BoxActivityId",
                table: "ActivityMaterials",
                column: "BoxActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityMaterials_MaterialId",
                table: "ActivityMaterials",
                column: "MaterialId");

            migrationBuilder.AddForeignKey(
                name: "FK_MaterialTransactions_BoxActivities_BoxActivityId",
                table: "MaterialTransactions",
                column: "BoxActivityId",
                principalTable: "BoxActivities",
                principalColumn: "BoxActivityId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_MaterialTransactions_Materials_MaterialId",
                table: "MaterialTransactions",
                column: "MaterialId",
                principalTable: "Materials",
                principalColumn: "MaterialId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaterialTransactions_BoxActivities_BoxActivityId",
                table: "MaterialTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_MaterialTransactions_Materials_MaterialId",
                table: "MaterialTransactions");

            migrationBuilder.DropTable(
                name: "ActivityMaterials");

            migrationBuilder.DropIndex(
                name: "IX_MaterialTransactions_BoxActivityId",
                table: "MaterialTransactions");

            migrationBuilder.DropColumn(
                name: "BoxActivityId",
                table: "MaterialTransactions");

            migrationBuilder.AlterColumn<string>(
                name: "TransactionType",
                table: "MaterialTransactions",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaterialsNeeded",
                table: "BoxActivities",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MaterialTransactions_Materials_MaterialId",
                table: "MaterialTransactions",
                column: "MaterialId",
                principalTable: "Materials",
                principalColumn: "MaterialId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
