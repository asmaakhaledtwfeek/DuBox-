using Dubox.Domain.Services;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;

namespace Dubox.Application.Services;

public class BarcodeService : IBarcodeService
{
    private const string BARCODE_PREFIX = "PNL";
    
    public string GeneratePanelBarcode(Guid boxPanelId, string projectCode)
    {
        // Format: PNL-{ProjectCode}-{ShortGuid}
        // Example: PNL-PRJ001-ABC123XYZ
        var shortGuid = ConvertToShortGuid(boxPanelId);
        return $"{BARCODE_PREFIX}-{projectCode}-{shortGuid}";
    }

    public async Task<string> GenerateQRCodeImageAsync(string data, int width = 300, int height = 300)
    {
        await Task.CompletedTask; // Keep async signature
        
        using var qrGenerator = new QRCodeGenerator();
        using var qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
        using var qrCode = new QRCode(qrCodeData);
        using var qrCodeImage = qrCode.GetGraphic(20);
        
        // Convert to base64 string
        using var ms = new MemoryStream();
        qrCodeImage.Save(ms, ImageFormat.Png);
        var imageBytes = ms.ToArray();
        return $"data:image/png;base64,{Convert.ToBase64String(imageBytes)}";
    }

    public async Task<byte[]> GenerateBarcodeImageAsync(string barcode, int width = 400, int height = 100)
    {
        await Task.CompletedTask; // Keep async signature
        
        // For simplicity, we'll use QR code for now
        // In production, you might want to use a proper barcode library like ZXing
        using var qrGenerator = new QRCodeGenerator();
        using var qrCodeData = qrGenerator.CreateQrCode(barcode, QRCodeGenerator.ECCLevel.Q);
        using var qrCode = new QRCode(qrCodeData);
        using var qrCodeImage = qrCode.GetGraphic(10);
        
        using var ms = new MemoryStream();
        qrCodeImage.Save(ms, ImageFormat.Png);
        return ms.ToArray();
    }

    public bool IsValidBarcode(string barcode)
    {
        if (string.IsNullOrWhiteSpace(barcode))
            return false;

        // Format: PNL-{ProjectCode}-{ShortGuid}
        var parts = barcode.Split('-');
        return parts.Length == 3 && parts[0] == BARCODE_PREFIX;
    }

    public (string projectCode, Guid panelId) ParsePanelBarcode(string barcode)
    {
        if (!IsValidBarcode(barcode))
            throw new ArgumentException("Invalid barcode format", nameof(barcode));

        var parts = barcode.Split('-');
        var projectCode = parts[1];
        var panelId = ConvertFromShortGuid(parts[2]);
        
        return (projectCode, panelId);
    }

    /// <summary>
    /// Convert GUID to a shorter alphanumeric string (base36)
    /// </summary>
    private string ConvertToShortGuid(Guid guid)
    {
        var bytes = guid.ToByteArray();
        var base36 = Convert.ToBase64String(bytes)
            .Replace("/", "")
            .Replace("+", "")
            .Replace("=", "")
            .Substring(0, 10)
            .ToUpper();
        return base36;
    }

    /// <summary>
    /// Convert short GUID back to full GUID (This is a simplified approach)
    /// In production, you'd want to store the mapping in the database
    /// </summary>
    private Guid ConvertFromShortGuid(string shortGuid)
    {
        // This is a placeholder - in reality, you'd query the database
        // by the barcode field to get the actual GUID
        throw new NotImplementedException("Barcode lookup should be done via database query");
    }
}

