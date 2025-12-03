namespace Dubox.Domain.Services;

public interface ISerialNumberService
{
    /// <summary>
    /// Generates a unique serial number for a box in the format: SN-YYYY-NNNNNN
    /// </summary>
    /// <param name="year">The year for the serial number (defaults to current year)</param>
    /// <returns>A unique serial number string</returns>
    string GenerateSerialNumber(int? year = null);
}

