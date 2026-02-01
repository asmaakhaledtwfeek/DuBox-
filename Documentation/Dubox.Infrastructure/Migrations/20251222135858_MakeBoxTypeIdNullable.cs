using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MakeBoxTypeIdNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Boxes_BoxTypes_BoxTypeId",
                table: "Boxes");

            migrationBuilder.AlterColumn<int>(
                name: "BoxTypeId",
                table: "Boxes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Boxes_BoxTypes_BoxTypeId",
                table: "Boxes",
                column: "BoxTypeId",
                principalTable: "BoxTypes",
                principalColumn: "BoxTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Boxes_BoxTypes_BoxTypeId",
                table: "Boxes");

            migrationBuilder.AlterColumn<int>(
                name: "BoxTypeId",
                table: "Boxes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Boxes_BoxTypes_BoxTypeId",
                table: "Boxes",
                column: "BoxTypeId",
                principalTable: "BoxTypes",
                principalColumn: "BoxTypeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
