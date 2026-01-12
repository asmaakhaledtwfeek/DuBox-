using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removeImageData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "WIRCheckpointImages");

            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "QualityIssueImages");

            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "ProgressUpdateImages");

            migrationBuilder.AddColumn<string>(
                name: "ImageFileName",
                table: "WIRCheckpointImages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageFileName",
                table: "QualityIssueImages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageFileName",
                table: "ProgressUpdateImages",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageFileName",
                table: "WIRCheckpointImages");

            migrationBuilder.DropColumn(
                name: "ImageFileName",
                table: "QualityIssueImages");

            migrationBuilder.DropColumn(
                name: "ImageFileName",
                table: "ProgressUpdateImages");

            migrationBuilder.AddColumn<string>(
                name: "ImageData",
                table: "WIRCheckpointImages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageData",
                table: "QualityIssueImages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageData",
                table: "ProgressUpdateImages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
