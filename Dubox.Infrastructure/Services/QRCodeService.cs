using Dubox.Domain.Services;
using QRCoder;

namespace Dubox.Infrastructure.Services
{
    public class QRCodeService : IQRCodeService
    {
        public string GenerateQRCodeBase64(string qrCodeText, int pixelsPerModule = 20)
        {

            if (string.IsNullOrEmpty(qrCodeText))
            {
                return string.Empty;
            }

            using (var qrGenerator = new QRCodeGenerator())
            {
                var qrCodeData = qrGenerator.CreateQrCode(qrCodeText, QRCodeGenerator.ECCLevel.Q);

                using (var qrCode = new PngByteQRCode(qrCodeData))
                {
                    var qrCodeBytes = qrCode.GetGraphic(pixelsPerModule);

                    return Convert.ToBase64String(qrCodeBytes);
                }
            }
        }
    }
}
