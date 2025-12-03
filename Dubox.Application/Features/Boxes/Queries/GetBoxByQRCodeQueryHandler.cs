using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Boxes.Queries;

public class GetBoxByQRCodeQueryHandler : IRequestHandler<GetBoxByQRCodeQuery, Result<BoxDto>>
{
    private readonly IDbContext _dbContext;

    public GetBoxByQRCodeQueryHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<BoxDto>> Handle(GetBoxByQRCodeQuery request, CancellationToken cancellationToken)
    {
        // Try exact match first (for new structured format)
        var box = await _dbContext.Boxes
            .Include(b => b.Project)
            .FirstOrDefaultAsync(b => b.QRCodeString == request.QRCodeString, cancellationToken);

        // If not found, try parsing the structured format and searching by components
        if (box == null && request.QRCodeString.Contains("ProjectCode:") && request.QRCodeString.Contains("BoxTag:"))
        {
            var lines = request.QRCodeString.Split('\n');
            string? projectCode = null;
            string? boxTag = null;
            string? serialNumber = null;

            foreach (var line in lines)
            {
                if (line.StartsWith("ProjectCode:", StringComparison.OrdinalIgnoreCase))
                    projectCode = line.Substring("ProjectCode:".Length).Trim();
                else if (line.StartsWith("BoxTag:", StringComparison.OrdinalIgnoreCase))
                    boxTag = line.Substring("BoxTag:".Length).Trim();
                else if (line.StartsWith("SerialNumber:", StringComparison.OrdinalIgnoreCase))
                    serialNumber = line.Substring("SerialNumber:".Length).Trim();
            }

            if (!string.IsNullOrEmpty(projectCode) && !string.IsNullOrEmpty(boxTag))
            {
                box = await _dbContext.Boxes
                    .Include(b => b.Project)
                    .FirstOrDefaultAsync(b => 
                        b.Project.ProjectCode == projectCode && 
                        b.BoxTag == boxTag &&
                        (string.IsNullOrEmpty(serialNumber) || b.SerialNumber == serialNumber), 
                        cancellationToken);
            }
        }

        // If still not found, try old format (ProjectCode_BoxTag)
        if (box == null && request.QRCodeString.Contains('_'))
        {
            var parts = request.QRCodeString.Split('_');
            if (parts.Length >= 2)
            {
                var projectCode = parts[0];
                var boxTag = string.Join("_", parts.Skip(1));

                box = await _dbContext.Boxes
                    .Include(b => b.Project)
                    .FirstOrDefaultAsync(b => 
                        b.Project.ProjectCode == projectCode && 
                        b.BoxTag == boxTag, 
                        cancellationToken);
            }
        }

        if (box == null)
            return Result.Failure<BoxDto>("Box not found with this QR code");

        var boxDto = box.Adapt<BoxDto>() with { ProjectCode = box.Project.ProjectCode };

        return Result.Success(boxDto);
    }
}

