using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSerialNumberToBox : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add SerialNumber column as nullable first to handle existing boxes
            migrationBuilder.AddColumn<string>(
                name: "SerialNumber",
                table: "Boxes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            // Generate serial numbers for existing boxes (format: SN-YYYY-XXXXXX)
            // This will be handled by the application when boxes are accessed/updated
            // For now, we'll leave existing boxes with null SerialNumber
            // New boxes will always have SerialNumber generated

            // Make it required for new boxes (but keep nullable for backward compatibility with existing data)
            // Note: The entity requires it, but we allow null in DB for existing records
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SerialNumber",
                table: "Boxes");
        }
    }
}

