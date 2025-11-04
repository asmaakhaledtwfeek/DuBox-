using Dubox.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Infrastructure.Seeding;

public static class ActivityMasterSeedData
{
    public static void SeedActivityMaster(ModelBuilder modelBuilder)
    {
        var seedDate = new DateTime(2024, 11, 1, 0, 0, 0, DateTimeKind.Utc);

        var activities = new List<ActivityMaster>
        {
            // Stage 1: Precast Production (3 activities)
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000001-0000-0000-0000-000000000001"),
                ActivityCode = "STAGE1-FAB",
                ActivityName = "Fabrication of boxes",
                Stage = "Stage 1: Precast Production",
                StageNumber = 1,
                SequenceInStage = 1,
                OverallSequence = 1,
                Description = "Manufacturing and fabrication of precast box components",
                EstimatedDurationDays = 3,
                IsWIRCheckpoint = false,
                IsActive = true,
                CreatedDate = seedDate
            },
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000001-0000-0000-0000-000000000002"),
                ActivityCode = "STAGE1-DEL",
                ActivityName = "Delivery of elements",
                Stage = "Stage 1: Precast Production",
                StageNumber = 1,
                SequenceInStage = 2,
                OverallSequence = 2,
                Description = "Transportation and delivery of precast elements to site",
                EstimatedDurationDays = 1,
                IsWIRCheckpoint = false,
                IsActive = true,
                CreatedDate = seedDate
            },
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000001-0000-0000-0000-000000000003"),
                ActivityCode = "STAGE1-QC",
                ActivityName = "Storage and QC",
                Stage = "Stage 1: Precast Production",
                StageNumber = 1,
                SequenceInStage = 3,
                OverallSequence = 3,
                Description = "Storage of elements and quality control inspection",
                EstimatedDurationDays = 1,
                IsWIRCheckpoint = false,
                IsActive = true,
                CreatedDate = seedDate
            },

            // Stage 2: Module Assembly (6 activities)
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000002-0000-0000-0000-000000000001"),
                ActivityCode = "STAGE2-ASM",
                ActivityName = "Assembly & joints",
                Stage = "Stage 2: Module Assembly",
                StageNumber = 2,
                SequenceInStage = 1,
                OverallSequence = 4,
                Description = "Assembly of box components and joint connections",
                EstimatedDurationDays = 2,
                IsWIRCheckpoint = false,
                IsActive = true,
                CreatedDate = seedDate
            },
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000002-0000-0000-0000-000000000002"),
                ActivityCode = "STAGE2-POD",
                ActivityName = "PODS installation",
                Stage = "Stage 2: Module Assembly",
                StageNumber = 2,
                SequenceInStage = 2,
                OverallSequence = 5,
                Description = "Installation of pre-assembled bathroom and kitchen PODs",
                EstimatedDurationDays = 1,
                IsWIRCheckpoint = false,
                IsActive = true,
                CreatedDate = seedDate
            },
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000002-0000-0000-0000-000000000003"),
                ActivityCode = "STAGE2-MEP",
                ActivityName = "MEP Cage installation",
                Stage = "Stage 2: Module Assembly",
                StageNumber = 2,
                SequenceInStage = 3,
                OverallSequence = 6,
                Description = "Installation of pre-assembled MEP cages",
                EstimatedDurationDays = 1,
                IsWIRCheckpoint = false,
                IsActive = true,
                CreatedDate = seedDate
            },
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000002-0000-0000-0000-000000000004"),
                ActivityCode = "STAGE2-ELC",
                ActivityName = "Electrical Containment",
                Stage = "Stage 2: Module Assembly",
                StageNumber = 2,
                SequenceInStage = 4,
                OverallSequence = 7,
                Description = "Installation of electrical conduits and containment systems",
                EstimatedDurationDays = 1,
                IsWIRCheckpoint = false,
                IsActive = true,
                CreatedDate = seedDate
            },
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000002-0000-0000-0000-000000000005"),
                ActivityCode = "STAGE2-CLO",
                ActivityName = "Box Closure",
                Stage = "Stage 2: Module Assembly",
                StageNumber = 2,
                SequenceInStage = 5,
                OverallSequence = 8,
                Description = "Final closure and sealing of box module",
                EstimatedDurationDays = 1,
                IsWIRCheckpoint = false,
                IsActive = true,
                CreatedDate = seedDate
            },
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000002-0000-0000-0000-000000000006"),
                ActivityCode = "STAGE2-WIR1",
                ActivityName = "WIR-1",
                Stage = "Stage 2: Module Assembly",
                StageNumber = 2,
                SequenceInStage = 6,
                OverallSequence = 9,
                Description = "Work Inspection Request - Stage 2 Completion",
                EstimatedDurationDays = 1,
                IsWIRCheckpoint = true,
                WIRCode = "WIR-1",
                IsActive = true,
                CreatedDate = seedDate
            },

            // Stage 3: MEP Phase 1 (6 activities)
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000003-0000-0000-0000-000000000001"),
                ActivityCode = "STAGE3-FCU",
                ActivityName = "Fan Coil Units",
                Stage = "Stage 3: MEP Phase 1",
                StageNumber = 3,
                SequenceInStage = 1,
                OverallSequence = 10,
                Description = "Installation of fan coil units for HVAC",
                EstimatedDurationDays = 1,
                IsWIRCheckpoint = false,
                IsActive = true,
                CreatedDate = seedDate
            },
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000003-0000-0000-0000-000000000002"),
                ActivityCode = "STAGE3-DCT",
                ActivityName = "Ducts & Insulation",
                Stage = "Stage 3: MEP Phase 1",
                StageNumber = 3,
                SequenceInStage = 2,
                OverallSequence = 11,
                Description = "Installation and insulation of HVAC ductwork",
                EstimatedDurationDays = 1,
                IsWIRCheckpoint = false,
                IsActive = true,
                CreatedDate = seedDate
            },
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000003-0000-0000-0000-000000000003"),
                ActivityCode = "STAGE3-DRN",
                ActivityName = "Drainage piping",
                Stage = "Stage 3: MEP Phase 1",
                StageNumber = 3,
                SequenceInStage = 3,
                OverallSequence = 12,
                Description = "Installation of drainage and wastewater piping",
                EstimatedDurationDays = 1,
                IsWIRCheckpoint = false,
                IsActive = true,
                CreatedDate = seedDate
            },
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000003-0000-0000-0000-000000000004"),
                ActivityCode = "STAGE3-WTR",
                ActivityName = "Water Piping",
                Stage = "Stage 3: MEP Phase 1",
                StageNumber = 3,
                SequenceInStage = 4,
                OverallSequence = 13,
                Description = "Installation of domestic water supply piping",
                EstimatedDurationDays = 1,
                IsWIRCheckpoint = false,
                IsActive = true,
                CreatedDate = seedDate
            },
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000003-0000-0000-0000-000000000005"),
                ActivityCode = "STAGE3-FF",
                ActivityName = "Fire Fighting Piping",
                Stage = "Stage 3: MEP Phase 1",
                StageNumber = 3,
                SequenceInStage = 5,
                OverallSequence = 14,
                Description = "Installation of fire protection and sprinkler piping",
                EstimatedDurationDays = 1,
                IsWIRCheckpoint = false,
                IsActive = true,
                CreatedDate = seedDate
            },
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000003-0000-0000-0000-000000000006"),
                ActivityCode = "STAGE3-WIR2",
                ActivityName = "WIR-2",
                Stage = "Stage 3: MEP Phase 1",
                StageNumber = 3,
                SequenceInStage = 6,
                OverallSequence = 15,
                Description = "Work Inspection Request - Stage 3 Completion",
                EstimatedDurationDays = 1,
                IsWIRCheckpoint = true,
                WIRCode = "WIR-2",
                IsActive = true,
                CreatedDate = seedDate
            },

            // Stage 4: Electrical & Framing (5 activities)
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000004-0000-0000-0000-000000000001"),
                ActivityCode = "STAGE4-ELCC",
                ActivityName = "Electrical Containment",
                Stage = "Stage 4: Electrical & Framing",
                StageNumber = 4,
                SequenceInStage = 1,
                OverallSequence = 16,
                Description = "Final electrical conduit and containment installation",
                EstimatedDurationDays = 1,
                IsWIRCheckpoint = false,
                IsActive = true,
                CreatedDate = seedDate
            },
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000004-0000-0000-0000-000000000002"),
                ActivityCode = "STAGE4-WIRE",
                ActivityName = "Electrical Wiring",
                Stage = "Stage 4: Electrical & Framing",
                StageNumber = 4,
                SequenceInStage = 2,
                OverallSequence = 17,
                Description = "Pulling and termination of electrical wiring",
                EstimatedDurationDays = 1,
                IsWIRCheckpoint = false,
                IsActive = true,
                CreatedDate = seedDate
            },
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000004-0000-0000-0000-000000000003"),
                ActivityCode = "STAGE4-DB",
                ActivityName = "DB and ONU Panel",
                Stage = "Stage 4: Electrical & Framing",
                StageNumber = 4,
                SequenceInStage = 3,
                OverallSequence = 18,
                Description = "Installation of distribution board and ONU network panels",
                EstimatedDurationDays = 1,
                IsWIRCheckpoint = false,
                IsActive = true,
                CreatedDate = seedDate
            },
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000004-0000-0000-0000-000000000004"),
                ActivityCode = "STAGE4-DRYWALL",
                ActivityName = "Dry Wall Framing",
                Stage = "Stage 4: Electrical & Framing",
                StageNumber = 4,
                SequenceInStage = 4,
                OverallSequence = 19,
                Description = "Installation of drywall framing and metal studs",
                EstimatedDurationDays = 2,
                IsWIRCheckpoint = false,
                IsActive = true,
                CreatedDate = seedDate
            },
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000004-0000-0000-0000-000000000005"),
                ActivityCode = "STAGE4-WIR3",
                ActivityName = "WIR-3",
                Stage = "Stage 4: Electrical & Framing",
                StageNumber = 4,
                SequenceInStage = 5,
                OverallSequence = 20,
                Description = "Work Inspection Request - Stage 4 Completion",
                EstimatedDurationDays = 1,
                IsWIRCheckpoint = true,
                WIRCode = "WIR-3",
                IsActive = true,
                CreatedDate = seedDate
            },

            // Stage 5: Interior Finishing (7 activities)
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000005-0000-0000-0000-000000000001"),
                ActivityCode = "STAGE5-CEILING",
                ActivityName = "False Ceiling",
                Stage = "Stage 5: Interior Finishing",
                StageNumber = 5,
                SequenceInStage = 1,
                OverallSequence = 21,
                Description = "Installation of suspended ceiling systems",
                EstimatedDurationDays = 1,
                IsWIRCheckpoint = false,
                IsActive = true,
                CreatedDate = seedDate
            },
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000005-0000-0000-0000-000000000002"),
                ActivityCode = "STAGE5-TILE",
                ActivityName = "Tile Fixing",
                Stage = "Stage 5: Interior Finishing",
                StageNumber = 5,
                SequenceInStage = 2,
                OverallSequence = 22,
                Description = "Installation of floor and wall tiles",
                EstimatedDurationDays = 2,
                IsWIRCheckpoint = false,
                IsActive = true,
                CreatedDate = seedDate
            },
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000005-0000-0000-0000-000000000003"),
                ActivityCode = "STAGE5-PAINT",
                ActivityName = "Painting",
                Stage = "Stage 5: Interior Finishing",
                StageNumber = 5,
                SequenceInStage = 3,
                OverallSequence = 23,
                Description = "Interior painting and finishing",
                EstimatedDurationDays = 2,
                IsWIRCheckpoint = false,
                IsActive = true,
                CreatedDate = seedDate
            },
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000005-0000-0000-0000-000000000004"),
                ActivityCode = "STAGE5-KITCHEN",
                ActivityName = "Kitchenette & Counters",
                Stage = "Stage 5: Interior Finishing",
                StageNumber = 5,
                SequenceInStage = 4,
                OverallSequence = 24,
                Description = "Installation of kitchen units and countertops",
                EstimatedDurationDays = 1,
                IsWIRCheckpoint = false,
                ApplicableBoxTypes = "Kitchen,Living Room",
                IsActive = true,
                CreatedDate = seedDate
            },
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000005-0000-0000-0000-000000000005"),
                ActivityCode = "STAGE5-DOORS",
                ActivityName = "Doors",
                Stage = "Stage 5: Interior Finishing",
                StageNumber = 5,
                SequenceInStage = 5,
                OverallSequence = 25,
                Description = "Installation of interior doors and frames",
                EstimatedDurationDays = 1,
                IsWIRCheckpoint = false,
                IsActive = true,
                CreatedDate = seedDate
            },
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000005-0000-0000-0000-000000000006"),
                ActivityCode = "STAGE5-WINDOWS",
                ActivityName = "Windows",
                Stage = "Stage 5: Interior Finishing",
                StageNumber = 5,
                SequenceInStage = 6,
                OverallSequence = 26,
                Description = "Installation of windows and glazing",
                EstimatedDurationDays = 1,
                IsWIRCheckpoint = false,
                IsActive = true,
                CreatedDate = seedDate
            },
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000005-0000-0000-0000-000000000007"),
                ActivityCode = "STAGE5-WIR4",
                ActivityName = "WIR-4",
                Stage = "Stage 5: Interior Finishing",
                StageNumber = 5,
                SequenceInStage = 7,
                OverallSequence = 27,
                Description = "Work Inspection Request - Stage 5 Completion",
                EstimatedDurationDays = 1,
                IsWIRCheckpoint = true,
                WIRCode = "WIR-4",
                IsActive = true,
                CreatedDate = seedDate
            },

            // Stage 6: MEP Phase 2 (9 activities)
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000006-0000-0000-0000-000000000001"),
                ActivityCode = "STAGE6-SWITCH",
                ActivityName = "Switches & Sockets",
                Stage = "Stage 6: MEP Phase 2",
                StageNumber = 6,
                SequenceInStage = 1,
                OverallSequence = 28,
                Description = "Installation of electrical switches and power sockets",
                EstimatedDurationDays = 1,
                IsWIRCheckpoint = false,
                IsActive = true,
                CreatedDate = seedDate
            },
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000006-0000-0000-0000-000000000002"),
                ActivityCode = "STAGE6-LIGHTS",
                ActivityName = "Light Fittings",
                Stage = "Stage 6: MEP Phase 2",
                StageNumber = 6,
                SequenceInStage = 2,
                OverallSequence = 29,
                Description = "Installation of light fixtures and fittings",
                EstimatedDurationDays = 1,
                IsWIRCheckpoint = false,
                IsActive = true,
                CreatedDate = seedDate
            },
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000006-0000-0000-0000-000000000003"),
                ActivityCode = "STAGE6-COPPER",
                ActivityName = "Copper Piping",
                Stage = "Stage 6: MEP Phase 2",
                StageNumber = 6,
                SequenceInStage = 3,
                OverallSequence = 30,
                Description = "Installation of copper piping for specialized systems",
                EstimatedDurationDays = 1,
                IsWIRCheckpoint = false,
                IsActive = true,
                CreatedDate = seedDate
            },
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000006-0000-0000-0000-000000000004"),
                ActivityCode = "STAGE6-SANITARY",
                ActivityName = "Sanitary Fittings",
                Stage = "Stage 6: MEP Phase 2",
                StageNumber = 6,
                SequenceInStage = 4,
                OverallSequence = 31,
                Description = "Installation of bathroom and sanitary fixtures",
                EstimatedDurationDays = 1,
                IsWIRCheckpoint = false,
                IsActive = true,
                CreatedDate = seedDate
            },
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000006-0000-0000-0000-000000000005"),
                ActivityCode = "STAGE6-THERMO",
                ActivityName = "Thermostats",
                Stage = "Stage 6: MEP Phase 2",
                StageNumber = 6,
                SequenceInStage = 5,
                OverallSequence = 32,
                Description = "Installation of HVAC thermostats and controls",
                EstimatedDurationDays = 1,
                IsWIRCheckpoint = false,
                IsActive = true,
                CreatedDate = seedDate
            },
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000006-0000-0000-0000-000000000006"),
                ActivityCode = "STAGE6-AIROUT",
                ActivityName = "Air Outlet",
                Stage = "Stage 6: MEP Phase 2",
                StageNumber = 6,
                SequenceInStage = 6,
                OverallSequence = 33,
                Description = "Installation of HVAC air outlets and diffusers",
                EstimatedDurationDays = 1,
                IsWIRCheckpoint = false,
                IsActive = true,
                CreatedDate = seedDate
            },
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000006-0000-0000-0000-000000000007"),
                ActivityCode = "STAGE6-SPRINKLER",
                ActivityName = "Sprinkler",
                Stage = "Stage 6: MEP Phase 2",
                StageNumber = 6,
                SequenceInStage = 7,
                OverallSequence = 34,
                Description = "Installation of fire sprinkler heads",
                EstimatedDurationDays = 1,
                IsWIRCheckpoint = false,
                IsActive = true,
                CreatedDate = seedDate
            },
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000006-0000-0000-0000-000000000008"),
                ActivityCode = "STAGE6-SMOKE",
                ActivityName = "Smoke Detector",
                Stage = "Stage 6: MEP Phase 2",
                StageNumber = 6,
                SequenceInStage = 8,
                OverallSequence = 35,
                Description = "Installation of smoke detectors and fire alarm devices",
                EstimatedDurationDays = 1,
                IsWIRCheckpoint = false,
                IsActive = true,
                CreatedDate = seedDate
            },
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000006-0000-0000-0000-000000000009"),
                ActivityCode = "STAGE6-WIR5",
                ActivityName = "WIR-5",
                Stage = "Stage 6: MEP Phase 2",
                StageNumber = 6,
                SequenceInStage = 9,
                OverallSequence = 36,
                Description = "Work Inspection Request - Final MEP Inspection",
                EstimatedDurationDays = 1,
                IsWIRCheckpoint = true,
                WIRCode = "WIR-5",
                IsActive = true,
                CreatedDate = seedDate
            },

            // Stage 7: Final Inspection & Dispatch (4 activities)
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000007-0000-0000-0000-000000000001"),
                ActivityCode = "STAGE7-IRON",
                ActivityName = "Ironmongery Installation",
                Stage = "Stage 7: Final Inspection & Dispatch",
                StageNumber = 7,
                SequenceInStage = 1,
                OverallSequence = 37,
                Description = "Installation of door handles, locks, hinges and other hardware",
                EstimatedDurationDays = 1,
                IsWIRCheckpoint = false,
                IsActive = true,
                CreatedDate = seedDate
            },
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000007-0000-0000-0000-000000000002"),
                ActivityCode = "STAGE7-INSP",
                ActivityName = "Final Inspection",
                Stage = "Stage 7: Final Inspection & Dispatch",
                StageNumber = 7,
                SequenceInStage = 2,
                OverallSequence = 38,
                Description = "Comprehensive final quality inspection of completed module",
                EstimatedDurationDays = 1,
                IsWIRCheckpoint = false,
                IsActive = true,
                CreatedDate = seedDate
            },
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000007-0000-0000-0000-000000000003"),
                ActivityCode = "STAGE7-WRAP",
                ActivityName = "Module Wrapping",
                Stage = "Stage 7: Final Inspection & Dispatch",
                StageNumber = 7,
                SequenceInStage = 3,
                OverallSequence = 39,
                Description = "Protective wrapping of module for delivery to site",
                EstimatedDurationDays = 1,
                IsWIRCheckpoint = false,
                IsActive = true,
                CreatedDate = seedDate
            },
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000007-0000-0000-0000-000000000004"),
                ActivityCode = "STAGE7-WIR6",
                ActivityName = "WIR-6",
                Stage = "Stage 7: Final Inspection & Dispatch",
                StageNumber = 7,
                SequenceInStage = 4,
                OverallSequence = 40,
                Description = "Work Inspection Request - Final QC Clearance for Dispatch",
                EstimatedDurationDays = 1,
                IsWIRCheckpoint = true,
                WIRCode = "WIR-6",
                IsActive = true,
                CreatedDate = seedDate
            },

            // Stage 8: Site Installation (3 activities)
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000008-0000-0000-0000-000000000001"),
                ActivityCode = "STAGE8-RFID",
                ActivityName = "RFID Tracking to Site",
                Stage = "Stage 8: Site Installation",
                StageNumber = 8,
                SequenceInStage = 1,
                OverallSequence = 41,
                Description = "RFID tag activation and tracking during transport to site",
                EstimatedDurationDays = 1,
                IsWIRCheckpoint = false,
                IsActive = true,
                CreatedDate = seedDate
            },
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000008-0000-0000-0000-000000000002"),
                ActivityCode = "STAGE8-INST",
                ActivityName = "Installation on Project",
                Stage = "Stage 8: Site Installation",
                StageNumber = 8,
                SequenceInStage = 2,
                OverallSequence = 42,
                Description = "Physical installation and positioning of module at project site",
                EstimatedDurationDays = 1,
                IsWIRCheckpoint = false,
                IsActive = true,
                CreatedDate = seedDate
            },
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000008-0000-0000-0000-000000000003"),
                ActivityCode = "STAGE8-COMP",
                ActivityName = "Box Completion",
                Stage = "Stage 8: Site Installation",
                StageNumber = 8,
                SequenceInStage = 3,
                OverallSequence = 43,
                Description = "Final verification and completion signoff at site",
                EstimatedDurationDays = 1,
                IsWIRCheckpoint = false,
                IsActive = true,
                CreatedDate = seedDate
            }
        };

        modelBuilder.Entity<ActivityMaster>().HasData(activities);
    }
}
