using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Boxes.Commands
{
    public record DefineBoxMaterialRequirementCommand(
  Guid BoxId,
  List<MaterialRequirementItem> Requirements
) : IRequest<Result<List<BoxMaterialDto>>>;

    public record MaterialRequirementItem(
        Guid MaterialId,
        decimal RequiredQuantity,
        string Unit
    );
}

