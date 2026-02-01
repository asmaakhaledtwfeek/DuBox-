using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedCompleteWIRChecklistData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "PredefinedChecklistItems",
                columns: new[] { "PredefinedItemId", "CategoryId", "CheckpointDescription", "CreatedDate", "IsActive", "ItemNumber", "ReferenceId", "Sequence", "WIRNumber" },
                values: new object[,]
                {
                    { new Guid("20000001-0000-0000-0001-000000000001"), new Guid("40000001-0000-0000-0000-000000000015"), "Is there material approval for received item?", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "1", new Guid("30000001-0000-0000-0000-000000000005"), 1, "WIR-1" },
                    { new Guid("20000001-0000-0000-0001-000000000002"), new Guid("40000001-0000-0000-0000-000000000015"), "Is Manufacturer Name as per material approval?", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "2", new Guid("30000001-0000-0000-0000-000000000003"), 2, "WIR-1" },
                    { new Guid("20000001-0000-0000-0001-000000000003"), new Guid("40000001-0000-0000-0000-000000000015"), "Is Supplier Name as per material approval?", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "3", new Guid("30000001-0000-0000-0000-000000000003"), 3, "WIR-1" },
                    { new Guid("20000001-0000-0000-0001-000000000004"), new Guid("40000001-0000-0000-0000-000000000015"), "Is received material matching with approved sample?", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "4", new Guid("30000001-0000-0000-0000-000000000036"), 4, "WIR-1" },
                    { new Guid("20000001-0000-0000-0001-000000000005"), new Guid("40000001-0000-0000-0000-000000000015"), "Related mill test certificate (or) test reports?", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "5", new Guid("30000001-0000-0000-0000-000000000027"), 5, "WIR-1" },
                    { new Guid("20000001-0000-0000-0001-000000000006"), new Guid("40000001-0000-0000-0000-000000000015"), "Check for any defects", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "6", new Guid("30000001-0000-0000-0000-000000000004"), 6, "WIR-1" },
                    { new Guid("20000001-0000-0000-0001-000000000007"), new Guid("40000001-0000-0000-0000-000000000015"), "Check Expiry date", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "7", new Guid("30000001-0000-0000-0000-000000000004"), 7, "WIR-1" },
                    { new Guid("20000001-0000-0000-0001-000000000008"), new Guid("40000001-0000-0000-0000-000000000015"), "Check Item / product description", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "8", new Guid("30000001-0000-0000-0000-000000000004"), 8, "WIR-1" },
                    { new Guid("20000001-0000-0000-0001-000000000009"), new Guid("40000001-0000-0000-0000-000000000015"), "Check Item / product code", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "9", new Guid("30000001-0000-0000-0000-000000000004"), 9, "WIR-1" },
                    { new Guid("20000001-0000-0000-0001-000000000010"), new Guid("40000001-0000-0000-0000-000000000015"), "Check Dimensions (length, width, thickness etc.)", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "10", new Guid("30000001-0000-0000-0000-000000000002"), 10, "WIR-1" },
                    { new Guid("20000001-0000-0000-0001-000000000011"), new Guid("40000001-0000-0000-0000-000000000015"), "Check Colour", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "11", new Guid("30000001-0000-0000-0000-000000000030"), 11, "WIR-1" },
                    { new Guid("20000001-0000-0000-0001-000000000012"), new Guid("40000001-0000-0000-0000-000000000015"), "Check for Packaging Conditions", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "12", new Guid("30000001-0000-0000-0000-000000000004"), 12, "WIR-1" },
                    { new Guid("20000001-0000-0000-0001-000000000013"), new Guid("40000001-0000-0000-0000-000000000015"), "Check for received Quantity (approx) as per DO", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "13", new Guid("30000001-0000-0000-0000-000000000004"), 13, "WIR-1" },
                    { new Guid("20000001-0000-0000-0001-000000000014"), new Guid("40000001-0000-0000-0000-000000000015"), "Check the area of storage as per MSDS", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "14", new Guid("30000001-0000-0000-0000-000000000025"), 14, "WIR-1" },
                    { new Guid("20000001-0000-0000-0001-000000000101"), new Guid("40000001-0000-0000-0000-000000000016"), "Review documents for received materials", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "1", new Guid("30000001-0000-0000-0000-000000000004"), 101, "WIR-1" },
                    { new Guid("20000001-0000-0000-0001-000000000102"), new Guid("40000001-0000-0000-0000-000000000016"), "Materials outside visual checking", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "2", new Guid("30000001-0000-0000-0000-000000000004"), 102, "WIR-1" },
                    { new Guid("20000001-0000-0000-0001-000000000103"), new Guid("40000001-0000-0000-0000-000000000016"), "Check for any damages (General & Visual)", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "3", new Guid("30000001-0000-0000-0000-000000000004"), 103, "WIR-1" },
                    { new Guid("20000001-0000-0000-0001-000000000104"), new Guid("40000001-0000-0000-0000-000000000016"), "Verify original bill of landing / Delivery Note", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "4", new Guid("30000001-0000-0000-0000-000000000004"), 104, "WIR-1" },
                    { new Guid("20000001-0000-0000-0001-000000000105"), new Guid("40000001-0000-0000-0000-000000000016"), "Supplier Certificate / Warranty letter", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "5", new Guid("30000001-0000-0000-0000-000000000026"), 105, "WIR-1" },
                    { new Guid("20000001-0000-0000-0001-000000000106"), new Guid("40000001-0000-0000-0000-000000000016"), "Check and Verify the material as per delivery list / details", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "6", new Guid("30000001-0000-0000-0000-000000000004"), 106, "WIR-1" },
                    { new Guid("20000001-0000-0000-0001-000000000107"), new Guid("40000001-0000-0000-0000-000000000016"), "Check the accessories", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "7", new Guid("30000001-0000-0000-0000-000000000004"), 107, "WIR-1" },
                    { new Guid("20000001-0000-0000-0001-000000000108"), new Guid("40000001-0000-0000-0000-000000000016"), "Check the Name Plate", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "8", new Guid("30000001-0000-0000-0000-000000000002"), 108, "WIR-1" },
                    { new Guid("20000001-0000-0000-0001-000000000109"), new Guid("40000001-0000-0000-0000-000000000016"), "Materials Storage and preservation as per manufacturer", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "9", new Guid("30000001-0000-0000-0000-000000000026"), 109, "WIR-1" },
                    { new Guid("20000001-0000-0000-0001-000000000110"), new Guid("40000001-0000-0000-0000-000000000016"), "Check the identification of components", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "10", new Guid("30000001-0000-0000-0000-000000000004"), 110, "WIR-1" },
                    { new Guid("20000001-0000-0000-0001-000000000111"), new Guid("40000001-0000-0000-0000-000000000016"), "Check the rating as per approved drawing", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "11", new Guid("30000001-0000-0000-0000-000000000002"), 111, "WIR-1" },
                    { new Guid("20000001-0000-0000-0001-000000000112"), new Guid("40000001-0000-0000-0000-000000000016"), "Check the loose part", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "12", new Guid("30000001-0000-0000-0000-000000000004"), 112, "WIR-1" },
                    { new Guid("20000001-0000-0000-0001-000000000113"), new Guid("40000001-0000-0000-0000-000000000016"), "Check the dimension of delivered equipment as per approved drawing", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "13", new Guid("30000001-0000-0000-0000-000000000002"), 113, "WIR-1" },
                    { new Guid("20000001-0000-0000-0001-000000000114"), new Guid("40000001-0000-0000-0000-000000000016"), "Check the availability of spare breakers / relays/ terminals", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "14", new Guid("30000001-0000-0000-0000-000000000002"), 114, "WIR-1" },
                    { new Guid("20000001-0000-0000-0001-000000000115"), new Guid("40000001-0000-0000-0000-000000000016"), "Delivered material photos", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "15", new Guid("30000001-0000-0000-0000-000000000004"), 115, "WIR-1" },
                    { new Guid("20000001-0000-0000-0001-000000000116"), new Guid("40000001-0000-0000-0000-000000000016"), "Material Site test", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "16", new Guid("30000001-0000-0000-0000-000000000008"), 116, "WIR-1" },
                    { new Guid("20000001-0000-0000-0004-000000000001"), new Guid("40000001-0000-0000-0000-000000000017"), "Ensure method statement, materials and shop drawings are approved", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A1", new Guid("30000001-0000-0000-0000-000000000007"), 1, "WIR-4" },
                    { new Guid("20000001-0000-0000-0004-000000000002"), new Guid("40000001-0000-0000-0000-000000000017"), "Ensure materials are stored under dry, clean, shaded area, away from sunlight and other sources of heat", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A2", new Guid("30000001-0000-0000-0000-000000000004"), 2, "WIR-4" },
                    { new Guid("20000001-0000-0000-0004-000000000003"), new Guid("40000001-0000-0000-0000-000000000017"), "Check the expiry date of the material prior to applications", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A3", new Guid("30000001-0000-0000-0000-000000000004"), 3, "WIR-4" },
                    { new Guid("20000001-0000-0000-0004-000000000004"), new Guid("40000001-0000-0000-0000-000000000018"), "Drawing Stamp & Signature", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "B1", new Guid("30000001-0000-0000-0000-000000000002"), 4, "WIR-4" },
                    { new Guid("20000001-0000-0000-0004-000000000005"), new Guid("40000001-0000-0000-0000-000000000018"), "Element Tag, QC Approval for Element", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "B2", new Guid("30000001-0000-0000-0000-000000000004"), 5, "WIR-4" },
                    { new Guid("20000001-0000-0000-0004-000000000006"), new Guid("40000001-0000-0000-0000-000000000018"), "Floor Setting Out", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "B3", new Guid("30000001-0000-0000-0000-000000000002"), 6, "WIR-4" },
                    { new Guid("20000001-0000-0000-0004-000000000007"), new Guid("40000001-0000-0000-0000-000000000019"), "Erection with temporary support", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "C1", new Guid("30000001-0000-0000-0000-000000000002"), 7, "WIR-4" },
                    { new Guid("20000001-0000-0000-0004-000000000008"), new Guid("40000001-0000-0000-0000-000000000019"), "Panel to panel connections", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "C2", new Guid("30000001-0000-0000-0000-000000000002"), 8, "WIR-4" },
                    { new Guid("20000001-0000-0000-0004-000000000009"), new Guid("40000001-0000-0000-0000-000000000019"), "Dimensions (outer, inner and diagonal), line and level", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "C3", new Guid("30000001-0000-0000-0000-000000000002"), 9, "WIR-4" },
                    { new Guid("20000001-0000-0000-0004-000000000010"), new Guid("40000001-0000-0000-0000-000000000020"), "Backer Rod / Shim Pad", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "D1", new Guid("30000001-0000-0000-0000-000000000002"), 10, "WIR-4" },
                    { new Guid("20000001-0000-0000-0004-000000000011"), new Guid("40000001-0000-0000-0000-000000000020"), "Slab Position / Level", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "D2", new Guid("30000001-0000-0000-0000-000000000002"), 11, "WIR-4" },
                    { new Guid("20000001-0000-0000-0004-000000000012"), new Guid("40000001-0000-0000-0000-000000000020"), "Panel to Slab Connections", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "D3", new Guid("30000001-0000-0000-0000-000000000002"), 12, "WIR-4" },
                    { new Guid("20000001-0000-0000-0004-000000000013"), new Guid("40000001-0000-0000-0000-000000000020"), "1000mm FFL to be marked clearly", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "D4", new Guid("30000001-0000-0000-0000-000000000002"), 13, "WIR-4" },
                    { new Guid("20000001-0000-0000-0004-000000000014"), new Guid("40000001-0000-0000-0000-000000000020"), "MEP Clearance", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "D5", new Guid("30000001-0000-0000-0000-000000000004"), 14, "WIR-4" },
                    { new Guid("20000001-0000-0000-0004-000000000015"), new Guid("40000001-0000-0000-0000-000000000020"), "Check the Slab level (shall read 1016mm at 1000m FFL line)", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "D6", new Guid("30000001-0000-0000-0000-000000000002"), 15, "WIR-4" },
                    { new Guid("20000001-0000-0000-0004-000000000016"), new Guid("40000001-0000-0000-0000-000000000020"), "Grouting", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "D7", new Guid("30000001-0000-0000-0000-000000000008"), 16, "WIR-4" },
                    { new Guid("20000001-0000-0000-0004-000000000017"), new Guid("40000001-0000-0000-0000-000000000021"), "Erection with Temporary Support", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "E1", new Guid("30000001-0000-0000-0000-000000000002"), 17, "WIR-4" },
                    { new Guid("20000001-0000-0000-0004-000000000018"), new Guid("40000001-0000-0000-0000-000000000021"), "Panel to Panel Connections", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "E2", new Guid("30000001-0000-0000-0000-000000000002"), 18, "WIR-4" },
                    { new Guid("20000001-0000-0000-0004-000000000019"), new Guid("40000001-0000-0000-0000-000000000021"), "Dimensions (outer, inner and diagonal), Line and Level", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "E3", new Guid("30000001-0000-0000-0000-000000000002"), 19, "WIR-4" },
                    { new Guid("20000001-0000-0000-0004-000000000020"), new Guid("40000001-0000-0000-0000-000000000021"), "Grouting", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "E4", new Guid("30000001-0000-0000-0000-000000000008"), 20, "WIR-4" },
                    { new Guid("20000001-0000-0000-0004-000000000021"), new Guid("40000001-0000-0000-0000-000000000022"), "Backer Rod / Shim Pad", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "F1", new Guid("30000001-0000-0000-0000-000000000002"), 21, "WIR-4" },
                    { new Guid("20000001-0000-0000-0004-000000000022"), new Guid("40000001-0000-0000-0000-000000000022"), "Slab Position / Top Height", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "F2", new Guid("30000001-0000-0000-0000-000000000002"), 22, "WIR-4" },
                    { new Guid("20000001-0000-0000-0004-000000000023"), new Guid("40000001-0000-0000-0000-000000000022"), "Panel to Slab Connections", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "F3", new Guid("30000001-0000-0000-0000-000000000002"), 23, "WIR-4" },
                    { new Guid("20000001-0000-0000-0004-000000000024"), new Guid("40000001-0000-0000-0000-000000000022"), "MEP Clearance", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "F4", new Guid("30000001-0000-0000-0000-000000000004"), 24, "WIR-4" },
                    { new Guid("20000001-0000-0000-0004-000000000025"), new Guid("40000001-0000-0000-0000-000000000022"), "Grouting", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "F5", new Guid("30000001-0000-0000-0000-000000000008"), 25, "WIR-4" },
                    { new Guid("20000001-0000-0000-0004-000000000026"), new Guid("40000001-0000-0000-0000-000000000022"), "Curing", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "F6", new Guid("30000001-0000-0000-0000-000000000008"), 26, "WIR-4" },
                    { new Guid("20000001-0000-0000-0004-000000000027"), new Guid("40000001-0000-0000-0000-000000000023"), "Internal and External Dimension of Box", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "G1", new Guid("30000001-0000-0000-0000-000000000002"), 27, "WIR-4" },
                    { new Guid("20000001-0000-0000-0004-000000000028"), new Guid("40000001-0000-0000-0000-000000000023"), "Check for edges + Angles + grooves + chamfer + Pin holes + Cracks before moving to finishing area", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "G2", new Guid("30000001-0000-0000-0000-000000000004"), 28, "WIR-4" },
                    { new Guid("20000001-0000-0000-0005-000000000001"), new Guid("40000001-0000-0000-0000-000000000024"), "Ensure method statement, materials and drawings (finishing schedule) are approved", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A1", new Guid("30000001-0000-0000-0000-000000000007"), 1, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000002"), new Guid("40000001-0000-0000-0000-000000000024"), "Ensure materials are stored as per manufacturers recommendations", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A2", new Guid("30000001-0000-0000-0000-000000000026"), 2, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000003"), new Guid("40000001-0000-0000-0000-000000000024"), "Verify the expiry date and number of coats of the material prior to applications", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A3", new Guid("30000001-0000-0000-0000-000000000004"), 3, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000004"), new Guid("40000001-0000-0000-0000-000000000025"), "Check substrate is clean, free from contaminants like dust, traces of curing compound, oil and grease", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "B1", new Guid("30000001-0000-0000-0000-000000000004"), 4, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000005"), new Guid("40000001-0000-0000-0000-000000000025"), "Check for repair of surface imperfection and protrusions (if any)", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "B2", new Guid("30000001-0000-0000-0000-000000000004"), 5, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000006"), new Guid("40000001-0000-0000-0000-000000000025"), "Moisture content for the substrate and environmental conditions as per manufacturer recommendations", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "B3", new Guid("30000001-0000-0000-0000-000000000026"), 6, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000007"), new Guid("40000001-0000-0000-0000-000000000025"), "Check the MEP clearance prior to start Painting works", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "B4", new Guid("30000001-0000-0000-0000-000000000004"), 7, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000008"), new Guid("40000001-0000-0000-0000-000000000026"), "Ensure application of Primer as per manufacturers recommendation", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "C1", new Guid("30000001-0000-0000-0000-000000000026"), 8, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000009"), new Guid("40000001-0000-0000-0000-000000000026"), "Ensure application of Stucco as per manufacturers recommendation", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "C2", new Guid("30000001-0000-0000-0000-000000000026"), 9, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000010"), new Guid("40000001-0000-0000-0000-000000000026"), "Touchup, grinding, undulations, corner repairs and pinholes are filled properly", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "C3", new Guid("30000001-0000-0000-0000-000000000004"), 10, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000011"), new Guid("40000001-0000-0000-0000-000000000026"), "Application of first coat of Paint as per manufacturers recommendation", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "C4", new Guid("30000001-0000-0000-0000-000000000026"), 11, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000012"), new Guid("40000001-0000-0000-0000-000000000026"), "Line between two color shades is straight, no Brush marks should be visible", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "C5", new Guid("30000001-0000-0000-0000-000000000004"), 12, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000013"), new Guid("40000001-0000-0000-0000-000000000027"), "Ensure application of Primer as per manufacturers recommendation", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "D1", new Guid("30000001-0000-0000-0000-000000000026"), 13, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000014"), new Guid("40000001-0000-0000-0000-000000000027"), "Ensure application of Filler Coats as per manufacturers recommendation", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "D2", new Guid("30000001-0000-0000-0000-000000000026"), 14, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000015"), new Guid("40000001-0000-0000-0000-000000000027"), "Touch up, grinding, undulations, corner repairs and pinholes are filled properly", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "D3", new Guid("30000001-0000-0000-0000-000000000004"), 15, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000016"), new Guid("40000001-0000-0000-0000-000000000027"), "Application of final coat of Texture Paint as per manufacturers recommendation", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "D4", new Guid("30000001-0000-0000-0000-000000000026"), 16, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000017"), new Guid("40000001-0000-0000-0000-000000000028"), "Ensure method statement, materials and drawings (finishing schedule) are approved", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "E1", new Guid("30000001-0000-0000-0000-000000000007"), 17, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000018"), new Guid("40000001-0000-0000-0000-000000000028"), "Check the Location, Colour & Type of tile as per the approved shop drawings / material submittal", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "E2", new Guid("30000001-0000-0000-0000-000000000023"), 18, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000019"), new Guid("40000001-0000-0000-0000-000000000028"), "Check the setting out / pattern of wall and floor tiles as per the approved drawings", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "E3", new Guid("30000001-0000-0000-0000-000000000002"), 19, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000020"), new Guid("40000001-0000-0000-0000-000000000028"), "Verify the slope and level of the tiles as per the approved drawings", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "E4", new Guid("30000001-0000-0000-0000-000000000002"), 20, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000021"), new Guid("40000001-0000-0000-0000-000000000028"), "Check the application of tile grout as per the manufacturer recommendations", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "E5", new Guid("30000001-0000-0000-0000-000000000026"), 21, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000022"), new Guid("40000001-0000-0000-0000-000000000029"), "Ensure method statement, material submittal and drawings are approved", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "F1", new Guid("30000001-0000-0000-0000-000000000007"), 22, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000023"), new Guid("40000001-0000-0000-0000-000000000029"), "Verify the marking and setting out of the partition walls as per the approved drawings", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "F2", new Guid("30000001-0000-0000-0000-000000000002"), 23, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000024"), new Guid("40000001-0000-0000-0000-000000000029"), "Verify the location, spacing and fixation of the supporting grid as per the approved drawings", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "F3", new Guid("30000001-0000-0000-0000-000000000002"), 24, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000025"), new Guid("40000001-0000-0000-0000-000000000029"), "Verify the fixation of the board as per the approved drawings", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "F4", new Guid("30000001-0000-0000-0000-000000000002"), 25, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000026"), new Guid("40000001-0000-0000-0000-000000000030"), "Ensure method statement, material submittal and drawings are approved", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "G1", new Guid("30000001-0000-0000-0000-000000000007"), 26, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000027"), new Guid("40000001-0000-0000-0000-000000000030"), "Verify the marking of the false ceiling level on the walls as per the approved drawings", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "G2", new Guid("30000001-0000-0000-0000-000000000002"), 27, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000028"), new Guid("40000001-0000-0000-0000-000000000030"), "Verify the location, spacing and fixation of the grid as per the approved drawings", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "G3", new Guid("30000001-0000-0000-0000-000000000002"), 28, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000029"), new Guid("40000001-0000-0000-0000-000000000030"), "Verify the type, fixation, level and alignment of the false ceiling board / tiles", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "G4", new Guid("30000001-0000-0000-0000-000000000002"), 29, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000030"), new Guid("40000001-0000-0000-0000-000000000031"), "Ensure method statement and materials are approved", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "H1", new Guid("30000001-0000-0000-0000-000000000007"), 30, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000031"), new Guid("40000001-0000-0000-0000-000000000031"), "Check substrate is clean, free from contaminants", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "H2", new Guid("30000001-0000-0000-0000-000000000004"), 31, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000032"), new Guid("40000001-0000-0000-0000-000000000031"), "Check the application of coats as per project requirements / manufacturer recommendations", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "H3", new Guid("30000001-0000-0000-0000-000000000026"), 32, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000033"), new Guid("40000001-0000-0000-0000-000000000031"), "Check for any water seepage / leakage after 24 hours or as per the project requirements", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "H4", new Guid("30000001-0000-0000-0000-000000000008"), 33, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000034"), new Guid("40000001-0000-0000-0000-000000000032"), "Ensure method statement, material submittal and drawings are approved", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "I1", new Guid("30000001-0000-0000-0000-000000000007"), 34, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000035"), new Guid("40000001-0000-0000-0000-000000000032"), "Check the color, type, material, coating of door and window materials as per approval", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "I2", new Guid("30000001-0000-0000-0000-000000000003"), 35, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000036"), new Guid("40000001-0000-0000-0000-000000000032"), "Verify the location and clear opening of doors / windows as per the approved drawings", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "I3", new Guid("30000001-0000-0000-0000-000000000002"), 36, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000037"), new Guid("40000001-0000-0000-0000-000000000032"), "Ensure the location and No. of Door hinges provided as per the approved drawings", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "I4", new Guid("30000001-0000-0000-0000-000000000031"), 37, "WIR-5" },
                    { new Guid("20000001-0000-0000-0005-000000000038"), new Guid("40000001-0000-0000-0000-000000000032"), "Ensure required Iron mongery sets are provided as per the door schedule drawings", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "I5", new Guid("30000001-0000-0000-0000-000000000031"), 38, "WIR-5" },
                    { new Guid("20000001-0000-0000-0006-000000000001"), new Guid("40000001-0000-0000-0000-000000000033"), "Ensure method statement, ITP, materials and shop drawings are approved", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "1.1", new Guid("30000001-0000-0000-0000-000000000007"), 1, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000002"), new Guid("40000001-0000-0000-0000-000000000033"), "Check identification tag of the modular", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "1.2", new Guid("30000001-0000-0000-0000-000000000004"), 2, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000003"), new Guid("40000001-0000-0000-0000-000000000033"), "Visually inspect the modular for any defects or damages", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "1.3", new Guid("30000001-0000-0000-0000-000000000004"), 3, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000004"), new Guid("40000001-0000-0000-0000-000000000033"), "Verify the method of loading as per the project / design requirements", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "1.4", new Guid("30000001-0000-0000-0000-000000000002"), 4, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000005"), new Guid("40000001-0000-0000-0000-000000000033"), "Internal and External Dimensions of the modular", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "2.1", new Guid("30000001-0000-0000-0000-000000000002"), 5, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000006"), new Guid("40000001-0000-0000-0000-000000000034"), "Location and color of Painting as per the App Drawing", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "3.1", new Guid("30000001-0000-0000-0000-000000000002"), 6, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000007"), new Guid("40000001-0000-0000-0000-000000000034"), "Internal Paint (Application of Primer, Stucco and 1st Coat of Paint)", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "3.2", new Guid("30000001-0000-0000-0000-000000000029"), 7, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000008"), new Guid("40000001-0000-0000-0000-000000000034"), "External Paint(Application of Primer, Filler and Final Coat Texture Paint)", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "3.3", new Guid("30000001-0000-0000-0000-000000000029"), 8, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000009"), new Guid("40000001-0000-0000-0000-000000000034"), "Ensure Paint touch ups are completed around installed items", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "3.4", new Guid("30000001-0000-0000-0000-000000000004"), 9, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000010"), new Guid("40000001-0000-0000-0000-000000000034"), "Bitumen Applied at required Areas", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "3.5", new Guid("30000001-0000-0000-0000-000000000002"), 10, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000011"), new Guid("40000001-0000-0000-0000-000000000034"), "Damages, If any", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "3.6", new Guid("30000001-0000-0000-0000-000000000004"), 11, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000012"), new Guid("40000001-0000-0000-0000-000000000035"), "Layout and Fixing of Tiles as per App Drawing", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "4.1", new Guid("30000001-0000-0000-0000-000000000002"), 12, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000013"), new Guid("40000001-0000-0000-0000-000000000035"), "Line, Level and Spacer for the Installed Tiles", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "4.2", new Guid("30000001-0000-0000-0000-000000000004"), 13, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000014"), new Guid("40000001-0000-0000-0000-000000000035"), "Skirting is installed/fixed properly and truly vertical", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "4.3", new Guid("30000001-0000-0000-0000-000000000002"), 14, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000015"), new Guid("40000001-0000-0000-0000-000000000035"), "Grouting of all Joints is done properly", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "4.4", new Guid("30000001-0000-0000-0000-000000000004"), 15, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000016"), new Guid("40000001-0000-0000-0000-000000000035"), "Elastomeric sealant under skirting is provided properly", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "4.5", new Guid("30000001-0000-0000-0000-000000000026"), 16, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000017"), new Guid("40000001-0000-0000-0000-000000000035"), "Ensure Drain holes are free from any debris and properly closed (if applicable)", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "4.6", new Guid("30000001-0000-0000-0000-000000000004"), 17, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000018"), new Guid("40000001-0000-0000-0000-000000000035"), "Damages, if any", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "4.7", new Guid("30000001-0000-0000-0000-000000000004"), 18, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000019"), new Guid("40000001-0000-0000-0000-000000000036"), "Layout, location and position of dry wall is as per App Drawing", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "5.1", new Guid("30000001-0000-0000-0000-000000000002"), 19, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000020"), new Guid("40000001-0000-0000-0000-000000000036"), "Thickness of Dry wall is as per App Drawing", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "5.2", new Guid("30000001-0000-0000-0000-000000000002"), 20, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000021"), new Guid("40000001-0000-0000-0000-000000000036"), "Opening for MEP services are cut properly", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "5.3", new Guid("30000001-0000-0000-0000-000000000002"), 21, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000022"), new Guid("40000001-0000-0000-0000-000000000036"), "Ensure Gypsum surface are Crack free at joints", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "5.4", new Guid("30000001-0000-0000-0000-000000000001"), 22, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000023"), new Guid("40000001-0000-0000-0000-000000000036"), "Damages, if any", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "5.5", new Guid("30000001-0000-0000-0000-000000000001"), 23, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000024"), new Guid("40000001-0000-0000-0000-000000000037"), "Layout of False Ceiling tiles and bulk head as per App Drawing", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "6.1", new Guid("30000001-0000-0000-0000-000000000002"), 24, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000025"), new Guid("40000001-0000-0000-0000-000000000037"), "Height of the False Ceiling as per App Drawing", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "6.2", new Guid("30000001-0000-0000-0000-000000000002"), 25, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000026"), new Guid("40000001-0000-0000-0000-000000000037"), "Access panels/ Ceiling tiles are Fixed Properly", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "6.3", new Guid("30000001-0000-0000-0000-000000000001"), 26, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000027"), new Guid("40000001-0000-0000-0000-000000000037"), "Ensure Gypsum surface are Crack free at joints", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "6.4", new Guid("30000001-0000-0000-0000-000000000001"), 27, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000028"), new Guid("40000001-0000-0000-0000-000000000038"), "Location of Window as per App Drawing", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "7.1", new Guid("30000001-0000-0000-0000-000000000002"), 28, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000029"), new Guid("40000001-0000-0000-0000-000000000038"), "Fixing of Glass/panels", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "7.2", new Guid("30000001-0000-0000-0000-000000000001"), 29, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000030"), new Guid("40000001-0000-0000-0000-000000000038"), "Fixing of Iron-Mongery and Accessories", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "7.3", new Guid("30000001-0000-0000-0000-000000000001"), 30, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000031"), new Guid("40000001-0000-0000-0000-000000000038"), "Fixing of Silicone Sealant", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "7.4", new Guid("30000001-0000-0000-0000-000000000015"), 31, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000032"), new Guid("40000001-0000-0000-0000-000000000038"), "Water leak test performed and passed", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "7.5", new Guid("30000001-0000-0000-0000-000000000005"), 32, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000033"), new Guid("40000001-0000-0000-0000-000000000038"), "Paint touch completed around the frame", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "7.6", new Guid("30000001-0000-0000-0000-000000000001"), 33, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000034"), new Guid("40000001-0000-0000-0000-000000000039"), "Location of Doors as per App Drawing", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "8.1", new Guid("30000001-0000-0000-0000-000000000002"), 34, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000035"), new Guid("40000001-0000-0000-0000-000000000039"), "Direction of doors swing as per App Drawing", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "8.2", new Guid("30000001-0000-0000-0000-000000000031"), 35, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000036"), new Guid("40000001-0000-0000-0000-000000000039"), "Main entrance door as per App Drawing", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "8.3", new Guid("30000001-0000-0000-0000-000000000031"), 36, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000037"), new Guid("40000001-0000-0000-0000-000000000039"), "Lock of Main entrance door is installed", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "8.4", new Guid("30000001-0000-0000-0000-000000000001"), 37, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000038"), new Guid("40000001-0000-0000-0000-000000000040"), "Kitchen cabinets, counter top and accessories installed as per app drawing", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "8.10", new Guid("30000001-0000-0000-0000-000000000002"), 38, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000039"), new Guid("40000001-0000-0000-0000-000000000040"), "Kitchen sink and sink mixer installed", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "8.11", new Guid("30000001-0000-0000-0000-000000000001"), 39, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000040"), new Guid("40000001-0000-0000-0000-000000000040"), "Wardrobe installed as per approved drawings", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "8.12", new Guid("30000001-0000-0000-0000-000000000002"), 40, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000041"), new Guid("40000001-0000-0000-0000-000000000040"), "Wardrobe doors and drawers functioning smoothly and free from scratches", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "8.13", new Guid("30000001-0000-0000-0000-000000000001"), 41, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000042"), new Guid("40000001-0000-0000-0000-000000000041"), "Mirror installed and free from damage", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "9.1", new Guid("30000001-0000-0000-0000-000000000001"), 42, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000043"), new Guid("40000001-0000-0000-0000-000000000041"), "Threshold installed and free from damage", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "9.2", new Guid("30000001-0000-0000-0000-000000000001"), 43, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000044"), new Guid("40000001-0000-0000-0000-000000000041"), "Glass Partition installed and free from damage", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "9.3", new Guid("30000001-0000-0000-0000-000000000001"), 44, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000045"), new Guid("40000001-0000-0000-0000-000000000041"), "Floor drain and covers installed and free from damages", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "9.4", new Guid("30000001-0000-0000-0000-000000000001"), 45, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000046"), new Guid("40000001-0000-0000-0000-000000000041"), "Vanity installed and free from damage", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "9.5", new Guid("30000001-0000-0000-0000-000000000001"), 46, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000047"), new Guid("40000001-0000-0000-0000-000000000041"), "WC and cover installed and free from damage", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "9.6", new Guid("30000001-0000-0000-0000-000000000001"), 47, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000048"), new Guid("40000001-0000-0000-0000-000000000041"), "Shower installed and free from damage", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "9.7", new Guid("30000001-0000-0000-0000-000000000001"), 48, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000049"), new Guid("40000001-0000-0000-0000-000000000041"), "Toilet accessories installed and free from damage", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "9.8", new Guid("30000001-0000-0000-0000-000000000001"), 49, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000050"), new Guid("40000001-0000-0000-0000-000000000041"), "Firestop sealant, fire rated sealant & General sealant applied around penetration pipes", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "9.9", new Guid("30000001-0000-0000-0000-000000000034"), 50, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000051"), new Guid("40000001-0000-0000-0000-000000000041"), "Painted walls are clean and free from stains", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "9.10", new Guid("30000001-0000-0000-0000-000000000001"), 51, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000052"), new Guid("40000001-0000-0000-0000-000000000041"), "Tiles are fixed with grouting properly and free from damage", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "9.11", new Guid("30000001-0000-0000-0000-000000000001"), 52, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000053"), new Guid("40000001-0000-0000-0000-000000000042"), "Check Final Condition of outside of the room and ensure its damage free", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "10.1", new Guid("30000001-0000-0000-0000-000000000001"), 53, "WIR-6" },
                    { new Guid("20000001-0000-0000-0006-000000000054"), new Guid("40000001-0000-0000-0000-000000000042"), "Sign the delivery note for accepting the loading of precast modular in good condition", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "10.2", new Guid("30000001-0000-0000-0000-000000000001"), 54, "WIR-6" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0001-000000000001"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0001-000000000002"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0001-000000000003"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0001-000000000004"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0001-000000000005"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0001-000000000006"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0001-000000000007"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0001-000000000008"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0001-000000000009"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0001-000000000010"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0001-000000000011"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0001-000000000012"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0001-000000000013"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0001-000000000014"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0001-000000000101"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0001-000000000102"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0001-000000000103"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0001-000000000104"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0001-000000000105"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0001-000000000106"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0001-000000000107"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0001-000000000108"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0001-000000000109"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0001-000000000110"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0001-000000000111"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0001-000000000112"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0001-000000000113"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0001-000000000114"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0001-000000000115"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0001-000000000116"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0004-000000000001"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0004-000000000002"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0004-000000000003"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0004-000000000004"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0004-000000000005"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0004-000000000006"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0004-000000000007"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0004-000000000008"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0004-000000000009"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0004-000000000010"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0004-000000000011"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0004-000000000012"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0004-000000000013"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0004-000000000014"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0004-000000000015"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0004-000000000016"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0004-000000000017"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0004-000000000018"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0004-000000000019"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0004-000000000020"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0004-000000000021"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0004-000000000022"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0004-000000000023"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0004-000000000024"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0004-000000000025"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0004-000000000026"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0004-000000000027"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0004-000000000028"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000001"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000002"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000003"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000004"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000005"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000006"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000007"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000008"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000009"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000010"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000011"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000012"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000013"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000014"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000015"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000016"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000017"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000018"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000019"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000020"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000021"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000022"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000023"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000024"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000025"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000026"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000027"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000028"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000029"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000030"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000031"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000032"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000033"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000034"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000035"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000036"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000037"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0005-000000000038"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000001"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000002"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000003"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000004"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000005"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000006"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000007"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000008"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000009"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000010"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000011"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000012"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000013"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000014"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000015"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000016"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000017"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000018"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000019"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000020"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000021"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000022"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000023"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000024"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000025"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000026"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000027"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000028"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000029"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000030"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000031"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000032"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000033"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000034"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000035"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000036"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000037"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000038"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000039"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000040"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000041"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000042"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000043"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000044"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000045"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000046"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000047"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000048"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000049"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000050"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000051"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000052"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000053"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0006-000000000054"));
        }
    }
}
