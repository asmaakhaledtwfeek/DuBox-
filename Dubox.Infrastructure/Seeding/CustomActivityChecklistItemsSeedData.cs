using Dubox.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Infrastructure.Seeding;

/// <summary>
/// Seeds custom ActivityCheckListItem data linking Activity Masters with specific predefined checklist items
/// </summary>
public static class CustomActivityChecklistItemsSeedData
{
    private static readonly DateTime SeedDate = new DateTime(2024, 11, 1, 0, 0, 0, DateTimeKind.Utc);

    public static void SeedCustomActivityChecklistItems(ModelBuilder modelBuilder)
    {
        SeedActivityMaster1(modelBuilder);
        SeedActivityMaster2(modelBuilder);
    }

    /// <summary>
    /// Seeds Activity Master 10000001-0000-0000-0000-000000000001 with custom predefined checklist items
    /// </summary>
    private static void SeedActivityMaster1(ModelBuilder modelBuilder)
    {
        var activityMasterId = Guid.Parse("10000001-0000-0000-0000-000000000001");

        var predefinedChecklistItemIds = new[]
        {
            "BFAEBE11-4B44-4046-8353-05B4AA658C74",
            "B5A597A9-1CBF-49D8-AF83-0CF43698E8E8",
            "F134EEEF-7A92-4274-9115-125CF9D5373F",
            "441AFC7F-A0CB-4CB2-B386-312C14F24160",
            "66CDD933-7C82-466D-A3C0-43C1651064EC",
            "2B6382A0-29A5-485B-918C-4B04719AC277",
            "955140B4-00AA-4AB2-A1F1-5AB51FC3F08D",
            "2C65CE6C-4AEB-4DCF-ADF5-79FD96A74FEF",
            "F24A7845-057A-4B89-B83B-817822F8E4C6",
            "7BFDA768-0861-456F-BB67-81E44547F005",
            "09B3E941-A3FC-426F-BA68-980E3A7A6A27",
            "7E4C3F32-9931-4CCA-9672-9EDBD662A8D9",
            "26F83DAF-F89D-4ECE-A1B3-A9D6C00307A5",
            "AB057B3A-5B18-46D3-8AF1-C608CBD564E0",
            "40CC3CB6-1BC2-4C4C-83E8-C79B360F0E4A",
            "E05BC064-2C78-4DCE-BB83-CAA05E894CA0",
            "DB840B56-F8A1-42BC-B984-DF7E134AD5A5",
            "93EDEFF9-3C3F-4090-BF19-EDCE625C26C8",
            "16DF93A1-6764-46E4-AE7C-F7C5E9CC094F",
            "A372B33C-04F9-4D86-8316-F9264775CCDA"
        };

        var activityCheckListItems = new[]
        {
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F1A00000-0000-0000-0000-000000000001"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse(predefinedChecklistItemIds[0]),
                Sequence = 1,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F1A00000-0000-0000-0000-000000000002"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse(predefinedChecklistItemIds[1]),
                Sequence = 2,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F1A00000-0000-0000-0000-000000000003"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse(predefinedChecklistItemIds[2]),
                Sequence = 3,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F1A00000-0000-0000-0000-000000000004"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse(predefinedChecklistItemIds[3]),
                Sequence = 4,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F1A00000-0000-0000-0000-000000000005"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse(predefinedChecklistItemIds[4]),
                Sequence = 5,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F1A00000-0000-0000-0000-000000000006"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse(predefinedChecklistItemIds[5]),
                Sequence = 6,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F1A00000-0000-0000-0000-000000000007"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse(predefinedChecklistItemIds[6]),
                Sequence = 7,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F1A00000-0000-0000-0000-000000000008"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse(predefinedChecklistItemIds[7]),
                Sequence = 8,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F1A00000-0000-0000-0000-000000000009"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse(predefinedChecklistItemIds[8]),
                Sequence = 9,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F1A00000-0000-0000-0000-000000000010"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse(predefinedChecklistItemIds[9]),
                Sequence = 10,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F1A00000-0000-0000-0000-000000000011"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse(predefinedChecklistItemIds[10]),
                Sequence = 11,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F1A00000-0000-0000-0000-000000000012"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse(predefinedChecklistItemIds[11]),
                Sequence = 12,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F1A00000-0000-0000-0000-000000000013"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse(predefinedChecklistItemIds[12]),
                Sequence = 13,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F1A00000-0000-0000-0000-000000000014"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse(predefinedChecklistItemIds[13]),
                Sequence = 14,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F1A00000-0000-0000-0000-000000000015"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse(predefinedChecklistItemIds[14]),
                Sequence = 15,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F1A00000-0000-0000-0000-000000000016"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse(predefinedChecklistItemIds[15]),
                Sequence = 16,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F1A00000-0000-0000-0000-000000000017"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse(predefinedChecklistItemIds[16]),
                Sequence = 17,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F1A00000-0000-0000-0000-000000000018"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse(predefinedChecklistItemIds[17]),
                Sequence = 18,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F1A00000-0000-0000-0000-000000000019"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse(predefinedChecklistItemIds[18]),
                Sequence = 19,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F1A00000-0000-0000-0000-000000000020"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse(predefinedChecklistItemIds[19]),
                Sequence = 20,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            }
        };

