using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.MaterialTemplates.Commands;

public class RemoveTemplateFromProjectCommandHandler : IRequestHandler<RemoveTemplateFromProjectCommand, Result<bool>>
{
    private readonly IDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveTemplateFromProjectCommandHandler(IDbContext context, IUnitOfWork unitOfWork)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(RemoveTemplateFromProjectCommand request, CancellationToken cancellationToken)
    {
        var assignment = await _context.ProjectMaterialTemplates
            .FirstOrDefaultAsync(pmt => pmt.ProjectId == request.ProjectId && 
                                       pmt.MaterialTemplateId == request.MaterialTemplateId, 
                                 cancellationToken);

        if (assignment == null)
            return Result.Failure<bool>("Template assignment not found.");

        _unitOfWork.Repository<ProjectMaterialTemplate>().Delete(assignment);
        await _unitOfWork.CompleteAsync(cancellationToken);

        // Note: Materials remain on boxes even after template is removed
        // This is by design - template acts as a snapshot

        return Result.Success(true);
    }
}

