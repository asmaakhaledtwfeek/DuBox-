using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Boxes.Queries;

public class GetAllBoxesQueryHandler : IRequestHandler<GetAllBoxesQuery, Result<List<BoxDto>>>
{
    private readonly IDbContext _dbContext;
    private readonly IQRCodeService _qrCodeService;

    public GetAllBoxesQueryHandler(IDbContext dbContext, IQRCodeService qRCodeService)
    {
        _dbContext = dbContext;
        _qrCodeService = qRCodeService;
    }

    public async Task<Result<List<BoxDto>>> Handle(GetAllBoxesQuery request, CancellationToken cancellationToken)
    {
        var boxes = await _dbContext.Boxes
            .Include(b => b.Project)
            .OrderByDescending(b => b.CreatedDate)
            .ToListAsync(cancellationToken);

        var boxDtos = boxes.Select(b =>
        {
            var dto = b.Adapt<BoxDto>();
            return dto with
            {
                ProjectCode = b.Project.ProjectCode,
                QRCodeImage = _qrCodeService.GenerateQRCodeBase64(b.QRCodeString)
            };
        }).ToList();

        return Result.Success(boxDtos);
    }
}

