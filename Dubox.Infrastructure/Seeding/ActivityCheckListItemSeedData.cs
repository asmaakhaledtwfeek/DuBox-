using Dubox.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Infrastructure.Seeding;

/// <summary>
/// Seeds ActivityCheckListItem data linking predefined checklist items to activities
/// Based on Platform ID mapping from client inspection sequence data
/// </summary>
public static partial class ActivityCheckListItemSeedData
{
    // Static seed date to avoid dynamic model changes warning
    private static readonly DateTime SeedDate = new DateTime(2024, 11, 1, 0, 0, 0, DateTimeKind.Utc);
    
    public static void SeedActivityCheckListItems(ModelBuilder modelBuilder)
    {
        // First, seed ALL 698 PredefinedChecklistItems from JSON
        // Method defined in PredefinedChecklistItemsData.cs (partial class)
        SeedPredefinedChecklistItemsFromJSON(modelBuilder, SeedDate);
        
        // Then, seed the 1,238 ActivityCheckListItem links that connect items to activities
        // Method defined in ActivityCheckListItemLinksData.cs (partial class)
        SeedActivityCheckListItemLinks(modelBuilder, SeedDate);
    }

}
