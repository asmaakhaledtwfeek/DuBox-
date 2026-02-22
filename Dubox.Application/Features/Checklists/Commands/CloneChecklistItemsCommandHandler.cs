using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Checklists.Commands;

public class CloneChecklistItemsCommandHandler : IRequestHandler<CloneChecklistItemsCommand, Result<CloneChecklistItemsResult>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CloneChecklistItemsCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CloneChecklistItemsResult>> Handle(CloneChecklistItemsCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.ItemIds == null || request.ItemIds.Count == 0)
            {
                return Result.Failure<CloneChecklistItemsResult>("No items selected for cloning");
            }

            if (string.IsNullOrWhiteSpace(request.TargetWIRCode))
            {
                return Result.Failure<CloneChecklistItemsResult>("Target WIR code is required");
            }

            // Get all selected items with their parent sections and checklists
            var itemsToClone = _unitOfWork.Repository<PredefinedChecklistItem>()
                .GetWithSpec( new GetPredefineChecklistItemsWithIncludesSpecification(request.ItemIds, true)).Data
                .ToList();

            if (itemsToClone.Count == 0)
            {
                return Result.Failure<CloneChecklistItemsResult>("No items found with the provided IDs");
            }

            // Group items by their parent checklist
            var checklistGroups = itemsToClone
                .Where(i => i.ChecklistSection?.Checklist != null)
                .GroupBy(i => i.ChecklistSection!.Checklist!)
                .ToList();

            var newChecklistIds = new List<Guid>();
            var checklistsCloned = 0;
            var sectionsCloned = 0;
            var itemsCloned = 0;

            // For each checklist, clone it and its relevant sections/items
            foreach (var checklistGroup in checklistGroups)
            {
                var originalChecklist = checklistGroup.Key;

                // Check if a checklist with the same code already exists for the target WIR code
                var existingChecklist = _unitOfWork.Repository<Checklist>()
                    .GetEntityWithSpec(new ChecklistSpecification(originalChecklist.Code, request.TargetWIRCode));
                    

                Checklist targetChecklist;
                
                if (existingChecklist != null)
                {
                    // Use existing checklist
                    targetChecklist = existingChecklist;
                }
                else
                {
                    // Clone the checklist with new WIR code
                    targetChecklist = new Checklist
                    {
                        ChecklistId = Guid.NewGuid(),
                        Name = originalChecklist.Name,
                        Code = originalChecklist.Code,
                        Discipline = originalChecklist.Discipline,
                        SubDiscipline = originalChecklist.SubDiscipline,
                        PageNumber = originalChecklist.PageNumber,
                        WIRCode = request.TargetWIRCode,
                        ReferenceDocumentsJson = originalChecklist.ReferenceDocumentsJson,
                        SignatureRolesJson = originalChecklist.SignatureRolesJson,
                        IsActive = true,
                        CreatedDate = DateTime.UtcNow,
                        Sections = new List<ChecklistSection>()
                    };

                    await _unitOfWork.Repository<Checklist>().AddAsync(targetChecklist);
                    newChecklistIds.Add(targetChecklist.ChecklistId);
                    checklistsCloned++;
                }

                // Group items by section within this checklist
                var sectionGroups = checklistGroup
                    .Where(i => i.ChecklistSection != null)
                    .GroupBy(i => i.ChecklistSection!)
                    .ToList();

                foreach (var sectionGroup in sectionGroups)
                {
                    var originalSection = sectionGroup.Key;
                    
                    // Check if section already exists in target checklist
                    var existingSection = targetChecklist.Sections
                        .FirstOrDefault(s => s.Title == originalSection.Title && s.Order == originalSection.Order);

                    ChecklistSection targetSection;
                    
                    if (existingSection != null)
                    {
                        // Use existing section
                        targetSection = existingSection;
                    }
                    else
                    {
                        // Clone the section
                        targetSection = new ChecklistSection
                        {
                            ChecklistSectionId = Guid.NewGuid(),
                            Title = originalSection.Title,
                            Order = originalSection.Order,
                            ChecklistId = targetChecklist.ChecklistId,
                            IsActive = true,
                            CreatedDate = DateTime.UtcNow,
                            Items = new List<PredefinedChecklistItem>()
                        };

                        targetChecklist.Sections.Add(targetSection);
                        await _unitOfWork.Repository<ChecklistSection>().AddAsync(targetSection);
                        sectionsCloned++;
                    }

                    // Clone each item in this section (if not already exists)
                    foreach (var item in sectionGroup)
                    {
                        // Check if item with same description and sequence already exists
                        var existingItem = targetSection.Items
                            .FirstOrDefault(i => i.Description == item.Description && i.Sequence == item.Sequence);

                        if (existingItem == null)
                        {
                            var newItem = new PredefinedChecklistItem
                            {
                                PredefinedItemId = Guid.NewGuid(),
                                Description = item.Description,
                                Sequence = item.Sequence,
                                Reference = item.Reference,
                                ChecklistSectionId = targetSection.ChecklistSectionId,
                                IsActive = true,
                                CreatedDate = DateTime.UtcNow
                            };

                            targetSection.Items.Add(newItem);
                            await _unitOfWork.Repository<PredefinedChecklistItem>().AddAsync(newItem);
                            itemsCloned++;
                        }
                    }
                }
            }

            await _unitOfWork.CompleteAsync();

            var result = new CloneChecklistItemsResult(
                checklistsCloned,
                sectionsCloned,
                itemsCloned,
                newChecklistIds
            );

            return Result.Success(result);
        }
        catch (Exception ex)
        {
            return Result.Failure<CloneChecklistItemsResult>($"Failed to clone checklist items: {ex.Message}");
        }
    }
}
