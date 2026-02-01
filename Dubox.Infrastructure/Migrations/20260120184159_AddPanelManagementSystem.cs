using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPanelManagementSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ActualArrivalDate",
                table: "BoxPanels",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Barcode",
                table: "BoxPanels",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrentLocationStatus",
                table: "BoxPanels",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryNoteNumber",
                table: "BoxPanels",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryNoteUrl",
                table: "BoxPanels",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Dimensions",
                table: "BoxPanels",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DispatchedDate",
                table: "BoxPanels",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EstimatedArrivalDate",
                table: "BoxPanels",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "FirstApprovalBy",
                table: "BoxPanels",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FirstApprovalDate",
                table: "BoxPanels",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstApprovalNotes",
                table: "BoxPanels",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstApprovalStatus",
                table: "BoxPanels",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "InstalledDate",
                table: "BoxPanels",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ManufacturedDate",
                table: "BoxPanels",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ManufacturerName",
                table: "BoxPanels",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "BoxPanels",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PanelDeliveryNoteDeliveryNoteId",
                table: "BoxPanels",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PanelTypeId",
                table: "BoxPanels",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QRCodeUrl",
                table: "BoxPanels",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ScannedAtFactory",
                table: "BoxPanels",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SecondApprovalBy",
                table: "BoxPanels",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SecondApprovalDate",
                table: "BoxPanels",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecondApprovalNotes",
                table: "BoxPanels",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecondApprovalStatus",
                table: "BoxPanels",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Weight",
                table: "BoxPanels",
                type: "decimal(10,2)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PanelDeliveryNotes",
                columns: table => new
                {
                    DeliveryNoteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeliveryNoteNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FactoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SupplierName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DriverName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    VehicleNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    QRCodeUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DocumentUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PanelDeliveryNotes", x => x.DeliveryNoteId);
                    table.ForeignKey(
                        name: "FK_PanelDeliveryNotes_Factories_FactoryId",
                        column: x => x.FactoryId,
                        principalTable: "Factories",
                        principalColumn: "FactoryId");
                    table.ForeignKey(
                        name: "FK_PanelDeliveryNotes_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PanelScanLogs",
                columns: table => new
                {
                    ScanLogId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BoxPanelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Barcode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ScanType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ScanLocation = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ScannedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ScannedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Latitude = table.Column<decimal>(type: "decimal(10,8)", nullable: true),
                    Longitude = table.Column<decimal>(type: "decimal(11,8)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PanelScanLogs", x => x.ScanLogId);
                    table.ForeignKey(
                        name: "FK_PanelScanLogs_BoxPanels_BoxPanelId",
                        column: x => x.BoxPanelId,
                        principalTable: "BoxPanels",
                        principalColumn: "BoxPanelId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PanelTypes",
                columns: table => new
                {
                    PanelTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PanelTypeName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PanelTypeCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PanelTypes", x => x.PanelTypeId);
                    table.ForeignKey(
                        name: "FK_PanelTypes_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BoxPanels_PanelDeliveryNoteDeliveryNoteId",
                table: "BoxPanels",
                column: "PanelDeliveryNoteDeliveryNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_BoxPanels_PanelTypeId",
                table: "BoxPanels",
                column: "PanelTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PanelDeliveryNotes_FactoryId",
                table: "PanelDeliveryNotes",
                column: "FactoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PanelDeliveryNotes_ProjectId",
                table: "PanelDeliveryNotes",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_PanelScanLogs_BoxPanelId",
                table: "PanelScanLogs",
                column: "BoxPanelId");

            migrationBuilder.CreateIndex(
                name: "IX_PanelTypes_ProjectId",
                table: "PanelTypes",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_BoxPanels_PanelDeliveryNotes_PanelDeliveryNoteDeliveryNoteId",
                table: "BoxPanels",
                column: "PanelDeliveryNoteDeliveryNoteId",
                principalTable: "PanelDeliveryNotes",
                principalColumn: "DeliveryNoteId");

            migrationBuilder.AddForeignKey(
                name: "FK_BoxPanels_PanelTypes_PanelTypeId",
                table: "BoxPanels",
                column: "PanelTypeId",
                principalTable: "PanelTypes",
                principalColumn: "PanelTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BoxPanels_PanelDeliveryNotes_PanelDeliveryNoteDeliveryNoteId",
                table: "BoxPanels");

            migrationBuilder.DropForeignKey(
                name: "FK_BoxPanels_PanelTypes_PanelTypeId",
                table: "BoxPanels");

            migrationBuilder.DropTable(
                name: "PanelDeliveryNotes");

            migrationBuilder.DropTable(
                name: "PanelScanLogs");

            migrationBuilder.DropTable(
                name: "PanelTypes");

            migrationBuilder.DropIndex(
                name: "IX_BoxPanels_PanelDeliveryNoteDeliveryNoteId",
                table: "BoxPanels");

            migrationBuilder.DropIndex(
                name: "IX_BoxPanels_PanelTypeId",
                table: "BoxPanels");

            migrationBuilder.DropColumn(
                name: "ActualArrivalDate",
                table: "BoxPanels");

            migrationBuilder.DropColumn(
                name: "Barcode",
                table: "BoxPanels");

            migrationBuilder.DropColumn(
                name: "CurrentLocationStatus",
                table: "BoxPanels");

            migrationBuilder.DropColumn(
                name: "DeliveryNoteNumber",
                table: "BoxPanels");

            migrationBuilder.DropColumn(
                name: "DeliveryNoteUrl",
                table: "BoxPanels");

            migrationBuilder.DropColumn(
                name: "Dimensions",
                table: "BoxPanels");

            migrationBuilder.DropColumn(
                name: "DispatchedDate",
                table: "BoxPanels");

            migrationBuilder.DropColumn(
                name: "EstimatedArrivalDate",
                table: "BoxPanels");

            migrationBuilder.DropColumn(
                name: "FirstApprovalBy",
                table: "BoxPanels");

            migrationBuilder.DropColumn(
                name: "FirstApprovalDate",
                table: "BoxPanels");

            migrationBuilder.DropColumn(
                name: "FirstApprovalNotes",
                table: "BoxPanels");

            migrationBuilder.DropColumn(
                name: "FirstApprovalStatus",
                table: "BoxPanels");

            migrationBuilder.DropColumn(
                name: "InstalledDate",
                table: "BoxPanels");

            migrationBuilder.DropColumn(
                name: "ManufacturedDate",
                table: "BoxPanels");

            migrationBuilder.DropColumn(
                name: "ManufacturerName",
                table: "BoxPanels");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "BoxPanels");

            migrationBuilder.DropColumn(
                name: "PanelDeliveryNoteDeliveryNoteId",
                table: "BoxPanels");

            migrationBuilder.DropColumn(
                name: "PanelTypeId",
                table: "BoxPanels");

            migrationBuilder.DropColumn(
                name: "QRCodeUrl",
                table: "BoxPanels");

            migrationBuilder.DropColumn(
                name: "ScannedAtFactory",
                table: "BoxPanels");

            migrationBuilder.DropColumn(
                name: "SecondApprovalBy",
                table: "BoxPanels");

            migrationBuilder.DropColumn(
                name: "SecondApprovalDate",
                table: "BoxPanels");

            migrationBuilder.DropColumn(
                name: "SecondApprovalNotes",
                table: "BoxPanels");

            migrationBuilder.DropColumn(
                name: "SecondApprovalStatus",
                table: "BoxPanels");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "BoxPanels");
        }
    }
}
