using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ActivitySeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: new Guid("10000001-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: new Guid("10000001-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: new Guid("10000001-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: new Guid("10000002-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: new Guid("10000002-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: new Guid("10000002-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: new Guid("10000002-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: new Guid("10000002-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: new Guid("10000002-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: new Guid("10000003-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: new Guid("10000003-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: new Guid("10000003-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: new Guid("10000003-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: new Guid("10000003-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: new Guid("10000003-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: new Guid("10000004-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: new Guid("10000004-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: new Guid("10000004-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: new Guid("10000004-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: new Guid("10000004-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: new Guid("10000005-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: new Guid("10000005-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: new Guid("10000005-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: new Guid("10000005-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: new Guid("10000005-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: new Guid("10000005-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: new Guid("10000005-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: new Guid("10000006-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: new Guid("10000006-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: new Guid("10000006-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: new Guid("10000006-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: new Guid("10000006-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: new Guid("10000006-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: new Guid("10000006-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: new Guid("10000006-0000-0000-0000-000000000008"));

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: new Guid("10000006-0000-0000-0000-000000000009"));

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: new Guid("10000007-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: new Guid("10000007-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: new Guid("10000007-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: new Guid("10000007-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: new Guid("10000008-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: new Guid("10000008-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: new Guid("10000008-0000-0000-0000-000000000003"));
        }
    }
}
