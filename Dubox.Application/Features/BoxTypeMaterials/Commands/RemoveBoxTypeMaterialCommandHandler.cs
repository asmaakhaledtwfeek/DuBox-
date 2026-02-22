using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.BoxTypeMaterials.Commands;

public class RemoveBoxTypeMaterialCommandHandler : IRequestHandler<RemoveBoxTypeMaterialCommand, Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDbContext _context;

    public RemoveBoxTypeMaterialCommandHandler(IUnitOfWork unitOfWork, IDbContext context)
    {
        _unitOfWork = unitOfWork;
        _context = context;
    }

    public async Task<Result<bool>> Handle(RemoveBoxTypeMaterialCommand request, CancellationToken cancellationToken)
    {
        // Find the box type material
        var boxTypeMaterial = await _context.BoxTypeMaterials
            .Include(btm => btm.Material)
            .Include(btm => btm.ProjectBoxType)
            .FirstOrDefaultAsync(btm => btm.BoxTypeMaterialId == request.BoxTypeMaterialId, cancellationToken);

        if (boxTypeMaterial == null)
            return Result.Failure<bool>("Box type material not found.");

        // Get the project ID from the box type
        var projectBoxType = await _context.ProjectBoxTypes
            .FirstOrDefaultAsync(pbt => pbt.Id == boxTypeMaterial.ProjectBoxTypeId, cancellationToken);

        if (projectBoxType != null)
        {
            // Find all boxes of this type in the project
            var boxesOfType = await _context.Boxes
                .Where(b => b.ProjectBoxTypeId == boxTypeMaterial.ProjectBoxTypeId
                    && b.ProjectId == projectBoxType.ProjectId
                    && b.IsActive)
                .Select(b => b.BoxId)
                .ToListAsync(cancellationToken);

            if (boxesOfType.Any())
            {
                // Find all box materials for these boxes with the same material
                var boxMaterialsToDelete = await _context.BoxMaterials
                    .Where(bm => boxesOfType.Contains(bm.BoxId)
                        && bm.MaterialId == boxTypeMaterial.MaterialId)
                    .ToListAsync(cancellationToken);

                // Delete all matching box materials
                foreach (var boxMaterial in boxMaterialsToDelete)
                {
                    _unitOfWork.Repository<BoxMaterial>().Delete(boxMaterial);
                }
            }
        }

        // Delete the box type material
        _unitOfWork.Repository<BoxTypeMaterial>().Delete(boxTypeMaterial);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success(true);
    }
}






