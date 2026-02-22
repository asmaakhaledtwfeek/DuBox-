using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.MaterialTemplates.Commands;

public class UpdateMaterialTemplateCommandHandler : IRequestHandler<UpdateMaterialTemplateCommand, Result<bool>>
{
    private readonly IDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateMaterialTemplateCommandHandler(IDbContext context, IUnitOfWork unitOfWork)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(UpdateMaterialTemplateCommand request, CancellationToken cancellationToken)
    {
        var template = await _unitOfWork.Repository<MaterialTemplate>()
            .GetByIdAsync(request.MaterialTemplateId, cancellationToken);

        if (template == null)
            return Result.Failure<bool>("Template not found.");

        // Update template properties
        template.TemplateName = request.TemplateName;
        template.Description = request.Description;
        template.Category = request.Category;

        _unitOfWork.Repository<MaterialTemplate>().Update(template);

        // Remove existing items
        var existingItems = await _context.MaterialTemplateItems
            .Where(mti => mti.MaterialTemplateId == request.MaterialTemplateId)
            .ToListAsync(cancellationToken);

        foreach (var item in existingItems)
        {
            _unitOfWork.Repository<MaterialTemplateItem>().Delete(item);
        }

        // Add new items
        foreach (var itemDto in request.Items)
        {
            var material = await _unitOfWork.Repository<Material>()
                .GetByIdAsync(itemDto.MaterialId, cancellationToken);
            
            if (material == null)
                return Result.Failure<bool>($"Material with ID {itemDto.MaterialId} not found.");

            var item = new MaterialTemplateItem
            {
                MaterialTemplateId = request.MaterialTemplateId,
                MaterialId = itemDto.MaterialId,
                RequiredBeforeDays = itemDto.RequiredBeforeDays,
                IsRequired = itemDto.IsRequired,
                Notes = itemDto.Notes,
                DisplayOrder = itemDto.DisplayOrder
            };

            await _unitOfWork.Repository<MaterialTemplateItem>().AddAsync(item, cancellationToken);
        }

        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success(true);
    }
}

