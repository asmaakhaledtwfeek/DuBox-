using Dubox.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Domain.Abstraction;

public interface IDbContext
{
    DbSet<User> Users { get; }
    DbSet<Role> Roles { get; }
    DbSet<Group> Groups { get; }
    DbSet<UserRole> UserRoles { get; }
    DbSet<UserGroup> UserGroups { get; }
    DbSet<GroupRole> GroupRoles { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

