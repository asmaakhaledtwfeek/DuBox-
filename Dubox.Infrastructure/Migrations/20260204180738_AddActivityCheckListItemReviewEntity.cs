using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddActivityCheckListItemReviewEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActivityCheckListItemReviews",
                columns: table => new
                {
                    ActivityCheckListItemReviewId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BoxActivityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActivityCheckListItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ReviewedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReviewedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityCheckListItemReviews", x => x.ActivityCheckListItemReviewId);
                    table.ForeignKey(
                        name: "FK_ActivityCheckListItemReviews_ActivityCheckListItems_ActivityCheckListItemId",
                        column: x => x.ActivityCheckListItemId,
                        principalTable: "ActivityCheckListItems",
                        principalColumn: "ActivityCheckListItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActivityCheckListItemReviews_BoxActivities_BoxActivityId",
                        column: x => x.BoxActivityId,
                        principalTable: "BoxActivities",
                        principalColumn: "BoxActivityId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActivityCheckListItemReviews_Users_ReviewedBy",
                        column: x => x.ReviewedBy,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityCheckListItemReviews_ActivityCheckListItemId",
                table: "ActivityCheckListItemReviews",
                column: "ActivityCheckListItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityCheckListItemReviews_BoxActivityId",
                table: "ActivityCheckListItemReviews",
                column: "BoxActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityCheckListItemReviews_ReviewedBy",
                table: "ActivityCheckListItemReviews",
                column: "ReviewedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityCheckListItemReviews");
        }
    }
}
