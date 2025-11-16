using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Boxes.Queries;

public class GetBoxByIdQueryHandler : IRequestHandler<GetBoxByIdQuery, Result<BoxDto>>
{
    private readonly IDbContext _dbContext;
    private readonly IQRCodeService _qrCodeService;

    public GetBoxByIdQueryHandler(IDbContext dbContext, IQRCodeService qRCodeService)
    {
        _dbContext = dbContext;
        _qrCodeService = qRCodeService;
    }

    public async Task<Result<BoxDto>> Handle(GetBoxByIdQuery request, CancellationToken cancellationToken)
    {
        var box = await _dbContext.Boxes
            .Include(b => b.Project)
            .FirstOrDefaultAsync(b => b.BoxId == request.BoxId, cancellationToken);

        if (box == null)
            return Result.Failure<BoxDto>("Box not found");

        var boxDto = box.Adapt<BoxDto>() with
        {
            ProjectCode = box.Project.ProjectCode,
            QRCodeImage = _qrCodeService.GenerateQRCodeBase64(box.QRCodeString)
        };

        return Result.Success(boxDto);
    }
}

