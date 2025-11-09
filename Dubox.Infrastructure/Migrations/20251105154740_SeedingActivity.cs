using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedingActivity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "ActivityMaster",
                keyColumn: "ActivityMasterId",
                keyValue: 33);
        }
    }
}
