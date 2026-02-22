using Dubox.Domain.Abstraction;
using Dubox.Domain.Interfaces;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.BoxTypeMaterials.Commands;

public class UpdateBoxTypeMaterialQuantityCommandHandler 
    : IRequestHandler<UpdateBoxTypeMaterialQuantityCommand, Result<bool>>
{
    private readonly IDbContext _context;

    public UpdateBoxTypeMaterialQuantityCommandHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<Result<bool>> Handle(
        UpdateBoxTypeMaterialQuantityCommand request, 
        CancellationToken cancellationToken)
    {
        // Find the box type material
        var boxTypeMaterial = await _context.BoxTypeMaterials
            .FirstOrDefaultAsync(
                btm => btm.BoxTypeMaterialId == request.BoxTypeMaterialId, 
                cancellationToken);

        if (boxTypeMaterial == null)
        {
            return Result.Failure<bool>("Box type material not found");
        }

        // Validate quantity
        if (request.QuantityPerBox < 0)
        {
            return Result.Failure<bool>("Quantity per box cannot be negative");
        }

        // Update the quantity per box
        boxTypeMaterial.QuantityPerBox = request.QuantityPerBox;

        await _context.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}
