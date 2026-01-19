using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using FluentValidation;

namespace Dubox.Application.Features.Cost.Commands;

public class CreateProjectCostCommandValidator : AbstractValidator<CreateProjectCostCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateProjectCostCommandValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

        RuleFor(x => x.ProjectId)
            .NotEmpty()
            .WithMessage("Project ID is required")
            .MustAsync(ProjectExists)
            .WithMessage("Project does not exist");

        RuleFor(x => x.BoxId)
            .MustAsync(BoxExists)
            .WithMessage("Box does not exist")
            .When(x => x.BoxId.HasValue && x.BoxId != Guid.Empty);

        RuleFor(x => x.Cost)
            .NotEmpty()
            .WithMessage("Cost amount is required")
            .GreaterThan(0)
            .WithMessage("Cost must be greater than 0")
            .Must(BeValidDecimal)
            .WithMessage("Cost must have maximum 2 decimal places and cannot exceed 999999999999999.99");

        // Cost Code Master fields (cascading)
        RuleFor(x => x.CostCodeLevel1)
            .NotEmpty()
            .WithMessage("Cost Code Level 1 is required")
            .MaximumLength(50)
            .WithMessage("Cost Code Level 1 must not exceed 50 characters");

        RuleFor(x => x.CostCodeLevel2)
            .NotEmpty()
            .WithMessage("Cost Code Level 2 is required")
            .MaximumLength(50)
            .WithMessage("Cost Code Level 2 must not exceed 50 characters");

        RuleFor(x => x.CostCodeLevel3)
            .MaximumLength(50)
            .WithMessage("Cost Code Level 3 must not exceed 50 characters")
            .When(x => !string.IsNullOrEmpty(x.CostCodeLevel3));

        // HRC Code fields (cascading)
        RuleFor(x => x.Chapter)
            .NotEmpty()
            .WithMessage("Chapter is required")
            .MaximumLength(100)
            .WithMessage("Chapter must not exceed 100 characters");

        RuleFor(x => x.SubChapter)
            .NotEmpty()
            .WithMessage("Sub Chapter is required")
            .MaximumLength(100)
            .WithMessage("Sub Chapter must not exceed 100 characters");

        RuleFor(x => x.Classification)
            .NotEmpty()
            .WithMessage("Classification is required")
            .MaximumLength(100)
            .WithMessage("Classification must not exceed 100 characters");

        RuleFor(x => x.SubClassification)
            .NotEmpty()
            .WithMessage("Sub Classification is required")
            .MaximumLength(100)
            .WithMessage("Sub Classification must not exceed 100 characters");

        RuleFor(x => x.Units)
            .NotEmpty()
            .WithMessage("Units is required")
            .MaximumLength(20)
            .WithMessage("Units must not exceed 20 characters");

        RuleFor(x => x.Type)
            .NotEmpty()
            .WithMessage("Type is required")
            .MaximumLength(50)
            .WithMessage("Type must not exceed 50 characters");
    }

    private async Task<bool> ProjectExists(Guid projectId, CancellationToken cancellationToken)
    {
        if (projectId == Guid.Empty)
            return false;
            
        var project = await _unitOfWork.Repository<Project>().GetByIdAsync(projectId, cancellationToken);
        return project != null;
    }

    private async Task<bool> BoxExists(Guid? boxId, CancellationToken cancellationToken)
    {
        if (!boxId.HasValue || boxId.Value == Guid.Empty)
            return true; // Skip validation if no box provided
            
        var box = await _unitOfWork.Repository<Box>().GetByIdAsync(boxId.Value, cancellationToken);
        return box != null;
    }

    private bool BeValidDecimal(decimal cost)
    {
        // Check if decimal has maximum 2 decimal places
        var decimalPlaces = BitConverter.GetBytes(decimal.GetBits(cost)[3])[2];
        if (decimalPlaces > 2)
            return false;

        // Check if value doesn't exceed decimal(18,2) max value
        return cost <= 9999999999999999.99m;
    }
}

