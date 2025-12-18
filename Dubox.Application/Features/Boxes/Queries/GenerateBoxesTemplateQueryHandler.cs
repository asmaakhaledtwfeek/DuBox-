using Dubox.Application.DTOs;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Boxes.Queries;

public class GenerateBoxesTemplateQueryHandler : IRequestHandler<GenerateBoxesTemplateQuery, Result<byte[]>>
{
    private readonly IExcelService _excelService;

    private static readonly string[] Headers = new[]
    {
        "Box Tag",
        "Box Name",
        "Box Type",
        "Floor",
        "Building Number",
        "Box Letter",
        "Zone",
        "Length",
        "Width",
        "Height",
        "Notes"
    };

    public GenerateBoxesTemplateQueryHandler(IExcelService excelService)
    {
        _excelService = excelService;
    }

    public async Task<Result<byte[]>> Handle(GenerateBoxesTemplateQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var templateBytes = _excelService.GenerateTemplate<ImportBoxFromExcelDto>(Headers);
            return await Task.FromResult(Result.Success(templateBytes));
        }
        catch (Exception ex)
        {
            return Result.Failure<byte[]>($"Error generating template: {ex.Message}");
        }
    }
}

