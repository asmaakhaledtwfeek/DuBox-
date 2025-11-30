using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Roles.Commands;

public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteRoleCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _unitOfWork.Repository<Role>()
            .GetByIdAsync(request.RoleId, cancellationToken);

        if (role == null)
            return Result.Failure("Role not found.");

        // Note: UserRoles and GroupRoles will cascade delete automatically
        // based on the database configuration (DeleteBehavior.Cascade)

        // Attempt to delete the role
        try
        {
            _unitOfWork.Repository<Role>().Delete(role);
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
                return Result.Failure("Cannot delete role due to existing relationships. Please remove all role associations first.");
            }
            throw; // Re-throw if it's a different error
        }
        catch (Exception ex)
        {
            // For other exceptions, check for constraint-related messages
            if (ex.Message.Contains("foreign key", StringComparison.OrdinalIgnoreCase) ||
                ex.Message.Contains("constraint", StringComparison.OrdinalIgnoreCase))
            {
                return Result.Failure("Cannot delete role due to existing relationships. Please remove all role associations first.");
            }
            throw; // Re-throw if it's a different error
        }
    }
}
