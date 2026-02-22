using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Checklists.Commands;

public class CreateChecklistCommandHandler : IRequestHandler<CreateChecklistCommand, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateChecklistCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateChecklistCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Check if checklist code already exists
            var existingChecklist = _unitOfWork.Repository<Checklist>()
                .Get()
                .FirstOrDefault(c => c.Code == request.Code);

            if (existingChecklist != null)
            {
                return Result.Failure<Guid>($"Checklist with code '{request.Code}' already exists");
            }

            // Create new checklist
            var checklist = new Checklist
            {
                ChecklistId = Guid.NewGuid(),
                Name = request.Name,
                Code = request.Code,
                Discipline = request.Discipline,
                SubDiscipline = request.SubDiscipline,
                PageNumber = request.PageNumber,
                WIRCode = request.WIRCode,
                ReferenceDocuments = request.ReferenceDocuments ?? new List<string>(),
                SignatureRoles = request.SignatureRoles ?? new List<string>(),
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            };

            await _unitOfWork.Repository<Checklist>().AddAsync(checklist);
            await _unitOfWork.CompleteAsync();

            return Result.Success(checklist.ChecklistId);
        }
        catch (Exception ex)
        {
            return Result.Failure<Guid>($"Failed to create checklist: {ex.Message}");
        }
    }
}
