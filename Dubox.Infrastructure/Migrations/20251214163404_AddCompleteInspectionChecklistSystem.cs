using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCompleteInspectionChecklistSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "PredefinedChecklistItems",
                columns: new[] { "PredefinedItemId", "CategoryId", "CheckpointDescription", "CreatedDate", "IsActive", "ItemNumber", "ReferenceId", "Sequence", "WIRNumber" },
                values: new object[,]
                {
                    { new Guid("20000001-0000-0000-0000-000000000001"), new Guid("40000001-0000-0000-0000-000000000002"), "Ensure the materials as per approved material submittal.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A1", new Guid("30000001-0000-0000-0000-000000000003"), 1, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000002"), new Guid("40000001-0000-0000-0000-000000000002"), "Ensure drawings are used for installation are current and approved.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A2", new Guid("30000001-0000-0000-0000-000000000002"), 2, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000003"), new Guid("40000001-0000-0000-0000-000000000001"), "Check that only in properly fabricated fittings are used for changes in directions, shapes,sizes and connections.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A3", new Guid("30000001-0000-0000-0000-000000000004"), 3, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000004"), new Guid("40000001-0000-0000-0000-000000000001"), "The joints and flanges are correctly made, jointed and sealed.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A4", new Guid("30000001-0000-0000-0000-000000000004"), 4, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000005"), new Guid("40000001-0000-0000-0000-000000000001"), "Check that duct joints are sealed externally with approved sealant.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A5", new Guid("30000001-0000-0000-0000-000000000004"), 5, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000006"), new Guid("40000001-0000-0000-0000-000000000001"), "Check fixing of supports and spacing as approved drawings & submittal.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A6", new Guid("30000001-0000-0000-0000-000000000002"), 6, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000007"), new Guid("40000001-0000-0000-0000-000000000001"), "Check acoustic lining is properly fastened and un damaged.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A7", new Guid("30000001-0000-0000-0000-000000000002"), 7, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000008"), new Guid("40000001-0000-0000-0000-000000000001"), "Check nuts, bolts, screws, brackets drop rods etc. are tight and aligned properly.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A8", new Guid("30000001-0000-0000-0000-000000000002"), 8, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000009"), new Guid("40000001-0000-0000-0000-000000000001"), "All the access doors, fire dampers,VCDs etc are installed as per approved drawings, specification and manufacturer instructions as applicable.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A9", new Guid("30000001-0000-0000-0000-000000000002"), 9, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000010"), new Guid("40000001-0000-0000-0000-000000000001"), "Ensure that the identification & labeling are provided for duct works.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A10", new Guid("30000001-0000-0000-0000-000000000002"), 10, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000011"), new Guid("40000001-0000-0000-0000-000000000003"), "Ensure the materials are approved", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A1", new Guid("30000001-0000-0000-0000-000000000005"), 11, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000012"), new Guid("40000001-0000-0000-0000-000000000003"), "No visible damage on the materials", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A2", new Guid("30000001-0000-0000-0000-000000000004"), 12, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000013"), new Guid("40000001-0000-0000-0000-000000000003"), "Pipe sizes are as per approved shop drawing", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A3", new Guid("30000001-0000-0000-0000-000000000002"), 13, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000014"), new Guid("40000001-0000-0000-0000-000000000003"), "Pipe layout/routing as per approved shop drawing", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A4", new Guid("30000001-0000-0000-0000-000000000002"), 14, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000015"), new Guid("40000001-0000-0000-0000-000000000003"), "Slope of the pipes as per approved shop drawing", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A5", new Guid("30000001-0000-0000-0000-000000000002"), 15, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000016"), new Guid("40000001-0000-0000-0000-000000000003"), "Installation as per approved method statement", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A6", new Guid("30000001-0000-0000-0000-000000000007"), 16, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000017"), new Guid("40000001-0000-0000-0000-000000000003"), "Sleeves are provided for the pipes passing through the walls /slabs", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A7", new Guid("30000001-0000-0000-0000-000000000002"), 17, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000018"), new Guid("40000001-0000-0000-0000-000000000003"), "Check the pipes are supported well with approved clamps.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A8", new Guid("30000001-0000-0000-0000-000000000002"), 18, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000019"), new Guid("40000001-0000-0000-0000-000000000003"), "Ensure Drainage pipes are not passing above electrical services", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A9", new Guid("30000001-0000-0000-0000-000000000002"), 19, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000020"), new Guid("40000001-0000-0000-0000-000000000003"), "Installed pipes are free of sag & bend", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A10", new Guid("30000001-0000-0000-0000-000000000004"), 20, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000021"), new Guid("40000001-0000-0000-0000-000000000003"), "Drainage pipes are connected to the vent system", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A11", new Guid("30000001-0000-0000-0000-000000000002"), 21, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000022"), new Guid("40000001-0000-0000-0000-000000000003"), "Pipe joints are properly made and are tight / secure", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A12", new Guid("30000001-0000-0000-0000-000000000004"), 22, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000023"), new Guid("40000001-0000-0000-0000-000000000004"), "Installation of Drainage Pipe has completed", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A1", new Guid("30000001-0000-0000-0000-000000000002"), 23, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000024"), new Guid("40000001-0000-0000-0000-000000000004"), "All pipe joints shall be inspected.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A2", new Guid("30000001-0000-0000-0000-000000000004"), 24, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000025"), new Guid("40000001-0000-0000-0000-000000000004"), "Drainage pipe should generally be subjected to an internal pressure test of 3m head of water above the crown of the pipe at the high end", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A3", new Guid("30000001-0000-0000-0000-000000000008"), 25, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000026"), new Guid("40000001-0000-0000-0000-000000000005"), "Ensure the materials are approved", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A1", new Guid("30000001-0000-0000-0000-000000000005"), 26, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000027"), new Guid("40000001-0000-0000-0000-000000000005"), "No visible damage on the materials.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A2", new Guid("30000001-0000-0000-0000-000000000004"), 27, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000028"), new Guid("40000001-0000-0000-0000-000000000005"), "Pipe sizes are as per approved shop drawing.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A3", new Guid("30000001-0000-0000-0000-000000000002"), 28, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000029"), new Guid("40000001-0000-0000-0000-000000000005"), "Pipe layout/routing as per approved shop drawing.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A4", new Guid("30000001-0000-0000-0000-000000000002"), 29, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000030"), new Guid("40000001-0000-0000-0000-000000000005"), "Installation as per approved method statement.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A5", new Guid("30000001-0000-0000-0000-000000000007"), 30, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000031"), new Guid("40000001-0000-0000-0000-000000000005"), "Sleeves are provided for the pipes passing through the walls /slabs.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A6", new Guid("30000001-0000-0000-0000-000000000004"), 31, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000032"), new Guid("40000001-0000-0000-0000-000000000005"), "Check the Horizontal pipes are supported well and with approved clamps.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A7", new Guid("30000001-0000-0000-0000-000000000002"), 32, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000033"), new Guid("40000001-0000-0000-0000-000000000005"), "Check the vertical riser of the pipes are supported well with approved clamp.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A8", new Guid("30000001-0000-0000-0000-000000000004"), 33, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000034"), new Guid("40000001-0000-0000-0000-000000000005"), "Water pipes are not passing above electrical services.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A9", new Guid("30000001-0000-0000-0000-000000000004"), 34, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000035"), new Guid("40000001-0000-0000-0000-000000000005"), "Installed pipes are free of sag & bend.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A10", new Guid("30000001-0000-0000-0000-000000000004"), 35, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000036"), new Guid("40000001-0000-0000-0000-000000000005"), "Pipe joints are properly made and are tight / secure", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A11", new Guid("30000001-0000-0000-0000-000000000004"), 36, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000037"), new Guid("40000001-0000-0000-0000-000000000006"), "Installation of Water Supply pipes completed", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A1", new Guid("30000001-0000-0000-0000-000000000002"), 37, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000038"), new Guid("40000001-0000-0000-0000-000000000006"), "Ensure Pressure Gauge used is Calibrated", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A2", new Guid("30000001-0000-0000-0000-000000000010"), 38, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000039"), new Guid("40000001-0000-0000-0000-000000000006"), "The system shall be subjected to a hydrostatic pressure test, to the satisfaction and in the presence of the Engineer. Pressure shall be 1.5 times than working pressure", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A3", new Guid("30000001-0000-0000-0000-000000000011"), 39, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000040"), new Guid("40000001-0000-0000-0000-000000000007"), "Check the piping, fitting, installation and valves materials are as per specification and approved material submittal", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A1", new Guid("30000001-0000-0000-0000-000000000005"), 40, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000041"), new Guid("40000001-0000-0000-0000-000000000007"), "Checks the piping layouts are as per the approved shop drawing and site conditions.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A2", new Guid("30000001-0000-0000-0000-000000000002"), 41, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000042"), new Guid("40000001-0000-0000-0000-000000000007"), "Check the approved supports& accessories are used for installation firefighting piping &Accessories.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A3", new Guid("30000001-0000-0000-0000-000000000005"), 42, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000043"), new Guid("40000001-0000-0000-0000-000000000007"), "Checks the distance between the supports are maintained as per specification and method statement.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A4", new Guid("30000001-0000-0000-0000-000000000014"), 43, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000044"), new Guid("40000001-0000-0000-0000-000000000007"), "Checks the proper sizing's of hangers are used as per specification.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A5", new Guid("30000001-0000-0000-0000-000000000002"), 44, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000045"), new Guid("40000001-0000-0000-0000-000000000007"), "Check threaded joints are provided for 2\" and below and grooved fitting are provided for 2 ½\" and above in piping.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A6", new Guid("30000001-0000-0000-0000-000000000013"), 45, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000046"), new Guid("40000001-0000-0000-0000-000000000007"), "Check location / size of valves are provided as per approved shop drawing and specification.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A7", new Guid("30000001-0000-0000-0000-000000000002"), 46, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000047"), new Guid("40000001-0000-0000-0000-000000000007"), "Check unions are provided for 2\" and below piping in direction of flow.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A8", new Guid("30000001-0000-0000-0000-000000000002"), 47, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000048"), new Guid("40000001-0000-0000-0000-000000000007"), "Check flexible pipe connector/expansion compensator are installed in expansion joints and equipment connections.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A9", new Guid("30000001-0000-0000-0000-000000000002"), 48, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000049"), new Guid("40000001-0000-0000-0000-000000000007"), "Check drain points are provided in lowest piping points.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A10", new Guid("30000001-0000-0000-0000-000000000002"), 49, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000050"), new Guid("40000001-0000-0000-0000-000000000007"), "Check air vents are provided in highest piping points.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A11", new Guid("30000001-0000-0000-0000-000000000002"), 50, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000051"), new Guid("40000001-0000-0000-0000-000000000008"), "Fire fighting pipes installation completed", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A1", new Guid("30000001-0000-0000-0000-000000000002"), 51, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000052"), new Guid("40000001-0000-0000-0000-000000000008"), "Ensure the Pressure Gauge used are calibrated", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A2", new Guid("30000001-0000-0000-0000-000000000010"), 52, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000053"), new Guid("40000001-0000-0000-0000-000000000008"), "The system shall be subjected to a hydrostatic pressure test, to the satisfaction and in the presence of the Engineer. Pressure shall be 1.5 times than working pressure in accordance to NFPA 13. Test shall be maintained for two hours as a minimum.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A3", new Guid("30000001-0000-0000-0000-000000000015"), 53, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000054"), new Guid("40000001-0000-0000-0000-000000000009"), "Ensure the materials as per approved material submittal.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A1", new Guid("30000001-0000-0000-0000-000000000003"), 54, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000055"), new Guid("40000001-0000-0000-0000-000000000009"), "No visible damage on the materials.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A2", new Guid("30000001-0000-0000-0000-000000000004"), 55, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000056"), new Guid("40000001-0000-0000-0000-000000000009"), "Pipe sizes are as per approved shop drawing.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A3", new Guid("30000001-0000-0000-0000-000000000002"), 56, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000057"), new Guid("40000001-0000-0000-0000-000000000009"), "Pipe layout/routing as per approved shop drawing.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A4", new Guid("30000001-0000-0000-0000-000000000002"), 57, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000058"), new Guid("40000001-0000-0000-0000-000000000009"), "Installation as per approved method statement.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A5", new Guid("30000001-0000-0000-0000-000000000007"), 58, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000059"), new Guid("40000001-0000-0000-0000-000000000009"), "Sleeves are provided for the pipes passing through the walls/slabs.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A6", new Guid("30000001-0000-0000-0000-000000000002"), 59, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000060"), new Guid("40000001-0000-0000-0000-000000000009"), "Check the Horizontal pipes are supported well and with approved clamps.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A7", new Guid("30000001-0000-0000-0000-000000000002"), 60, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000061"), new Guid("40000001-0000-0000-0000-000000000009"), "Check the vertical riser of the pipes are supported well with approved clamp.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A8", new Guid("30000001-0000-0000-0000-000000000002"), 61, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000062"), new Guid("40000001-0000-0000-0000-000000000009"), "Pipe joints are properly brazed with no leak", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A9", new Guid("30000001-0000-0000-0000-000000000004"), 62, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000063"), new Guid("40000001-0000-0000-0000-000000000009"), "Ensure the proper insulation done.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A10", new Guid("30000001-0000-0000-0000-000000000002"), 63, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000064"), new Guid("40000001-0000-0000-0000-000000000009"), "Ensure the cladding and sealant on the cladding joints.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A11", new Guid("30000001-0000-0000-0000-000000000002"), 64, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000065"), new Guid("40000001-0000-0000-0000-000000000009"), "Ensure the system charged with approved refrigerant", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A12", new Guid("30000001-0000-0000-0000-000000000004"), 65, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000066"), new Guid("40000001-0000-0000-0000-000000000010"), "Installation of FAHU, FCU & VRF units are completed", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A1", new Guid("30000001-0000-0000-0000-000000000018"), 66, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000067"), new Guid("40000001-0000-0000-0000-000000000010"), "Installation of Refrigerant pipes completed", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A2", new Guid("30000001-0000-0000-0000-000000000017"), 67, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000068"), new Guid("40000001-0000-0000-0000-000000000010"), "Installation inspection completed", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A3", new Guid("30000001-0000-0000-0000-000000000017"), 68, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000069"), new Guid("40000001-0000-0000-0000-000000000010"), "Pressure Test: After installation, charge system and test for leaks. Repair leaks and retest until no leaks exist.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A4", new Guid("30000001-0000-0000-0000-000000000017"), 69, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000070"), new Guid("40000001-0000-0000-0000-000000000011"), "Check all materials installed are as per approved material submittal.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A1", new Guid("30000001-0000-0000-0000-000000000019"), 70, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000071"), new Guid("40000001-0000-0000-0000-000000000011"), "Dimensions as per approved drawings", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A2", new Guid("30000001-0000-0000-0000-000000000002"), 71, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000072"), new Guid("40000001-0000-0000-0000-000000000011"), "the cable/wire provided with Ties appropriately?", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A3", new Guid("30000001-0000-0000-0000-000000000004"), 72, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000073"), new Guid("40000001-0000-0000-0000-000000000011"), "Verify measurement of lengths.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A4", new Guid("30000001-0000-0000-0000-000000000004"), 73, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000074"), new Guid("40000001-0000-0000-0000-000000000011"), "Ensure that there are no damages to the cables", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A5", new Guid("30000001-0000-0000-0000-000000000005"), 74, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000075"), new Guid("40000001-0000-0000-0000-000000000011"), "Verify that identification, grouping, spacing, markings and clamps are as required.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A6", new Guid("30000001-0000-0000-0000-000000000002"), 75, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000076"), new Guid("40000001-0000-0000-0000-000000000011"), "Ensure the cable laying in trunking, trays and conduit installations are conducted in approved and accepted manner & as per project specification", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A7", new Guid("30000001-0000-0000-0000-000000000002"), 76, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000077"), new Guid("40000001-0000-0000-0000-000000000011"), "Ensure that cable type and routes are as per approved Drawing", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A8", new Guid("30000001-0000-0000-0000-000000000002"), 77, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000078"), new Guid("40000001-0000-0000-0000-000000000011"), "Is the cable/wire termination done correctly", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A9", new Guid("30000001-0000-0000-0000-000000000002"), 78, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000079"), new Guid("40000001-0000-0000-0000-000000000011"), "Ensure that correct types and sizes of wires and LV Cables are installed", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A9", new Guid("30000001-0000-0000-0000-000000000002"), 79, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000080"), new Guid("40000001-0000-0000-0000-000000000011"), "Verify that routes and marked locations are correct as per approved drawings", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A10", new Guid("30000001-0000-0000-0000-000000000002"), 80, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000081"), new Guid("40000001-0000-0000-0000-000000000011"), "Before pulling wires in conduit check that inside of conduit (and raceway in general) is free of burrs and is dry and clean.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A10", new Guid("30000001-0000-0000-0000-000000000004"), 81, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000082"), new Guid("40000001-0000-0000-0000-000000000011"), "Ensure that extra Length left at every branch circuit outlet and pull-box, every cable passing through in order to allow inspection and for connections to be made.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A11", new Guid("30000001-0000-0000-0000-000000000004"), 82, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000083"), new Guid("40000001-0000-0000-0000-000000000011"), "Is cable/wire provided with Lugs?", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A11", new Guid("30000001-0000-0000-0000-000000000004"), 83, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000084"), new Guid("40000001-0000-0000-0000-000000000011"), "Ensure that tightening of electrical connectors and terminals including screws and bolts, in accordance with manufacturers recommendation", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A12", new Guid("30000001-0000-0000-0000-000000000004"), 84, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000085"), new Guid("40000001-0000-0000-0000-000000000012"), "Is the Cable/Wire Continuity test Setup, okay?", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A1", new Guid("30000001-0000-0000-0000-000000000020"), 85, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000086"), new Guid("40000001-0000-0000-0000-000000000012"), "Is the Cable/Wire Megger test Setup, okay?", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A2", new Guid("30000001-0000-0000-0000-000000000020"), 86, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000087"), new Guid("40000001-0000-0000-0000-000000000012"), "Ensure Megger test is performed for Cables and accepted", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A3", new Guid("30000001-0000-0000-0000-000000000021"), 87, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000088"), new Guid("40000001-0000-0000-0000-000000000012"), "Ensure Continuity test performed and accepted", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A4", new Guid("30000001-0000-0000-0000-000000000021"), 88, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000089"), new Guid("40000001-0000-0000-0000-000000000013"), "Verify the installed Panel boards have approved submittals.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A1", new Guid("30000001-0000-0000-0000-000000000019"), 89, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000090"), new Guid("40000001-0000-0000-0000-000000000013"), "Ensure the drawings used for installation are correct and approved.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A2", new Guid("30000001-0000-0000-0000-000000000023"), 90, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000091"), new Guid("40000001-0000-0000-0000-000000000013"), "Align, level and securely fasten panelboards to structure", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A3", new Guid("30000001-0000-0000-0000-000000000004"), 91, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000092"), new Guid("40000001-0000-0000-0000-000000000013"), "Check the Name Plate and Identification labels as per load schedule and approved submittals.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A4", new Guid("30000001-0000-0000-0000-000000000023"), 92, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000093"), new Guid("40000001-0000-0000-0000-000000000013"), "Check all meters, circuit breakers, indication lamp, handles and locks are correct and undamaged.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A5", new Guid("30000001-0000-0000-0000-000000000005"), 93, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000094"), new Guid("40000001-0000-0000-0000-000000000013"), "Fix surface mounted outdoor panelboards at least 25mm from wall ensuring supporting members do not prevent flow of air.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A6", new Guid("30000001-0000-0000-0000-000000000004"), 94, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000095"), new Guid("40000001-0000-0000-0000-000000000013"), "Wiring: Inside Panelboards: Neatly arranged, accessible and strapped to prevent tension on circuit breaker terminals. Tap-off connections shall be split and bolted type, fully insulated.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A7", new Guid("30000001-0000-0000-0000-000000000004"), 95, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000096"), new Guid("40000001-0000-0000-0000-000000000013"), "Panelboard Interiors: Do not install in cabinets until all conduit connections to cabinet have been completed", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A8", new Guid("30000001-0000-0000-0000-000000000004"), 96, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000097"), new Guid("40000001-0000-0000-0000-000000000013"), "Do not use connecting conduits to support panelboards.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A9", new Guid("30000001-0000-0000-0000-000000000004"), 97, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000098"), new Guid("40000001-0000-0000-0000-000000000013"), "Equipment grounding for LV Panel Board is provided", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A10", new Guid("30000001-0000-0000-0000-000000000023"), 98, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000099"), new Guid("40000001-0000-0000-0000-000000000013"), "Ensure that all unused openings are closed in the panels", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A11", new Guid("30000001-0000-0000-0000-000000000004"), 99, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000100"), new Guid("40000001-0000-0000-0000-000000000013"), "Ensure that updated circuit directory and other relevant documents provided in the panel directory accordingly.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A12", new Guid("30000001-0000-0000-0000-000000000023"), 100, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000101"), new Guid("40000001-0000-0000-0000-000000000013"), "Ensure that the Panel fixing bolts have been tightened with the suitable nut and washer.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A13", new Guid("30000001-0000-0000-0000-000000000004"), 101, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000102"), new Guid("40000001-0000-0000-0000-000000000013"), "Touch up and cleaning of the panel", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A14", new Guid("30000001-0000-0000-0000-000000000004"), 102, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000103"), new Guid("40000001-0000-0000-0000-000000000013"), "Ensure that each Panel Marking should be done as per approved drawing", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A15", new Guid("30000001-0000-0000-0000-000000000023"), 103, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000104"), new Guid("40000001-0000-0000-0000-000000000014"), "Check the conduits and accessories are as per approved material submittal.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A1", new Guid("30000001-0000-0000-0000-000000000003"), 104, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000105"), new Guid("40000001-0000-0000-0000-000000000014"), "Check and ensure the drawings used for installation are current and approved.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A2", new Guid("30000001-0000-0000-0000-000000000023"), 105, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000106"), new Guid("40000001-0000-0000-0000-000000000014"), "Check the conduits and other associated material are new and undamaged.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A3", new Guid("30000001-0000-0000-0000-000000000005"), 106, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000107"), new Guid("40000001-0000-0000-0000-000000000014"), "Check that the conduits are leveled and aligned properly.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A4", new Guid("30000001-0000-0000-0000-000000000004"), 107, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000108"), new Guid("40000001-0000-0000-0000-000000000014"), "Check that the conduits are securely fixed.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A5", new Guid("30000001-0000-0000-0000-000000000004"), 108, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000109"), new Guid("40000001-0000-0000-0000-000000000014"), "Check and ensure that the conduits and back boxes are sizely adequated.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A6", new Guid("30000001-0000-0000-0000-000000000023"), 109, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000110"), new Guid("40000001-0000-0000-0000-000000000014"), "Check that the bottom hight of the back boxes is as per the shop drawing.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A7", new Guid("30000001-0000-0000-0000-000000000023"), 110, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000111"), new Guid("40000001-0000-0000-0000-000000000014"), "Check that a minimum of 5mm thick cover is provided for conduits concealed in walls.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A8", new Guid("30000001-0000-0000-0000-000000000023"), 111, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000112"), new Guid("40000001-0000-0000-0000-000000000014"), "Check the installation of conduits is co-ordinated with other services.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A9", new Guid("30000001-0000-0000-0000-000000000004"), 112, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000113"), new Guid("40000001-0000-0000-0000-000000000014"), "Check the installation of conduiting as per approved drawings.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A10", new Guid("30000001-0000-0000-0000-000000000023"), 113, "WIR-2" },
                    { new Guid("20000001-0000-0000-0000-000000000114"), new Guid("40000001-0000-0000-0000-000000000011"), "Check all materials installed are as per approved material submittal.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A1", new Guid("30000001-0000-0000-0000-000000000019"), 114, "WIR-3" },
                    { new Guid("20000001-0000-0000-0000-000000000115"), new Guid("40000001-0000-0000-0000-000000000011"), "Dimensions as per approved drawings.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A2", new Guid("30000001-0000-0000-0000-000000000002"), 115, "WIR-3" },
                    { new Guid("20000001-0000-0000-0000-000000000116"), new Guid("40000001-0000-0000-0000-000000000011"), "the cable/wire provided with Ties appropriately?", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A3", new Guid("30000001-0000-0000-0000-000000000004"), 116, "WIR-3" },
                    { new Guid("20000001-0000-0000-0000-000000000117"), new Guid("40000001-0000-0000-0000-000000000011"), "Verify measurement of lengths.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A4", new Guid("30000001-0000-0000-0000-000000000004"), 117, "WIR-3" },
                    { new Guid("20000001-0000-0000-0000-000000000118"), new Guid("40000001-0000-0000-0000-000000000011"), "Ensure that there are no damages to the cables.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A5", new Guid("30000001-0000-0000-0000-000000000005"), 118, "WIR-3" },
                    { new Guid("20000001-0000-0000-0000-000000000119"), new Guid("40000001-0000-0000-0000-000000000011"), "Verify that identification, grouping, spacing, markings and clamps are as required.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A6", new Guid("30000001-0000-0000-0000-000000000002"), 119, "WIR-3" },
                    { new Guid("20000001-0000-0000-0000-000000000120"), new Guid("40000001-0000-0000-0000-000000000011"), "Ensure the cable laying in trunking, trays and conduit installations are conducted in approved and accepted manner & as per project specification", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A7", new Guid("30000001-0000-0000-0000-000000000002"), 120, "WIR-3" },
                    { new Guid("20000001-0000-0000-0000-000000000121"), new Guid("40000001-0000-0000-0000-000000000011"), "Ensure that cable type and routes are as per approved Drawing.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A8", new Guid("30000001-0000-0000-0000-000000000002"), 121, "WIR-3" },
                    { new Guid("20000001-0000-0000-0000-000000000122"), new Guid("40000001-0000-0000-0000-000000000011"), "Ensure that correct types and sizes of wires and LV Cables are installed.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A9", new Guid("30000001-0000-0000-0000-000000000002"), 122, "WIR-3" },
                    { new Guid("20000001-0000-0000-0000-000000000123"), new Guid("40000001-0000-0000-0000-000000000011"), "Verify that routes and marked locations are correct as per approved drawings.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A10", new Guid("30000001-0000-0000-0000-000000000002"), 123, "WIR-3" },
                    { new Guid("20000001-0000-0000-0000-000000000124"), new Guid("40000001-0000-0000-0000-000000000011"), "Is cable/wire provided with Lugs?", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A11", new Guid("30000001-0000-0000-0000-000000000004"), 124, "WIR-3" },
                    { new Guid("20000001-0000-0000-0000-000000000125"), new Guid("40000001-0000-0000-0000-000000000011"), "Is the cable/wire termination done correctly?", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A9", new Guid("30000001-0000-0000-0000-000000000002"), 125, "WIR-3" },
                    { new Guid("20000001-0000-0000-0000-000000000126"), new Guid("40000001-0000-0000-0000-000000000011"), "Before pulling wires in conduit check that inside of conduit (and raceway in general) is free of burrs and is dry and clean.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A10", new Guid("30000001-0000-0000-0000-000000000004"), 126, "WIR-3" },
                    { new Guid("20000001-0000-0000-0000-000000000127"), new Guid("40000001-0000-0000-0000-000000000011"), "Ensure that extra Length left at every branch circuit outlet and pull-box, every cable passing through in order to allow inspection and for connections to be made.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A11", new Guid("30000001-0000-0000-0000-000000000004"), 127, "WIR-3" },
                    { new Guid("20000001-0000-0000-0000-000000000128"), new Guid("40000001-0000-0000-0000-000000000011"), "Ensure that tightening of electrical connectors and terminals including screws and bolts, in accordance with manufacturers recommendation", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A12", new Guid("30000001-0000-0000-0000-000000000004"), 128, "WIR-3" },
                    { new Guid("20000001-0000-0000-0000-000000000129"), new Guid("40000001-0000-0000-0000-000000000012"), "Is the Cable/Wire Continuity test Setup, okay?", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A1", new Guid("30000001-0000-0000-0000-000000000020"), 129, "WIR-3" },
                    { new Guid("20000001-0000-0000-0000-000000000130"), new Guid("40000001-0000-0000-0000-000000000012"), "Is the Cable/Wire Megger test Setup, okay?", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A2", new Guid("30000001-0000-0000-0000-000000000020"), 130, "WIR-3" },
                    { new Guid("20000001-0000-0000-0000-000000000131"), new Guid("40000001-0000-0000-0000-000000000012"), "Ensure Megger test is performed for Cables and accepted.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A3", new Guid("30000001-0000-0000-0000-000000000021"), 131, "WIR-3" },
                    { new Guid("20000001-0000-0000-0000-000000000132"), new Guid("40000001-0000-0000-0000-000000000012"), "Ensure Continuity test performed and accepted.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A4", new Guid("30000001-0000-0000-0000-000000000021"), 132, "WIR-3" },
                    { new Guid("20000001-0000-0000-0000-000000000133"), new Guid("40000001-0000-0000-0000-000000000013"), "Verify the installed Panel boards have approved submittals.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A1", new Guid("30000001-0000-0000-0000-000000000019"), 133, "WIR-3" },
                    { new Guid("20000001-0000-0000-0000-000000000134"), new Guid("40000001-0000-0000-0000-000000000013"), "Ensure the drawings used for installation are correct and approved.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A2", new Guid("30000001-0000-0000-0000-000000000023"), 134, "WIR-3" },
                    { new Guid("20000001-0000-0000-0000-000000000135"), new Guid("40000001-0000-0000-0000-000000000013"), "Align, level and securely fasten panelboards to structure.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A3", new Guid("30000001-0000-0000-0000-000000000004"), 135, "WIR-3" },
                    { new Guid("20000001-0000-0000-0000-000000000136"), new Guid("40000001-0000-0000-0000-000000000013"), "Check the Name Plate and Identification labels as per load schedule and approved submittals.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A4", new Guid("30000001-0000-0000-0000-000000000023"), 136, "WIR-3" },
                    { new Guid("20000001-0000-0000-0000-000000000137"), new Guid("40000001-0000-0000-0000-000000000013"), "Check all meters, circuit breakers, indication lamp, handles and locks are correct and undamaged.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A5", new Guid("30000001-0000-0000-0000-000000000005"), 137, "WIR-3" },
                    { new Guid("20000001-0000-0000-0000-000000000138"), new Guid("40000001-0000-0000-0000-000000000013"), "Fix surface mounted outdoor panelboards at least 25mm from wall ensuring supporting members do not prevent flow of air.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A6", new Guid("30000001-0000-0000-0000-000000000004"), 138, "WIR-3" },
                    { new Guid("20000001-0000-0000-0000-000000000139"), new Guid("40000001-0000-0000-0000-000000000013"), "Do not use connecting conduits to support panelboards.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A7", new Guid("30000001-0000-0000-0000-000000000004"), 139, "WIR-3" },
                    { new Guid("20000001-0000-0000-0000-000000000140"), new Guid("40000001-0000-0000-0000-000000000013"), "Panelboard Interiors: Do not install in cabinets until all conduit connections to abinet have been completed.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A8", new Guid("30000001-0000-0000-0000-000000000004"), 140, "WIR-3" },
                    { new Guid("20000001-0000-0000-0000-000000000141"), new Guid("40000001-0000-0000-0000-000000000013"), "Wiring: Inside Panelboards: Neatly arranged, accessible and strapped to prevent tension on circuit breaker terminals. Tap-off connections shall be split and bolted type, fully insulated.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A9", new Guid("30000001-0000-0000-0000-000000000004"), 141, "WIR-3" },
                    { new Guid("20000001-0000-0000-0000-000000000142"), new Guid("40000001-0000-0000-0000-000000000013"), "Equipment grounding for LV Panel Board is provided.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A10", new Guid("30000001-0000-0000-0000-000000000023"), 142, "WIR-3" },
                    { new Guid("20000001-0000-0000-0000-000000000143"), new Guid("40000001-0000-0000-0000-000000000013"), "Ensure that all unused openings are closed in the panels.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A11", new Guid("30000001-0000-0000-0000-000000000004"), 143, "WIR-3" },
                    { new Guid("20000001-0000-0000-0000-000000000144"), new Guid("40000001-0000-0000-0000-000000000013"), "Ensure that updated circuit directory and other relevant documents provided in the panel directory accordingly.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A12", new Guid("30000001-0000-0000-0000-000000000023"), 144, "WIR-3" },
                    { new Guid("20000001-0000-0000-0000-000000000145"), new Guid("40000001-0000-0000-0000-000000000013"), "Ensure that the Panel fixing bolts have been tightened with the suitable nut and washer.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A13", new Guid("30000001-0000-0000-0000-000000000004"), 145, "WIR-3" },
                    { new Guid("20000001-0000-0000-0000-000000000146"), new Guid("40000001-0000-0000-0000-000000000013"), "Touch up and cleaning of the pane.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A14", new Guid("30000001-0000-0000-0000-000000000004"), 146, "WIR-3" },
                    { new Guid("20000001-0000-0000-0000-000000000147"), new Guid("40000001-0000-0000-0000-000000000013"), "Ensure that each Panel Marking should be done as per approved drawing.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A15", new Guid("30000001-0000-0000-0000-000000000023"), 147, "WIR-3" },
                    { new Guid("20000001-0000-0000-0000-000000000148"), new Guid("40000001-0000-0000-0000-000000000014"), "Check the conduits and accessories are as per approved material submittal.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A1", new Guid("30000001-0000-0000-0000-000000000003"), 148, "WIR-3" },
                    { new Guid("20000001-0000-0000-0000-000000000149"), new Guid("40000001-0000-0000-0000-000000000014"), "Check and ensure the drawings used for installation are current and approved.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A2", new Guid("30000001-0000-0000-0000-000000000023"), 149, "WIR-3" },
                    { new Guid("20000001-0000-0000-0000-000000000150"), new Guid("40000001-0000-0000-0000-000000000014"), "Check the conduits and other associated material are new and undamaged.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A3", new Guid("30000001-0000-0000-0000-000000000005"), 150, "WIR-3" },
                    { new Guid("20000001-0000-0000-0000-000000000151"), new Guid("40000001-0000-0000-0000-000000000014"), "Check that the conduits are leveled and aligned properly.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A4", new Guid("30000001-0000-0000-0000-000000000004"), 151, "WIR-3" },
                    { new Guid("20000001-0000-0000-0000-000000000152"), new Guid("40000001-0000-0000-0000-000000000014"), "Check that the conduits are securely fixed.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A5", new Guid("30000001-0000-0000-0000-000000000004"), 152, "WIR-3" },
                    { new Guid("20000001-0000-0000-0000-000000000153"), new Guid("40000001-0000-0000-0000-000000000014"), "Check and ensure that the conduits and back boxes are sizely adequated.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A6", new Guid("30000001-0000-0000-0000-000000000023"), 153, "WIR-3" },
                    { new Guid("20000001-0000-0000-0000-000000000154"), new Guid("40000001-0000-0000-0000-000000000014"), "Check that the bottom hight of the back boxes is as per the shop drawing.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A7", new Guid("30000001-0000-0000-0000-000000000023"), 154, "WIR-3" },
                    { new Guid("20000001-0000-0000-0000-000000000155"), new Guid("40000001-0000-0000-0000-000000000014"), "Check that a minimum of 5mm thick cover is provided for conduits concealed in walls.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A8", new Guid("30000001-0000-0000-0000-000000000004"), 155, "WIR-3" },
                    { new Guid("20000001-0000-0000-0000-000000000156"), new Guid("40000001-0000-0000-0000-000000000014"), "Check the installation of conduits is co-ordinated with other services.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A9", new Guid("30000001-0000-0000-0000-000000000023"), 156, "WIR-3" },
                    { new Guid("20000001-0000-0000-0000-000000000157"), new Guid("40000001-0000-0000-0000-000000000014"), "Check the installation of conduiting as per approved drawings.", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "A10", new Guid("30000001-0000-0000-0000-000000000023"), 157, "WIR-3" }
                });

            migrationBuilder.InsertData(
                table: "WIRMasters",
                columns: new[] { "WIRMasterId", "CreatedDate", "Description", "Discipline", "IsActive", "Phase", "Sequence", "WIRName", "WIRNumber" },
                values: new object[,]
                {
                    { new Guid("10000001-0000-0000-0000-000000000001"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Verify all materials meet specifications and are properly documented before use in production", "Both", true, "Material", 1, "Material Receiving & Verification", "WIR-1" },
                    { new Guid("10000001-0000-0000-0000-000000000002"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Install and test all MEP systems including HVAC, plumbing, fire fighting, and refrigerant piping", "MEP", true, "Installation", 2, "MEP Installation & Testing", "WIR-2" },
                    { new Guid("10000001-0000-0000-0000-000000000003"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Install and test all electrical systems including cables, wires, panels, and conduits", "Electrical", true, "Installation", 3, "Electrical Installation & Testing", "WIR-3" },
                    { new Guid("10000001-0000-0000-0000-000000000004"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Assemble and erect precast concrete modular elements including walls, slabs, and partitions", "Civil", true, "Assembly", 4, "Structural Assembly & Erection", "WIR-4" },
                    { new Guid("10000001-0000-0000-0000-000000000005"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Apply all interior and exterior finishes including painting, tiling, ceilings, doors, windows, and woodwork", "Civil", true, "Finishing", 5, "Finishing Works", "WIR-5" },
                    { new Guid("10000001-0000-0000-0000-000000000006"), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Comprehensive final inspection of completed modular before loading and transportation to site", "Both", true, "Final", 6, "Final Pre-Loading Inspection", "WIR-6" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000008"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000009"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000010"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000011"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000012"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000013"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000014"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000015"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000016"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000017"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000018"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000019"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000020"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000021"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000022"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000023"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000024"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000025"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000026"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000027"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000028"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000029"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000030"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000031"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000032"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000033"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000034"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000035"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000036"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000037"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000038"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000039"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000040"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000041"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000042"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000043"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000044"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000045"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000046"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000047"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000048"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000049"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000050"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000051"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000052"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000053"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000054"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000055"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000056"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000057"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000058"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000059"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000060"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000061"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000062"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000063"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000064"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000065"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000066"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000067"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000068"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000069"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000070"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000071"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000072"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000073"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000074"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000075"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000076"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000077"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000078"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000079"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000080"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000081"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000082"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000083"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000084"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000085"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000086"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000087"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000088"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000089"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000090"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000091"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000092"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000093"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000094"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000095"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000096"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000097"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000098"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000099"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000100"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000101"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000102"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000103"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000104"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000105"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000106"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000107"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000108"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000109"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000110"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000111"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000112"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000113"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000114"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000115"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000116"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000117"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000118"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000119"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000120"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000121"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000122"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000123"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000124"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000125"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000126"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000127"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000128"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000129"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000130"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000131"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000132"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000133"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000134"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000135"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000136"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000137"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000138"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000139"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000140"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000141"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000142"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000143"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000144"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000145"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000146"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000147"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000148"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000149"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000150"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000151"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000152"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000153"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000154"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000155"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000156"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20000001-0000-0000-0000-000000000157"));

            migrationBuilder.DeleteData(
                table: "WIRMasters",
                keyColumn: "WIRMasterId",
                keyValue: new Guid("10000001-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "WIRMasters",
                keyColumn: "WIRMasterId",
                keyValue: new Guid("10000001-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "WIRMasters",
                keyColumn: "WIRMasterId",
                keyValue: new Guid("10000001-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "WIRMasters",
                keyColumn: "WIRMasterId",
                keyValue: new Guid("10000001-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "WIRMasters",
                keyColumn: "WIRMasterId",
                keyValue: new Guid("10000001-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "WIRMasters",
                keyColumn: "WIRMasterId",
                keyValue: new Guid("10000001-0000-0000-0000-000000000006"));
        }
    }
}
