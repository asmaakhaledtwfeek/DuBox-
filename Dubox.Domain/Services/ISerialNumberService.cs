namespace Dubox.Domain.Services;

public interface ISerialNumberService
{
    string GenerateSerialNumber(string boxLetter, int lastSeq, string? year = null);
}

