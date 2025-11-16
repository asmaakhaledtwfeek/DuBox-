namespace Dubox.Domain.Services
{
    public interface IQRCodeService
    {
        string GenerateQRCodeBase64(string value, int pixelsPerModule = 20);
    }
}
