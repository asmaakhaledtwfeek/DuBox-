using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Boxes.Queries
{
    public class GenerateBoxQRCodeByIdQueryHandler : IRequestHandler<GenerateBoxQRCodeByIdQuery, Result<string>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IQRCodeService _qrCodeService;
        public GenerateBoxQRCodeByIdQueryHandler(IUnitOfWork unitOfWork, IQRCodeService qrCodeService)
        {
            _unitOfWork = unitOfWork;
            _qrCodeService = qrCodeService;
        }
        public async Task<Result<string>> Handle(GenerateBoxQRCodeByIdQuery request, CancellationToken cancellationToken)
        {
            var box = await _unitOfWork.Repository<Box>()
                .GetByIdAsync(request.BoxId, cancellationToken);

            if (box == null)
                return Result.Failure<string>("Box not found");

            try
            {
                // Generate QR code with public URL for box view
                var qrCodeBase64 = _qrCodeService.GeneratePublicBoxViewQRCode(request.BoxId);

                return Result.Success<string>(qrCodeBase64);
            }
            catch (Exception ex)
            {
                return Result.Failure<string>($"Error generating QR Code: {ex.Message}");
            }
        }
    }
}
