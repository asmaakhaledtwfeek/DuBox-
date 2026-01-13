using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Infrastructure.Services;

public class SerialNumberService : ISerialNumberService
{
    private readonly IUnitOfWork _unitOfWork;

    public SerialNumberService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public string GenerateSerialNumber(string boxLetter,int lastSeq,string? year = null)
    {
        var areaCode = "AE08";
        var currentYear = year ?? DateTime.UtcNow.Year.ToString().Substring(2,2);
        var companyCode = "DBX";
        var prefix = $"SN-{currentYear}-";
        var newSeq = lastSeq + 1;
        return $"{areaCode}-{year}{newSeq:D2}-{companyCode}-{boxLetter}";
    }
}

