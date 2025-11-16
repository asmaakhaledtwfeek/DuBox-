using Dubox.Application.Features.Materials.Commands;
using Dubox.Application.Features.Materials.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dubox.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MaterialsController : ControllerBase
{
    private readonly IMediator _mediator;

    public MaterialsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllMaterials(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllMaterialsQuery(), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("low-stock")]
    public async Task<IActionResult> GetLowStockMaterials(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetLowStockMaterialsQuery(), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateMaterial([FromBody] CreateMaterialCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
    [HttpPut("restock/{materialId}")]
    public async Task<IActionResult> RestockMaterial(Guid materialId, [FromBody] RestockMaterialCommand command, CancellationToken cancellationToken)
    {
        if (materialId != command.MaterialId)
            return BadRequest("Material ID mismatch");

        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
    [HttpGet("{materialId}")]
    public async Task<IActionResult> GetMaterialById(Guid materialId, CancellationToken cancellationToken)
    {

        var result = await _mediator.Send(new GetMaterialByIdQuery(materialId), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
    [HttpGet("transactions/{materialId}")]
    public async Task<IActionResult> GetMaterialTransactionsById(Guid materialId, CancellationToken cancellationToken)
    {

        var result = await _mediator.Send(new GetAllMaterialTransactionsByMaterialIdQuery(materialId), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut("update/{materialId}")]
    public async Task<IActionResult> UpdateMaterial(Guid materialId, [FromBody] UpdateMaterialCommand command, CancellationToken cancellationToken)
    {
        if (materialId != command.MaterialId)
            return BadRequest("Material ID mismatch");

        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("template")]
    public async Task<IActionResult> DownloadTemplate(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GenerateMaterialsTemplateQuery(), cancellationToken);

        if (!result.IsSuccess)
            return BadRequest(result);

        return File(result.Data!,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "MaterialsImportTemplate.xlsx");
    }


    [HttpPost("import")]
    [RequestSizeLimit(10_485_760)] // 10 MB
    public async Task<IActionResult> ImportFromExcel([FromForm] IFormFile file, CancellationToken cancellationToken)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded");

        using var stream = file.OpenReadStream();
        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream, cancellationToken);
        memoryStream.Position = 0;

        var command = new ImportMaterialsFromExcelCommand(memoryStream, file.FileName);
        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}

