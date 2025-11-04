using Dubox.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Infrastructure.Seeding
{

    public static class ActivityMasterSeedData
    {
        public static void SeedActivityMaster(ModelBuilder modelBuilder)
        {
            var activities = new List<ActivityMaster>
            {

                new ActivityMaster
                {
                    ActivityMasterId = 1,
                    ActivityCode = "ACT-001",
                    ActivityName = "Fabrication of boxes",
                    ActivityDescription = "Manufacture precast walls, slabs, and structural elements",
                    Department = "Civil",
                    Trade = "Precast",
                    StandardDuration = 5,
                    Sequence = 1,
                    IsWIRCheckpoint = false,
                    IsActive = true
                },
                new ActivityMaster
                {
                    ActivityMasterId = 2,
                    ActivityCode = "ACT-002",
                    ActivityName = "Delivery of elements",
                    ActivityDescription = "Transport precast elements to assembly area",
                    Department = "Civil",
                    Trade = "Logistics",
                    StandardDuration = 3,
                    Sequence = 2,
                    IsWIRCheckpoint = false,
                    IsActive = true
                },
                new ActivityMaster
                {
                    ActivityMasterId = 3,
                    ActivityCode = "ACT-003",
                    ActivityName = "Storage and QC",
                    ActivityDescription = "Perform quality checks and store elements for assembly",
                    Department = "QC",
                    Trade = "Quality Control",
                    StandardDuration = 1,
                    Sequence = 3,
                    IsWIRCheckpoint = false,
                    IsActive = true
                },

                // ========== STAGE 2: MODULE ASSEMBLY ==========
                new ActivityMaster
                {
                    ActivityMasterId = 4,
                    ActivityCode = "ACT-004",
                    ActivityName = "Assembly & joints",
                    ActivityDescription = "Assemble modules and seal structural joints",
                    Department = "Civil",
                    Trade = "Assembly",
                    StandardDuration = 4,
                    Sequence = 4,
                    IsWIRCheckpoint = false,
                    IsActive = true
                },
                new ActivityMaster
                {
                    ActivityMasterId = 5,
                    ActivityCode = "ACT-005",
                    ActivityName = "PODS installation",
                    ActivityDescription = "Install preassembled bathroom PODs",
                    Department = "Civil",
                    Trade = "Assembly",
                    StandardDuration = 2,
                    Sequence = 5,
                    IsWIRCheckpoint = false,
                    IsActive = true
                },
                new ActivityMaster
                {
                    ActivityMasterId = 6,
                    ActivityCode = "ACT-006",
                    ActivityName = "MEP Cage installation",
                    ActivityDescription = "Install preassembled MEP cage",
                    Department = "MEP",
                    Trade = "Assembly",
                    StandardDuration = 2,
                    Sequence = 6,
                    IsWIRCheckpoint = false,
                    IsActive = true
                },
                new ActivityMaster
                {
                    ActivityMasterId = 7,
                    ActivityCode = "ACT-007",
                    ActivityName = "Electrical Containment (Assembly)",
                    ActivityDescription = "Install electrical conduits during assembly",
                    Department = "MEP",
                    Trade = "Electrical",
                    StandardDuration = 2,
                    Sequence = 7,
                    IsWIRCheckpoint = false,
                    IsActive = true
                },
                new ActivityMaster
                {
                    ActivityMasterId = 8,
                    ActivityCode = "ACT-008",
                    ActivityName = "Box Closure",
                    ActivityDescription = "Complete box closures and initial QC inspection",
                    Department = "Civil",
                    Trade = "Assembly",
                    StandardDuration = 1,
                    Sequence = 8,
                    IsWIRCheckpoint = true,
                    WIRNumber = "WIR-1",
                    IsActive = true
                },

                // ========== STAGE 3: MEP INSTALLATION PHASE 1 ==========
                new ActivityMaster
                {
                    ActivityMasterId = 9,
                    ActivityCode = "ACT-009",
                    ActivityName = "Fan Coil Units",
                    ActivityDescription = "Install AC units",
                    Department = "MEP",
                    Trade = "Mechanical",
                    StandardDuration = 2,
                    Sequence = 9,
                    IsWIRCheckpoint = false,
                    IsActive = true
                },
                new ActivityMaster
                {
                    ActivityMasterId = 10,
                    ActivityCode = "ACT-010",
                    ActivityName = "Ducts & Insulation",
                    ActivityDescription = "Install ducts and insulation",
                    Department = "MEP",
                    Trade = "Mechanical",
                    StandardDuration = 2,
                    Sequence = 10,
                    IsWIRCheckpoint = false,
                    IsActive = true
                },
                new ActivityMaster
                {
                    ActivityMasterId = 11,
                    ActivityCode = "ACT-011",
                    ActivityName = "Drainage piping",
                    ActivityDescription = "Complete drainage piping",
                    Department = "MEP",
                    Trade = "Mechanical",
                    StandardDuration = 2,
                    Sequence = 11,
                    IsWIRCheckpoint = false,
                    IsActive = true
                },
                new ActivityMaster
                {
                    ActivityMasterId = 12,
                    ActivityCode = "ACT-012",
                    ActivityName = "Water Piping",
                    ActivityDescription = "Complete water piping",
                    Department = "MEP",
                    Trade = "Mechanical",
                    StandardDuration = 2,
                    Sequence = 12,
                    IsWIRCheckpoint = false,
                    IsActive = true
                },
                new ActivityMaster
                {
                    ActivityMasterId = 13,
                    ActivityCode = "ACT-013",
                    ActivityName = "Fire Fighting Piping",
                    ActivityDescription = "Complete firefighting piping",
                    Department = "MEP",
                    Trade = "Mechanical",
                    StandardDuration = 2,
                    Sequence = 13,
                    IsWIRCheckpoint = true,
                    WIRNumber = "WIR-2",
                    IsActive = true
                },

                // ========== STAGE 4: ELECTRICAL & STRUCTURAL FRAMING ==========
                new ActivityMaster
                {
                    ActivityMasterId = 14,
                    ActivityCode = "ACT-014",
                    ActivityName = "Electrical Containment",
                    ActivityDescription = "Install electrical containment",
                    Department = "MEP",
                    Trade = "Electrical",
                    StandardDuration = 2,
                    Sequence = 14,
                    IsWIRCheckpoint = false,
                    IsActive = true
                },
                new ActivityMaster
                {
                    ActivityMasterId = 15,
                    ActivityCode = "ACT-015",
                    ActivityName = "Electrical Wiring",
                    ActivityDescription = "Complete electrical wiring",
                    Department = "MEP",
                    Trade = "Electrical",
                    StandardDuration = 2,
                    Sequence = 15,
                    IsWIRCheckpoint = false,
                    IsActive = true
                },
                new ActivityMaster
                {
                    ActivityMasterId = 16,
                    ActivityCode = "ACT-016",
                    ActivityName = "DB and ONU Panel",
                    ActivityDescription = "Install distribution board and ONU panel",
                    Department = "MEP",
                    Trade = "Electrical",
                    StandardDuration = 2,
                    Sequence = 16,
                    IsWIRCheckpoint = false,
                    IsActive = true
                },
                new ActivityMaster
                {
                    ActivityMasterId = 17,
                    ActivityCode = "ACT-017",
                    ActivityName = "Dry Wall Framing",
                    ActivityDescription = "Complete drywall framing for partitions",
                    Department = "Civil",
                    Trade = "Finishing",
                    StandardDuration = 1,
                    Sequence = 17,
                    IsWIRCheckpoint = true,
                    WIRNumber = "WIR-3",
                    IsActive = true
                },

                // ========== STAGE 5: INTERIOR FINISHING ==========
                new ActivityMaster
                {
                    ActivityMasterId = 18,
                    ActivityCode = "ACT-018",
                    ActivityName = "False Ceiling",
                    ActivityDescription = "Install false ceilings",
                    Department = "Civil",
                    Trade = "Finishing",
                    StandardDuration = 1,
                    Sequence = 18,
                    IsWIRCheckpoint = false,
                    IsActive = true
                },
                new ActivityMaster
                {
                    ActivityMasterId = 19,
                    ActivityCode = "ACT-019",
                    ActivityName = "Tile Fixing",
                    ActivityDescription = "Install floor and wall tiles",
                    Department = "Civil",
                    Trade = "Finishing",
                    StandardDuration = 2,
                    Sequence = 19,
                    IsWIRCheckpoint = false,
                    IsActive = true
                },
                new ActivityMaster
                {
                    ActivityMasterId = 20,
                    ActivityCode = "ACT-020",
                    ActivityName = "Painting (Internal & External)",
                    ActivityDescription = "Complete painting",
                    Department = "Civil",
                    Trade = "Finishing",
                    StandardDuration = 2,
                    Sequence = 20,
                    IsWIRCheckpoint = false,
                    IsActive = true
                },
                new ActivityMaster
                {
                    ActivityMasterId = 21,
                    ActivityCode = "ACT-021",
                    ActivityName = "Kitchenette and Counters",
                    ActivityDescription = "Fix kitchenettes and counters",
                    Department = "Civil",
                    Trade = "Finishing",
                    StandardDuration = 2,
                    Sequence = 21,
                    IsWIRCheckpoint = false,
                    IsActive = true
                },
                new ActivityMaster
                {
                    ActivityMasterId = 22,
                    ActivityCode = "ACT-022",
                    ActivityName = "Doors",
                    ActivityDescription = "Install doors",
                    Department = "Civil",
                    Trade = "Finishing",
                    StandardDuration = 1,
                    Sequence = 22,
                    IsWIRCheckpoint = false,
                    IsActive = true
                },
                new ActivityMaster
                {
                    ActivityMasterId = 23,
                    ActivityCode = "ACT-023",
                    ActivityName = "Windows",
                    ActivityDescription = "Install windows",
                    Department = "Civil",
                    Trade = "Finishing",
                    StandardDuration = 1,
                    Sequence = 23,
                    IsWIRCheckpoint = true,
                    WIRNumber = "WIR-4",
                    IsActive = true
                },

                // ========== STAGE 6: MEP INSTALLATION PHASE 2 (FINAL FIX) ==========
                new ActivityMaster
                {
                    ActivityMasterId = 24,
                    ActivityCode = "ACT-024",
                    ActivityName = "Switches & Sockets",
                    ActivityDescription = "Install switches and sockets",
                    Department = "MEP",
                    Trade = "Electrical",
                    StandardDuration = 2,
                    Sequence = 24,
                    IsWIRCheckpoint = false,
                    IsActive = true
                },
                new ActivityMaster
                {
                    ActivityMasterId = 25,
                    ActivityCode = "ACT-025",
                    ActivityName = "Light Fittings",
                    ActivityDescription = "Install light fittings",
                    Department = "MEP",
                    Trade = "Electrical",
                    StandardDuration = 2,
                    Sequence = 25,
                    IsWIRCheckpoint = false,
                    IsActive = true
                },
                new ActivityMaster
                {
                    ActivityMasterId = 26,
                    ActivityCode = "ACT-026",
                    ActivityName = "Copper Piping",
                    ActivityDescription = "Install chilled water piping",
                    Department = "MEP",
                    Trade = "Mechanical",
                    StandardDuration = 2,
                    Sequence = 26,
                    IsWIRCheckpoint = false,
                    IsActive = true
                },
                new ActivityMaster
                {
                    ActivityMasterId = 27,
                    ActivityCode = "ACT-027",
                    ActivityName = "Sanitary Fittings - Kitchen",
                    ActivityDescription = "Install sanitary fixtures",
                    Department = "MEP",
                    Trade = "Mechanical",
                    StandardDuration = 2,
                    Sequence = 27,
                    IsWIRCheckpoint = false,
                    IsActive = true
                },
                new ActivityMaster
                {
                    ActivityMasterId = 28,
                    ActivityCode = "ACT-028",
                    ActivityName = "Thermostats",
                    ActivityDescription = "Install thermostats",
                    Department = "MEP",
                    Trade = "Mechanical",
                    StandardDuration = 2,
                    Sequence = 28,
                    IsWIRCheckpoint = false,
                    IsActive = true
                },
                new ActivityMaster
                {
                    ActivityMasterId = 29,
                    ActivityCode = "ACT-029",
                    ActivityName = "Air Outlet",
                    ActivityDescription = "Install air outlets",
                    Department = "MEP",
                    Trade = "Mechanical",
                    StandardDuration = 2,
                    Sequence = 29,
                    IsWIRCheckpoint = false,
                    IsActive = true
                },
                new ActivityMaster
                {
                    ActivityMasterId = 30,
                    ActivityCode = "ACT-030",
                    ActivityName = "Sprinkler",
                    ActivityDescription = "Install sprinkler system",
                    Department = "MEP",
                    Trade = "Mechanical",
                    StandardDuration = 2,
                    Sequence = 30,
                    IsWIRCheckpoint = false,
                    IsActive = true
                },
                new ActivityMaster
                {
                    ActivityMasterId = 31,
                    ActivityCode = "ACT-031",
                    ActivityName = "Smoke Detector",
                    ActivityDescription = "Install smoke detectors",
                    Department = "MEP",
                    Trade = "Mechanical",
                    StandardDuration = 2,
                    Sequence = 31,
                    IsWIRCheckpoint = true,
                    WIRNumber = "WIR-5",
                    IsActive = true
                },

                // ========== STAGE 7: FINAL INSPECTION & DISPATCH ==========
                new ActivityMaster
                {
                    ActivityMasterId = 32,
                    ActivityCode = "ACT-032",
                    ActivityName = "Iron Mongeries",
                    ActivityDescription = "Install ironmongery (locks, handles, accessories)",
                    Department = "Civil",
                    Trade = "Finishing",
                    StandardDuration = 2,
                    Sequence = 32,
                    IsWIRCheckpoint = false,
                    IsActive = true
                },
                new ActivityMaster
                {
                    ActivityMasterId = 33,
                    ActivityCode = "ACT-033",
                    ActivityName = "Inspection & Wrapping",
                    ActivityDescription = "Conduct comprehensive final inspection and wrap modules for delivery",
                    Department = "QC",
                    Trade = "Quality Control",
                    StandardDuration = 1,
                    Sequence = 33,
                    IsWIRCheckpoint = true,
                    WIRNumber = "WIR-6",
                    IsActive = true
                }
            };

            modelBuilder.Entity<ActivityMaster>().HasData(activities);
        }
    }
}
