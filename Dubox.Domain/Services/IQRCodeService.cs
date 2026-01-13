namespace Dubox.Domain.Services
{
    public interface IQRCodeService
    {
        string GenerateQRCodeBase64(string value, int pixelsPerModule = 20);
        
        /// <summary>
        /// Generates a QR code containing the public box view URL
        /// </summary>
        /// <param name="boxId">The box ID</param>
        /// <param name="pixelsPerModule">QR code pixel density</param>
        /// <returns>Base64 encoded QR code image</returns>
        string GeneratePublicBoxViewQRCode(Guid boxId, int pixelsPerModule = 20);
        
        /// <summary>
        /// Gets the public URL for viewing a box
        /// </summary>
        /// <param name="boxId">The box ID</param>
        /// <returns>Public box view URL</returns>
        string GetPublicBoxViewUrl(Guid boxId);
    }
}
