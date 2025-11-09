using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Boxes.Queries;

public class GetBoxByIdQueryHandler : IRequestHandler<GetBoxByIdQuery, Result<BoxDto>>
{
    private readonly IDbContext _dbContext;

    public GetBoxByIdQueryHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<BoxDto>> Handle(GetBoxByIdQuery request, CancellationToken cancellationToken)
    {
        var box = await _dbContext.Boxes
            .Include(b => b.Project)
            .FirstOrDefaultAsync(b => b.BoxId == request.BoxId, cancellationToken);

        if (box == null)
            return Result.Failure<BoxDto>("Box not found");

        var boxDto = box.Adapt<BoxDto>() with { ProjectCode = box.Project.ProjectCode };

        return Result.Success(boxDto);
    }
}

