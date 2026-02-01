using Dubox.Application.DTOs;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Materials.Queries;

public class GenerateMaterialsTemplateQueryHandler : IRequestHandler<GenerateMaterialsTemplateQuery, Result<byte[]>>
{
    private readonly IExcelService _excelService;

    private static readonly string[] Headers = new[]
    {
        "MaterialCode",
        "MaterialName",
        "MaterialCategory",
        "Unit",
        "UnitCost",
        "CurrentStock",
        "MinimumStock",
        "ReorderLevel",
        "SupplierName"
    };

    public GenerateMaterialsTemplateQueryHandler(IExcelService excelService)
    {
        _excelService = excelService;
    }

    public async Task<Result<byte[]>> Handle(GenerateMaterialsTemplateQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var templateBytes = _excelService.GenerateTemplate<ImportMaterialDto>(Headers);
            return await Task.FromResult(Result.Success(templateBytes));
        }
        catch (Exception ex)
        {
            return Result.Failure<byte[]>($"Error generating template: {ex.Message}");
        }
    }
}

