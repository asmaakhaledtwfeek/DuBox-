using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changePhotoUrlToBase64 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoUrls",
                table: "WIRRecords");

            migrationBuilder.DropColumn(
                name: "AttachmentPath",
                table: "WIRCheckpoints");

            migrationBuilder.DropColumn(
                name: "PhotoPath",
                table: "QualityIssues");

            migrationBuilder.DropColumn(
                name: "PhotoUrls",
                table: "ProgressUpdates");

            migrationBuilder.AddColumn<string>(
                name: "Photo",
                table: "WIRRecords",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Photo",
                table: "WIRCheckpoints",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Photo",
                table: "QualityIssues",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Photo",
                table: "ProgressUpdates",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Photo",
                table: "WIRRecords");

            migrationBuilder.DropColumn(
                name: "Photo",
                table: "WIRCheckpoints");

            migrationBuilder.DropColumn(
                name: "Photo",
                table: "QualityIssues");

            migrationBuilder.DropColumn(
                name: "Photo",
                table: "ProgressUpdates");

            migrationBuilder.AddColumn<string>(
                name: "PhotoUrls",
                table: "WIRRecords",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AttachmentPath",
                table: "WIRCheckpoints",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhotoPath",
                table: "QualityIssues",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhotoUrls",
                table: "ProgressUpdates",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);
        }
    }
}
