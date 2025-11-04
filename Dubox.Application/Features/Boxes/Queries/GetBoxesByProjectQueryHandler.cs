using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Boxes.Queries;

public class GetBoxesByProjectQueryHandler : IRequestHandler<GetBoxesByProjectQuery, Result<List<BoxDto>>>
{
    private readonly IDbContext _dbContext;

    public GetBoxesByProjectQueryHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<List<BoxDto>>> Handle(GetBoxesByProjectQuery request, CancellationToken cancellationToken)
    {
        var boxes = await _dbContext.Boxes
            .Include(b => b.Project)
            .Where(b => b.ProjectId == request.ProjectId)
            .OrderBy(b => b.BoxTag)
            .ToListAsync(cancellationToken);

        var boxDtos = boxes.Select(b =>
        {
            var dto = b.Adapt<BoxDto>();
            return dto with { ProjectCode = b.Project.ProjectCode };
        }).ToList();

        return Result.Success(boxDtos);
    }
}

