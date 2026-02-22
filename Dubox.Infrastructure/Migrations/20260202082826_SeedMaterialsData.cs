using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional


namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedMaterialsData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Materials",
                columns: new[] { "MaterialId", "AllocatedStock", "CurrentStock", "DefaultRequiredBeforeDays", "IsActive", "MaterialCategory", "MaterialCode", "MaterialName", "MinimumStock", "ReorderLevel", "SupplierName", "Unit", "UnitCost" },
                values: new object[,]
                {
                    { new Guid("00000001-0000-0000-0000-000000000000"), 0m, 0m, 30, true, "Structural Materials", "MAT-STRUCT-001", "Insulation", 0m, 0m, null, "m²", null },
                    { new Guid("00000002-0000-0000-0000-000000000000"), 0m, 0m, 30, true, "Structural Materials", "MAT-STRUCT-002", "PC Strand", 0m, 0m, null, "ton", null },
                    { new Guid("00000003-0000-0000-0000-000000000000"), 0m, 0m, 30, true, "Structural Materials", "MAT-STRUCT-003", "Embedded Items", 0m, 0m, null, "pcs", null },
                    { new Guid("00000004-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "Structural Materials", "MAT-STRUCT-004", "Pre-Cast", 0m, 0m, null, "m³", null },
                    { new Guid("00000005-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "Structural Materials", "MAT-STRUCT-005", "Dry Mix Concrete", 0m, 0m, null, "m³", null },
                    { new Guid("00000006-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "Structural Materials", "MAT-STRUCT-006", "Grout", 0m, 0m, null, "kg", null },
                    { new Guid("00000007-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "Structural Materials", "MAT-STRUCT-007", "Sealant", 0m, 0m, null, "tube", null },
                    { new Guid("00000008-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "Structural Materials", "MAT-STRUCT-008", "Silicon", 0m, 0m, null, "tube", null },
                    { new Guid("00000009-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "Structural Materials", "MAT-STRUCT-009", "Backer Rod", 0m, 0m, null, "m", null },
                    { new Guid("0000000a-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "Structural Materials", "MAT-STRUCT-010", "PVC Shims", 0m, 0m, null, "pcs", null },
                    { new Guid("0000000b-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "Structural Materials", "MAT-STRUCT-011", "Bonding Agent", 0m, 0m, null, "liter", null },
                    { new Guid("0000000c-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "Structural Materials", "MAT-STRUCT-012", "Curing Compound", 0m, 0m, null, "liter", null },
                    { new Guid("0000000d-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "Structural Materials", "MAT-STRUCT-013", "Concrete Repair", 0m, 0m, null, "kg", null },
                    { new Guid("0000000e-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "Structural Materials", "MAT-STRUCT-014", "Steel Rebar", 0m, 0m, null, "ton", null },
                    { new Guid("0000000f-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "Structural Materials", "MAT-STRUCT-015", "Plywood", 0m, 0m, null, "sheet", null },
                    { new Guid("00000010-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "Structural Materials", "MAT-STRUCT-016", "Cement", 0m, 0m, null, "bag", null },
                    { new Guid("00000011-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "Finishing Materials", "MAT-FINISH-001", "Paint", 0m, 0m, null, "liter", null },
                    { new Guid("00000012-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "Finishing Materials", "MAT-FINISH-002", "Tiles", 0m, 0m, null, "m²", null },
                    { new Guid("00000013-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "Finishing Materials", "MAT-FINISH-003", "Tile Grout", 0m, 0m, null, "kg", null },
                    { new Guid("00000014-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "Finishing Materials", "MAT-FINISH-004", "Tile Adhesive", 0m, 0m, null, "kg", null },
                    { new Guid("00000015-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "Finishing Materials", "MAT-FINISH-005", "Fire Sealant", 0m, 0m, null, "tube", null },
                    { new Guid("00000016-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "Finishing Materials", "MAT-FINISH-006", "Bitumen", 0m, 0m, null, "kg", null },
                    { new Guid("00000017-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "Finishing Materials", "MAT-FINISH-007", "Waterproof", 0m, 0m, null, "liter", null },
                    { new Guid("00000018-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "Finishing Materials", "MAT-FINISH-008", "Gypsum Board", 0m, 0m, null, "sheet", null },
                    { new Guid("00000019-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "Finishing Materials", "MAT-FINISH-009", "Threshold", 0m, 0m, null, "pcs", null },
                    { new Guid("0000001a-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "Doors & Windows", "MAT-DOOR-001", "Wooden Door", 0m, 0m, null, "pcs", null },
                    { new Guid("0000001b-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "Doors & Windows", "MAT-DOOR-002", "Metal Door", 0m, 0m, null, "pcs", null },
                    { new Guid("0000001c-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "Doors & Windows", "MAT-DOOR-003", "Door Hardware", 0m, 0m, null, "set", null },
                    { new Guid("0000001d-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "Doors & Windows", "MAT-DOOR-004", "Aluminum Door", 0m, 0m, null, "pcs", null },
                    { new Guid("0000001e-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "Doors & Windows", "MAT-DOOR-005", "Aluminum Window", 0m, 0m, null, "pcs", null },
                    { new Guid("0000001f-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "Carpentry", "MAT-CARP-001", "Kitchen Cabinet", 0m, 0m, null, "set", null },
                    { new Guid("00000020-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "Carpentry", "MAT-CARP-002", "Countertop (Corian)", 0m, 0m, null, "m²", null },
                    { new Guid("00000021-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "Carpentry", "MAT-CARP-003", "Wardrobe", 0m, 0m, null, "set", null },
                    { new Guid("00000022-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "MEP - HVAC", "MAT-HVAC-001", "FCU", 0m, 0m, null, "unit", null },
                    { new Guid("00000023-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "MEP - HVAC", "MAT-HVAC-002", "FAHU", 0m, 0m, null, "unit", null },
                    { new Guid("00000024-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "MEP - HVAC", "MAT-HVAC-003", "Ventilation Fan", 0m, 0m, null, "unit", null },
                    { new Guid("00000025-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "MEP - HVAC", "MAT-HVAC-004", "Duct", 0m, 0m, null, "m", null },
                    { new Guid("00000026-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "MEP - HVAC", "MAT-HVAC-005", "Air Outlet", 0m, 0m, null, "pcs", null },
                    { new Guid("00000027-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "MEP - HVAC", "MAT-HVAC-006", "Damper", 0m, 0m, null, "pcs", null },
                    { new Guid("00000028-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "MEP - HVAC", "MAT-HVAC-007", "Exhaust Fan", 0m, 0m, null, "unit", null },
                    { new Guid("00000029-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "MEP - HVAC", "MAT-HVAC-008", "Thermostat", 0m, 0m, null, "unit", null },
                    { new Guid("0000002a-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "MEP - HVAC", "MAT-HVAC-009", "Thermal Insulation", 0m, 0m, null, "m²", null },
                    { new Guid("0000002b-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "MEP - Plumbing", "MAT-PLUMB-001", "PPR Pipes & Fittings", 0m, 0m, null, "m", null },
                    { new Guid("0000002c-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "MEP - Plumbing", "MAT-PLUMB-002", "Water Hammer Arrestor", 0m, 0m, null, "pcs", null },
                    { new Guid("0000002d-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "MEP - Plumbing", "MAT-PLUMB-003", "Water Heaters", 0m, 0m, null, "unit", null },
                    { new Guid("0000002e-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "MEP - Plumbing", "MAT-PLUMB-004", "PVC Pipes & Fittings", 0m, 0m, null, "m", null },
                    { new Guid("0000002f-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "MEP - Plumbing", "MAT-PLUMB-005", "HDPE Pipes & Fittings", 0m, 0m, null, "m", null },
                    { new Guid("00000030-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "MEP - Plumbing", "MAT-PLUMB-006", "Floor Drain", 0m, 0m, null, "pcs", null },
                    { new Guid("00000031-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "MEP - Plumbing", "MAT-PLUMB-007", "Floor Cleanout", 0m, 0m, null, "pcs", null },
                    { new Guid("00000032-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "MEP - Plumbing", "MAT-PLUMB-008", "Sanitary Fixture", 0m, 0m, null, "set", null },
                    { new Guid("00000033-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "MEP - Plumbing", "MAT-PLUMB-009", "Mechanical Hangers & Support", 0m, 0m, null, "set", null },
                    { new Guid("00000034-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "MEP - Plumbing", "MAT-PLUMB-010", "Bottle Trap", 0m, 0m, null, "pcs", null },
                    { new Guid("00000035-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "MEP - Plumbing", "MAT-PLUMB-011", "Angle Valve", 0m, 0m, null, "pcs", null },
                    { new Guid("00000036-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "MEP - Plumbing", "MAT-PLUMB-012", "PEX Pipe & Fittings", 0m, 0m, null, "m", null },
                    { new Guid("00000037-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "MEP - Plumbing", "MAT-PLUMB-013", "Kitchen Mixer", 0m, 0m, null, "unit", null },
                    { new Guid("00000038-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "MEP - Plumbing", "MAT-PLUMB-014", "Kitchen Sink", 0m, 0m, null, "unit", null },
                    { new Guid("00000039-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "MEP - Plumbing", "MAT-PLUMB-015", "Kitchen Hood", 0m, 0m, null, "unit", null },
                    { new Guid("0000003a-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "MEP - Plumbing", "MAT-PLUMB-016", "Copper Pipe & Fitting", 0m, 0m, null, "m", null },
                    { new Guid("0000003b-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "MEP - Plumbing", "MAT-PLUMB-017", "Rain Water Down Spout", 0m, 0m, null, "m", null },
                    { new Guid("0000003c-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "MEP - Electrical", "MAT-ELEC-001", "LV Cables", 0m, 0m, null, "m", null },
                    { new Guid("0000003d-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "MEP - Electrical", "MAT-ELEC-002", "LV Panels", 0m, 0m, null, "unit", null },
                    { new Guid("0000003e-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "MEP - Electrical", "MAT-ELEC-003", "Wire Connectors", 0m, 0m, null, "pcs", null },
                    { new Guid("0000003f-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "MEP - Electrical", "MAT-ELEC-004", "Light Fittings", 0m, 0m, null, "unit", null },
                    { new Guid("00000040-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "MEP - Electrical", "MAT-ELEC-005", "Earthing", 0m, 0m, null, "set", null },
                    { new Guid("00000041-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "MEP - Electrical", "MAT-ELEC-006", "UPS", 0m, 0m, null, "unit", null },
                    { new Guid("00000042-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "MEP - Electrical", "MAT-ELEC-007", "Isolators", 0m, 0m, null, "pcs", null },
                    { new Guid("00000043-0000-0000-0000-000000000000"), 0m, 0m, 30, true, "MEP - Electrical", "MAT-ELEC-008", "PVC Conduits, Box, GI Back Box", 0m, 0m, null, "m", null },
                    { new Guid("00000044-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "MEP - Electrical", "MAT-ELEC-009", "EMT Conduits & Acc", 0m, 0m, null, "m", null },
                    { new Guid("00000045-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "MEP - Electrical", "MAT-ELEC-010", "GI Flexible Conduits & Acc", 0m, 0m, null, "m", null },
                    { new Guid("00000046-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "MEP - Electrical", "MAT-ELEC-011", "Cable Management System (Trays)", 0m, 0m, null, "m", null },
                    { new Guid("00000047-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "MEP - Electrical", "MAT-ELEC-012", "Floor Box", 0m, 0m, null, "pcs", null },
                    { new Guid("00000048-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "MEP - Electrical", "MAT-ELEC-013", "Wiring Accessories", 0m, 0m, null, "set", null },
                    { new Guid("00000049-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "MEP - Electrical", "MAT-ELEC-014", "Ceiling Fan", 0m, 0m, null, "unit", null },
                    { new Guid("0000004a-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "MEP - Electrical", "MAT-ELEC-015", "Control Panel", 0m, 0m, null, "unit", null },
                    { new Guid("0000004b-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "MEP - Systems", "MAT-SYS-001", "CCTV System", 0m, 0m, null, "set", null },
                    { new Guid("0000004c-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "MEP - Systems", "MAT-SYS-002", "Monitor Sensor", 0m, 0m, null, "pcs", null },
                    { new Guid("0000004d-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "MEP - Systems", "MAT-SYS-003", "Fire Alarm System", 0m, 0m, null, "set", null },
                    { new Guid("0000004e-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "MEP - Systems", "MAT-SYS-004", "IBMS", 0m, 0m, null, "set", null },
                    { new Guid("0000004f-0000-0000-0000-000000000000"), 0m, 0m, 7, true, "MEP - Systems", "MAT-SYS-005", "Access Control System", 0m, 0m, null, "set", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("00000001-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("00000002-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("00000003-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("00000004-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("00000005-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("00000006-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("00000007-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("00000008-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("00000009-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("0000000a-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("0000000b-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("0000000c-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("0000000d-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("0000000e-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("0000000f-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("00000010-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("00000011-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("00000012-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("00000013-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("00000014-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("00000015-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("00000016-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("00000017-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("00000018-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("00000019-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("0000001a-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("0000001b-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("0000001c-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("0000001d-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("0000001e-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("0000001f-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("00000020-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("00000021-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("00000022-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("00000023-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("00000024-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("00000025-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("00000026-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("00000027-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("00000028-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("00000029-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("0000002a-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("0000002b-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("0000002c-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("0000002d-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("0000002e-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("0000002f-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("00000030-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("00000031-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("00000032-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("00000033-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("00000034-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("00000035-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("00000036-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("00000037-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("00000038-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("00000039-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("0000003a-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("0000003b-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("0000003c-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("0000003d-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("0000003e-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("0000003f-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("00000040-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("00000041-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("00000042-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("00000043-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("00000044-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("00000045-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("00000046-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("00000047-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("00000048-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("00000049-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("0000004a-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("0000004b-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("0000004c-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("0000004d-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("0000004e-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "MaterialId",
                keyValue: new Guid("0000004f-0000-0000-0000-000000000000"));
        }
    }
}
