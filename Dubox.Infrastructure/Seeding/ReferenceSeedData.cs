using Dubox.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Infrastructure.Seeding;

public static class ReferenceSeedData
{
    public static void SeedReferences(ModelBuilder modelBuilder)
    {
        var seedDate = new DateTime(2024, 11, 1, 0, 0, 0, DateTimeKind.Utc);

        var references = new List<Reference>
        {
            new Reference
            {
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000001"),
                ReferenceName = "Project Specifications Section- 233113 & 230713",
                CreatedDate = seedDate
            },
            new Reference
            {
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000002"),
                ReferenceName = "Approved Drawing",
                CreatedDate = seedDate
            },
            new Reference
            {
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000003"),
                ReferenceName = "MA",
                CreatedDate = seedDate
            },
            new Reference
            {
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000004"),
                ReferenceName = "General",
                CreatedDate = seedDate
            },
            new Reference
            {
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000005"),
                ReferenceName = "MIR",
                CreatedDate = seedDate
            },
            new Reference
            {
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000006"),
                ReferenceName = "Project Specification: Section- 221300",
                CreatedDate = seedDate
            },
            new Reference
            {
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000007"),
                ReferenceName = "Approved MTS",
                CreatedDate = seedDate
            },
            new Reference
            {
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000008"),
                ReferenceName = "Project specification",
                CreatedDate = seedDate
            },
            new Reference
            {
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000009"),
                ReferenceName = "Project Specification: Section- 221116",
                CreatedDate = seedDate
            },
            new Reference
            {
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000010"),
                ReferenceName = "Calibration Certificate",
                CreatedDate = seedDate
            },
            new Reference
            {
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000011"),
                ReferenceName = "Project Spec.-221116",
                CreatedDate = seedDate
            },
            new Reference
            {
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000012"),
                ReferenceName = "Project Specification: Section- 211100",
                CreatedDate = seedDate
            },
            new Reference
            {
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000013"),
                ReferenceName = "Project Specification & Approved MTS",
                CreatedDate = seedDate
            },
            new Reference
            {
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000014"),
                ReferenceName = "Approved Drawing & MTS",
                CreatedDate = seedDate
            },
            new Reference
            {
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000015"),
                ReferenceName = "Project Spec",
                CreatedDate = seedDate
            },
            new Reference
            {
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000016"),
                ReferenceName = "Project Specifications Section- 232300",
                CreatedDate = seedDate
            },
            new Reference
            {
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000017"),
                ReferenceName = "Project Spec. - 232300",
                CreatedDate = seedDate
            },
            new Reference
            {
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000018"),
                ReferenceName = "Project Spec. - 237400 & 238129",
                CreatedDate = seedDate
            },
            new Reference
            {
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000019"),
                ReferenceName = "MAR",
                CreatedDate = seedDate
            },
            new Reference
            {
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000020"),
                ReferenceName = "Project Spec.-260513",
                CreatedDate = seedDate
            },
            new Reference
            {
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000021"),
                ReferenceName = "Project Spec.-260513-1.7 C",
                CreatedDate = seedDate
            },
            new Reference
            {
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000022"),
                ReferenceName = "Project Specification: Section- 262416",
                CreatedDate = seedDate
            },
            new Reference
            {
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000023"),
                ReferenceName = "Approved Shop Drawing",
                CreatedDate = seedDate
            },
            new Reference
            {
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000024"),
                ReferenceName = "Project Specification: Section- 260533",
                CreatedDate = seedDate
            }
        };

        modelBuilder.Entity<Reference>().HasData(references);
    }
}

