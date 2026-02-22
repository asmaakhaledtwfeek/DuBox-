using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Checklists.Commands;

public class CreateChecklistSectionCommandHandler : IRequestHandler<CreateChecklistSectionCommand, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateChecklistSectionCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateChecklistSectionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Check if checklist exists
            var checklist = await _unitOfWork.Repository<Checklist>()
                .GetByIdAsync(request.ChecklistId);

            if (checklist == null)
            {
                return Result.Failure<Guid>("Checklist not found");
            }

            // Create new section
            var section = new ChecklistSection
            {
                ChecklistSectionId = Guid.NewGuid(),
                ChecklistId = request.ChecklistId,
                Title = request.Title,
                Order = request.Order,
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            };

            await _unitOfWork.Repository<ChecklistSection>().AddAsync(section);
            await _unitOfWork.CompleteAsync();

            return Result.Success(section.ChecklistSectionId);
        }
        catch (Exception ex)
        {
            return Result.Failure<Guid>($"Failed to create section: {ex.Message}");
        }
    }
}
