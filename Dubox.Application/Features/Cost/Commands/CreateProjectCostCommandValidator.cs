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

        RuleFor(x => x.BoxId)
            .NotEmpty()
            .WithMessage("Box ID is required")
            .MustAsync(BoxExists)
            .WithMessage("Box does not exist");

        RuleFor(x => x.Cost)
            .NotEmpty()
            .WithMessage("Cost is required")
            .GreaterThan(0)
            .WithMessage("Cost must be greater than 0")
            .Must(BeValidDecimal)
            .WithMessage("Cost must have maximum 2 decimal places and cannot exceed 999999999999999.99");

        RuleFor(x => x.CostType)
            .NotEmpty()
            .WithMessage("Cost type is required")
            .MaximumLength(100)
            .WithMessage("Cost type cannot exceed 100 characters");
    }

    private async Task<bool> BoxExists(Guid boxId, CancellationToken cancellationToken)
    {
        var box = await _unitOfWork.Repository<Box>().GetByIdAsync(boxId, cancellationToken);
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

