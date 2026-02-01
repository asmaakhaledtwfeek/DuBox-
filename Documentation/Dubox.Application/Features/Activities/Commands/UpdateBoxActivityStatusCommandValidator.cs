using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;

namespace Dubox.Application.Features.Activities.Commands
{

    using FluentValidation;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class UpdateBoxActivityStatusCommandValidator : AbstractValidator<UpdateBoxActivityStatusCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateBoxActivityStatusCommandValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(x => x.BoxActivityId)
                .NotEmpty().WithMessage("Box Activity ID is required.");

            RuleFor(x => x.Status)
                .Must(status => Enum.IsDefined(typeof(BoxStatusEnum), status))
                .WithMessage("Invalid status value provided.")
                .Must(status => status != BoxStatusEnum.Completed)
                .WithMessage("Activity status must be updated to Completed through the dedicated approval/completion process, not manually.");

            RuleFor(x => x)
                .MustAsync(BeValidStatusTransition)
                .WithMessage("The activity status change is invalid due to existing actual dates or scheduling conflicts.")
                .When(x => x.Status != BoxStatusEnum.NotStarted);
        }

        private async Task<bool> BeValidStatusTransition(UpdateBoxActivityStatusCommand command, CancellationToken cancellationToken)
        {
            var activity = await _unitOfWork.Repository<BoxActivity>().GetByIdAsync(command.BoxActivityId, cancellationToken);

            if (activity == null) return false;

            var newStatus = command.Status;

            if (newStatus == BoxStatusEnum.NotStarted && activity.ActualStartDate.HasValue)
                return false;

            return true;
        }
    }
}