        modelBuilder.Entity<ActivityCheckListItem>().HasData(activityCheckListItems);
    }

    /// <summary>
    /// Seeds Activity Master 10000001-0000-0000-0000-000000000002 with predefined checklist items 
    /// where ChecklistNumber=1 and Part=3
    /// </summary>
    private static void SeedActivityMaster2(ModelBuilder modelBuilder)
    {
        var activityMasterId = Guid.Parse("10000001-0000-0000-0000-000000000002");

        // Predefined checklist item IDs where ChecklistNumber=1 and Part=3
        var predefinedChecklistItemIds = new[]
        {
            "C76908FD-B7F1-4ACE-B87C-EA30BAFDA313", // Pod installed as per drawing - Level and alignment without any damages
            "CD97222F-0AE5-4822-84A9-398A52188DA1", // Erection & Connection of Roof Slab as per approved drawing and ensure box clear height
            "1416A5DE-152C-4C95-BCC9-F983C1E4AD88", // Check If wet connections are proper and have no wavy, depressed or bulge surface
            "0DCD763B-9CBF-43C8-93A3-49B24A0592B5", // All joints on paint able surfaces filled with sealant properly
            "6D78CE26-DE03-4B4E-9198-DEDD505A35C4", // All internal cracks are repaired (if any)
            "681D9573-580C-47AD-A080-6B4E544C8EC4", // Internal and External Dimension of Box
            "0C083254-BBF4-4496-85C5-173DEC86DDEF"  // Check for edges + Angles + grooves + chamfer + Pin holes + Cracks before moving to finishing area
        };

        var activityCheckListItems = new[]
        {
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F2A00000-0000-0000-0000-000000000001"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse(predefinedChecklistItemIds[0]),
                Sequence = 1,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F2A00000-0000-0000-0000-000000000002"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse(predefinedChecklistItemIds[1]),
                Sequence = 2,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F2A00000-0000-0000-0000-000000000003"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse(predefinedChecklistItemIds[2]),
                Sequence = 3,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F2A00000-0000-0000-0000-000000000004"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse(predefinedChecklistItemIds[3]),
                Sequence = 4,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F2A00000-0000-0000-0000-000000000005"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse(predefinedChecklistItemIds[4]),
                Sequence = 5,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F2A00000-0000-0000-0000-000000000006"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse(predefinedChecklistItemIds[5]),
                Sequence = 6,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F2A00000-0000-0000-0000-000000000007"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse(predefinedChecklistItemIds[6]),
                Sequence = 7,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            }
        };

        modelBuilder.Entity<ActivityCheckListItem>().HasData(activityCheckListItems);
    }
}
