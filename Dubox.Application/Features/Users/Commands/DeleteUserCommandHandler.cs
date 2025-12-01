using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Users.Commands;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteUserCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Repository<User>()
            .GetByIdAsync(request.UserId, cancellationToken);

        if (user == null)
            return Result.Failure("User not found.");

        // Check if user has WIRRecords (RequestedBy or InspectedBy)
        var hasWIRRecordsAsRequester = await _unitOfWork.Repository<WIRRecord>()
            .IsExistAsync(w => w.RequestedBy == request.UserId, cancellationToken);

        if (hasWIRRecordsAsRequester)
            return Result.Failure("Cannot delete user. User has WIR records as requester. Please reassign or delete the WIR records first.");

        var hasWIRRecordsAsInspector = await _unitOfWork.Repository<WIRRecord>()
            .IsExistAsync(w => w.InspectedBy == request.UserId, cancellationToken);

        if (hasWIRRecordsAsInspector)
            return Result.Failure("Cannot delete user. User has WIR records as inspector. Please reassign or delete the WIR records first.");

        // Check if user has ProgressUpdates
        var hasProgressUpdates = await _unitOfWork.Repository<ProgressUpdate>()
            .IsExistAsync(p => p.UpdatedBy == request.UserId, cancellationToken);

        if (hasProgressUpdates)
            return Result.Failure("Cannot delete user. User has progress updates. Please reassign or delete the progress updates first.");

        // Check if user is a department manager
        var isDepartmentManager = await _unitOfWork.Repository<Department>()
            .IsExistAsync(d => d.ManagerId == request.UserId, cancellationToken);

        if (isDepartmentManager)
            return Result.Failure("Cannot delete user. User is managing a department. Please assign a new manager to the department first.");

        // Note: TeamMember has Cascade delete from User, and BoxActivity has SetNull for AssignedMember
        // So TeamMembers will be deleted automatically, and BoxActivities will have AssignedMemberId set to null
        // No need to check for TeamMember relationships

        // Note: UserRoles and UserGroups will cascade delete automatically
        // Department relationship has Restrict behavior
        // If user has a DepartmentId, the database constraint will prevent deletion
        // We'll handle this in the try-catch block below

        // Attempt to delete the user
        // If DepartmentId is required and Restrict, this will fail with a database constraint
        // In that case, we'll catch the exception and return a meaningful error
        try
        {
            _unitOfWork.Repository<User>().Delete(user);
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
                // Check if it's related to Department
                if (user.DepartmentId.HasValue)
                {
                    return Result.Failure("Cannot delete user. User is assigned to a department. Please remove the user from the department first.");
                }
                return Result.Failure("Cannot delete user due to existing relationships. Please remove all user associations first.");
            }
            throw; // Re-throw if it's a different error
        }
        catch (Exception ex)
        {
            // For other exceptions, check for constraint-related messages
            if (ex.Message.Contains("foreign key", StringComparison.OrdinalIgnoreCase) ||
                ex.Message.Contains("constraint", StringComparison.OrdinalIgnoreCase))
            {
                if (user.DepartmentId.HasValue)
                {
                    return Result.Failure("Cannot delete user. User is assigned to a department. Please remove the user from the department first.");
                }
                return Result.Failure("Cannot delete user due to existing relationships. Please remove all user associations first.");
            }
            throw; // Re-throw if it's a different error
        }
    }
}
