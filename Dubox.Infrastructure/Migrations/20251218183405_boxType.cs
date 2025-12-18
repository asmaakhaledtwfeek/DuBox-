using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class boxType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BoxType",
                table: "Boxes");

            migrationBuilder.AddColumn<int>(
                name: "BoxSubTypeId",
                table: "Boxes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BoxTypeId",
                table: "Boxes",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Boxes_BoxSubTypeId",
                table: "Boxes",
                column: "BoxSubTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Boxes_BoxTypeId",
                table: "Boxes",
                column: "BoxTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Boxes_BoxSubTypes_BoxSubTypeId",
                table: "Boxes",
                column: "BoxSubTypeId",
                principalTable: "BoxSubTypes",
                principalColumn: "BoxSubTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Boxes_BoxTypes_BoxTypeId",
                table: "Boxes",
                column: "BoxTypeId",
                principalTable: "BoxTypes",
                principalColumn: "BoxTypeId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Boxes_BoxSubTypes_BoxSubTypeId",
                table: "Boxes");

            migrationBuilder.DropForeignKey(
                name: "FK_Boxes_BoxTypes_BoxTypeId",
                table: "Boxes");

            migrationBuilder.DropIndex(
                name: "IX_Boxes_BoxSubTypeId",
                table: "Boxes");

            migrationBuilder.DropIndex(
                name: "IX_Boxes_BoxTypeId",
                table: "Boxes");

            migrationBuilder.DropColumn(
                name: "BoxSubTypeId",
                table: "Boxes");

            migrationBuilder.DropColumn(
                name: "BoxTypeId",
                table: "Boxes");

            migrationBuilder.AddColumn<string>(
                name: "BoxType",
                table: "Boxes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
