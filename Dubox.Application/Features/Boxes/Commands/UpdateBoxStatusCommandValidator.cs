using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using FluentValidation;

namespace Dubox.Application.Features.Boxes.Commands
{


    public class UpdateBoxStatusCommandValidator : AbstractValidator<UpdateBoxStatusCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateBoxStatusCommandValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(x => x.BoxId)
                .NotEmpty().WithMessage("Box ID is required.");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Box status is required.")
                .Must(status => Enum.IsDefined(typeof(BoxStatusEnum), status))
                .WithMessage("Invalid box status value provided.");

            RuleFor(x => x)
                .MustAsync(BeValidStatusTransition)
                .WithMessage("The status change is invalid. Status cannot be set to 'Completed' manually or 'InProgress' via this command.");

        }

        private async Task<bool> BeValidStatusTransition(UpdateBoxStatusCommand command, CancellationToken cancellationToken)
        {
            var box = await _unitOfWork.Repository<Box>().GetByIdAsync(command.BoxId, cancellationToken);

            if (box == null) return false;

            var newStatus = (BoxStatusEnum)command.Status;
            var currentStatus = box.Status;
            if (newStatus == BoxStatusEnum.Completed)
                return false;

            if (newStatus == BoxStatusEnum.InProgress)
            {
                if (currentStatus != BoxStatusEnum.OnHold)
                    return false;
            }

            if (newStatus == BoxStatusEnum.NotStarted && box.ActualStartDate.HasValue)
                return false;

            return true;
        }
    }
}
