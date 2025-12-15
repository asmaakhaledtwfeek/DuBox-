using Dubox.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Infrastructure.Seeding;

public static class CategorySeedData
{
    public static void SeedCategories(ModelBuilder modelBuilder)
    {
        var seedDate = new DateTime(2024, 11, 1, 0, 0, 0, DateTimeKind.Utc);

        var categories = new List<Category>
        {
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000001"),
                CategoryName = "Installation of HVAC Duct",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000002"),
                CategoryName = "General",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000003"),
                CategoryName = "Installation of Above Ground Drainage Pipes",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000004"),
                CategoryName = "Leak Test of Above Ground Drainage Pipes",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000005"),
                CategoryName = "Installation of Above ground Water Supply pipes and fittings",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000006"),
                CategoryName = "Testing of Above ground Water Supply pipes and fittings",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000007"),
                CategoryName = "Installation of Above Ground Fire Fighting pipes system",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000008"),
                CategoryName = "Testing of Above Ground Fire Fighting pipes and fittings",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000009"),
                CategoryName = "Installation of Refrigerant Pipe",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000010"),
                CategoryName = "Pressure Testing of Refrigerant Pipe",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000011"),
                CategoryName = "Installation of LV Cables & Wires",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000012"),
                CategoryName = "Testing of LV Cables & Wires",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000013"),
                CategoryName = "Installation of LV Panels",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000014"),
                CategoryName = "Installation of Conduits & accessories",
                CreatedDate = seedDate
            },
            // WIR-1 Categories
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000015"),
                CategoryName = "WIR-1: Material Verification Inspection",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000016"),
                CategoryName = "WIR-1: Material Receiving Inspection - MEP",
                CreatedDate = seedDate
            },
            // WIR-4 Categories
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000017"),
                CategoryName = "WIR-4: General Requirements",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000018"),
                CategoryName = "WIR-4: Preparation & Setting Out",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000019"),
                CategoryName = "WIR-4: External Walls Erection",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000020"),
                CategoryName = "WIR-4: Floor Slab Installation",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000021"),
                CategoryName = "WIR-4: Internal Partition Installation",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000022"),
                CategoryName = "WIR-4: Top Slab Installation",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000023"),
                CategoryName = "WIR-4: Final Verification",
                CreatedDate = seedDate
            },
            // WIR-5 Categories
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000024"),
                CategoryName = "WIR-5: Painting - General",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000025"),
                CategoryName = "WIR-5: Painting - Surface Preparation",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000026"),
                CategoryName = "WIR-5: Painting - Internal Application",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000027"),
                CategoryName = "WIR-5: Painting - External Application",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000028"),
                CategoryName = "WIR-5: Ceramic Tiling",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000029"),
                CategoryName = "WIR-5: Gypsum Partition Installation",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000030"),
                CategoryName = "WIR-5: False Ceiling Installation",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000031"),
                CategoryName = "WIR-5: Wet Area Waterproofing",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000032"),
                CategoryName = "WIR-5: Doors & Windows Installation",
                CreatedDate = seedDate
            },
            // WIR-6 Categories
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000033"),
                CategoryName = "WIR-6: General & Structural Verification",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000034"),
                CategoryName = "WIR-6: Painting Inspection",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000035"),
                CategoryName = "WIR-6: Floor & Wall Tiling",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000036"),
                CategoryName = "WIR-6: Dry Wall Inspection",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000037"),
                CategoryName = "WIR-6: False Ceiling Inspection",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000038"),
                CategoryName = "WIR-6: Aluminium & Glazing Inspection",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000039"),
                CategoryName = "WIR-6: Doors & Windows Inspection",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000040"),
                CategoryName = "WIR-6: Wood Works Inspection",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000041"),
                CategoryName = "WIR-6: Other Finishes Inspection",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000042"),
                CategoryName = "WIR-6: MEP Systems Inspection",
                CreatedDate = seedDate
            }
        };

        modelBuilder.Entity<Category>().HasData(categories);
    }
}

