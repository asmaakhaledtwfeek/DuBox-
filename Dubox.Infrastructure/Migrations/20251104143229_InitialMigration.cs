using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActivityMaster",
                columns: table => new
                {
                    ActivityMasterId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActivityCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ActivityName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ActivityDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Department = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Trade = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    StandardDuration = table.Column<int>(type: "int", nullable: false),
                    Sequence = table.Column<int>(type: "int", nullable: false),
                    IsWIRCheckpoint = table.Column<bool>(type: "bit", nullable: false),
                    WIRNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityMaster", x => x.ActivityMasterId);
                });

            migrationBuilder.CreateTable(
                name: "AuditLog",
                columns: table => new
                {
                    AuditId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TableName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RecordId = table.Column<int>(type: "int", nullable: true),
                    Action = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OldValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChangedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ChangedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IPAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DeviceInfo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLog", x => x.AuditId);
                });

            migrationBuilder.CreateTable(
                name: "CostCategories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ParentCategoryId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CostCategories", x => x.CategoryId);
                    table.ForeignKey(
                        name: "FK_CostCategories_CostCategories_ParentCategoryId",
                        column: x => x.ParentCategoryId,
                        principalTable: "CostCategories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FactoryLocations",
                columns: table => new
                {
                    LocationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LocationCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LocationName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    LocationType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Bay = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Row = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Position = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Capacity = table.Column<int>(type: "int", nullable: true),
                    CurrentOccupancy = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FactoryLocations", x => x.LocationId);
                });

            migrationBuilder.CreateTable(
                name: "Materials",
                columns: table => new
                {
                    MaterialId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaterialCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MaterialName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    MaterialCategory = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Unit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UnitCost = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CurrentStock = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MinimumStock = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ReorderLevel = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SupplierName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materials", x => x.MaterialId);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProjectName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ClientName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Location = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PlannedEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActualEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Active"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.ProjectId);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    TeamId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeamCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TeamName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Department = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Trade = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TeamLeaderName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TeamSize = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.TeamId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Role = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Department = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LastLoginDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Boxes",
                columns: table => new
                {
                    BoxId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BoxTag = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BoxName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    BoxType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Floor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Building = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Zone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    QRCode = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    QRCodeImage = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    RFIDTag = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CurrentStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Not Started"),
                    CurrentLocation = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ProgressPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: false, defaultValue: 0m),
                    PlannedStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActualStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PlannedEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActualEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boxes", x => x.BoxId);
                    table.ForeignKey(
                        name: "FK_Boxes_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeamMembers",
                columns: table => new
                {
                    TeamMemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeamId = table.Column<int>(type: "int", nullable: false),
                    EmployeeCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EmployeeName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Role = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MobileNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamMembers", x => x.TeamMemberId);
                    table.ForeignKey(
                        name: "FK_TeamMembers_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BoxActivities",
                columns: table => new
                {
                    BoxActivityId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BoxId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActivityMasterId = table.Column<int>(type: "int", nullable: false),
                    ActivitySequence = table.Column<int>(type: "int", nullable: false),
                    PlannedStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PlannedEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PlannedDuration = table.Column<int>(type: "int", nullable: true),
                    ActualStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActualEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActualDuration = table.Column<int>(type: "int", nullable: true),
                    ProgressPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: false, defaultValue: 0m),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Not Started"),
                    AssignedTeamId = table.Column<int>(type: "int", nullable: true),
                    AssignedTo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoxActivities", x => x.BoxActivityId);
                    table.ForeignKey(
                        name: "FK_BoxActivities_ActivityMaster_ActivityMasterId",
                        column: x => x.ActivityMasterId,
                        principalTable: "ActivityMaster",
                        principalColumn: "ActivityMasterId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BoxActivities_Boxes_BoxId",
                        column: x => x.BoxId,
                        principalTable: "Boxes",
                        principalColumn: "BoxId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BoxActivities_Teams_AssignedTeamId",
                        column: x => x.AssignedTeamId,
                        principalTable: "Teams",
                        principalColumn: "TeamId");
                });

            migrationBuilder.CreateTable(
                name: "BoxAssets",
                columns: table => new
                {
                    AssetId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BoxId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssetType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AssetCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AssetDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    Unit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoxAssets", x => x.AssetId);
                    table.ForeignKey(
                        name: "FK_BoxAssets_Boxes_BoxId",
                        column: x => x.BoxId,
                        principalTable: "Boxes",
                        principalColumn: "BoxId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BoxCosts",
                columns: table => new
                {
                    CostId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BoxId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    CostType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BudgetedCost = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ActualCost = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CostDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Reference = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoxCosts", x => x.CostId);
                    table.ForeignKey(
                        name: "FK_BoxCosts_Boxes_BoxId",
                        column: x => x.BoxId,
                        principalTable: "Boxes",
                        principalColumn: "BoxId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BoxCosts_CostCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "CostCategories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BoxLocationHistory",
                columns: table => new
                {
                    HistoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BoxId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    MovedFromLocationId = table.Column<int>(type: "int", nullable: true),
                    MovedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MovedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RFIDReadTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoxLocationHistory", x => x.HistoryId);
                    table.ForeignKey(
                        name: "FK_BoxLocationHistory_Boxes_BoxId",
                        column: x => x.BoxId,
                        principalTable: "Boxes",
                        principalColumn: "BoxId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BoxLocationHistory_FactoryLocations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "FactoryLocations",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BoxLocationHistory_FactoryLocations_MovedFromLocationId",
                        column: x => x.MovedFromLocationId,
                        principalTable: "FactoryLocations",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BoxMaterials",
                columns: table => new
                {
                    BoxMaterialId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BoxId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaterialId = table.Column<int>(type: "int", nullable: false),
                    RequiredQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AllocatedQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ConsumedQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AllocatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ConsumedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoxMaterials", x => x.BoxMaterialId);
                    table.ForeignKey(
                        name: "FK_BoxMaterials_Boxes_BoxId",
                        column: x => x.BoxId,
                        principalTable: "Boxes",
                        principalColumn: "BoxId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BoxMaterials_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "MaterialId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DailyProductionLog",
                columns: table => new
                {
                    LogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LogDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BoxId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeamId = table.Column<int>(type: "int", nullable: true),
                    ActivityMasterId = table.Column<int>(type: "int", nullable: true),
                    ManHours = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    ProgressAchieved = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyProductionLog", x => x.LogId);
                    table.ForeignKey(
                        name: "FK_DailyProductionLog_ActivityMaster_ActivityMasterId",
                        column: x => x.ActivityMasterId,
                        principalTable: "ActivityMaster",
                        principalColumn: "ActivityMasterId");
                    table.ForeignKey(
                        name: "FK_DailyProductionLog_Boxes_BoxId",
                        column: x => x.BoxId,
                        principalTable: "Boxes",
                        principalColumn: "BoxId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DailyProductionLog_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "TeamId");
                });

            migrationBuilder.CreateTable(
                name: "MaterialTransactions",
                columns: table => new
                {
                    TransactionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaterialId = table.Column<int>(type: "int", nullable: false),
                    BoxId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TransactionType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reference = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PerformedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialTransactions", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_MaterialTransactions_Boxes_BoxId",
                        column: x => x.BoxId,
                        principalTable: "Boxes",
                        principalColumn: "BoxId");
                    table.ForeignKey(
                        name: "FK_MaterialTransactions_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "MaterialId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Risks",
                columns: table => new
                {
                    RiskId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BoxId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RiskCategory = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RiskDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Impact = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Probability = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    MitigationPlan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Owner = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IdentifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReviewDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Risks", x => x.RiskId);
                    table.ForeignKey(
                        name: "FK_Risks_Boxes_BoxId",
                        column: x => x.BoxId,
                        principalTable: "Boxes",
                        principalColumn: "BoxId");
                    table.ForeignKey(
                        name: "FK_Risks_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WIRCheckpoints",
                columns: table => new
                {
                    WIRId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BoxId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WIRNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WIRName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    WIRDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RequestedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RequestedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    InspectionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InspectorName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    InspectorRole = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ApprovalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AttachmentPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WIRCheckpoints", x => x.WIRId);
                    table.ForeignKey(
                        name: "FK_WIRCheckpoints_Boxes_BoxId",
                        column: x => x.BoxId,
                        principalTable: "Boxes",
                        principalColumn: "BoxId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActivityDependencies",
                columns: table => new
                {
                    DependencyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BoxActivityId = table.Column<int>(type: "int", nullable: false),
                    PredecessorActivityId = table.Column<int>(type: "int", nullable: false),
                    DependencyType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    LagDays = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityDependencies", x => x.DependencyId);
                    table.ForeignKey(
                        name: "FK_ActivityDependencies_BoxActivities_BoxActivityId",
                        column: x => x.BoxActivityId,
                        principalTable: "BoxActivities",
                        principalColumn: "BoxActivityId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActivityDependencies_BoxActivities_PredecessorActivityId",
                        column: x => x.PredecessorActivityId,
                        principalTable: "BoxActivities",
                        principalColumn: "BoxActivityId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    NotificationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotificationType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Priority = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RelatedBoxId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RelatedActivityId = table.Column<int>(type: "int", nullable: true),
                    TargetRole = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TargetUser = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    ReadDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_Notifications_BoxActivities_RelatedActivityId",
                        column: x => x.RelatedActivityId,
                        principalTable: "BoxActivities",
                        principalColumn: "BoxActivityId");
                    table.ForeignKey(
                        name: "FK_Notifications_Boxes_RelatedBoxId",
                        column: x => x.RelatedBoxId,
                        principalTable: "Boxes",
                        principalColumn: "BoxId");
                });

            migrationBuilder.CreateTable(
                name: "ProgressUpdates",
                columns: table => new
                {
                    UpdateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BoxId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BoxActivityId = table.Column<int>(type: "int", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TeamId = table.Column<int>(type: "int", nullable: true),
                    ProgressPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    WorkDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IssuesEncountered = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhotoPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Latitude = table.Column<decimal>(type: "decimal(10,8)", nullable: true),
                    Longitude = table.Column<decimal>(type: "decimal(11,8)", nullable: true),
                    DeviceInfo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgressUpdates", x => x.UpdateId);
                    table.ForeignKey(
                        name: "FK_ProgressUpdates_BoxActivities_BoxActivityId",
                        column: x => x.BoxActivityId,
                        principalTable: "BoxActivities",
                        principalColumn: "BoxActivityId");
                    table.ForeignKey(
                        name: "FK_ProgressUpdates_Boxes_BoxId",
                        column: x => x.BoxId,
                        principalTable: "Boxes",
                        principalColumn: "BoxId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProgressUpdates_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "TeamId");
                });

            migrationBuilder.CreateTable(
                name: "QualityIssues",
                columns: table => new
                {
                    IssueId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BoxId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WIRId = table.Column<int>(type: "int", nullable: true),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IssueType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Severity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IssueDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReportedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AssignedTo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ResolutionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResolutionDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhotoPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QualityIssues", x => x.IssueId);
                    table.ForeignKey(
                        name: "FK_QualityIssues_Boxes_BoxId",
                        column: x => x.BoxId,
                        principalTable: "Boxes",
                        principalColumn: "BoxId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QualityIssues_WIRCheckpoints_WIRId",
                        column: x => x.WIRId,
                        principalTable: "WIRCheckpoints",
                        principalColumn: "WIRId");
                });

            migrationBuilder.CreateTable(
                name: "WIRChecklistItems",
                columns: table => new
                {
                    ChecklistItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WIRId = table.Column<int>(type: "int", nullable: false),
                    CheckpointDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ReferenceDocument = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sequence = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WIRChecklistItems", x => x.ChecklistItemId);
                    table.ForeignKey(
                        name: "FK_WIRChecklistItems_WIRCheckpoints_WIRId",
                        column: x => x.WIRId,
                        principalTable: "WIRCheckpoints",
                        principalColumn: "WIRId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ActivityMaster",
                columns: new[] { "ActivityMasterId", "ActivityCode", "ActivityDescription", "ActivityName", "Department", "IsActive", "IsWIRCheckpoint", "Sequence", "StandardDuration", "Trade", "WIRNumber" },
                values: new object[,]
                {
                    { 1, "ACT-001", "Manufacture precast walls, slabs, and structural elements", "Fabrication of boxes", "Civil", true, false, 1, 5, "Precast", null },
                    { 2, "ACT-002", "Transport precast elements to assembly area", "Delivery of elements", "Civil", true, false, 2, 3, "Logistics", null },
                    { 3, "ACT-003", "Perform quality checks and store elements for assembly", "Storage and QC", "QC", true, false, 3, 1, "Quality Control", null },
                    { 4, "ACT-004", "Assemble modules and seal structural joints", "Assembly & joints", "Civil", true, false, 4, 4, "Assembly", null },
                    { 5, "ACT-005", "Install preassembled bathroom PODs", "PODS installation", "Civil", true, false, 5, 2, "Assembly", null },
                    { 6, "ACT-006", "Install preassembled MEP cage", "MEP Cage installation", "MEP", true, false, 6, 2, "Assembly", null },
                    { 7, "ACT-007", "Install electrical conduits during assembly", "Electrical Containment (Assembly)", "MEP", true, false, 7, 2, "Electrical", null },
                    { 8, "ACT-008", "Complete box closures and initial QC inspection", "Box Closure", "Civil", true, true, 8, 1, "Assembly", "WIR-1" },
                    { 9, "ACT-009", "Install AC units", "Fan Coil Units", "MEP", true, false, 9, 2, "Mechanical", null },
                    { 10, "ACT-010", "Install ducts and insulation", "Ducts & Insulation", "MEP", true, false, 10, 2, "Mechanical", null },
                    { 11, "ACT-011", "Complete drainage piping", "Drainage piping", "MEP", true, false, 11, 2, "Mechanical", null },
                    { 12, "ACT-012", "Complete water piping", "Water Piping", "MEP", true, false, 12, 2, "Mechanical", null },
                    { 13, "ACT-013", "Complete firefighting piping", "Fire Fighting Piping", "MEP", true, true, 13, 2, "Mechanical", "WIR-2" },
                    { 14, "ACT-014", "Install electrical containment", "Electrical Containment", "MEP", true, false, 14, 2, "Electrical", null },
                    { 15, "ACT-015", "Complete electrical wiring", "Electrical Wiring", "MEP", true, false, 15, 2, "Electrical", null },
                    { 16, "ACT-016", "Install distribution board and ONU panel", "DB and ONU Panel", "MEP", true, false, 16, 2, "Electrical", null },
                    { 17, "ACT-017", "Complete drywall framing for partitions", "Dry Wall Framing", "Civil", true, true, 17, 1, "Finishing", "WIR-3" },
                    { 18, "ACT-018", "Install false ceilings", "False Ceiling", "Civil", true, false, 18, 1, "Finishing", null },
                    { 19, "ACT-019", "Install floor and wall tiles", "Tile Fixing", "Civil", true, false, 19, 2, "Finishing", null },
                    { 20, "ACT-020", "Complete painting", "Painting (Internal & External)", "Civil", true, false, 20, 2, "Finishing", null },
                    { 21, "ACT-021", "Fix kitchenettes and counters", "Kitchenette and Counters", "Civil", true, false, 21, 2, "Finishing", null },
                    { 22, "ACT-022", "Install doors", "Doors", "Civil", true, false, 22, 1, "Finishing", null },
                    { 23, "ACT-023", "Install windows", "Windows", "Civil", true, true, 23, 1, "Finishing", "WIR-4" },
                    { 24, "ACT-024", "Install switches and sockets", "Switches & Sockets", "MEP", true, false, 24, 2, "Electrical", null },
                    { 25, "ACT-025", "Install light fittings", "Light Fittings", "MEP", true, false, 25, 2, "Electrical", null },
                    { 26, "ACT-026", "Install chilled water piping", "Copper Piping", "MEP", true, false, 26, 2, "Mechanical", null },
                    { 27, "ACT-027", "Install sanitary fixtures", "Sanitary Fittings - Kitchen", "MEP", true, false, 27, 2, "Mechanical", null },
                    { 28, "ACT-028", "Install thermostats", "Thermostats", "MEP", true, false, 28, 2, "Mechanical", null },
                    { 29, "ACT-029", "Install air outlets", "Air Outlet", "MEP", true, false, 29, 2, "Mechanical", null },
                    { 30, "ACT-030", "Install sprinkler system", "Sprinkler", "MEP", true, false, 30, 2, "Mechanical", null },
                    { 31, "ACT-031", "Install smoke detectors", "Smoke Detector", "MEP", true, true, 31, 2, "Mechanical", "WIR-5" },
                    { 32, "ACT-032", "Install ironmongery (locks, handles, accessories)", "Iron Mongeries", "Civil", true, false, 32, 2, "Finishing", null },
                    { 33, "ACT-033", "Conduct comprehensive final inspection and wrap modules for delivery", "Inspection & Wrapping", "QC", true, true, 33, 1, "Quality Control", "WIR-6" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityDependencies_BoxActivityId",
                table: "ActivityDependencies",
                column: "BoxActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityDependencies_PredecessorActivityId",
                table: "ActivityDependencies",
                column: "PredecessorActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityMaster_ActivityCode",
                table: "ActivityMaster",
                column: "ActivityCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BoxActivities_ActivityMasterId",
                table: "BoxActivities",
                column: "ActivityMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_BoxActivities_AssignedTeamId",
                table: "BoxActivities",
                column: "AssignedTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_BoxActivities_BoxId",
                table: "BoxActivities",
                column: "BoxId");

            migrationBuilder.CreateIndex(
                name: "IX_BoxActivities_BoxId_Status",
                table: "BoxActivities",
                columns: new[] { "BoxId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_BoxActivities_Status",
                table: "BoxActivities",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_BoxAssets_BoxId",
                table: "BoxAssets",
                column: "BoxId");

            migrationBuilder.CreateIndex(
                name: "IX_BoxCosts_BoxId",
                table: "BoxCosts",
                column: "BoxId");

            migrationBuilder.CreateIndex(
                name: "IX_BoxCosts_CategoryId",
                table: "BoxCosts",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Boxes_BoxTag",
                table: "Boxes",
                column: "BoxTag",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Boxes_ProjectId",
                table: "Boxes",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Boxes_QRCode",
                table: "Boxes",
                column: "QRCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BoxLocationHistory_BoxId",
                table: "BoxLocationHistory",
                column: "BoxId");

            migrationBuilder.CreateIndex(
                name: "IX_BoxLocationHistory_LocationId",
                table: "BoxLocationHistory",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_BoxLocationHistory_MovedFromLocationId",
                table: "BoxLocationHistory",
                column: "MovedFromLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_BoxMaterials_BoxId",
                table: "BoxMaterials",
                column: "BoxId");

            migrationBuilder.CreateIndex(
                name: "IX_BoxMaterials_MaterialId",
                table: "BoxMaterials",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_CostCategories_CategoryCode",
                table: "CostCategories",
                column: "CategoryCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CostCategories_ParentCategoryId",
                table: "CostCategories",
                column: "ParentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyProductionLog_ActivityMasterId",
                table: "DailyProductionLog",
                column: "ActivityMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyProductionLog_BoxId",
                table: "DailyProductionLog",
                column: "BoxId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyProductionLog_LogDate_TeamId",
                table: "DailyProductionLog",
                columns: new[] { "LogDate", "TeamId" });

            migrationBuilder.CreateIndex(
                name: "IX_DailyProductionLog_TeamId",
                table: "DailyProductionLog",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_FactoryLocations_LocationCode",
                table: "FactoryLocations",
                column: "LocationCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Materials_MaterialCode",
                table: "Materials",
                column: "MaterialCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MaterialTransactions_BoxId",
                table: "MaterialTransactions",
                column: "BoxId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialTransactions_MaterialId",
                table: "MaterialTransactions",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_RelatedActivityId",
                table: "Notifications",
                column: "RelatedActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_RelatedBoxId",
                table: "Notifications",
                column: "RelatedBoxId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgressUpdates_BoxActivityId",
                table: "ProgressUpdates",
                column: "BoxActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgressUpdates_BoxId",
                table: "ProgressUpdates",
                column: "BoxId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgressUpdates_BoxId_UpdateDate",
                table: "ProgressUpdates",
                columns: new[] { "BoxId", "UpdateDate" });

            migrationBuilder.CreateIndex(
                name: "IX_ProgressUpdates_TeamId",
                table: "ProgressUpdates",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgressUpdates_UpdateDate",
                table: "ProgressUpdates",
                column: "UpdateDate");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ProjectCode",
                table: "Projects",
                column: "ProjectCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_QualityIssues_BoxId",
                table: "QualityIssues",
                column: "BoxId");

            migrationBuilder.CreateIndex(
                name: "IX_QualityIssues_Status",
                table: "QualityIssues",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_QualityIssues_WIRId",
                table: "QualityIssues",
                column: "WIRId");

            migrationBuilder.CreateIndex(
                name: "IX_Risks_BoxId",
                table: "Risks",
                column: "BoxId");

            migrationBuilder.CreateIndex(
                name: "IX_Risks_ProjectId",
                table: "Risks",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamMembers_TeamId",
                table: "TeamMembers",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_TeamCode",
                table: "Teams",
                column: "TeamCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WIRChecklistItems_WIRId",
                table: "WIRChecklistItems",
                column: "WIRId");

            migrationBuilder.CreateIndex(
                name: "IX_WIRCheckpoints_BoxId",
                table: "WIRCheckpoints",
                column: "BoxId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityDependencies");

            migrationBuilder.DropTable(
                name: "AuditLog");

            migrationBuilder.DropTable(
                name: "BoxAssets");

            migrationBuilder.DropTable(
                name: "BoxCosts");

            migrationBuilder.DropTable(
                name: "BoxLocationHistory");

            migrationBuilder.DropTable(
                name: "BoxMaterials");

            migrationBuilder.DropTable(
                name: "DailyProductionLog");

            migrationBuilder.DropTable(
                name: "MaterialTransactions");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "ProgressUpdates");

            migrationBuilder.DropTable(
                name: "QualityIssues");

            migrationBuilder.DropTable(
                name: "Risks");

            migrationBuilder.DropTable(
                name: "TeamMembers");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "WIRChecklistItems");

            migrationBuilder.DropTable(
                name: "CostCategories");

            migrationBuilder.DropTable(
                name: "FactoryLocations");

            migrationBuilder.DropTable(
                name: "Materials");

            migrationBuilder.DropTable(
                name: "BoxActivities");

            migrationBuilder.DropTable(
                name: "WIRCheckpoints");

            migrationBuilder.DropTable(
                name: "ActivityMaster");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "Boxes");

            migrationBuilder.DropTable(
                name: "Projects");
        }
    }
}
