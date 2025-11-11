namespace Dubox.Domain.Services;

/// <summary>
/// Service for handling Excel import/export operations
/// </summary>
public interface IExcelService
{
    /// <summary>
    /// Generate an Excel template file with specified columns
    /// </summary>
    byte[] GenerateTemplate<T>(string[] headers) where T : class;
    
    /// <summary>
    /// Read data from Excel file and convert to list of objects
    /// </summary>
    Task<List<T>> ReadFromExcelAsync<T>(Stream fileStream, Func<Dictionary<string, object?>, T> mapper) where T : class;
    
    /// <summary>
    /// Export data to Excel file
    /// </summary>
    byte[] ExportToExcel<T>(List<T> data, string[] headers) where T : class;
    
    /// <summary>
    /// Validate Excel file structure
    /// </summary>
    Task<(bool IsValid, List<string> Errors)> ValidateExcelStructureAsync(Stream fileStream, string[] requiredHeaders);
}

