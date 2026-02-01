using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removeQRCodeInformation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Boxes_QRCodeString",
                table: "Boxes");

            migrationBuilder.DropColumn(
                name: "QRCodeImageUrl",
                table: "Boxes");

            migrationBuilder.DropColumn(
                name: "QRCodeString",
                table: "Boxes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "QRCodeImageUrl",
                table: "Boxes",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QRCodeString",
                table: "Boxes",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Boxes_QRCodeString",
                table: "Boxes",
                column: "QRCodeString",
                unique: true);
        }
    }
}
