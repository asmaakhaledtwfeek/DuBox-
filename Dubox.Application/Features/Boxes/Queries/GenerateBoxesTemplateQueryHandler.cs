using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Boxes.Queries;

public class GenerateBoxesTemplateQueryHandler : IRequestHandler<GenerateBoxesTemplateQuery, Result<byte[]>>
{
    private readonly IExcelService _excelService;
    private readonly IUnitOfWork _unitOfWork;

    private static readonly string[] Headers = new[]
    {
        "Box Name",
        "Box Type",
        "Box Sub Type",
        "Floor",
        "Building Number",
        "Box Function",
        "Zone",
        "Length",
        "Width",
        "Height",
        "Notes",
        "Box Tag (Auto-Generated)"
    };

    public GenerateBoxesTemplateQueryHandler(IExcelService excelService, IUnitOfWork unitOfWork)
    {
        _excelService = excelService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<byte[]>> Handle(GenerateBoxesTemplateQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Validate project exists
            var project = await _unitOfWork.Repository<Project>().GetByIdAsync(request.ProjectId, cancellationToken);
            if (project == null)
                return Result.Failure<byte[]>("Project not found");

            // Get project configuration data
            var boxTypesWithSubTypes = _unitOfWork.Repository<ProjectBoxType>()
                .Get()
                .Where(bt => bt.ProjectId == request.ProjectId && bt.IsActive)
                .OrderBy(bt => bt.DisplayOrder)
                .ToList();

            var boxTypes = boxTypesWithSubTypes.Select(bt => bt.TypeName).ToList();

            // Get all sub-types with their parent type names for organized reference
            var boxSubTypesGrouped = new Dictionary<string, List<string>>();
            foreach (var boxType in boxTypesWithSubTypes.Where(bt => bt.HasSubTypes))
            {
                var subTypes = _unitOfWork.Repository<ProjectBoxSubType>()
                    .Get()
                    .Where(st => st.ProjectBoxTypeId == boxType.Id && st.IsActive)
                    .OrderBy(st => st.DisplayOrder)
                    .Select(st => st.SubTypeName)
                    .ToList();
                
                if (subTypes.Any())
                {
                    boxSubTypesGrouped[boxType.TypeName] = subTypes;
                }
            }

            var buildings = _unitOfWork.Repository<ProjectBuilding>()
                .Get()
                .Where(b => b.ProjectId == request.ProjectId && b.IsActive)
                .OrderBy(b => b.DisplayOrder)
                .Select(b => b.BuildingCode)
                .ToList();

            var floors = _unitOfWork.Repository<ProjectLevel>()
                .Get()
                .Where(l => l.ProjectId == request.ProjectId && l.IsActive)
                .OrderBy(l => l.DisplayOrder)
                .Select(l => l.LevelCode)
                .ToList();

            var zones = _unitOfWork.Repository<ProjectZone>()
                .Get()
                .Where(z => z.ProjectId == request.ProjectId && z.IsActive)
                .OrderBy(z => z.DisplayOrder)
                .Select(z => z.ZoneCode)
                .ToList();

            var functions = _unitOfWork.Repository<ProjectBoxFunction>()
                .Get()
                .Where(f => f.ProjectId == request.ProjectId && f.IsActive)
                .OrderBy(f => f.DisplayOrder)
                .Select(f => f.FunctionName)
                .ToList();

            // Create reference data for validation sheets
            var referenceData = new Dictionary<string, List<string>>
            {
                { "Box Types", boxTypes },
                { "Buildings", buildings },
                { "Floors", floors },
                { "Zones", zones },
                { "Functions", functions }
            };

            // Generate template with project-specific configuration
            var templateBytes = _excelService.GenerateTemplateWithReference<ImportBoxFromExcelDto>(
                Headers, 
                referenceData,
                boxSubTypesGrouped,
                project.ProjectCode,
                $"Box Import Template - {project.ProjectCode}");

            return await Task.FromResult(Result.Success(templateBytes));
        }
        catch (Exception ex)
        {
            return Result.Failure<byte[]>($"Error generating template: {ex.Message}");
        }
    }
}

