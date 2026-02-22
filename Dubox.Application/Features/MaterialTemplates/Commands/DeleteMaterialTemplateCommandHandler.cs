using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.MaterialTemplates.Commands;

public class DeleteMaterialTemplateCommandHandler : IRequestHandler<DeleteMaterialTemplateCommand, Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteMaterialTemplateCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(DeleteMaterialTemplateCommand request, CancellationToken cancellationToken)
    {
        var template = await _unitOfWork.Repository<MaterialTemplate>()
            .GetByIdAsync(request.MaterialTemplateId, cancellationToken);

        if (template == null)
            return Result.Failure<bool>("Template not found.");

        // Soft delete - mark as inactive
        template.IsActive = false;
        _unitOfWork.Repository<MaterialTemplate>().Update(template);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success(true);
    }
}

