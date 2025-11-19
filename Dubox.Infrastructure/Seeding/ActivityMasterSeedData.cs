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
            // ID 1 - Assembly & joints
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000001-0000-0000-0000-000000000001"),
                ActivityCode = "STAGE2-ASM",
                ActivityName = "Assembly & joints",
                Stage = "Stage 2: Module Assembly",
                StageNumber = 2,
                SequenceInStage = 1,
                OverallSequence = 1,
                Description = "Assembly of box components and joint connections",
                EstimatedDurationDays = 1,
                IsWIRCheckpoint = false,
                IsActive = true,
                CreatedDate = seedDate
            },
            
            // ID 2 - PODS
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000001-0000-0000-0000-000000000002"),
                ActivityCode = "STAGE2-POD",
                ActivityName = "PODS installation",
                Stage = "Stage 2: Module Assembly",
                StageNumber = 2,
                SequenceInStage = 2,
                OverallSequence = 2,
                Description = "Installation of pre-assembled bathroom and kitchen PODs",
                EstimatedDurationDays = 1,
                IsWIRCheckpoint = false,
                IsActive = true,
                CreatedDate = seedDate
            },
            
            // ID 3 - M/C Units (MEP Cage)
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000001-0000-0000-0000-000000000003"),
                ActivityCode = "STAGE2-MEP",
                ActivityName = "MEP Cage installation",
                Stage = "Stage 2: Module Assembly",
                StageNumber = 2,
                SequenceInStage = 3,
                OverallSequence = 3,
                Description = "Installation of pre-assembled MEP cages",
                EstimatedDurationDays = 1,
                IsWIRCheckpoint = false,
                IsActive = true,
                CreatedDate = seedDate
            },
            
            // ID 4 - Box Closure (TRIGGERS WIR-1)
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000001-0000-0000-0000-000000000004"),
                ActivityCode = "STAGE2-CLO",
                ActivityName = "Box Closure",
                Stage = "Stage 2: Module Assembly",
                StageNumber = 2,
                SequenceInStage = 4,
                OverallSequence = 4,
                Description = "Final closure and sealing of box module - triggers WIR-1",
                EstimatedDurationDays = 1,
                IsWIRCheckpoint = true,
                WIRCode = "WIR-1",
                IsActive = true,
                CreatedDate = seedDate
            },

            // ID 5 - Ducts & Insulation
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000002-0000-0000-0000-000000000001"),
                ActivityCode = "STAGE3-DCT",
                ActivityName = "Ducts & Insulation",
                Stage = "Stage 3: MEP Phase 1",
                StageNumber = 3,
                SequenceInStage = 1,
                OverallSequence = 5,
                Description = "Installation and insulation of HVAC ductwork",
                EstimatedDurationDays = 2,
                IsWIRCheckpoint = false,
                IsActive = true,
                CreatedDate = seedDate
            },
            
            // ID 6 - Drainage piping
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000002-0000-0000-0000-000000000002"),
                ActivityCode = "STAGE3-DRN",
                ActivityName = "Drainage piping",
                Stage = "Stage 3: MEP Phase 1",
                StageNumber = 3,
                SequenceInStage = 2,
                OverallSequence = 6,
                Description = "Installation of drainage and wastewater piping",
                EstimatedDurationDays = 2,
                IsWIRCheckpoint = false,
                IsActive = true,
                CreatedDate = seedDate
            },
            
            // ID 7 - Water Piping
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000002-0000-0000-0000-000000000003"),
                ActivityCode = "STAGE3-WTR",
                ActivityName = "Water Piping",
                Stage = "Stage 3: MEP Phase 1",
                StageNumber = 3,
                SequenceInStage = 3,
                OverallSequence = 7,
                Description = "Installation of domestic water supply piping",
                EstimatedDurationDays = 2,
                IsWIRCheckpoint = false,
                IsActive = true,
                CreatedDate = seedDate
            },
            
            // ID 8 - Fire Fighting Piping (TRIGGERS WIR-2)
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000002-0000-0000-0000-000000000004"),
                ActivityCode = "STAGE3-FF",
                ActivityName = "Fire Fighting Piping",
                Stage = "Stage 3: MEP Phase 1",
                StageNumber = 3,
                SequenceInStage = 4,
                OverallSequence = 8,
                Description = "Installation of fire protection and sprinkler piping - triggers WIR-2",
                EstimatedDurationDays = 2,
                IsWIRCheckpoint = true,
                WIRCode = "WIR-2",
                IsActive = true,
                CreatedDate = seedDate
            },

            // ID 9 - Electrical Containment
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000003-0000-0000-0000-000000000001"),
                ActivityCode = "STAGE4-ELCC",
                ActivityName = "Electrical Containment",
                Stage = "Stage 4: Electrical & Framing",
                StageNumber = 4,
                SequenceInStage = 1,
                OverallSequence = 9,
                Description = "Final electrical conduit and containment installation",
                EstimatedDurationDays = 2,
                IsWIRCheckpoint = false,
                IsActive = true,
                CreatedDate = seedDate
            },
            
            // ID 10 - Electrical Wiring
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000003-0000-0000-0000-000000000002"),
                ActivityCode = "STAGE4-WIRE",
                ActivityName = "Electrical Wiring",
                Stage = "Stage 4: Electrical & Framing",
                StageNumber = 4,
                SequenceInStage = 2,
                OverallSequence = 10,
                Description = "Pulling and termination of electrical wiring",
                EstimatedDurationDays = 2,
                IsWIRCheckpoint = false,
                IsActive = true,
                CreatedDate = seedDate
            },
            
            // ID 11 - Dry Wall Framing
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000003-0000-0000-0000-000000000003"),
                ActivityCode = "STAGE4-DRYWALL",
                ActivityName = "Dry Wall Framing",
                Stage = "Stage 4: Electrical & Framing",
                StageNumber = 4,
                SequenceInStage = 3,
                OverallSequence = 11,
                Description = "Installation of drywall framing and metal studs",
                EstimatedDurationDays = 2,
                IsWIRCheckpoint = false,
                IsActive = true,
                CreatedDate = seedDate
            },
            
            // ID 12 - DB and ONU Panel (TRIGGERS WIR-3)
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000003-0000-0000-0000-000000000004"),
                ActivityCode = "STAGE4-DB",
                ActivityName = "DB and ONU Panel",
                Stage = "Stage 4: Electrical & Framing",
                StageNumber = 4,
                SequenceInStage = 4,
                OverallSequence = 12,
                Description = "Installation of distribution board and ONU network panels - triggers WIR-3",
                EstimatedDurationDays = 2,
                IsWIRCheckpoint = true,
                WIRCode = "WIR-3",
                IsActive = true,
                CreatedDate = seedDate
            },

            // ID 13 - False Ceiling
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000004-0000-0000-0000-000000000001"),
                ActivityCode = "STAGE5-CEILING",
                ActivityName = "False Ceiling",
                Stage = "Stage 5: Interior Finishing",
                StageNumber = 5,
                SequenceInStage = 1,
                OverallSequence = 13,
                Description = "Installation of suspended ceiling systems",
                EstimatedDurationDays = 1,
                IsWIRCheckpoint = false,
                IsActive = true,
                CreatedDate = seedDate
            },
            
            // ID 14 - Tile Fixing
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000004-0000-0000-0000-000000000002"),
                ActivityCode = "STAGE5-TILE",
                ActivityName = "Tile Fixing",
                Stage = "Stage 5: Interior Finishing",
                StageNumber = 5,
                SequenceInStage = 2,
                OverallSequence = 14,
                Description = "Installation of floor and wall tiles",
                EstimatedDurationDays = 2,
                IsWIRCheckpoint = false,
                IsActive = true,
                CreatedDate = seedDate
            },
            
            // ID 15 - Painting (Internal & External)
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000004-0000-0000-0000-000000000003"),
                ActivityCode = "STAGE5-PAINT",
                ActivityName = "Painting (Internal & External)",
                Stage = "Stage 5: Interior Finishing",
                StageNumber = 5,
                SequenceInStage = 3,
                OverallSequence = 15,
                Description = "Interior and exterior painting and finishing",
                EstimatedDurationDays = 2,
                IsWIRCheckpoint = false,
                IsActive = true,
                CreatedDate = seedDate
            },
            
            // ID 16 - Kitchenette and Counters
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000004-0000-0000-0000-000000000004"),
                ActivityCode = "STAGE5-KITCHEN",
                ActivityName = "Kitchenette and Counters",
                Stage = "Stage 5: Interior Finishing",
                StageNumber = 5,
                SequenceInStage = 4,
                OverallSequence = 16,
                Description = "Installation of kitchen units and countertops",
                EstimatedDurationDays = 1,
                IsWIRCheckpoint = false,
                ApplicableBoxTypes = "Kitchen,Living Room",
                IsActive = true,
                CreatedDate = seedDate
            },
            
            // ID 17 - Doors
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000004-0000-0000-0000-000000000005"),
                ActivityCode = "STAGE5-DOORS",
                ActivityName = "Doors",
                Stage = "Stage 5: Interior Finishing",
                StageNumber = 5,
                SequenceInStage = 5,
                OverallSequence = 17,
                Description = "Installation of interior doors and frames",
                EstimatedDurationDays = 1,
                IsWIRCheckpoint = false,
                IsActive = true,
                CreatedDate = seedDate
            },
            
            // ID 18 - Windows (TRIGGERS WIR-4)
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000004-0000-0000-0000-000000000006"),
                ActivityCode = "STAGE5-WINDOWS",
                ActivityName = "Windows",
                Stage = "Stage 5: Interior Finishing",
                StageNumber = 5,
                SequenceInStage = 6,
                OverallSequence = 18,
                Description = "Installation of windows and glazing - triggers WIR-4",
                EstimatedDurationDays = 1,
                IsWIRCheckpoint = true,
                WIRCode = "WIR-4",
                IsActive = true,
                CreatedDate = seedDate
            },

            // ID 19 - Switches & Sockets
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000005-0000-0000-0000-000000000001"),
                ActivityCode = "STAGE6-SWITCH",
                ActivityName = "Switches & Sockets",
                Stage = "Stage 6: MEP Phase 2",
                StageNumber = 6,
                SequenceInStage = 1,
                OverallSequence = 19,
                Description = "Installation of electrical switches and power sockets",
                EstimatedDurationDays = 2,
                IsWIRCheckpoint = false,
                IsActive = true,
                CreatedDate = seedDate
            },
            
            // ID 20 - Light Fittings
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000005-0000-0000-0000-000000000002"),
                ActivityCode = "STAGE6-LIGHTS",
                ActivityName = "Light Fittings",
                Stage = "Stage 6: MEP Phase 2",
                StageNumber = 6,
                SequenceInStage = 2,
                OverallSequence = 20,
                Description = "Installation of light fixtures and fittings",
                EstimatedDurationDays = 2,
                IsWIRCheckpoint = false,
                IsActive = true,
                CreatedDate = seedDate
            },
            
            // ID 21 - Copper Piping
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000005-0000-0000-0000-000000000003"),
                ActivityCode = "STAGE6-COPPER",
                ActivityName = "Copper Piping",
                Stage = "Stage 6: MEP Phase 2",
                StageNumber = 6,
                SequenceInStage = 3,
                OverallSequence = 21,
                Description = "Installation of copper piping for specialized systems",
                EstimatedDurationDays = 2,
                IsWIRCheckpoint = false,
                IsActive = true,
                CreatedDate = seedDate
            },
            
            // ID 22 - Sanitary Fittings - Kitchen
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000005-0000-0000-0000-000000000004"),
                ActivityCode = "STAGE6-SANITARY",
                ActivityName = "Sanitary Fittings - Kitchen",
                Stage = "Stage 6: MEP Phase 2",
                StageNumber = 6,
                SequenceInStage = 4,
                OverallSequence = 22,
                Description = "Installation of kitchen and sanitary fixtures",
                EstimatedDurationDays = 2,
                IsWIRCheckpoint = false,
                IsActive = true,
                CreatedDate = seedDate
            },
            
            // ID 23 - Thermostats
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000005-0000-0000-0000-000000000005"),
                ActivityCode = "STAGE6-THERMO",
                ActivityName = "Thermostats",
                Stage = "Stage 6: MEP Phase 2",
                StageNumber = 6,
                SequenceInStage = 5,
                OverallSequence = 23,
                Description = "Installation of HVAC thermostats and controls",
                EstimatedDurationDays = 2,
                IsWIRCheckpoint = false,
                IsActive = true,
                CreatedDate = seedDate
            },
            
            // ID 24 - Air Outlet
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000005-0000-0000-0000-000000000006"),
                ActivityCode = "STAGE6-AIROUT",
                ActivityName = "Air Outlet",
                Stage = "Stage 6: MEP Phase 2",
                StageNumber = 6,
                SequenceInStage = 6,
                OverallSequence = 24,
                Description = "Installation of HVAC air outlets and diffusers",
                EstimatedDurationDays = 2,
                IsWIRCheckpoint = false,
                IsActive = true,
                CreatedDate = seedDate
            },
            
            // ID 25 - Sprinkler
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000005-0000-0000-0000-000000000007"),
                ActivityCode = "STAGE6-SPRINKLER",
                ActivityName = "Sprinkler",
                Stage = "Stage 6: MEP Phase 2",
                StageNumber = 6,
                SequenceInStage = 7,
                OverallSequence = 25,
                Description = "Installation of fire sprinkler heads",
                EstimatedDurationDays = 2,
                IsWIRCheckpoint = false,
                IsActive = true,
                CreatedDate = seedDate
            },
            
            // ID 26 - Smoke Detector (TRIGGERS WIR-5)
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000005-0000-0000-0000-000000000008"),
                ActivityCode = "STAGE6-SMOKE",
                ActivityName = "Smoke Detector",
                Stage = "Stage 6: MEP Phase 2",
                StageNumber = 6,
                SequenceInStage = 8,
                OverallSequence = 26,
                Description = "Installation of smoke detectors and fire alarm devices - triggers WIR-5",
                EstimatedDurationDays = 2,
                IsWIRCheckpoint = true,
                WIRCode = "WIR-5",
                IsActive = true,
                CreatedDate = seedDate
            },

            // ID 27 - Iron Mongeries
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000006-0000-0000-0000-000000000001"),
                ActivityCode = "STAGE7-IRON",
                ActivityName = "Iron Mongeries",
                Stage = "Stage 7: Final Inspection & Dispatch",
                StageNumber = 7,
                SequenceInStage = 1,
                OverallSequence = 27,
                Description = "Installation of door handles, locks, hinges and other hardware",
                EstimatedDurationDays = 1,
                IsWIRCheckpoint = false,
                IsActive = true,
                CreatedDate = seedDate
            },
            
            // ID 28 - Inspection & Wrapping (TRIGGERS WIR-6)
            new ActivityMaster
            {
                ActivityMasterId = Guid.Parse("10000006-0000-0000-0000-000000000002"),
                ActivityCode = "STAGE7-WRAP",
                ActivityName = "Inspection & Wrapping",
                Stage = "Stage 7: Final Inspection & Dispatch",
                StageNumber = 7,
                SequenceInStage = 2,
                OverallSequence = 28,
                Description = "Comprehensive final quality inspection and protective wrapping - triggers WIR-6",
                EstimatedDurationDays = 1,
                IsWIRCheckpoint = true,
                WIRCode = "WIR-6",
                IsActive = true,
                CreatedDate = seedDate
            }
        };

        modelBuilder.Entity<ActivityMaster>().HasData(activities);
    }
}
