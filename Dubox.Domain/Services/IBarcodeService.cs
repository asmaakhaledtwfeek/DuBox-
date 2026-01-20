namespace Dubox.Domain.Services;

/// <summary>
/// Service for generating barcodes and QR codes for panels
/// </summary>
public interface IBarcodeService
{
    /// <summary>
    /// Generate a unique barcode for a panel
    /// </summary>
    string GeneratePanelBarcode(Guid boxPanelId, string projectCode);

    /// <summary>
    /// Generate QR code image URL for a panel barcode
    /// </summary>
    Task<string> GenerateQRCodeImageAsync(string data, int width = 300, int height = 300);

    /// <summary>
    /// Generate barcode image as byte array
    /// </summary>
    Task<byte[]> GenerateBarcodeImageAsync(string barcode, int width = 400, int height = 100);

    /// <summary>
    /// Validate barcode format
    /// </summary>
    bool IsValidBarcode(string barcode);

    /// <summary>
    /// Parse barcode to extract panel information
    /// </summary>
    (string projectCode, Guid panelId) ParsePanelBarcode(string barcode);
}

