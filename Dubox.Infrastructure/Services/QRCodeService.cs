using Dubox.Domain.Services;
using Microsoft.Extensions.Configuration;
using QRCoder;

namespace Dubox.Infrastructure.Services
{
    public class QRCodeService : IQRCodeService
    {
        private readonly string _publicAppUrl;

        public QRCodeService(IConfiguration configuration)
        {
            _publicAppUrl = configuration.GetValue<string>("PublicAppSettings:PublicAppUrl") 
                ?? "http://localhost:4200";
        }

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

        /// <summary>
        /// Generates a QR code containing the public box view URL
        /// </summary>
        public string GeneratePublicBoxViewQRCode(Guid boxId, int pixelsPerModule = 20)
        {
            var publicUrl = GetPublicBoxViewUrl(boxId);
            return GenerateQRCodeBase64(publicUrl, pixelsPerModule);
        }

        /// <summary>
        /// Gets the public URL for viewing a box
        /// </summary>
        public string GetPublicBoxViewUrl(Guid boxId)
        {
            return $"{_publicAppUrl.TrimEnd('/')}/box/view/{boxId}";
        }
    }
}
