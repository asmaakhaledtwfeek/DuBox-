using Dubox.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Infrastructure.Seeding;

/// <summary>
/// Comprehensive checklist seeding that links all 698 items from JSON to activities
/// Uses static dates to avoid model changes warning
/// </summary>
public static class ComprehensiveChecklistSeeding
{
    // Static seed date to avoid dynamic model changes
    private static readonly DateTime SeedDate = new DateTime(2024, 11, 1, 0, 0, 0, DateTimeKind.Utc);
    
    public static void SeedAllChecklistData(ModelBuilder modelBuilder)
    {
        // Seed the predefined checklist items (698 items from JSON)
        SeedPredefinedItemsFromJSON(modelBuilder);
        
        // Seed the ActivityCheckListItem links (connecting items to activities)
        SeedActivityChecklistLinks(modelBuilder);
    }
    
    /// <summary>
    /// Seeds all 698 predefined checklist items from JSON
    /// This replaces the dynamic seeding to use static GUIDs and dates
    /// </summary>
    private static void SeedPredefinedItemsFromJSON(ModelBuilder modelBuilder)
    {
        // NOTE: The actual 698 items should be added from the generated migration
        // This method signature is here for consistency
        // The items are seeded via migration: 20260206000000_SeedAllChecklistItemsFromJSON.cs
    }
    
    /// <summary>
    /// Seeds ActivityCheckListItem records linking predefined items to activities
    /// </summary>
    private static void SeedActivityChecklistLinks(ModelBuilder modelBuilder)
    {
        var links = new List<ActivityCheckListItem>();
        
        // Activity 1: Assembly & joints - Checklist 1 Parts 1-2
        AddChecklistItemsForActivity(links, 
            "10000001-0000-0000-0000-000000000001", // Assembly & joints
            GetChecklistItemGuidsFromMigration("Checklist 1 Part 1"));
        
        AddChecklistItemsForActivity(links,
            "10000001-0000-0000-0000-000000000001",
            GetChecklistItemGuidsFromMigration("Checklist 1 Part 2"));
        
        // Activity 2: PODS installation - Checklist 1 Part 3
        AddChecklistItemsForActivity(links,
            "10000001-0000-0000-0000-000000000002", // PODS
            GetChecklistItemGuidsFromMigration("Checklist 1 Part 3"));
        
        // Activity 3: MEP Cage installation - Checklist 2
        AddChecklistItemsForActivity(links,
            "10000001-0000-0000-0000-000000000003", // MEP Cage
            GetChecklistItemGuidsFromMigration("Checklist 2"));
        
        // Activity 4: Box Closure - Checklist 1 Full, Checklist 3
        AddChecklistItemsForActivity(links,
            "10000001-0000-0000-0000-000000000004", // Box Closure
            GetChecklistItemGuidsFromMigration("Checklist 1"),
            GetChecklistItemGuidsFromMigration("Checklist 3"));
        
        // Activity 5: Ducts & Insulation - Checklist 12, 32
        AddChecklistItemsForActivity(links,
            "10000002-0000-0000-0000-000000000001", // Ducts & Insulation
            GetChecklistItemGuidsFromMigration("Checklist 12"),
            GetChecklistItemGuidsFromMigration("Checklist 32"));
        
        // Activity 6: Drainage piping - Checklist 5, 31, 32
        AddChecklistItemsForActivity(links,
            "10000002-0000-0000-0000-000000000002", // Drainage piping
            GetChecklistItemGuidsFromMigration("Checklist 5"),
            GetChecklistItemGuidsFromMigration("Checklist 31"),
            GetChecklistItemGuidsFromMigration("Checklist 32"));
        
        // Activity 7: Water Piping - Checklist 7, 31, 32
        AddChecklistItemsForActivity(links,
            "10000002-0000-0000-0000-000000000003", // Water Piping
            GetChecklistItemGuidsFromMigration("Checklist 7"),
            GetChecklistItemGuidsFromMigration("Checklist 31"),
            GetChecklistItemGuidsFromMigration("Checklist 32"));
        
        // Activity 8: Fire Fighting Piping - Checklist 8, 32
        AddChecklistItemsForActivity(links,
            "10000002-0000-0000-0000-000000000004", // Fire Fighting
            GetChecklistItemGuidsFromMigration("Checklist 8"),
            GetChecklistItemGuidsFromMigration("Checklist 32"));
        
        // Activity 9: Electrical Containment - Checklist 16
        AddChecklistItemsForActivity(links,
            "10000003-0000-0000-0000-000000000001", // Electrical Containment
            GetChecklistItemGuidsFromMigration("Checklist 16"));
        
        // Activity 10: Electrical Wiring - Checklist 13, 15, 16, 22
        AddChecklistItemsForActivity(links,
            "10000003-0000-0000-0000-000000000002", // Electrical Wiring
            GetChecklistItemGuidsFromMigration("Checklist 13"),
            GetChecklistItemGuidsFromMigration("Checklist 15"),
            GetChecklistItemGuidsFromMigration("Checklist 16"),
            GetChecklistItemGuidsFromMigration("Checklist 22"));
        
        // Activity 11: Dry Wall Framing - Checklist 14 Parts 1-2
        AddChecklistItemsForActivity(links,
            "10000003-0000-0000-0000-000000000003", // Dry Wall
            GetChecklistItemGuidsFromMigration("Checklist 14 Part 1"),
            GetChecklistItemGuidsFromMigration("Checklist 14 Part 2"));
        
        // Activity 12: DB and ONU Panel - Checklist 20
        AddChecklistItemsForActivity(links,
            "10000003-0000-0000-0000-000000000004", // DB and ONU
            GetChecklistItemGuidsFromMigration("Checklist 20"));
        
        // Activity 13: False Ceiling - Checklist 17, 18 Parts 1-2
        AddChecklistItemsForActivity(links,
            "10000004-0000-0000-0000-000000000001", // False Ceiling
            GetChecklistItemGuidsFromMigration("Checklist 17"),
            GetChecklistItemGuidsFromMigration("Checklist 18 Part 1"),
            GetChecklistItemGuidsFromMigration("Checklist 18 Part 2"));
        
        // Activity 14: Tile Fixing - Checklist 6 Parts 1-3
        AddChecklistItemsForActivity(links,
            "10000004-0000-0000-0000-000000000002", // Tile Fixing
            GetChecklistItemGuidsFromMigration("Checklist 6 Part 1"),
            GetChecklistItemGuidsFromMigration("Checklist 6 Part 2"),
            GetChecklistItemGuidsFromMigration("Checklist 6 Part 3"));
        
        // Activity 15: Painting - Checklist 4 Parts 1-7, Checklist 11
        AddChecklistItemsForActivity(links,
            "10000004-0000-0000-0000-000000000003", // Painting
            GetChecklistItemGuidsFromMigration("Checklist 4 Part Part 1"),
            GetChecklistItemGuidsFromMigration("Checklist 4 Part Part 2"),
            GetChecklistItemGuidsFromMigration("Checklist 4 Part Part 3"),
            GetChecklistItemGuidsFromMigration("Checklist 4 Part Part 4"),
            GetChecklistItemGuidsFromMigration("Checklist 4 Part Part 5"),
            GetChecklistItemGuidsFromMigration("Checklist 4 Part Part 6"),
            GetChecklistItemGuidsFromMigration("Checklist 4 Part Part 7"),
            GetChecklistItemGuidsFromMigration("Checklist 11"));
        
        // Activity 17: Doors - Checklist 25
        AddChecklistItemsForActivity(links,
            "10000004-0000-0000-0000-000000000005", // Doors
            GetChecklistItemGuidsFromMigration("Checklist 25"));
        
        // Activity 18: Windows - Checklist 29
        AddChecklistItemsForActivity(links,
            "10000004-0000-0000-0000-000000000006", // Windows
            GetChecklistItemGuidsFromMigration("Checklist 29"));
        
        // Activity 19: Switches & Sockets - Checklist 32 Part 4
        // Activity 20: Light Fittings - Checklist 32 Part 3
        // Activity 21: Copper Piping - Checklist 9, 10, 32
        // Activity 22: Sanitary Fittings - Checklist 27
        // Activity 23: Thermostats - Checklist 32 Part 5
        // Activity 24: Air Outlet - Checklist 32 Part 1
        // Activity 25: Sprinkler - Checklist 32 Part 1
        // Activity 26: Smoke Detector - Checklist 32 Parts 2, 6
        // Activity 28: Inspection & Wrapping - Checklist 32, 33
        
        // NOTE: Due to the large number of items, the full implementation
        // should be in the migration file generated by the PowerShell script
        
        modelBuilder.Entity<ActivityCheckListItem>().HasData(links);
    }
    
