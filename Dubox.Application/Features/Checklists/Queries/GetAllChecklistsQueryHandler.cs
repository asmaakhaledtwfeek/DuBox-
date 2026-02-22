using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Checklists.Queries;

public class GetAllChecklistsQueryHandler : IRequestHandler<GetAllChecklistsQuery, Result<List<ChecklistDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllChecklistsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<ChecklistDto>>> Handle(GetAllChecklistsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var checklists = _unitOfWork.Repository<Checklist>()
                .GetWithSpec(new ChecklistSpecification()).Data.ToList();

            var checklistDtos = checklists.Select(c => new ChecklistDto
            {
                ChecklistId = c.ChecklistId,
                Name = c.Name,
                Code = c.Code,
                Discipline = c.Discipline,
                SubDiscipline = c.SubDiscipline,
                PageNumber = c.PageNumber,
                WIRCode = c.WIRCode,
                ReferenceDocuments = c.ReferenceDocuments,
                SignatureRoles = c.SignatureRoles,
                IsActive = c.IsActive,
                CreatedDate = c.CreatedDate,
                Sections = c.Sections
                    .OrderBy(s => s.Order)
                    .Select(s => new ChecklistSectionDto
                    {
                        ChecklistSectionId = s.ChecklistSectionId,
                        Title = s.Title,
                        Order = s.Order,
                        IsActive = s.IsActive,
                        ItemCount = s.Items.Count,
                        Items = s.Items
                            .OrderBy(i => i.Sequence)
                            .Select(i => new ChecklistItemDto
                            {
                                ChecklistItemId = i.PredefinedItemId,
                                Description = i.Description,
                                Order = i.Sequence,
                                IsActive = i.IsActive
                            }).ToList()
                    }).ToList()
            }).ToList();

            return Result.Success(checklistDtos);
        }
        catch (Exception ex)
        {
            return Result.Failure<List<ChecklistDto>>($"Failed to retrieve checklists: {ex.Message}");
        }
    }
}


