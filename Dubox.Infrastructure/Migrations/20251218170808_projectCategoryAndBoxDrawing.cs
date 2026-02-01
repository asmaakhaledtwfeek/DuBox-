using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class projectCategoryAndBoxDrawing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Location",
                table: "Projects",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Projects",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "Zone",
                table: "Boxes",
                type: "int",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "BoxDrawings",
                columns: table => new
                {
                    BoxDrawingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BoxId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DrawingUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    FileData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OriginalFileName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FileExtension = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FileType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    FileSize = table.Column<long>(type: "bigint", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoxDrawings", x => x.BoxDrawingId);
                    table.ForeignKey(
                        name: "FK_BoxDrawings_Boxes_BoxId",
                        column: x => x.BoxId,
                        principalTable: "Boxes",
                        principalColumn: "BoxId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectTypeCategories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Abbreviation = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTypeCategories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "BoxTypes",
                columns: table => new
                {
                    BoxTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BoxTypeName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Abbreviation = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoxTypes", x => x.BoxTypeId);
                    table.ForeignKey(
                        name: "FK_BoxTypes_ProjectTypeCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "ProjectTypeCategories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BoxSubTypes",
                columns: table => new
                {
                    BoxSubTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BoxSubTypeName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Abbreviation = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    BoxTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoxSubTypes", x => x.BoxSubTypeId);
                    table.ForeignKey(
                        name: "FK_BoxSubTypes_BoxTypes_BoxTypeId",
                        column: x => x.BoxTypeId,
                        principalTable: "BoxTypes",
                        principalColumn: "BoxTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ProjectTypeCategories",
                columns: new[] { "CategoryId", "Abbreviation", "CategoryName" },
                values: new object[,]
                {
                    { 1, "RES", "Residential" },
                    { 2, "COM", "Commercial" },
                    { 3, "IND", "Industrial" },
                    { 4, "INS", "Institutional" },
                    { 5, "HCI", "Heavy Civil/Infrastructure" },
                    { 6, "MXU", "Mixed-Use" },
                    { 7, "SPC", "Specialized Construction" }
                });

            migrationBuilder.InsertData(
                table: "BoxTypes",
                columns: new[] { "BoxTypeId", "Abbreviation", "BoxTypeName", "CategoryId" },
                values: new object[,]
                {
                    { 1, "KIT", "Kitchen", 1 },
                    { 2, "LIV", "Living Room", 1 },
                    { 3, "DIN", "Dining Room", 1 },
                    { 4, "BED", "Bedrooms", 1 },
                    { 5, "BTH", "Bathrooms", 1 },
                    { 6, "LAU", "Laundry Room", 1 },
                    { 7, "GAR", "Garage", 1 },
                    { 8, "BSM", "Basement", 1 },
                    { 9, "ATT", "Attic", 1 },
                    { 10, "ENT", "Entry Hall", 1 },
                    { 11, "CLO", "Closets Rooms", 1 },
                    { 12, "HOM", "Home Office / Study", 1 },
                    { 13, "MUD", "Mudroom", 1 },
                    { 14, "PAN", "Pantry", 1 },
                    { 15, "BAL", "Balcony", 1 },
                    { 16, "UTL", "Utility Room", 1 },
                    { 17, "OFF", "Office Spaces", 2 },
                    { 18, "MTG", "Meeting Rooms", 2 },
                    { 19, "REC", "Reception", 2 },
                    { 20, "SAL", "Sales Floor", 2 },
                    { 21, "DRS", "Dressing Rooms", 2 },
                    { 22, "STK", "Stock Rooms", 2 },
                    { 23, "STF", "Staff Room", 2 },
                    { 24, "RST", "Restrooms", 2 },
                    { 25, "SRV", "Server Room", 2 },
                    { 26, "MAL", "Mail Room", 2 },
                    { 27, "STO", "Storage Areas", 2 },
                    { 28, "LOD", "Loading Dock", 2 },
                    { 29, "PRK", "Parking Garage", 2 },
                    { 30, "RDA", "Restaurant Dining Area", 2 },
                    { 31, "CKT", "Commercial Kitchen", 2 },
                    { 32, "HTL", "Hotel Rooms", 2 },
                    { 33, "FIT", "Fitness Center", 2 },
                    { 34, "ELV", "Elevators", 2 },
                    { 35, "PRD", "Production Floor", 3 },
                    { 36, "ASM", "Assembly Line", 3 },
                    { 37, "QC", "Quality Control", 3 },
                    { 38, "WHS", "Warehouse", 3 },
                    { 39, "LDG", "Loading", 3 },
                    { 40, "MCH", "Machine Shop", 3 },
                    { 41, "MNT", "Maintenance Workshop", 3 },
                    { 42, "CTL", "Control Room", 3 },
                    { 43, "RMS", "Raw Material Storage", 3 },
                    { 44, "FGS", "Finished Goods Storage", 3 },
                    { 45, "CLN", "Cleanroom", 3 },
                    { 46, "CHM", "Chemical Storage", 3 },
                    { 47, "BRK", "Break Room", 3 },
                    { 48, "LCK", "Locker Rooms", 3 },
                    { 49, "SHP", "Shipping", 3 },
                    { 50, "UTL", "Utility Room", 3 },
                    { 51, "OFC", "Office Area", 3 },
                    { 52, "LAB", "Laboratory", 3 },
                    { 53, "SCH", "Schools", 4 },
                    { 54, "HOS", "Hospitals", 4 },
                    { 55, "GOV", "Government Buildings", 4 },
                    { 56, "TOL", "Toll Booths", 5 },
                    { 57, "RST", "Rest Areas", 5 },
                    { 58, "MYD", "Maintenance Yards", 5 },
                    { 59, "ESF", "Equipment Storage Facilities", 5 },
                    { 60, "CCT", "Control Centers", 5 },
                    { 61, "PMP", "Pump Stations", 5 },
                    { 62, "TRT", "Treatment Facilities", 5 },
                    { 63, "TUN", "Tunnel Sections", 5 },
                    { 64, "BIA", "Bridge Inspection Areas", 5 },
                    { 65, "APT", "Airport Terminals", 5 },
                    { 66, "CON", "Concourses", 5 },
                    { 67, "BAG", "Baggage Claim", 5 },
                    { 68, "RWY", "Runways", 5 },
                    { 69, "TRN", "Train Platforms", 5 },
                    { 70, "STN", "Station Houses", 5 },
                    { 71, "RSU", "Residential Units", 6 },
                    { 72, "RTL", "Retail Spaces", 6 },
                    { 73, "OFS", "Office Spaces", 6 },
                    { 74, "RES", "Restaurant Spaces", 6 },
                    { 75, "CMA", "Common Areas", 6 },
                    { 76, "SAS", "Shared Amenity Spaces", 6 },
                    { 77, "PKS", "Parking Structures", 6 },
                    { 78, "TRC", "Transit Connections", 6 },
                    { 79, "GRN", "Green Buildings", 7 },
                    { 80, "CMT", "Communication Towers", 7 }
                });

            migrationBuilder.InsertData(
                table: "BoxSubTypes",
                columns: new[] { "BoxSubTypeId", "Abbreviation", "BoxSubTypeName", "BoxTypeId" },
                values: new object[,]
                {
                    { 1, "MBR", "Master", 4 },
                    { 2, "GBR", "Guest", 4 },
                    { 3, "CBR", "Children's", 4 },
                    { 4, "MBA", "Master", 5 },
                    { 5, "GBA", "Guest", 5 },
                    { 6, "PWD", "Powder Room", 5 },
                    { 7, "CLS", "Classrooms", 53 },
                    { 8, "SCI", "Science Labs", 53 },
                    { 9, "CMP", "Computer Labs", 53 },
                    { 10, "LIB", "Library Center", 53 },
                    { 11, "GYM", "Gymnasium", 53 },
                    { 12, "CAF", "Cafeteria", 53 },
                    { 13, "AUD", "Auditorium", 53 },
                    { 14, "MUS", "Music Room", 53 },
                    { 15, "ART", "Art Room", 53 },
                    { 16, "ADM", "Administrative Offices", 53 },
                    { 17, "NRS", "Nurse's Office", 53 },
                    { 18, "COR", "Corridors", 53 },
                    { 19, "PAT", "Patient Rooms", 54 },
                    { 20, "OR", "Operating Rooms", 54 },
                    { 21, "ER", "Emergency Room", 54 },
                    { 22, "ICU", "Intensive Care Unit", 54 },
                    { 23, "EXM", "Examination Rooms", 54 },
                    { 24, "RAD", "Radiology", 54 },
                    { 25, "LAB", "Laboratory", 54 },
                    { 26, "PHM", "Pharmacy", 54 },
                    { 27, "WAT", "Waiting Rooms", 54 },
                    { 28, "NST", "Nurses' Stations", 54 },
                    { 29, "SRG", "Surgery Recovery", 54 },
                    { 30, "CAF", "Cafeteria", 54 },
                    { 31, "OFF", "Offices", 55 },
                    { 32, "CRT", "Courtrooms", 55 },
                    { 33, "CNC", "Council Chambers", 55 },
                    { 34, "PSA", "Public Service Areas", 55 },
                    { 35, "ARC", "Archive/Records Room", 55 },
                    { 36, "SEC", "Security Checkpoints", 55 },
                    { 37, "GFL", "Ground Floor", 72 },
                    { 38, "MFL", "Mid Floors", 73 },
                    { 39, "SOL", "Solar Panel Arrays", 79 },
                    { 40, "GRF", "Green Roof Areas", 79 },
                    { 41, "RWH", "Rainwater Harvesting Areas", 79 },
                    { 42, "GEO", "Geothermal Equipment Rooms", 79 },
                    { 43, "EQS", "Equipment Shelters", 80 },
                    { 44, "GEN", "Generator Rooms", 80 },
                    { 45, "BAT", "Battery Backup Rooms", 80 },
                    { 46, "ANT", "Antenna Platforms", 80 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_CategoryId",
                table: "Projects",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BoxDrawings_BoxId",
                table: "BoxDrawings",
                column: "BoxId");

            migrationBuilder.CreateIndex(
                name: "IX_BoxSubTypes_BoxTypeId",
                table: "BoxSubTypes",
                column: "BoxTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BoxTypes_CategoryId",
                table: "BoxTypes",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_ProjectTypeCategories_CategoryId",
                table: "Projects",
                column: "CategoryId",
                principalTable: "ProjectTypeCategories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_ProjectTypeCategories_CategoryId",
                table: "Projects");

            migrationBuilder.DropTable(
                name: "BoxDrawings");

            migrationBuilder.DropTable(
                name: "BoxSubTypes");

            migrationBuilder.DropTable(
                name: "BoxTypes");

            migrationBuilder.DropTable(
                name: "ProjectTypeCategories");

            migrationBuilder.DropIndex(
                name: "IX_Projects_CategoryId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Projects");

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Projects",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Zone",
                table: "Boxes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 100,
                oldNullable: true);
        }
    }
}
