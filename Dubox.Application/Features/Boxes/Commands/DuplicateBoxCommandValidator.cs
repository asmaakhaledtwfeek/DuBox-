using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using FluentValidation;

namespace Dubox.Application.Features.Boxes.Commands;

public class DuplicateBoxCommandValidator : AbstractValidator<DuplicateBoxCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DuplicateBoxCommandValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

        RuleFor(x => x.BoxId)
            .NotEmpty()
            .WithMessage("BoxId is required.")
            .MustAsync(BoxExists)
            .WithMessage("Box with the specified ID does not exist.");
    }

    private async Task<bool> BoxExists(Guid boxId, CancellationToken cancellationToken)
    {
        var box = await _unitOfWork.Repository<Box>().GetByIdAsync(boxId, cancellationToken);
        return box != null;
    }
}



