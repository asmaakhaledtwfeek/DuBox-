using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Checklists.Queries;

public class GetChecklistsByWIRCodeQueryHandler : IRequestHandler<GetChecklistsByWIRCodeQuery, Result<List<ChecklistDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetChecklistsByWIRCodeQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<ChecklistDto>>> Handle(GetChecklistsByWIRCodeQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var checklists = _unitOfWork.Repository<Checklist>()
                .GetWithSpec(new ChecklistSpecification(request.WIRCode)).Data.ToList();

            //  var checklistDtos = checklists.Adapt<List<ChecklistDto>>();

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

public class ChecklistDto
{
    public Guid ChecklistId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Discipline { get; set; } = string.Empty;
    public string? SubDiscipline { get; set; }
    public int PageNumber { get; set; }
    public string? WIRCode { get; set; }
    public List<string>? ReferenceDocuments { get; set; }
    public List<string>? SignatureRoles { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
    public List<ChecklistSectionDto> Sections { get; set; } = new();
}

public class ChecklistSectionDto
{
    public Guid ChecklistSectionId { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Order { get; set; }
    public bool IsActive { get; set; }
    public int ItemCount { get; set; }
    public List<ChecklistItemDto> Items { get; set; } = new();
}

public class ChecklistItemDto
{
    public Guid ChecklistItemId { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? Reference { get; set; }
    public int Order { get; set; }
    public bool IsActive { get; set; }
}
