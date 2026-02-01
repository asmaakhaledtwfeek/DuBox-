namespace Dubox.Application.DTOs;

/// <summary>
/// DTO containing QR code image and public URL for a box
/// </summary>
public record BoxQRCodeDto
{
    /// <summary>
    /// Base64 encoded QR code image
    /// </summary>
    public string QRCodeImage { get; init; } = string.Empty;
    
    /// <summary>
    /// Public URL that the QR code points to
    /// </summary>
    public string PublicUrl { get; init; } = string.Empty;
    
    /// <summary>
    /// Box ID
    /// </summary>
    public Guid BoxId { get; init; }
}

