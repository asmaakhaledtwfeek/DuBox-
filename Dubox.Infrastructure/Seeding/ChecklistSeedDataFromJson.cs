using Dubox.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Dubox.Infrastructure.Seeding;

public static class ChecklistSeedDataFromJson
{
    public static void SeedChecklistsFromJson(ModelBuilder modelBuilder)
    {
        try
        {
            // Try multiple possible paths for the JSON file
            var possiblePaths = new[]
            {
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Seeding", "checklist_hierarchy_with_guids.json"),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "checklist_hierarchy_with_guids.json"),
                Path.Combine(Directory.GetCurrentDirectory(), "Seeding", "checklist_hierarchy_with_guids.json"),
                "Seeding/checklist_hierarchy_with_guids.json",
                "checklist_hierarchy_with_guids.json"
            };

            string? jsonFilePath = null;
            foreach (var path in possiblePaths)
            {
                if (File.Exists(path))
                {
                    jsonFilePath = path;
                    break;
                }
            }

            // Check if file exists
            if (jsonFilePath == null)
            {
                Console.WriteLine($"Warning: Checklist seed data file not found. Tried paths:");
                foreach (var path in possiblePaths)
                {
                    Console.WriteLine($"  - {Path.GetFullPath(path)}");
                }
                return;
            }

            Console.WriteLine($"Loading checklist seed data from: {jsonFilePath}");

            var jsonContent = File.ReadAllText(jsonFilePath);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var data = JsonSerializer.Deserialize<ChecklistHierarchyRoot>(jsonContent, options);

            if (data?.Checklists == null || !data.Checklists.Any())
            {
                Console.WriteLine("Warning: No checklist data found in JSON file");
                return;
            }

            var checklists = new List<Checklist>();
            var sections = new List<ChecklistSection>();
            var items = new List<PredefinedChecklistItem>();

            // Process each checklist
            foreach (var checklistDto in data.Checklists)
            {
                // Add checklist
                var checklist = new Checklist
                {
                    ChecklistId = Guid.Parse(checklistDto.ChecklistId),
                    Name = checklistDto.Name,
                    Code = checklistDto.Code,
                    Discipline = checklistDto.Discipline,
                    SubDiscipline = checklistDto.SubDiscipline,
                    PageNumber = checklistDto.PageNumber,
                    ReferenceDocumentsJson = checklistDto.ReferenceDocumentsJson,
                    SignatureRolesJson = checklistDto.SignatureRolesJson,
                    WIRCode = checklistDto.WirCode,
                    IsActive = checklistDto.IsActive,
                    CreatedDate = DateTime.Parse(checklistDto.CreatedDate)
                };
                checklists.Add(checklist);

                // Process sections
                if (checklistDto.Sections != null)
                {
                    foreach (var sectionDto in checklistDto.Sections)
                    {
                        var section = new ChecklistSection
                        {
                            ChecklistSectionId = Guid.Parse(sectionDto.ChecklistSectionId),
                            Title = sectionDto.Title,
                            Order = sectionDto.Order,
                            ChecklistId = Guid.Parse(sectionDto.ChecklistId),
                            IsActive = sectionDto.IsActive,
                            CreatedDate = DateTime.Parse(sectionDto.CreatedDate)
                        };
                        sections.Add(section);

                        // Process items
                        if (sectionDto.Items != null)
                        {
                            foreach (var itemDto in sectionDto.Items)
                            {
                                var item = new PredefinedChecklistItem
                                {
                                    PredefinedItemId = Guid.Parse(itemDto.PredefinedItemId),
                                    Description = itemDto.Description,
                                    Sequence = itemDto.Sequence,
                                    IsActive = itemDto.IsActive,
                                    CreatedDate = DateTime.Parse(itemDto.CreatedDate),
                                    ChecklistSectionId = Guid.Parse(itemDto.ChecklistSectionId),
                                    Reference = itemDto.Reference,
                                    ChecklistNumber = itemDto.ChecklistNumber,
                                    Part = itemDto.Part
                                };
                                items.Add(item);
                            }
                        }
                    }
                }
            }

            // Seed data using EF Core HasData
            modelBuilder.Entity<Checklist>().HasData(checklists);
            modelBuilder.Entity<ChecklistSection>().HasData(sections);
            modelBuilder.Entity<PredefinedChecklistItem>().HasData(items);

            Console.WriteLine($"Successfully seeded {checklists.Count} checklists, {sections.Count} sections, and {items.Count} items from JSON");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error seeding checklist data from JSON: {ex.Message}");
            throw;
        }
    }

    // DTOs for deserialization
    private class ChecklistHierarchyRoot
    {
        public List<ChecklistDto> Checklists { get; set; } = new();
    }

    private class ChecklistDto
    {
        public string ChecklistId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Discipline { get; set; } = string.Empty;
        public string? SubDiscipline { get; set; }
        public int PageNumber { get; set; }
        public string? ReferenceDocumentsJson { get; set; }
        public string? SignatureRolesJson { get; set; }
        public string? WirCode { get; set; }
        public bool IsActive { get; set; }
        public string CreatedDate { get; set; } = string.Empty;
        public List<ChecklistSectionDto>? Sections { get; set; }
    }

    private class ChecklistSectionDto
    {
        public string ChecklistSectionId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public int Order { get; set; }
        public string ChecklistId { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string CreatedDate { get; set; } = string.Empty;
        public List<PredefinedChecklistItemDto>? Items { get; set; }
    }

    private class PredefinedChecklistItemDto
    {
        public string PredefinedItemId { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Sequence { get; set; }
        public bool IsActive { get; set; }
        public string CreatedDate { get; set; } = string.Empty;
        public string ChecklistSectionId { get; set; } = string.Empty;
        public string? Reference { get; set; }
        public int? ChecklistNumber { get; set; }
        public int? Part { get; set; }
    }
}
