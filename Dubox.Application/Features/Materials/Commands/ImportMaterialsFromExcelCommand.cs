using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Materials.Commands;

public record ImportMaterialsFromExcelCommand(Stream FileStream, string FileName) : IRequest<Result<MaterialImportResultDto>>;

