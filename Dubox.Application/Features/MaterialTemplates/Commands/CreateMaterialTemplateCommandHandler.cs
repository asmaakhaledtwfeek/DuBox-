using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.MaterialTemplates.Commands;

public class CreateMaterialTemplateCommandHandler : IRequestHandler<CreateMaterialTemplateCommand, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateMaterialTemplateCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateMaterialTemplateCommand request, CancellationToken cancellationToken)
    {
        // Check if template code already exists
        var existingTemplates = await _unitOfWork.Repository<MaterialTemplate>()
            .FindAsync(mt => mt.TemplateCode == request.TemplateCode, cancellationToken);

        if (existingTemplates.Any())
            return Result.Failure<Guid>("A template with this code already exists.");

        // Validate all materials exist
        foreach (var item in request.Items)
        {
            var material = await _unitOfWork.Repository<Material>()
                .GetByIdAsync(item.MaterialId, cancellationToken);
            
            if (material == null)
                return Result.Failure<Guid>($"Material with ID {item.MaterialId} not found.");
        }

        var template = new MaterialTemplate
        {
            TemplateName = request.TemplateName,
            TemplateCode = request.TemplateCode,
            Description = request.Description,
            Category = request.Category,
            IsActive = true
        };

        await _unitOfWork.Repository<MaterialTemplate>().AddAsync(template, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        // Add template items
        foreach (var itemDto in request.Items)
        {
            var item = new MaterialTemplateItem
            {
                MaterialTemplateId = template.MaterialTemplateId,
                MaterialId = itemDto.MaterialId,
                RequiredBeforeDays = itemDto.RequiredBeforeDays,
                IsRequired = itemDto.IsRequired,
                Notes = itemDto.Notes,
                DisplayOrder = itemDto.DisplayOrder
            };

            await _unitOfWork.Repository<MaterialTemplateItem>().AddAsync(item, cancellationToken);
        }

        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success(template.MaterialTemplateId);
    }
}

