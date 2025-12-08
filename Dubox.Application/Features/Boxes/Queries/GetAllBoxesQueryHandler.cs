using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Boxes.Queries;

public class GetAllBoxesQueryHandler : IRequestHandler<GetAllBoxesQuery, Result<List<BoxDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IQRCodeService _qrCodeService;

    public GetAllBoxesQueryHandler(IUnitOfWork unitOfWork, IQRCodeService qRCodeService)
    {
        _unitOfWork = unitOfWork;
        _qrCodeService = qRCodeService;
    }

    public async Task<Result<List<BoxDto>>> Handle(GetAllBoxesQuery request, CancellationToken cancellationToken)
    {
        // Use AsNoTracking and ToListAsync for better performance
        var boxes = await _unitOfWork.Repository<Box>()
            .GetWithSpec(new GetAllBoxesWithIncludesSpecification()).Data
            .AsNoTracking()
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

