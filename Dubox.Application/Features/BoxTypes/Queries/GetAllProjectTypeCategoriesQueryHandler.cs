using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.BoxTypes.Queries;

public class GetAllProjectTypeCategoriesQueryHandler : IRequestHandler<GetAllProjectTypeCategoriesQuery, Result<List<ProjectTypeCategoryDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllProjectTypeCategoriesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork=unitOfWork;
    }

    public async Task<Result<List<ProjectTypeCategoryDto>>> Handle(GetAllProjectTypeCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories =  _unitOfWork.Repository<ProjectTypeCategory>().Get()
            .Select(c => new ProjectTypeCategoryDto
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName,
                Abbreviation = c.Abbreviation
            })
            .OrderBy(c => c.CategoryName)
            .ToList();

        return Result<List<ProjectTypeCategoryDto>>.Success(categories);
    }
}

