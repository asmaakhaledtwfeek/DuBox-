using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Boxes.Queries;

public class GetBoxQRCodeWithUrlQueryHandler : IRequestHandler<GetBoxQRCodeWithUrlQuery, Result<BoxQRCodeDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IQRCodeService _qrCodeService;

    public GetBoxQRCodeWithUrlQueryHandler(IUnitOfWork unitOfWork, IQRCodeService qrCodeService)
    {
        _unitOfWork = unitOfWork;
        _qrCodeService = qrCodeService;
    }

    public async Task<Result<BoxQRCodeDto>> Handle(GetBoxQRCodeWithUrlQuery request, CancellationToken cancellationToken)
    {
        var box = await _unitOfWork.Repository<Box>()
            .GetByIdAsync(request.BoxId, cancellationToken);

        if (box == null)
            return Result.Failure<BoxQRCodeDto>("Box not found");

        try
        {
            var qrCodeImage = _qrCodeService.GeneratePublicBoxViewQRCode(request.BoxId);
            var publicUrl = _qrCodeService.GetPublicBoxViewUrl(request.BoxId);

            var result = new BoxQRCodeDto
            {
                BoxId = request.BoxId,
                QRCodeImage = qrCodeImage,
                PublicUrl = publicUrl
            };

            return Result.Success(result);
        }
        catch (Exception ex)
        {
            return Result.Failure<BoxQRCodeDto>($"Error generating QR Code: {ex.Message}");
        }
    }
}

