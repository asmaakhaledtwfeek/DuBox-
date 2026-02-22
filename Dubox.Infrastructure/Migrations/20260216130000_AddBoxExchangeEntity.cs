using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBoxExchangeEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BoxExchanges",
                columns: table => new
                {
                    BoxExchangeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BoxId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QualityIssueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OldBuildingNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    OldFloor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OldBoxTag = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NewBuildingNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NewFloor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NewBoxTag = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RequestReason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    RequestedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequestedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RejectedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RejectedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RejectionReason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    AppliedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AppliedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoxExchanges", x => x.BoxExchangeId);
                    table.ForeignKey(
                        name: "FK_BoxExchanges_Boxes_BoxId",
                        column: x => x.BoxId,
                        principalTable: "Boxes",
                        principalColumn: "BoxId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BoxExchanges_QualityIssues_QualityIssueId",
                        column: x => x.QualityIssueId,
                        principalTable: "QualityIssues",
                        principalColumn: "IssueId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BoxExchanges_Users_RequestedBy",
                        column: x => x.RequestedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_BoxExchanges_Users_ApprovedBy",
                        column: x => x.ApprovedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_BoxExchanges_Users_RejectedBy",
                        column: x => x.RejectedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_BoxExchanges_Users_AppliedBy",
                        column: x => x.AppliedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BoxExchanges_BoxId",
                table: "BoxExchanges",
                column: "BoxId");

            migrationBuilder.CreateIndex(
                name: "IX_BoxExchanges_QualityIssueId",
                table: "BoxExchanges",
                column: "QualityIssueId");

            migrationBuilder.CreateIndex(
                name: "IX_BoxExchanges_RequestedBy",
                table: "BoxExchanges",
                column: "RequestedBy");

            migrationBuilder.CreateIndex(
                name: "IX_BoxExchanges_ApprovedBy",
                table: "BoxExchanges",
                column: "ApprovedBy");

            migrationBuilder.CreateIndex(
                name: "IX_BoxExchanges_RejectedBy",
                table: "BoxExchanges",
                column: "RejectedBy");

            migrationBuilder.CreateIndex(
                name: "IX_BoxExchanges_AppliedBy",
                table: "BoxExchanges",
                column: "AppliedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BoxExchanges");
        }
    }
}
