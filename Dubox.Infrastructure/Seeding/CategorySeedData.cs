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
            }
        };

        modelBuilder.Entity<Category>().HasData(categories);
    }
}

