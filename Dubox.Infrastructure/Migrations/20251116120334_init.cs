using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActivityMaster",
                columns: table => new
                {
                    ActivityMasterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActivityCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ActivityName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Stage = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StageNumber = table.Column<int>(type: "int", nullable: false),
                    SequenceInStage = table.Column<int>(type: "int", nullable: false),
                    OverallSequence = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    EstimatedDurationDays = table.Column<int>(type: "int", nullable: false),
                    IsWIRCheckpoint = table.Column<bool>(type: "bit", nullable: false),
                    WIRCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ApplicableBoxTypes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DependsOnActivities = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
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
                name: "Groups",
                columns: table => new
                {
                    GroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GroupName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.GroupId);
                });

            migrationBuilder.CreateTable(
                name: "Materials",
                columns: table => new
                {
                    MaterialId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaterialCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MaterialName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    MaterialCategory = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Unit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UnitCost = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CurrentStock = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AllocatedStock = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
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
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TotalBoxes = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.ProjectId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Boxes",
                columns: table => new
                {
                    BoxId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BoxTag = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BoxName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    BoxType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Floor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Building = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Zone = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    QRCodeString = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    QRCodeImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ProgressPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Length = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Width = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Height = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    UnitOfMeasure = table.Column<int>(type: "int", maxLength: 50, nullable: true),
                    BIMModelReference = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    RevitElementId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ActualStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PlannedEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActualEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                name: "GroupRoles",
                columns: table => new
                {
                    GroupRoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssignedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupRoles", x => x.GroupRoleId);
                    table.ForeignKey(
                        name: "FK_GroupRoles_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BoxAssets",
                columns: table => new
                {
                    BoxAssetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BoxId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssetType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AssetCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AssetName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Specifications = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoxAssets", x => x.BoxAssetId);
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
                    BoxMaterialId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BoxId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaterialId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequiredQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AllocatedQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ConsumedQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Status = table.Column<int>(type: "int", maxLength: 50, nullable: false),
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

            migrationBuilder.CreateTable(
                name: "ActivityDependencies",
                columns: table => new
                {
                    DependencyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BoxActivityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PredecessorActivityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DependencyType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    LagDays = table.Column<int>(type: "int", nullable: false),
                    BoxActivityId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BoxActivityId2 = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityDependencies", x => x.DependencyId);
                });

            migrationBuilder.CreateTable(
                name: "ActivityMaterials",
                columns: table => new
                {
                    ActivityMaterialId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaterialId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BoxActivityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuantityNeeded = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityMaterials", x => x.ActivityMaterialId);
                    table.ForeignKey(
                        name: "FK_ActivityMaterials_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "MaterialId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BoxActivities",
                columns: table => new
                {
                    BoxActivityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BoxId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActivityMasterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Sequence = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", maxLength: 50, nullable: false, defaultValue: 1),
                    ProgressPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    PlannedStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PlannedEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActualStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActualEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    WorkDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IssuesEncountered = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TeamId = table.Column<int>(type: "int", nullable: true),
                    AssignedMemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MaterialsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TeamMemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoxActivities", x => x.BoxActivityId);
                    table.ForeignKey(
                        name: "FK_BoxActivities_ActivityMaster_ActivityMasterId",
                        column: x => x.ActivityMasterId,
                        principalTable: "ActivityMaster",
                        principalColumn: "ActivityMasterId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BoxActivities_Boxes_BoxId",
                        column: x => x.BoxId,
                        principalTable: "Boxes",
                        principalColumn: "BoxId",
                        onDelete: ReferentialAction.Cascade);
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
                    RelatedActivityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                name: "DailyProductionLog",
                columns: table => new
                {
                    LogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LogDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BoxId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeamId = table.Column<int>(type: "int", nullable: true),
                    ActivityMasterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DepartmentName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Location = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ManagerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.DepartmentId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LastLoginDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "DepartmentId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MaterialTransactions",
                columns: table => new
                {
                    TransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaterialId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BoxId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BoxActivityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TransactionType = table.Column<int>(type: "int", maxLength: 50, nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reference = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PerformedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialTransactions", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_MaterialTransactions_BoxActivities_BoxActivityId",
                        column: x => x.BoxActivityId,
                        principalTable: "BoxActivities",
                        principalColumn: "BoxActivityId",
                        onDelete: ReferentialAction.SetNull);
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
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaterialTransactions_Users_PerformedById",
                        column: x => x.PerformedById,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "UserGroups",
                columns: table => new
                {
                    UserGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JoinedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroups", x => x.UserGroupId);
                    table.ForeignKey(
                        name: "FK_UserGroups_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserGroups_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserRoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssignedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.UserRoleId);
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WIRRecords",
                columns: table => new
                {
                    WIRRecordId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BoxActivityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WIRCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    RequestedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequestedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InspectedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    InspectionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InspectionNotes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PhotoUrls = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    RejectionReason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WIRRecords", x => x.WIRRecordId);
                    table.ForeignKey(
                        name: "FK_WIRRecords_BoxActivities_BoxActivityId",
                        column: x => x.BoxActivityId,
                        principalTable: "BoxActivities",
                        principalColumn: "BoxActivityId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WIRRecords_Users_InspectedBy",
                        column: x => x.InspectedBy,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WIRRecords_Users_RequestedBy",
                        column: x => x.RequestedBy,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProgressUpdates",
                columns: table => new
                {
                    ProgressUpdateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BoxId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BoxActivityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProgressPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<int>(type: "int", maxLength: 50, nullable: false),
                    WorkDescription = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IssuesEncountered = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Latitude = table.Column<double>(type: "float", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: true),
                    LocationDescription = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PhotoUrls = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    UpdateMethod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DeviceInfo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TeamId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgressUpdates", x => x.ProgressUpdateId);
                    table.ForeignKey(
                        name: "FK_ProgressUpdates_BoxActivities_BoxActivityId",
                        column: x => x.BoxActivityId,
                        principalTable: "BoxActivities",
                        principalColumn: "BoxActivityId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProgressUpdates_Boxes_BoxId",
                        column: x => x.BoxId,
                        principalTable: "Boxes",
                        principalColumn: "BoxId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProgressUpdates_Users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TeamMembers",
                columns: table => new
                {
                    TeamMemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeamId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EmployeeName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    MobileNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamMembers", x => x.TeamMemberId);
                    table.ForeignKey(
                        name: "FK_TeamMembers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    TeamId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeamCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TeamName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Trade = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TeamLeaderMemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.TeamId);
                    table.ForeignKey(
                        name: "FK_Teams_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "DepartmentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Teams_TeamMembers_TeamLeaderMemberId",
                        column: x => x.TeamLeaderMemberId,
                        principalTable: "TeamMembers",
                        principalColumn: "TeamMemberId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.InsertData(
                table: "ActivityMaster",
                columns: new[] { "ActivityMasterId", "ActivityCode", "ActivityName", "ApplicableBoxTypes", "CreatedDate", "DependsOnActivities", "Description", "EstimatedDurationDays", "IsActive", "IsWIRCheckpoint", "OverallSequence", "SequenceInStage", "Stage", "StageNumber", "WIRCode" },
                values: new object[,]
                {
                    { new Guid("10000001-0000-0000-0000-000000000001"), "STAGE1-FAB", "Fabrication of boxes", null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Manufacturing and fabrication of precast box components", 3, true, false, 1, 1, "Stage 1: Precast Production", 1, null },
                    { new Guid("10000001-0000-0000-0000-000000000002"), "STAGE1-DEL", "Delivery of elements", null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Transportation and delivery of precast elements to site", 1, true, false, 2, 2, "Stage 1: Precast Production", 1, null },
                    { new Guid("10000001-0000-0000-0000-000000000003"), "STAGE1-QC", "Storage and QC", null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Storage of elements and quality control inspection", 1, true, false, 3, 3, "Stage 1: Precast Production", 1, null },
                    { new Guid("10000002-0000-0000-0000-000000000001"), "STAGE2-ASM", "Assembly & joints", null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Assembly of box components and joint connections", 2, true, false, 4, 1, "Stage 2: Module Assembly", 2, null },
                    { new Guid("10000002-0000-0000-0000-000000000002"), "STAGE2-POD", "PODS installation", null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Installation of pre-assembled bathroom and kitchen PODs", 1, true, false, 5, 2, "Stage 2: Module Assembly", 2, null },
                    { new Guid("10000002-0000-0000-0000-000000000003"), "STAGE2-MEP", "MEP Cage installation", null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Installation of pre-assembled MEP cages", 1, true, false, 6, 3, "Stage 2: Module Assembly", 2, null },
                    { new Guid("10000002-0000-0000-0000-000000000004"), "STAGE2-ELC", "Electrical Containment", null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Installation of electrical conduits and containment systems", 1, true, false, 7, 4, "Stage 2: Module Assembly", 2, null },
                    { new Guid("10000002-0000-0000-0000-000000000005"), "STAGE2-CLO", "Box Closure", null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Final closure and sealing of box module", 1, true, false, 8, 5, "Stage 2: Module Assembly", 2, null },
                    { new Guid("10000002-0000-0000-0000-000000000006"), "STAGE2-WIR1", "WIR-1", null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Work Inspection Request - Stage 2 Completion", 1, true, true, 9, 6, "Stage 2: Module Assembly", 2, "WIR-1" },
                    { new Guid("10000003-0000-0000-0000-000000000001"), "STAGE3-FCU", "Fan Coil Units", null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Installation of fan coil units for HVAC", 1, true, false, 10, 1, "Stage 3: MEP Phase 1", 3, null },
                    { new Guid("10000003-0000-0000-0000-000000000002"), "STAGE3-DCT", "Ducts & Insulation", null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Installation and insulation of HVAC ductwork", 1, true, false, 11, 2, "Stage 3: MEP Phase 1", 3, null },
                    { new Guid("10000003-0000-0000-0000-000000000003"), "STAGE3-DRN", "Drainage piping", null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Installation of drainage and wastewater piping", 1, true, false, 12, 3, "Stage 3: MEP Phase 1", 3, null },
                    { new Guid("10000003-0000-0000-0000-000000000004"), "STAGE3-WTR", "Water Piping", null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Installation of domestic water supply piping", 1, true, false, 13, 4, "Stage 3: MEP Phase 1", 3, null },
                    { new Guid("10000003-0000-0000-0000-000000000005"), "STAGE3-FF", "Fire Fighting Piping", null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Installation of fire protection and sprinkler piping", 1, true, false, 14, 5, "Stage 3: MEP Phase 1", 3, null },
                    { new Guid("10000003-0000-0000-0000-000000000006"), "STAGE3-WIR2", "WIR-2", null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Work Inspection Request - Stage 3 Completion", 1, true, true, 15, 6, "Stage 3: MEP Phase 1", 3, "WIR-2" },
                    { new Guid("10000004-0000-0000-0000-000000000001"), "STAGE4-ELCC", "Electrical Containment", null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Final electrical conduit and containment installation", 1, true, false, 16, 1, "Stage 4: Electrical & Framing", 4, null },
                    { new Guid("10000004-0000-0000-0000-000000000002"), "STAGE4-WIRE", "Electrical Wiring", null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Pulling and termination of electrical wiring", 1, true, false, 17, 2, "Stage 4: Electrical & Framing", 4, null },
                    { new Guid("10000004-0000-0000-0000-000000000003"), "STAGE4-DB", "DB and ONU Panel", null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Installation of distribution board and ONU network panels", 1, true, false, 18, 3, "Stage 4: Electrical & Framing", 4, null },
                    { new Guid("10000004-0000-0000-0000-000000000004"), "STAGE4-DRYWALL", "Dry Wall Framing", null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Installation of drywall framing and metal studs", 2, true, false, 19, 4, "Stage 4: Electrical & Framing", 4, null },
                    { new Guid("10000004-0000-0000-0000-000000000005"), "STAGE4-WIR3", "WIR-3", null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Work Inspection Request - Stage 4 Completion", 1, true, true, 20, 5, "Stage 4: Electrical & Framing", 4, "WIR-3" },
                    { new Guid("10000005-0000-0000-0000-000000000001"), "STAGE5-CEILING", "False Ceiling", null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Installation of suspended ceiling systems", 1, true, false, 21, 1, "Stage 5: Interior Finishing", 5, null },
                    { new Guid("10000005-0000-0000-0000-000000000002"), "STAGE5-TILE", "Tile Fixing", null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Installation of floor and wall tiles", 2, true, false, 22, 2, "Stage 5: Interior Finishing", 5, null },
                    { new Guid("10000005-0000-0000-0000-000000000003"), "STAGE5-PAINT", "Painting", null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Interior painting and finishing", 2, true, false, 23, 3, "Stage 5: Interior Finishing", 5, null },
                    { new Guid("10000005-0000-0000-0000-000000000004"), "STAGE5-KITCHEN", "Kitchenette & Counters", "Kitchen,Living Room", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Installation of kitchen units and countertops", 1, true, false, 24, 4, "Stage 5: Interior Finishing", 5, null },
                    { new Guid("10000005-0000-0000-0000-000000000005"), "STAGE5-DOORS", "Doors", null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Installation of interior doors and frames", 1, true, false, 25, 5, "Stage 5: Interior Finishing", 5, null },
                    { new Guid("10000005-0000-0000-0000-000000000006"), "STAGE5-WINDOWS", "Windows", null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Installation of windows and glazing", 1, true, false, 26, 6, "Stage 5: Interior Finishing", 5, null },
                    { new Guid("10000005-0000-0000-0000-000000000007"), "STAGE5-WIR4", "WIR-4", null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Work Inspection Request - Stage 5 Completion", 1, true, true, 27, 7, "Stage 5: Interior Finishing", 5, "WIR-4" },
                    { new Guid("10000006-0000-0000-0000-000000000001"), "STAGE6-SWITCH", "Switches & Sockets", null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Installation of electrical switches and power sockets", 1, true, false, 28, 1, "Stage 6: MEP Phase 2", 6, null },
                    { new Guid("10000006-0000-0000-0000-000000000002"), "STAGE6-LIGHTS", "Light Fittings", null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Installation of light fixtures and fittings", 1, true, false, 29, 2, "Stage 6: MEP Phase 2", 6, null },
                    { new Guid("10000006-0000-0000-0000-000000000003"), "STAGE6-COPPER", "Copper Piping", null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Installation of copper piping for specialized systems", 1, true, false, 30, 3, "Stage 6: MEP Phase 2", 6, null },
                    { new Guid("10000006-0000-0000-0000-000000000004"), "STAGE6-SANITARY", "Sanitary Fittings", null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Installation of bathroom and sanitary fixtures", 1, true, false, 31, 4, "Stage 6: MEP Phase 2", 6, null },
                    { new Guid("10000006-0000-0000-0000-000000000005"), "STAGE6-THERMO", "Thermostats", null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Installation of HVAC thermostats and controls", 1, true, false, 32, 5, "Stage 6: MEP Phase 2", 6, null },
                    { new Guid("10000006-0000-0000-0000-000000000006"), "STAGE6-AIROUT", "Air Outlet", null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Installation of HVAC air outlets and diffusers", 1, true, false, 33, 6, "Stage 6: MEP Phase 2", 6, null },
                    { new Guid("10000006-0000-0000-0000-000000000007"), "STAGE6-SPRINKLER", "Sprinkler", null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Installation of fire sprinkler heads", 1, true, false, 34, 7, "Stage 6: MEP Phase 2", 6, null },
                    { new Guid("10000006-0000-0000-0000-000000000008"), "STAGE6-SMOKE", "Smoke Detector", null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Installation of smoke detectors and fire alarm devices", 1, true, false, 35, 8, "Stage 6: MEP Phase 2", 6, null },
                    { new Guid("10000006-0000-0000-0000-000000000009"), "STAGE6-WIR5", "WIR-5", null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Work Inspection Request - Final MEP Inspection", 1, true, true, 36, 9, "Stage 6: MEP Phase 2", 6, "WIR-5" },
                    { new Guid("10000007-0000-0000-0000-000000000001"), "STAGE7-IRON", "Ironmongery Installation", null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Installation of door handles, locks, hinges and other hardware", 1, true, false, 37, 1, "Stage 7: Final Inspection & Dispatch", 7, null },
                    { new Guid("10000007-0000-0000-0000-000000000002"), "STAGE7-INSP", "Final Inspection", null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Comprehensive final quality inspection of completed module", 1, true, false, 38, 2, "Stage 7: Final Inspection & Dispatch", 7, null },
                    { new Guid("10000007-0000-0000-0000-000000000003"), "STAGE7-WRAP", "Module Wrapping", null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Protective wrapping of module for delivery to site", 1, true, false, 39, 3, "Stage 7: Final Inspection & Dispatch", 7, null },
                    { new Guid("10000007-0000-0000-0000-000000000004"), "STAGE7-WIR6", "WIR-6", null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Work Inspection Request - Final QC Clearance for Dispatch", 1, true, true, 40, 4, "Stage 7: Final Inspection & Dispatch", 7, "WIR-6" },
                    { new Guid("10000008-0000-0000-0000-000000000001"), "STAGE8-RFID", "RFID Tracking to Site", null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "RFID tag activation and tracking during transport to site", 1, true, false, 41, 1, "Stage 8: Site Installation", 8, null },
                    { new Guid("10000008-0000-0000-0000-000000000002"), "STAGE8-INST", "Installation on Project", null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Physical installation and positioning of module at project site", 1, true, false, 42, 2, "Stage 8: Site Installation", 8, null },
                    { new Guid("10000008-0000-0000-0000-000000000003"), "STAGE8-COMP", "Box Completion", null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Final verification and completion signoff at site", 1, true, false, 43, 3, "Stage 8: Site Installation", 8, null }
                });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "DepartmentId", "Code", "CreatedDate", "DepartmentName", "Description", "IsActive", "Location", "ManagerId" },
                values: new object[,]
                {
                    { new Guid("d1000000-0000-0000-0000-000000000001"), "IT", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "IT", null, true, null, null },
                    { new Guid("d2000000-0000-0000-0000-000000000002"), "MGMT", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Management", null, true, null, null },
                    { new Guid("d3000000-0000-0000-0000-000000000003"), "ENG", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Engineering", null, true, null, null },
                    { new Guid("d4000000-0000-0000-0000-000000000004"), "CONST", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Construction", null, true, null, null },
                    { new Guid("d5000000-0000-0000-0000-000000000005"), "QLTY", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Quality", null, true, null, null },
                    { new Guid("d6000000-0000-0000-0000-000000000006"), "PROC", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Procurement", null, true, null, null },
                    { new Guid("d7000000-0000-0000-0000-000000000007"), "HSE", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "HSE", null, true, null, null },
                    { new Guid("d8000000-0000-0000-0000-000000000008"), "DBX", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "DuBox", null, true, null, null },
                    { new Guid("d9000000-0000-0000-0000-000000000009"), "DPD", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "DuPod", null, true, null, null }
                });

            migrationBuilder.InsertData(
                table: "Groups",
                columns: new[] { "GroupId", "CreatedDate", "Description", "GroupName", "IsActive" },
                values: new object[,]
                {
                    { new Guid("a1111111-1111-1111-1111-111111111111"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Senior management and administrators", "Management", true },
                    { new Guid("a2222222-2222-2222-2222-222222222222"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Engineering department - design and site", "Engineering", true },
                    { new Guid("a3333333-3333-3333-3333-333333333333"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Site construction team", "Construction", true },
                    { new Guid("a4444444-4444-4444-4444-444444444444"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Quality control and inspection team", "QualityControl", true },
                    { new Guid("a5555555-5555-5555-5555-555555555555"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Procurement and logistics team", "Procurement", true },
                    { new Guid("a6666666-6666-6666-6666-666666666666"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Health, Safety, and Environment team", "HSE", true },
                    { new Guid("a7777777-7777-7777-7777-777777777777"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Modular construction team - DuBox", "DuBoxTeam", true },
                    { new Guid("a8888888-8888-8888-8888-888888888888"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Plug-and-play modular solutions - DuPod", "DuPodTeam", true }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RoleId", "CreatedDate", "Description", "IsActive", "RoleName" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Full system administration access", true, "SystemAdmin" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Manage projects and teams", true, "ProjectManager" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Oversee construction site activities", true, "SiteEngineer" },
                    { new Guid("44444444-4444-4444-4444-444444444444"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Supervise construction workers", true, "Foreman" },
                    { new Guid("55555555-5555-5555-5555-555555555555"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Quality control and inspection", true, "QCInspector" },
                    { new Guid("66666666-6666-6666-6666-666666666666"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Handle material procurement", true, "ProcurementOfficer" },
                    { new Guid("77777777-7777-7777-7777-777777777777"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Health, Safety, and Environment oversight", true, "HSEOfficer" },
                    { new Guid("88888888-8888-8888-8888-888888888888"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Design and BIM modeling", true, "DesignEngineer" },
                    { new Guid("99999999-9999-9999-9999-999999999999"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Cost estimation and budgeting", true, "CostEstimator" },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Read-only access to projects", true, "Viewer" }
                });

            migrationBuilder.InsertData(
                table: "GroupRoles",
                columns: new[] { "GroupRoleId", "AssignedDate", "GroupId", "RoleId" },
                values: new object[,]
                {
                    { new Guid("b0000001-0000-0000-0000-000000000001"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("a8888888-8888-8888-8888-888888888888"), new Guid("88888888-8888-8888-8888-888888888888") },
                    { new Guid("b0000002-0000-0000-0000-000000000002"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("a8888888-8888-8888-8888-888888888888"), new Guid("22222222-2222-2222-2222-222222222222") },
                    { new Guid("b1111111-1111-1111-1111-111111111111"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("a1111111-1111-1111-1111-111111111111"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("b2222222-2222-2222-2222-222222222222"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("a1111111-1111-1111-1111-111111111111"), new Guid("22222222-2222-2222-2222-222222222222") },
                    { new Guid("b3333333-3333-3333-3333-333333333333"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("a2222222-2222-2222-2222-222222222222"), new Guid("33333333-3333-3333-3333-333333333333") },
                    { new Guid("b4444444-4444-4444-4444-444444444444"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("a2222222-2222-2222-2222-222222222222"), new Guid("88888888-8888-8888-8888-888888888888") },
                    { new Guid("b5555555-5555-5555-5555-555555555555"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("a2222222-2222-2222-2222-222222222222"), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa") },
                    { new Guid("b6666666-6666-6666-6666-666666666666"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("a3333333-3333-3333-3333-333333333333"), new Guid("44444444-4444-4444-4444-444444444444") },
                    { new Guid("b7777777-7777-7777-7777-777777777777"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("a3333333-3333-3333-3333-333333333333"), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa") },
                    { new Guid("b8888888-8888-8888-8888-888888888888"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("a4444444-4444-4444-4444-444444444444"), new Guid("55555555-5555-5555-5555-555555555555") },
                    { new Guid("b9999999-9999-9999-9999-999999999999"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("a4444444-4444-4444-4444-444444444444"), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa") },
                    { new Guid("baaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("a5555555-5555-5555-5555-555555555555"), new Guid("66666666-6666-6666-6666-666666666666") },
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("a5555555-5555-5555-5555-555555555555"), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa") },
                    { new Guid("bccccccc-cccc-cccc-cccc-cccccccccccc"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("a6666666-6666-6666-6666-666666666666"), new Guid("77777777-7777-7777-7777-777777777777") },
                    { new Guid("bddddddd-dddd-dddd-dddd-dddddddddddd"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("a6666666-6666-6666-6666-666666666666"), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa") },
                    { new Guid("beeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("a7777777-7777-7777-7777-777777777777"), new Guid("88888888-8888-8888-8888-888888888888") },
                    { new Guid("bfffffff-ffff-ffff-ffff-ffffffffffff"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("a7777777-7777-7777-7777-777777777777"), new Guid("22222222-2222-2222-2222-222222222222") }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CreatedDate", "DepartmentId", "Email", "FullName", "IsActive", "LastLoginDate", "PasswordHash" },
                values: new object[,]
                {
                    { new Guid("f0000001-0000-0000-0000-000000000001"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("d4000000-0000-0000-0000-000000000004"), "rania.khalifa@groupamana.com", "Rania Khalifa", true, null, "AQIDBAUGBwgJCgsMDQ4PEL47IPnruFDHjvgQn4gt+aHjj9Wvhi+9Lw6p4tvk8d7H" },
                    { new Guid("f0000002-0000-0000-0000-000000000002"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("d3000000-0000-0000-0000-000000000003"), "salim.rashid@groupamana.com", "Salim Rashid", true, null, "AQIDBAUGBwgJCgsMDQ4PEL47IPnruFDHjvgQn4gt+aHjj9Wvhi+9Lw6p4tvk8d7H" },
                    { new Guid("f0000003-0000-0000-0000-000000000003"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("d2000000-0000-0000-0000-000000000002"), "huda.almarri@groupamana.com", "Huda Al Marri", true, null, "AQIDBAUGBwgJCgsMDQ4PEL47IPnruFDHjvgQn4gt+aHjj9Wvhi+9Lw6p4tvk8d7H" },
                    { new Guid("f0000004-0000-0000-0000-000000000004"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("d8000000-0000-0000-0000-000000000008"), "faisal.sultan@groupamana.com", "Faisal Sultan", true, null, "AQIDBAUGBwgJCgsMDQ4PEL47IPnruFDHjvgQn4gt+aHjj9Wvhi+9Lw6p4tvk8d7H" },
                    { new Guid("f1111111-1111-1111-1111-111111111111"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("d1000000-0000-0000-0000-000000000001"), "admin@groupamana.com", "System Administrator", true, null, "AQIDBAUGBwgJCgsMDQ4PEL47IPnruFDHjvgQn4gt+aHjj9Wvhi+9Lw6p4tvk8d7H" },
                    { new Guid("f2222222-2222-2222-2222-222222222222"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("d2000000-0000-0000-0000-000000000002"), "ahmed.almazrouei@groupamana.com", "Ahmed Al Mazrouei", true, null, "AQIDBAUGBwgJCgsMDQ4PEL47IPnruFDHjvgQn4gt+aHjj9Wvhi+9Lw6p4tvk8d7H" },
                    { new Guid("f3333333-3333-3333-3333-333333333333"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("d3000000-0000-0000-0000-000000000003"), "sara.alkhan@groupamana.com", "Sara Al Khan", true, null, "AQIDBAUGBwgJCgsMDQ4PEL47IPnruFDHjvgQn4gt+aHjj9Wvhi+9Lw6p4tvk8d7H" },
                    { new Guid("f4444444-4444-4444-4444-444444444444"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("d4000000-0000-0000-0000-000000000004"), "mohammed.hassan@groupamana.com", "Mohammed Hassan", true, null, "AQIDBAUGBwgJCgsMDQ4PEL47IPnruFDHjvgQn4gt+aHjj9Wvhi+9Lw6p4tvk8d7H" },
                    { new Guid("f5555555-5555-5555-5555-555555555555"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("d5000000-0000-0000-0000-000000000005"), "fatima.alali@groupamana.com", "Fatima Al Ali", true, null, "AQIDBAUGBwgJCgsMDQ4PEL47IPnruFDHjvgQn4gt+aHjj9Wvhi+9Lw6p4tvk8d7H" },
                    { new Guid("f6666666-6666-6666-6666-666666666666"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("d1000000-0000-0000-0000-000000000001"), "khalid.omar@groupamana.com", "Khalid Omar", true, null, "AQIDBAUGBwgJCgsMDQ4PEL47IPnruFDHjvgQn4gt+aHjj9Wvhi+9Lw6p4tvk8d7H" },
                    { new Guid("f7777777-7777-7777-7777-777777777777"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("d6000000-0000-0000-0000-000000000006"), "ali.mohammed@groupamana.com", "Ali Mohammed", true, null, "AQIDBAUGBwgJCgsMDQ4PEL47IPnruFDHjvgQn4gt+aHjj9Wvhi+9Lw6p4tvk8d7H" },
                    { new Guid("f8888888-8888-8888-8888-888888888888"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("d6000000-0000-0000-0000-000000000006"), "omar.saleh@groupamana.com", "Omar Saleh", true, null, "AQIDBAUGBwgJCgsMDQ4PEL47IPnruFDHjvgQn4gt+aHjj9Wvhi+9Lw6p4tvk8d7H" },
                    { new Guid("f9999999-9999-9999-9999-999999999999"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("d7000000-0000-0000-0000-000000000007"), "youssef.ahmed@groupamana.com", "Youssef Ahmed", true, null, "AQIDBAUGBwgJCgsMDQ4PEL47IPnruFDHjvgQn4gt+aHjj9Wvhi+9Lw6p4tvk8d7H" },
                    { new Guid("faaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("d7000000-0000-0000-0000-000000000007"), "layla.ibrahim@groupamana.com", "Layla Ibrahim", true, null, "AQIDBAUGBwgJCgsMDQ4PEL47IPnruFDHjvgQn4gt+aHjj9Wvhi+9Lw6p4tvk8d7H" },
                    { new Guid("fbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("d8000000-0000-0000-0000-000000000008"), "hamza.khalil@groupamana.com", "Hamza Khalil", true, null, "AQIDBAUGBwgJCgsMDQ4PEL47IPnruFDHjvgQn4gt+aHjj9Wvhi+9Lw6p4tvk8d7H" },
                    { new Guid("fccccccc-cccc-cccc-cccc-cccccccccccc"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("d8000000-0000-0000-0000-000000000008"), "noor.alhassan@groupamana.com", "Noor Al Hassan", true, null, "AQIDBAUGBwgJCgsMDQ4PEL47IPnruFDHjvgQn4gt+aHjj9Wvhi+9Lw6p4tvk8d7H" },
                    { new Guid("fddddddd-dddd-dddd-dddd-dddddddddddd"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("d9000000-0000-0000-0000-000000000009"), "zaid.mansour@groupamana.com", "Zaid Mansour", true, null, "AQIDBAUGBwgJCgsMDQ4PEL47IPnruFDHjvgQn4gt+aHjj9Wvhi+9Lw6p4tvk8d7H" },
                    { new Guid("feeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("d9000000-0000-0000-0000-000000000009"), "maryam.Said@groupamana.com", "Maryam Said", true, null, "AQIDBAUGBwgJCgsMDQ4PEL47IPnruFDHjvgQn4gt+aHjj9Wvhi+9Lw6p4tvk8d7H" },
                    { new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("d5000000-0000-0000-0000-000000000005"), "tariq.abdullah@groupamana.com", "Tariq Abdullah", true, null, "AQIDBAUGBwgJCgsMDQ4PEL47IPnruFDHjvgQn4gt+aHjj9Wvhi+9Lw6p4tvk8d7H" }
                });

            migrationBuilder.InsertData(
                table: "UserGroups",
                columns: new[] { "UserGroupId", "GroupId", "JoinedDate", "UserId" },
                values: new object[,]
                {
                    { new Guid("c0000001-0000-0000-0000-000000000001"), new Guid("a7777777-7777-7777-7777-777777777777"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("f0000001-0000-0000-0000-000000000001") },
                    { new Guid("c0000002-0000-0000-0000-000000000002"), new Guid("a7777777-7777-7777-7777-777777777777"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("f0000002-0000-0000-0000-000000000002") },
                    { new Guid("c0000003-0000-0000-0000-000000000003"), new Guid("a8888888-8888-8888-8888-888888888888"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("f0000003-0000-0000-0000-000000000003") },
                    { new Guid("c0000004-0000-0000-0000-000000000004"), new Guid("a8888888-8888-8888-8888-888888888888"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("f0000004-0000-0000-0000-000000000004") },
                    { new Guid("c1111111-1111-1111-1111-111111111111"), new Guid("a1111111-1111-1111-1111-111111111111"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("f1111111-1111-1111-1111-111111111111") },
                    { new Guid("c2222222-2222-2222-2222-222222222222"), new Guid("a1111111-1111-1111-1111-111111111111"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("f2222222-2222-2222-2222-222222222222") },
                    { new Guid("c3333333-3333-3333-3333-333333333333"), new Guid("a1111111-1111-1111-1111-111111111111"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("f3333333-3333-3333-3333-333333333333") },
                    { new Guid("c4444444-4444-4444-4444-444444444444"), new Guid("a2222222-2222-2222-2222-222222222222"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("f4444444-4444-4444-4444-444444444444") },
                    { new Guid("c5555555-5555-5555-5555-555555555555"), new Guid("a2222222-2222-2222-2222-222222222222"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("f5555555-5555-5555-5555-555555555555") },
                    { new Guid("c6666666-6666-6666-6666-666666666666"), new Guid("a2222222-2222-2222-2222-222222222222"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("f6666666-6666-6666-6666-666666666666") },
                    { new Guid("c7777777-7777-7777-7777-777777777777"), new Guid("a3333333-3333-3333-3333-333333333333"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("f7777777-7777-7777-7777-777777777777") },
                    { new Guid("c8888888-8888-8888-8888-888888888888"), new Guid("a3333333-3333-3333-3333-333333333333"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("f8888888-8888-8888-8888-888888888888") },
                    { new Guid("c9999999-9999-9999-9999-999999999999"), new Guid("a3333333-3333-3333-3333-333333333333"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("f9999999-9999-9999-9999-999999999999") },
                    { new Guid("caaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new Guid("a4444444-4444-4444-4444-444444444444"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("faaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa") },
                    { new Guid("cbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new Guid("a4444444-4444-4444-4444-444444444444"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("fbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb") },
                    { new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), new Guid("a5555555-5555-5555-5555-555555555555"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("fccccccc-cccc-cccc-cccc-cccccccccccc") },
                    { new Guid("cddddddd-dddd-dddd-dddd-dddddddddddd"), new Guid("a5555555-5555-5555-5555-555555555555"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("fddddddd-dddd-dddd-dddd-dddddddddddd") },
                    { new Guid("ceeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), new Guid("a6666666-6666-6666-6666-666666666666"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("feeeeeee-eeee-eeee-eeee-eeeeeeeeeeee") },
                    { new Guid("cfffffff-ffff-ffff-ffff-ffffffffffff"), new Guid("a6666666-6666-6666-6666-666666666666"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff") }
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "UserRoleId", "AssignedDate", "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("d1111111-1111-1111-1111-111111111111"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999999"), new Guid("f2222222-2222-2222-2222-222222222222") },
                    { new Guid("d2222222-2222-2222-2222-222222222222"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("22222222-2222-2222-2222-222222222222"), new Guid("f4444444-4444-4444-4444-444444444444") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityDependencies_BoxActivityId",
                table: "ActivityDependencies",
                column: "BoxActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityDependencies_BoxActivityId1",
                table: "ActivityDependencies",
                column: "BoxActivityId1");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityDependencies_BoxActivityId2",
                table: "ActivityDependencies",
                column: "BoxActivityId2");

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
                name: "IX_ActivityMaster_StageNumber_SequenceInStage",
                table: "ActivityMaster",
                columns: new[] { "StageNumber", "SequenceInStage" });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityMaterials_BoxActivityId",
                table: "ActivityMaterials",
                column: "BoxActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityMaterials_MaterialId",
                table: "ActivityMaterials",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_BoxActivities_ActivityMasterId",
                table: "BoxActivities",
                column: "ActivityMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_BoxActivities_AssignedMemberId",
                table: "BoxActivities",
                column: "AssignedMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_BoxActivities_BoxId_Sequence",
                table: "BoxActivities",
                columns: new[] { "BoxId", "Sequence" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BoxActivities_BoxId_Status",
                table: "BoxActivities",
                columns: new[] { "BoxId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_BoxActivities_TeamId",
                table: "BoxActivities",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_BoxActivities_TeamMemberId",
                table: "BoxActivities",
                column: "TeamMemberId");

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
                name: "IX_Boxes_ProjectId_BoxTag",
                table: "Boxes",
                columns: new[] { "ProjectId", "BoxTag" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Boxes_QRCodeString",
                table: "Boxes",
                column: "QRCodeString",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Boxes_Status_ProjectId",
                table: "Boxes",
                columns: new[] { "Status", "ProjectId" });

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
                name: "IX_Departments_ManagerId",
                table: "Departments",
                column: "ManagerId",
                unique: true,
                filter: "[ManagerId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FactoryLocations_LocationCode",
                table: "FactoryLocations",
                column: "LocationCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroupRoles_GroupId_RoleId",
                table: "GroupRoles",
                columns: new[] { "GroupId", "RoleId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroupRoles_RoleId",
                table: "GroupRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_GroupName",
                table: "Groups",
                column: "GroupName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Materials_MaterialCode",
                table: "Materials",
                column: "MaterialCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MaterialTransactions_BoxActivityId",
                table: "MaterialTransactions",
                column: "BoxActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialTransactions_BoxId",
                table: "MaterialTransactions",
                column: "BoxId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialTransactions_MaterialId",
                table: "MaterialTransactions",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialTransactions_PerformedById",
                table: "MaterialTransactions",
                column: "PerformedById");

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
                name: "IX_ProgressUpdates_BoxId_UpdateDate",
                table: "ProgressUpdates",
                columns: new[] { "BoxId", "UpdateDate" });

            migrationBuilder.CreateIndex(
                name: "IX_ProgressUpdates_TeamId",
                table: "ProgressUpdates",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgressUpdates_UpdatedBy",
                table: "ProgressUpdates",
                column: "UpdatedBy");

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
                name: "IX_Roles_RoleName",
                table: "Roles",
                column: "RoleName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeamMembers_TeamId",
                table: "TeamMembers",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamMembers_UserId",
                table: "TeamMembers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_DepartmentId",
                table: "Teams",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_TeamCode",
                table: "Teams",
                column: "TeamCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teams_TeamLeaderMemberId",
                table: "Teams",
                column: "TeamLeaderMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroups_GroupId",
                table: "UserGroups",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroups_UserId_GroupId",
                table: "UserGroups",
                columns: new[] { "UserId", "GroupId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId_RoleId",
                table: "UserRoles",
                columns: new[] { "UserId", "RoleId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_DepartmentId",
                table: "Users",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WIRChecklistItems_WIRId",
                table: "WIRChecklistItems",
                column: "WIRId");

            migrationBuilder.CreateIndex(
                name: "IX_WIRCheckpoints_BoxId",
                table: "WIRCheckpoints",
                column: "BoxId");

            migrationBuilder.CreateIndex(
                name: "IX_WIRRecords_BoxActivityId_WIRCode",
                table: "WIRRecords",
                columns: new[] { "BoxActivityId", "WIRCode" });

            migrationBuilder.CreateIndex(
                name: "IX_WIRRecords_InspectedBy",
                table: "WIRRecords",
                column: "InspectedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WIRRecords_RequestedBy",
                table: "WIRRecords",
                column: "RequestedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WIRRecords_Status_RequestedDate",
                table: "WIRRecords",
                columns: new[] { "Status", "RequestedDate" });

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityDependencies_BoxActivities_BoxActivityId",
                table: "ActivityDependencies",
                column: "BoxActivityId",
                principalTable: "BoxActivities",
                principalColumn: "BoxActivityId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityDependencies_BoxActivities_BoxActivityId1",
                table: "ActivityDependencies",
                column: "BoxActivityId1",
                principalTable: "BoxActivities",
                principalColumn: "BoxActivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityDependencies_BoxActivities_BoxActivityId2",
                table: "ActivityDependencies",
                column: "BoxActivityId2",
                principalTable: "BoxActivities",
                principalColumn: "BoxActivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityDependencies_BoxActivities_PredecessorActivityId",
                table: "ActivityDependencies",
                column: "PredecessorActivityId",
                principalTable: "BoxActivities",
                principalColumn: "BoxActivityId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityMaterials_BoxActivities_BoxActivityId",
                table: "ActivityMaterials",
                column: "BoxActivityId",
                principalTable: "BoxActivities",
                principalColumn: "BoxActivityId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BoxActivities_TeamMembers_AssignedMemberId",
                table: "BoxActivities",
                column: "AssignedMemberId",
                principalTable: "TeamMembers",
                principalColumn: "TeamMemberId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_BoxActivities_TeamMembers_TeamMemberId",
                table: "BoxActivities",
                column: "TeamMemberId",
                principalTable: "TeamMembers",
                principalColumn: "TeamMemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_BoxActivities_Teams_TeamId",
                table: "BoxActivities",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_DailyProductionLog_Teams_TeamId",
                table: "DailyProductionLog",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Users_ManagerId",
                table: "Departments",
                column: "ManagerId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ProgressUpdates_Teams_TeamId",
                table: "ProgressUpdates",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_TeamMembers_Teams_TeamId",
                table: "TeamMembers",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teams_TeamMembers_TeamLeaderMemberId",
                table: "Teams");

            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Users_ManagerId",
                table: "Departments");

            migrationBuilder.DropTable(
                name: "ActivityDependencies");

            migrationBuilder.DropTable(
                name: "ActivityMaterials");

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
                name: "GroupRoles");

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
                name: "UserGroups");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "WIRChecklistItems");

            migrationBuilder.DropTable(
                name: "WIRRecords");

            migrationBuilder.DropTable(
                name: "CostCategories");

            migrationBuilder.DropTable(
                name: "FactoryLocations");

            migrationBuilder.DropTable(
                name: "Materials");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "WIRCheckpoints");

            migrationBuilder.DropTable(
                name: "BoxActivities");

            migrationBuilder.DropTable(
                name: "ActivityMaster");

            migrationBuilder.DropTable(
                name: "Boxes");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "TeamMembers");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Departments");
        }
    }
}
