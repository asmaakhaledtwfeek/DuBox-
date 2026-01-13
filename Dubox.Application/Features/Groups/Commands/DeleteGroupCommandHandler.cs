using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Groups.Commands;

public class DeleteGroupCommandHandler : IRequestHandler<DeleteGroupCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteGroupCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteGroupCommand request, CancellationToken cancellationToken)
    {
        var group = await _unitOfWork.Repository<Group>()
            .GetByIdAsync(request.GroupId, cancellationToken);

        if (group == null)
            return Result.Failure("Group not found.");

        // Note: UserGroups and GroupRoles will cascade delete automatically
        // based on the database configuration (DeleteBehavior.Cascade)

        // Attempt to delete the group
        try
        {
            _unitOfWork.Repository<Group>().Delete(group);
            await _unitOfWork.CompleteAsync(cancellationToken);
            return Result.Success();
        }
        catch (Microsoft.EntityFrameworkCore.DbUpdateException dbEx)
        {
            // Check if it's a foreign key constraint violation
            if (dbEx.InnerException?.Message.Contains("foreign key", StringComparison.OrdinalIgnoreCase) == true ||
                dbEx.InnerException?.Message.Contains("constraint", StringComparison.OrdinalIgnoreCase) == true ||
                dbEx.Message.Contains("foreign key", StringComparison.OrdinalIgnoreCase) ||
                dbEx.Message.Contains("constraint", StringComparison.OrdinalIgnoreCase))
            {
                return Result.Failure("Cannot delete group due to existing relationships. Please remove all group associations first.");
            }
            throw; // Re-throw if it's a different error
        }
        catch (Exception ex)
        {
            // For other exceptions, check for constraint-related messages
            if (ex.Message.Contains("foreign key", StringComparison.OrdinalIgnoreCase) ||
                ex.Message.Contains("constraint", StringComparison.OrdinalIgnoreCase))
            {
                return Result.Failure("Cannot delete group due to existing relationships. Please remove all group associations first.");
            }
            throw; // Re-throw if it's a different error
        }
    }
}
