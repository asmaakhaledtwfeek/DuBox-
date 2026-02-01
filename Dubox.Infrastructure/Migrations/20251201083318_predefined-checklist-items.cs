using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class predefinedchecklistitems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PredefinedItemId",
                table: "WIRChecklistItems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PredefinedChecklistItems",
                columns: table => new
                {
                    PredefinedItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CheckpointDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ReferenceDocument = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Sequence = table.Column<int>(type: "int", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PredefinedChecklistItems", x => x.PredefinedItemId);
                });

            migrationBuilder.InsertData(
                table: "PredefinedChecklistItems",
                columns: new[] { "PredefinedItemId", "Category", "CheckpointDescription", "CreatedDate", "IsActive", "ReferenceDocument", "Sequence" },
                values: new object[,]
                {
                    { new Guid("20000001-0000-0000-0000-000000000001"), "General", "Ensure method statement, material submittal and drawings are approved.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, null, 1 },
                    { new Guid("20000001-0000-0000-0000-000000000002"), "General", "Ensure materials (Gypsum board, cement board, insulation material, supporting system, etc.) are stored under dry, clean, shaded area, away from sunlight and other sources of heat.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, null, 2 },
                    { new Guid("20000001-0000-0000-0000-000000000003"), "General", "Check the color, type, material, fire rating and thickness are as per approved material approval and project requirements.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, null, 3 },
                    { new Guid("20000001-0000-0000-0000-000000000004"), "General", "Verify and record the DCL product conformity certificate for the insulation materials.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, null, 4 },
                    { new Guid("20000001-0000-0000-0000-000000000005"), "Setting Out", "Verify the marking and setting out of the partition walls as per the approved drawings.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, null, 5 },
                    { new Guid("20000001-0000-0000-0000-000000000006"), "Installation Activity", "Verify the completion of the required finishes of the adjacent substrates.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, null, 6 },
                    { new Guid("20000001-0000-0000-0000-000000000007"), "Installation Activity", "Verify the location, spacing and fixation of the supporting grid (vertical and horizontal channel, wall angle, etc.) as per the approved drawings.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, null, 7 },
                    { new Guid("20000001-0000-0000-0000-000000000008"), "Installation Activity", "Verify the fixation of the board (on one side of the supports) as per the approved drawings.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, null, 8 },
                    { new Guid("20000001-0000-0000-0000-000000000009"), "Installation Activity", "Ensure the completion of all embedded MEP and other discipline works prior to closure as per the approved drawings.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, null, 9 },
                    { new Guid("20000001-0000-0000-0000-000000000010"), "Installation Activity", "Ensure additional supports are provided for the wall mounted fixtures as applicable.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, null, 10 },
                    { new Guid("20000001-0000-0000-0000-000000000011"), "Installation Activity", "Verify the installation of insulation works (if applicable) as per the approved drawings.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, null, 11 },
                    { new Guid("20000001-0000-0000-0000-000000000012"), "Installation Activity", "Obtain approval (Civil / MEP) from consultant / Client to proceed with further works.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, null, 12 },
                    { new Guid("20000001-0000-0000-0000-000000000013"), "Installation Activity", "Verify the fixation of the board as per the approved drawings.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, null, 13 },
                    { new Guid("20000001-0000-0000-0000-000000000014"), "Installation Activity", "Ensure the completion of MEP and other discipline works above the false ceiling level and obtain clearance to proceed for further works.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, null, 14 },
                    { new Guid("20000001-0000-0000-0000-000000000015"), "Installation Activity", "Verify the marking, position and alignment of MEP & wall mounted fixtures in the wall as per the approved drawings.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, null, 15 },
                    { new Guid("20000001-0000-0000-0000-000000000016"), "Installation Activity", "Ensure the cutting of gypsum board on the marked locations as per the project requirements.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, null, 16 },
                    { new Guid("20000001-0000-0000-0000-000000000017"), "Installation Activity", "Verify the jointing & taping as per the manufacturer recommendations.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, null, 17 },
                    { new Guid("20000001-0000-0000-0000-000000000018"), "Installation Activity", "Approval obtain from Consultant/Client to proceed with further activities.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, null, 18 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PredefinedChecklistItems");

            migrationBuilder.DropColumn(
                name: "PredefinedItemId",
                table: "WIRChecklistItems");
        }
    }
}
