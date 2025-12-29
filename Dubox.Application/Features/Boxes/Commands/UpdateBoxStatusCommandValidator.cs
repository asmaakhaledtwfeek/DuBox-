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
                .WithMessage("Invalid status transition. The status change is not allowed based on the current box status and progress.");

        }

        private async Task<bool> BeValidStatusTransition(UpdateBoxStatusCommand command, CancellationToken cancellationToken)
        {
            var box = await _unitOfWork.Repository<Box>().GetByIdAsync(command.BoxId, cancellationToken);

            if (box == null) return false;

            var newStatus = (BoxStatusEnum)command.Status;
            var currentStatus = box.Status;
            var progress = box.ProgressPercentage;

            // Business rules for status transitions
            switch (currentStatus)
            {
                case BoxStatusEnum.NotStarted:
                    // NotStarted can only change to OnHold
                    if (newStatus != BoxStatusEnum.OnHold)
                        return false;
                    break;

                case BoxStatusEnum.InProgress:
                    // InProgress can only change to OnHold
                    if (newStatus != BoxStatusEnum.OnHold)
                        return false;
                    break;

                case BoxStatusEnum.Completed:
                    // Completed can only change to Dispatched or OnHold
                    if (newStatus != BoxStatusEnum.Dispatched && newStatus != BoxStatusEnum.OnHold)
                        return false;
                    break;

                case BoxStatusEnum.OnHold:
                    // OnHold transitions depend on progress
                    if (progress == 0)
                    {
                        // Can only change to NotStarted
                        if (newStatus != BoxStatusEnum.NotStarted)
                            return false;
                    }
                    else if (progress < 100)
                    {
                        // Can only change to InProgress
                        if (newStatus != BoxStatusEnum.InProgress)
                            return false;
                    }
                    else // progress >= 100
                    {
                        // Can change to Completed or Dispatched
                        if (newStatus != BoxStatusEnum.Completed && newStatus != BoxStatusEnum.Dispatched)
                            return false;
                    }
                    break;

                case BoxStatusEnum.Dispatched:
                    // Dispatched typically shouldn't be changed, but allow OnHold if needed
                    if (newStatus != BoxStatusEnum.OnHold)
                        return false;
                    break;

                default:
                    return false;
            }

            return true;
        }
    }
}
