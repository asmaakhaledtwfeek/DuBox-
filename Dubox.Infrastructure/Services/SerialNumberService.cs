using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;

namespace Dubox.Infrastructure.Services;

public class SerialNumberService : ISerialNumberService
{
    private readonly IUnitOfWork _unitOfWork;

    public SerialNumberService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public string GenerateSerialNumber(int? year = null)
    {
        var currentYear = year ?? DateTime.UtcNow.Year;
        var prefix = $"SN-{currentYear}-";

        // Get the highest serial number for the current year
        var existingBoxes = _unitOfWork.Repository<Box>()
            .Get()
            .Where(b => b.SerialNumber != null && b.SerialNumber.StartsWith(prefix))
            .ToList();

        int nextSequence = 1;

        if (existingBoxes.Any())
        {
            // Extract the numeric part from existing serial numbers
            var sequences = existingBoxes
                .Select(b =>
                {
                    var parts = b.SerialNumber.Split('-');
                    if (parts.Length >= 3 && int.TryParse(parts[2], out int seq))
                        return seq;
                    return 0;
                })
                .Where(seq => seq > 0)
                .ToList();

            if (sequences.Any())
            {
                nextSequence = sequences.Max() + 1;
            }
        }

        // Format: SN-YYYY-NNNNNN (6 digits with leading zeros)
        return $"{prefix}{nextSequence:D6}";
    }
}