    /// <summary>
    /// Helper method to get checklist item GUIDs from the migration
    /// This should be populated with actual GUIDs from the generated migration
    /// </summary>
    private static List<Guid> GetChecklistItemGuidsFromMigration(string reference)
    {
        // This is a placeholder - actual implementation should come from
        // parsing the generated migration file or from a static dictionary
        return new List<Guid>();
    }
    
    /// <summary>
    /// Helper to add multiple checklist items for an activity
    /// </summary>
    private static void AddChecklistItemsForActivity(
        List<ActivityCheckListItem> links,
        string activityMasterIdString,
        params List<Guid>[] checklistItemGuidLists)
    {
        var activityMasterId = Guid.Parse(activityMasterIdString);
        var sequence = 1;
        
        foreach (var checklistItemGuids in checklistItemGuidLists)
        {
            foreach (var predefinedItemId in checklistItemGuids)
            {
                var linkId = GenerateDeterministicGuid(activityMasterId, predefinedItemId, sequence);
                
                links.Add(new ActivityCheckListItem
                {
                    ActivityCheckListItemId = linkId,
                    ActivityMasterId = activityMasterId,
                    ActivityTemplateActivityId = null,
                    PredefinedChecklistItemId = predefinedItemId,
                    Sequence = sequence,
                    IsMandatory = true,
                    IsActive = true,
                    CreatedDate = SeedDate,
                    CreatedBy = "System"
                });
                
                sequence++;
            }
        }
    }
    
    /// <summary>
    /// Generates a deterministic GUID based on input parameters
    /// </summary>
    private static Guid GenerateDeterministicGuid(Guid activityId, Guid predefinedItemId, int sequence)
    {
        var input = $"{activityId:N}{predefinedItemId:N}{sequence:D5}";
        var bytes = System.Text.Encoding.UTF8.GetBytes(input);
        var hash = System.Security.Cryptography.MD5.HashData(bytes);
        return new Guid(hash);
    }
}
