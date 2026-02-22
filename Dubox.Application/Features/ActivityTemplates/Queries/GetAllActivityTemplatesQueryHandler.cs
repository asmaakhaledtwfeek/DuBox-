using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.ActivityTemplates.Queries;

public class GetAllActivityTemplatesQueryHandler : IRequestHandler<GetAllActivityTemplatesQuery, Result<List<ActivityTemplateDto>>>
{
    private readonly IDbContext _context;

    public GetAllActivityTemplatesQueryHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<ActivityTemplateDto>>> Handle(GetAllActivityTemplatesQuery request, CancellationToken cancellationToken)
    {
        var query = _context.ActivityTemplates
            .Include(t => t.TemplateActivities)
            .AsQueryable();

        // Filter by active status if requested
        if (request.ActiveOnly)
        {
            query = query.Where(t => t.IsActive);
        }

        // Filter by search term if provided
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchLower = request.SearchTerm.ToLower();
            query = query.Where(t => 
                t.TemplateName.ToLower().Contains(searchLower) ||
                (t.Description != null && t.Description.ToLower().Contains(searchLower))
            );
        }

        // Load templates with their activities first
        var templatesData = await query
            .OrderByDescending(t => t.CreatedDate)
            .ToListAsync(cancellationToken);

        // Project to DTOs after loading (client-side evaluation)
        var templates = templatesData
            .Select(t => new ActivityTemplateDto(
                t.ActivityTemplateId,
                t.TemplateName,
                t.Description,
                t.StageCount,
                t.IsActive,
                t.TemplateActivities?.Count ?? 0,
                t.CreatedDate,
                t.CreatedBy
            ))
            .ToList();

        return Result.Success(templates);
    }
}
