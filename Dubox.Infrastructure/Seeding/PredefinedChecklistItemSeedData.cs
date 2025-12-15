using Microsoft.EntityFrameworkCore;

namespace Dubox.Infrastructure.Seeding;

public static class PredefinedChecklistItemSeedData
{
    public static void SeedPredefinedChecklistItems(ModelBuilder modelBuilder)
    {
        //var seedDate = new DateTime(2024, 11, 1, 0, 0, 0, DateTimeKind.Utc);

        //var predefinedItems = new List<PredefinedChecklistItem>
        //{
        //    // Installation of HVAC Duct
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000001"),
        //        ItemNumber = "A1",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Ensure the materials as per approved material submittal.",
        //        Sequence = 1,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000002"),
        //        ItemNumber = "A2",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Ensure drawings are used for installation are current and approved.",
        //        Sequence = 2,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000003"),
        //        ItemNumber = "A3",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Check that only in properly fabricated fittings are used for changes in directions, shapes,sizes and connections.",
        //        Sequence = 3,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000004"),
        //        ItemNumber = "A4",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "The joints and flanges are correctly made, jointed and sealed.",
        //        Sequence = 4,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000005"),
        //        ItemNumber = "A5",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Check that duct joints are sealed externally with approved sealant.",
        //        Sequence = 5,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000006"),
        //        ItemNumber = "A6",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Check fixing of supports and spacing as approved drawings & submittal.",
        //        Sequence = 6,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000007"),
        //        ItemNumber = "A7",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Check acoustic lining is properly fastened and un damaged.",
        //        Sequence = 7,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000008"),
        //        ItemNumber = "A8",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Check nuts, bolts, screws, brackets drop rods etc. are tight and aligned properly.",
        //        Sequence = 8,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000009"),
        //        ItemNumber = "A9",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "All the access doors, fire dampers,VCDs etc are installed as per approved drawings, specification and manufacturer instructions as applicable.",
        //        Sequence = 9,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000010"),
        //        ItemNumber = "A10",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Ensure that the identification & labeling are provided for duct works.",
        //        Sequence = 10,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    // Installation of Above Ground Drainage Pipes
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000011"),
        //        ItemNumber = "A1",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Ensure the materials are approved",
        //        Sequence = 11,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000012"),
        //        ItemNumber = "A2",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "No visible damage on the materials",
        //        Sequence = 12,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000013"),
        //        ItemNumber = "A3",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Pipe sizes are as per approved shop drawing",
        //        Sequence = 13,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000014"),
        //        ItemNumber = "A4",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Pipe layout/routing as per approved shop drawing",
        //        Sequence = 14,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000015"),
        //        ItemNumber = "A5",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Slope of the pipes as per approved shop drawing",
        //        Sequence = 15,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000016"),
        //        ItemNumber = "A6",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Installation as per approved method statement",
        //        Sequence = 16,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000017"),
        //        ItemNumber = "A7",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Sleeves are provided for the pipes passing through the walls /slabs",
        //        Sequence = 17,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000018"),
        //        ItemNumber = "A8",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Check the pipes are supported well with approved clamps.",
        //        Sequence = 18,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000019"),
        //        ItemNumber = "A9",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Ensure Drainage pipes are not passing above electrical services",
        //        Sequence = 19,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000020"),
        //        ItemNumber = "A10",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Installed pipes are free of sag & bend",
        //        Sequence = 20,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000021"),
        //        ItemNumber = "A11",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Drainage pipes are connected to the vent system",
        //        Sequence = 21,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000022"),
        //        ItemNumber = "A12",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Pipe joints are properly made and are tight / secure",
        //        Sequence = 22,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    // Leak Test of Above Ground Drainage Pipes
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000023"),
        //        ItemNumber = "A1",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Installation of Drainage Pipe has completed",
        //        Sequence = 23,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000024"),
        //        ItemNumber = "A2",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "All pipe joints shall be inspected.",
        //        Sequence = 24,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000025"),
        //        ItemNumber = "A3",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Drainage pipe should generally be subjected to an internal pressure test of 3m head of water above the crown of the pipe at the high end",
        //        Sequence = 25,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    // Installation of Above ground Water Supply pipes and fittings
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000026"),
        //        ItemNumber = "A1",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Ensure the materials are approved",
        //        Sequence = 26,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000027"),
        //        ItemNumber = "A2",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "No visible damage on the materials.",
        //        Sequence = 27,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000028"),
        //        ItemNumber = "A3",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Pipe sizes are as per approved shop drawing.",
        //        Sequence = 28,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000029"),
        //        ItemNumber = "A4",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Pipe layout/routing as per approved shop drawing.",
        //        Sequence = 29,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000030"),
        //        ItemNumber = "A5",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Installation as per approved method statement.",
        //        Sequence = 30,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000031"),
        //        ItemNumber = "A6",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Sleeves are provided for the pipes passing through the walls /slabs.",
        //        Sequence = 31,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000032"),
        //        ItemNumber = "A7",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Check the Horizontal pipes are supported well and with approved clamps.",
        //        Sequence = 32,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000033"),
        //        ItemNumber = "A8",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Check the vertical riser of the pipes are supported well with approved clamp.",
        //        Sequence = 33,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000034"),
        //        ItemNumber = "A9",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Water pipes are not passing above electrical services.",
        //        Sequence = 34,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000035"),
        //        ItemNumber = "A10",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Installed pipes are free of sag & bend.",
        //        Sequence = 35,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000036"),
        //        ItemNumber = "A11",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Pipe joints are properly made and are tight / secure",
        //        Sequence = 36,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    // Testing of Above ground Water Supply pipes and fittings
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000037"),
        //        ItemNumber = "A1",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Installation of Water Supply pipes completed",
        //        Sequence = 37,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000038"),
        //        ItemNumber = "A2",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Ensure Pressure Gauge used is Calibrated",
        //        Sequence = 38,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000039"),
        //        ItemNumber = "A3",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "The system shall be subjected to a hydrostatic pressure test, to the satisfaction and in the presence of the Engineer. Pressure shall be 1.5 times than working pressure",
        //        Sequence = 39,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    // Installation of Above Ground Fire Fighting pipes system
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000040"),
        //        ItemNumber = "A1",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Check the piping, fitting, installation and valves materials are as per specification and approved material submittal",
        //        Sequence = 40,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000041"),
        //        ItemNumber = "A2",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Checks the piping layouts are as per the approved shop drawing and site conditions.",
        //        Sequence = 41,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000042"),
        //        ItemNumber = "A3",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Check the approved supports& accessories are used for installation firefighting piping &Accessories.",
        //        Sequence = 42,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000043"),
        //        ItemNumber = "A4",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Checks the distance between the supports are maintained as per specification and method statement.",
        //        Sequence = 43,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000044"),
        //        ItemNumber = "A5",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Checks the proper sizing's of hangers are used as per specification.",
        //        Sequence = 44,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000045"),
        //        ItemNumber = "A6",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Check threaded joints are provided for 2\" and below and grooved fitting are provided for 2 ½\" and above in piping.",
        //        Sequence = 45,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000046"),
        //        ItemNumber = "A7",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Check location / size of valves are provided as per approved shop drawing and specification.",
        //        Sequence = 46,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000047"),
        //        ItemNumber = "A8",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Check unions are provided for 2\" and below piping in direction of flow.",
        //        Sequence = 47,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000048"),
        //        ItemNumber = "A9",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Check flexible pipe connector/expansion compensator are installed in expansion joints and equipment connections.",
        //        Sequence = 48,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000049"),
        //        ItemNumber = "A10",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Check drain points are provided in lowest piping points.",
        //        Sequence = 49,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000050"),
        //        ItemNumber = "A11",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Check air vents are provided in highest piping points.",
        //        Sequence = 50,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    // Testing of Above Ground Fire Fighting pipes and fittings
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000051"),
        //        ItemNumber = "A1",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Fire fighting pipes installation completed",
        //        Sequence = 51,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000052"),
        //        ItemNumber = "A2",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Ensure the Pressure Gauge used are calibrated",
        //        Sequence = 52,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000053"),
        //        ItemNumber = "A3",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "The system shall be subjected to a hydrostatic pressure test, to the satisfaction and in the presence of the Engineer. Pressure shall be 1.5 times than working pressure in accordance to NFPA 13. Test shall be maintained for two hours as a minimum.",
        //        Sequence = 53,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    // Installation of Refrigerant Pipe
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000054"),
        //        ItemNumber = "A1",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Ensure the materials as per approved material submittal.",
        //        Sequence = 54,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000055"),
        //        ItemNumber = "A2",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "No visible damage on the materials.",
        //        Sequence = 55,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000056"),
        //        ItemNumber = "A3",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Pipe sizes are as per approved shop drawing.",
        //        Sequence = 56,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000057"),
        //        ItemNumber = "A4",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Pipe layout/routing as per approved shop drawing.",
        //        Sequence = 57,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000058"),
        //        ItemNumber = "A5",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Installation as per approved method statement.",
        //        Sequence = 58,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000059"),
        //        ItemNumber = "A6",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Sleeves are provided for the pipes passing through the walls/slabs.",
        //        Sequence = 59,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000060"),
        //        ItemNumber = "A7",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Check the Horizontal pipes are supported well and with approved clamps.",
        //        Sequence = 60,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000061"),
        //        ItemNumber = "A8",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Check the vertical riser of the pipes are supported well with approved clamp.",
        //        Sequence = 61,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000062"),
        //        ItemNumber = "A9",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Pipe joints are properly brazed with no leak",
        //        Sequence = 62,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000063"),
        //        ItemNumber = "A10",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Ensure the proper insulation done.",
        //        Sequence = 63,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000064"),
        //        ItemNumber = "A11",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Ensure the cladding and sealant on the cladding joints.",
        //        Sequence = 64,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000065"),
        //        ItemNumber = "A12",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Ensure the system charged with approved refrigerant",
        //        Sequence = 65,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    // Pressure Testing of Refrigerant Pipe
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000066"),
        //        ItemNumber = "A1",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Installation of FAHU, FCU & VRF units are completed",
        //        Sequence = 66,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000067"),
        //        ItemNumber = "A2",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Installation of Refrigerant pipes completed",
        //        Sequence = 67,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000068"),
        //        ItemNumber = "A3",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Installation inspection completed",
        //        Sequence = 68,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000069"),
        //        ItemNumber = "A4",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Pressure Test: After installation, charge system and test for leaks. Repair leaks and retest until no leaks exist.",
        //        Sequence = 69,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    // Installation of LV Cables & Wires
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000070"),
        //        ItemNumber = "A1",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Check all materials installed are as per approved material submittal.",
        //        Sequence = 70,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000071"),
        //        ItemNumber = "A2",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Dimensions as per approved drawings",
        //        Sequence = 71,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000072"),
        //        ItemNumber = "A3",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "the cable/wire provided with Ties appropriately?",
        //        Sequence = 72,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000073"),
        //        ItemNumber = "A4",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Verify measurement of lengths.",
        //        Sequence = 73,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000074"),
        //        ItemNumber = "A5",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Ensure that there are no damages to the cables",
        //        Sequence = 74,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000075"),
        //        ItemNumber = "A6",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Verify that identification, grouping, spacing, markings and clamps are as required.",
        //        Sequence = 75,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000076"),
        //        ItemNumber = "A7",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Ensure the cable laying in trunking, trays and conduit installations are conducted in approved and accepted manner & as per project specification",
        //        Sequence = 76,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000077"),
        //        ItemNumber = "A8",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Ensure that cable type and routes are as per approved Drawing",
        //        Sequence = 77,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000078"),
        //        ItemNumber = "A9",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Is the cable/wire termination done correctly",
        //        Sequence = 78,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000079"),
        //        ItemNumber = "A9",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Ensure that correct types and sizes of wires and LV Cables are installed",
        //        Sequence = 79,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000080"),
        //        ItemNumber = "A10",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Verify that routes and marked locations are correct as per approved drawings",
        //        Sequence = 80,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000081"),
        //        ItemNumber = "A10",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Before pulling wires in conduit check that inside of conduit (and raceway in general) is free of burrs and is dry and clean.",
        //        Sequence = 81,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000082"),
        //        ItemNumber = "A11",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Ensure that extra Length left at every branch circuit outlet and pull-box, every cable passing through in order to allow inspection and for connections to be made.",
        //        Sequence = 82,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000083"),
        //        ItemNumber = "A11",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Is cable/wire provided with Lugs?",
        //        Sequence = 83,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000084"),
        //        ItemNumber = "A12",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Ensure that tightening of electrical connectors and terminals including screws and bolts, in accordance with manufacturers recommendation",
        //        Sequence = 84,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    // Testing of LV Cables & Wires
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000085"),
        //        ItemNumber = "A1",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Is the Cable/Wire Continuity test Setup, okay?",
        //        Sequence = 85,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000086"),
        //        ItemNumber = "A2",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Is the Cable/Wire Megger test Setup, okay?",
        //        Sequence = 86,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000087"),
        //        ItemNumber = "A3",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Ensure Megger test is performed for Cables and accepted",
        //        Sequence = 87,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000088"),
        //        ItemNumber = "A4",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Ensure Continuity test performed and accepted",
        //        Sequence = 88,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    // Installation of LV Panels
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000089"),
        //        ItemNumber = "A1",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Verify the installed Panel boards have approved submittals.",
        //        Sequence = 89,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000090"),
        //        ItemNumber = "A2",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Ensure the drawings used for installation are correct and approved.",
        //        Sequence = 90,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000091"),
        //        ItemNumber = "A3",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Align, level and securely fasten panelboards to structure",
        //        Sequence = 91,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000092"),
        //        ItemNumber = "A4",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Check the Name Plate and Identification labels as per load schedule and approved submittals.",
        //        Sequence = 92,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000093"),
        //        ItemNumber = "A5",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Check all meters, circuit breakers, indication lamp, handles and locks are correct and undamaged.",
        //        Sequence = 93,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000094"),
        //        ItemNumber = "A6",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Fix surface mounted outdoor panelboards at least 25mm from wall ensuring supporting members do not prevent flow of air.",
        //        Sequence = 94,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000095"),
        //        ItemNumber = "A7",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Wiring: Inside Panelboards: Neatly arranged, accessible and strapped to prevent tension on circuit breaker terminals. Tap-off connections shall be split and bolted type, fully insulated.",
        //        Sequence = 95,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000096"),
        //        ItemNumber = "A8",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Panelboard Interiors: Do not install in cabinets until all conduit connections to cabinet have been completed",
        //        Sequence = 96,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000097"),
        //        ItemNumber = "A9",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Do not use connecting conduits to support panelboards.",
        //        Sequence = 97,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000098"),
        //        ItemNumber = "A10",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Equipment grounding for LV Panel Board is provided",
        //        Sequence = 98,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000099"),
        //        ItemNumber = "A11",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Ensure that all unused openings are closed in the panels",
        //        Sequence = 99,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000100"),
        //        ItemNumber = "A12",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Ensure that updated circuit directory and other relevant documents provided in the panel directory accordingly.",
        //        Sequence = 100,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000101"),
        //        ItemNumber = "A13",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Ensure that the Panel fixing bolts have been tightened with the suitable nut and washer.",
        //        Sequence = 101,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000102"),
        //        ItemNumber = "A14",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Touch up and cleaning of the panel",
        //        Sequence = 102,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000103"),
        //        ItemNumber = "A15",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Ensure that each Panel Marking should be done as per approved drawing",
        //        Sequence = 103,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    // Installation of Conduits & accessories
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000104"),
        //        ItemNumber = "A1",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Check the conduits and accessories are as per approved material submittal.",
        //        Sequence = 104,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000105"),
        //        ItemNumber = "A2",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Check and ensure the drawings used for installation are current and approved.",
        //        Sequence = 105,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000106"),
        //        ItemNumber = "A3",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Check the conduits and other associated material are new and undamaged.",
        //        Sequence = 106,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000107"),
        //        ItemNumber = "A4",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Check that the conduits are leveled and aligned properly.",
        //        Sequence = 107,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000108"),
        //        ItemNumber = "A5",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Check that the conduits are securely fixed.",
        //        Sequence = 108,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000109"),
        //        ItemNumber = "A6",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Check and ensure that the conduits and back boxes are sizely adequated.",
        //        Sequence = 109,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000110"),
        //        ItemNumber = "A7",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Check that the bottom hight of the back boxes is as per the shop drawing.",
        //        Sequence = 110,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000111"),
        //        ItemNumber = "A8",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Check that a minimum of 5mm thick cover is provided for conduits concealed in walls.",
        //        Sequence = 111,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000112"),
        //        ItemNumber = "A9",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Check the installation of conduits is co-ordinated with other services.",
        //        Sequence = 112,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000113"),
        //        ItemNumber = "A10",
        //        WIRNumber = "WIR-2",
        //        CheckpointDescription = "Check the installation of conduiting as per approved drawings.",
        //        Sequence = 113,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    // Additional Installation of LV Cables & Wires items (WIR-3)
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000114"),
        //        ItemNumber = "A1",
        //        WIRNumber = "WIR-3",
        //        CheckpointDescription = "Check all materials installed are as per approved material submittal.",
        //        Sequence = 114,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000115"),
        //        ItemNumber = "A2",
        //        WIRNumber = "WIR-3",
        //        CheckpointDescription = "Dimensions as per approved drawings.",
        //        Sequence = 115,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000116"),
        //        ItemNumber = "A3",
        //        WIRNumber = "WIR-3",
        //        CheckpointDescription = "the cable/wire provided with Ties appropriately?",
        //        Sequence = 116,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000117"),
        //        ItemNumber = "A4",
        //        WIRNumber = "WIR-3",
        //        CheckpointDescription = "Verify measurement of lengths.",
        //        Sequence = 117,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000118"),
        //        ItemNumber = "A5",
        //        WIRNumber = "WIR-3",
        //        CheckpointDescription = "Ensure that there are no damages to the cables.",
        //        Sequence = 118,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000119"),
        //        ItemNumber = "A6",
        //        WIRNumber = "WIR-3",
        //        CheckpointDescription = "Verify that identification, grouping, spacing, markings and clamps are as required.",
        //        Sequence = 119,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000120"),
        //        ItemNumber = "A7",
        //        WIRNumber = "WIR-3",
        //        CheckpointDescription = "Ensure the cable laying in trunking, trays and conduit installations are conducted in approved and accepted manner & as per project specification",
        //        Sequence = 120,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000121"),
        //        ItemNumber = "A8",
        //        WIRNumber = "WIR-3",
        //        CheckpointDescription = "Ensure that cable type and routes are as per approved Drawing.",
        //        Sequence = 121,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000122"),
        //        ItemNumber = "A9",
        //        WIRNumber = "WIR-3",
        //        CheckpointDescription = "Ensure that correct types and sizes of wires and LV Cables are installed.",
        //        Sequence = 122,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000123"),
        //        ItemNumber = "A10",
        //        WIRNumber = "WIR-3",
        //        CheckpointDescription = "Verify that routes and marked locations are correct as per approved drawings.",
        //        Sequence = 123,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000124"),
        //        ItemNumber = "A11",
        //        WIRNumber = "WIR-3",
        //        CheckpointDescription = "Is cable/wire provided with Lugs?",
        //        Sequence = 124,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000125"),
        //        ItemNumber = "A9",
        //        WIRNumber = "WIR-3",
        //        CheckpointDescription = "Is the cable/wire termination done correctly?",
        //        Sequence = 125,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000126"),
        //        ItemNumber = "A10",
        //        WIRNumber = "WIR-3",
        //        CheckpointDescription = "Before pulling wires in conduit check that inside of conduit (and raceway in general) is free of burrs and is dry and clean.",
        //        Sequence = 126,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000127"),
        //        ItemNumber = "A11",
        //        WIRNumber = "WIR-3",
        //        CheckpointDescription = "Ensure that extra Length left at every branch circuit outlet and pull-box, every cable passing through in order to allow inspection and for connections to be made.",
        //        Sequence = 127,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000128"),
        //        ItemNumber = "A12",
        //        WIRNumber = "WIR-3",
        //        CheckpointDescription = "Ensure that tightening of electrical connectors and terminals including screws and bolts, in accordance with manufacturers recommendation",
        //        Sequence = 128,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    // Additional Testing of LV Cables & Wires items (WIR-3)
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000129"),
        //        ItemNumber = "A1",
        //        WIRNumber = "WIR-3",
        //        CheckpointDescription = "Is the Cable/Wire Continuity test Setup, okay?",
        //        Sequence = 129,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000130"),
        //        ItemNumber = "A2",
        //        WIRNumber = "WIR-3",
        //        CheckpointDescription = "Is the Cable/Wire Megger test Setup, okay?",
        //        Sequence = 130,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000131"),
        //        ItemNumber = "A3",
        //        WIRNumber = "WIR-3",
        //        CheckpointDescription = "Ensure Megger test is performed for Cables and accepted.",
        //        Sequence = 131,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000132"),
        //        ItemNumber = "A4",
        //        WIRNumber = "WIR-3",
        //        CheckpointDescription = "Ensure Continuity test performed and accepted.",
        //        Sequence = 132,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    // Additional Installation of LV Panels items (WIR-3)
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000133"),
        //        ItemNumber = "A1",
        //        WIRNumber = "WIR-3",
        //        CheckpointDescription = "Verify the installed Panel boards have approved submittals.",
        //        Sequence = 133,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000134"),
        //        ItemNumber = "A2",
        //        WIRNumber = "WIR-3",
        //        CheckpointDescription = "Ensure the drawings used for installation are correct and approved.",
        //        Sequence = 134,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000135"),
        //        ItemNumber = "A3",
        //        WIRNumber = "WIR-3",
        //        CheckpointDescription = "Align, level and securely fasten panelboards to structure.",
        //        Sequence = 135,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000136"),
        //        ItemNumber = "A4",
        //        WIRNumber = "WIR-3",
        //        CheckpointDescription = "Check the Name Plate and Identification labels as per load schedule and approved submittals.",
        //        Sequence = 136,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000137"),
        //        ItemNumber = "A5",
        //        WIRNumber = "WIR-3",
        //        CheckpointDescription = "Check all meters, circuit breakers, indication lamp, handles and locks are correct and undamaged.",
        //        Sequence = 137,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000138"),
        //        ItemNumber = "A6",
        //        WIRNumber = "WIR-3",
        //        CheckpointDescription = "Fix surface mounted outdoor panelboards at least 25mm from wall ensuring supporting members do not prevent flow of air.",
        //        Sequence = 138,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000139"),
        //        ItemNumber = "A7",
        //        WIRNumber = "WIR-3",
        //        CheckpointDescription = "Do not use connecting conduits to support panelboards.",
        //        Sequence = 139,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000140"),
        //        ItemNumber = "A8",
        //        WIRNumber = "WIR-3",
        //        CheckpointDescription = "Panelboard Interiors: Do not install in cabinets until all conduit connections to abinet have been completed.",
        //        Sequence = 140,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000141"),
        //        ItemNumber = "A9",
        //        WIRNumber = "WIR-3",
        //        CheckpointDescription = "Wiring: Inside Panelboards: Neatly arranged, accessible and strapped to prevent tension on circuit breaker terminals. Tap-off connections shall be split and bolted type, fully insulated.",
        //        Sequence = 141,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000142"),
        //        ItemNumber = "A10",
        //        WIRNumber = "WIR-3",
        //        CheckpointDescription = "Equipment grounding for LV Panel Board is provided.",
        //        Sequence = 142,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000143"),
        //        ItemNumber = "A11",
        //        WIRNumber = "WIR-3",
        //        CheckpointDescription = "Ensure that all unused openings are closed in the panels.",
        //        Sequence = 143,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000144"),
        //        ItemNumber = "A12",
        //        WIRNumber = "WIR-3",
        //        CheckpointDescription = "Ensure that updated circuit directory and other relevant documents provided in the panel directory accordingly.",
        //        Sequence = 144,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000145"),
        //        ItemNumber = "A13",
        //        WIRNumber = "WIR-3",
        //        CheckpointDescription = "Ensure that the Panel fixing bolts have been tightened with the suitable nut and washer.",
        //        Sequence = 145,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000146"),
        //        ItemNumber = "A14",
        //        WIRNumber = "WIR-3",
        //        CheckpointDescription = "Touch up and cleaning of the pane.",
        //        Sequence = 146,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000147"),
        //        ItemNumber = "A15",
        //        WIRNumber = "WIR-3",
        //        CheckpointDescription = "Ensure that each Panel Marking should be done as per approved drawing.",
        //        Sequence = 147,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    // Additional Installation of Conduits & accessories items (WIR-3)
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000148"),
        //        ItemNumber = "A1",
        //        WIRNumber = "WIR-3",
        //        CheckpointDescription = "Check the conduits and accessories are as per approved material submittal.",
        //        Sequence = 148,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000149"),
        //        ItemNumber = "A2",
        //        WIRNumber = "WIR-3",
        //        CheckpointDescription = "Check and ensure the drawings used for installation are current and approved.",
        //        Sequence = 149,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000150"),
        //        ItemNumber = "A3",
        //        WIRNumber = "WIR-3",
        //        CheckpointDescription = "Check the conduits and other associated material are new and undamaged.",
        //        Sequence = 150,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000151"),
        //        ItemNumber = "A4",
        //        WIRNumber = "WIR-3",
        //        CheckpointDescription = "Check that the conduits are leveled and aligned properly.",
        //        Sequence = 151,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000152"),
        //        ItemNumber = "A5",
        //        WIRNumber = "WIR-3",
        //        CheckpointDescription = "Check that the conduits are securely fixed.",
        //        Sequence = 152,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000153"),
        //        ItemNumber = "A6",
        //        WIRNumber = "WIR-3",
        //        CheckpointDescription = "Check and ensure that the conduits and back boxes are sizely adequated.",
        //        Sequence = 153,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000154"),
        //        ItemNumber = "A7",
        //        WIRNumber = "WIR-3",
        //        CheckpointDescription = "Check that the bottom hight of the back boxes is as per the shop drawing.",
        //        Sequence = 154,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000155"),
        //        ItemNumber = "A8",
        //        WIRNumber = "WIR-3",
        //        CheckpointDescription = "Check that a minimum of 5mm thick cover is provided for conduits concealed in walls.",
        //        Sequence = 155,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000156"),
        //        ItemNumber = "A9",
        //        WIRNumber = "WIR-3",
        //        CheckpointDescription = "Check the installation of conduits is co-ordinated with other services.",
        //        Sequence = 156,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    },
        //    new PredefinedChecklistItem
        //    {
        //        PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000157"),
        //        ItemNumber = "A10",
        //        WIRNumber = "WIR-3",
        //        CheckpointDescription = "Check the installation of conduiting as per approved drawings.",
        //        Sequence = 157,
        //        IsActive = true,
        //        CreatedDate = seedDate
        //    }
        //};

        //modelBuilder.Entity<PredefinedChecklistItem>().HasData(predefinedItems);
    }
}
