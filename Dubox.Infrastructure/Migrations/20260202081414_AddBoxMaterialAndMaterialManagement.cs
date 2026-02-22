using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBoxMaterialAndMaterialManagement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BoxMaterials_Materials_MaterialId",
                table: "BoxMaterials");

            migrationBuilder.DropForeignKey(
                name: "FK_Materials_Projects_ProjectId",
                table: "Materials");

            migrationBuilder.DropIndex(
                name: "IX_Materials_ProjectId",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "AllocatedQuantity",
                table: "BoxMaterials");

            migrationBuilder.DropColumn(
                name: "ConsumedQuantity",
                table: "BoxMaterials");

            migrationBuilder.DropColumn(
                name: "RequiredQuantity",
                table: "BoxMaterials");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "BoxMaterials");

            migrationBuilder.RenameColumn(
                name: "ConsumedDate",
                table: "BoxMaterials",
                newName: "ModifiedDate");

            migrationBuilder.RenameColumn(
                name: "AllocatedDate",
                table: "BoxMaterials",
                newName: "LastNotificationDate");

            // NCR column already exists in QualityIssues table
            // migrationBuilder.AddColumn<int>(
            //     name: "NCR",
            //     table: "QualityIssues",
            //     type: "int",
            //     nullable: false,
            //     defaultValue: 0);

            // AllowCompletionWithConditionalApproval column already exists in Projects table
            // migrationBuilder.AddColumn<bool>(
            //     name: "AllowCompletionWithConditionalApproval",
            //     table: "Projects",
            //     type: "bit",
            //     nullable: false,
            //     defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "MaterialId",
                table: "Projects",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DefaultRequiredBeforeDays",
                table: "Materials",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "ArrivedBy",
                table: "BoxMaterials",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ArrivedDate",
                table: "BoxMaterials",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "BoxMaterials",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsArrived",
                table: "BoxMaterials",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "NotificationsSentCount",
                table: "BoxMaterials",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectMaterialId",
                table: "BoxMaterials",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "QualityIssueId",
                table: "BoxMaterials",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RequiredBeforeDays",
                table: "BoxMaterials",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "RequiredByDate",
                table: "BoxMaterials",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "ProjectMaterials",
                columns: table => new
                {
                    ProjectMaterialId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaterialId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequiredQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AllocatedQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ConsumedQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AllocatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ConsumedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsSelected = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectMaterials", x => x.ProjectMaterialId);
                    table.ForeignKey(
                        name: "FK_ProjectMaterials_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "MaterialId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectMaterials_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_MaterialId",
                table: "Projects",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_BoxMaterials_ArrivedBy",
                table: "BoxMaterials",
                column: "ArrivedBy");

            migrationBuilder.CreateIndex(
                name: "IX_BoxMaterials_IsArrived",
                table: "BoxMaterials",
                column: "IsArrived");

            migrationBuilder.CreateIndex(
                name: "IX_BoxMaterials_ProjectMaterialId",
                table: "BoxMaterials",
                column: "ProjectMaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_BoxMaterials_QualityIssueId",
                table: "BoxMaterials",
                column: "QualityIssueId");

            migrationBuilder.CreateIndex(
                name: "IX_BoxMaterials_RequiredByDate",
                table: "BoxMaterials",
                column: "RequiredByDate");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMaterials_MaterialId",
                table: "ProjectMaterials",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMaterials_ProjectId",
                table: "ProjectMaterials",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_BoxMaterials_Materials_MaterialId",
                table: "BoxMaterials",
                column: "MaterialId",
                principalTable: "Materials",
                principalColumn: "MaterialId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BoxMaterials_ProjectMaterials_ProjectMaterialId",
                table: "BoxMaterials",
                column: "ProjectMaterialId",
                principalTable: "ProjectMaterials",
                principalColumn: "ProjectMaterialId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_BoxMaterials_QualityIssues_QualityIssueId",
                table: "BoxMaterials",
                column: "QualityIssueId",
                principalTable: "QualityIssues",
                principalColumn: "IssueId");

            migrationBuilder.AddForeignKey(
                name: "FK_BoxMaterials_Users_ArrivedBy",
                table: "BoxMaterials",
                column: "ArrivedBy",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Materials_MaterialId",
                table: "Projects",
                column: "MaterialId",
                principalTable: "Materials",
                principalColumn: "MaterialId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BoxMaterials_Materials_MaterialId",
                table: "BoxMaterials");

            migrationBuilder.DropForeignKey(
                name: "FK_BoxMaterials_ProjectMaterials_ProjectMaterialId",
                table: "BoxMaterials");

            migrationBuilder.DropForeignKey(
                name: "FK_BoxMaterials_QualityIssues_QualityIssueId",
                table: "BoxMaterials");

            migrationBuilder.DropForeignKey(
                name: "FK_BoxMaterials_Users_ArrivedBy",
                table: "BoxMaterials");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Materials_MaterialId",
                table: "Projects");

            migrationBuilder.DropTable(
                name: "ProjectMaterials");

            migrationBuilder.DropIndex(
                name: "IX_Projects_MaterialId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_BoxMaterials_ArrivedBy",
                table: "BoxMaterials");

            migrationBuilder.DropIndex(
                name: "IX_BoxMaterials_IsArrived",
                table: "BoxMaterials");

            migrationBuilder.DropIndex(
                name: "IX_BoxMaterials_ProjectMaterialId",
                table: "BoxMaterials");

            migrationBuilder.DropIndex(
                name: "IX_BoxMaterials_QualityIssueId",
                table: "BoxMaterials");

            migrationBuilder.DropIndex(
                name: "IX_BoxMaterials_RequiredByDate",
                table: "BoxMaterials");

            migrationBuilder.DropColumn(
                name: "NCR",
                table: "QualityIssues");

            migrationBuilder.DropColumn(
                name: "AllowCompletionWithConditionalApproval",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "MaterialId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "DefaultRequiredBeforeDays",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "ArrivedBy",
                table: "BoxMaterials");

            migrationBuilder.DropColumn(
                name: "ArrivedDate",
                table: "BoxMaterials");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "BoxMaterials");

            migrationBuilder.DropColumn(
                name: "IsArrived",
                table: "BoxMaterials");

            migrationBuilder.DropColumn(
                name: "NotificationsSentCount",
                table: "BoxMaterials");

            migrationBuilder.DropColumn(
                name: "ProjectMaterialId",
                table: "BoxMaterials");

            migrationBuilder.DropColumn(
                name: "QualityIssueId",
                table: "BoxMaterials");

            migrationBuilder.DropColumn(
                name: "RequiredBeforeDays",
                table: "BoxMaterials");

            migrationBuilder.DropColumn(
                name: "RequiredByDate",
                table: "BoxMaterials");

            migrationBuilder.RenameColumn(
                name: "ModifiedDate",
                table: "BoxMaterials",
                newName: "ConsumedDate");

            migrationBuilder.RenameColumn(
                name: "LastNotificationDate",
                table: "BoxMaterials",
                newName: "AllocatedDate");

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "Materials",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "AllocatedQuantity",
                table: "BoxMaterials",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ConsumedQuantity",
                table: "BoxMaterials",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "RequiredQuantity",
                table: "BoxMaterials",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "BoxMaterials",
                type: "int",
                maxLength: 50,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Materials_ProjectId",
                table: "Materials",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_BoxMaterials_Materials_MaterialId",
                table: "BoxMaterials",
                column: "MaterialId",
                principalTable: "Materials",
                principalColumn: "MaterialId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Materials_Projects_ProjectId",
                table: "Materials",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "ProjectId");
        }
    }
}
