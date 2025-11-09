using Dubox.Domain.Specification;
using Dubox.Domain.Abstraction;
using Dubox.Infrastructure.ApplicationContext;
using Dubox.Infrastructure.Specification;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Dubox.Infrastructure.Repositories;

internal class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<T> _entity;
   
    public GenericRepository(ApplicationDbContext context)
    {
        _context = context;
        _entity = context.Set<T>();
    }

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);
        await _entity.AddAsync(entity, cancellationToken);
    }

    public async Task AddRangeAsync(List<T> entities, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entities);
        if (entities.Count > 0)
        {
            await _entity.AddRangeAsync(entities, cancellationToken);
        }
    }

    public void Update(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        _entity.Update(entity);
    }

    public void UpdateRange(IEnumerable<T> entities)
    {
        ArgumentNullException.ThrowIfNull(entities);
        _entity.UpdateRange(entities);
    }

    public void Delete(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        _entity.Remove(entity);
    }

    public void DeleteRange(IEnumerable<T> entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        _entity.RemoveRange(entity);
    }

    public async Task<T?> GetByIdAsync<TKey>(TKey id, CancellationToken cancellationToken = default)
        where TKey : notnull
    {
        ArgumentNullException.ThrowIfNull(id);
        return await _entity.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _entity.AsNoTracking().ToListAsync(cancellationToken);

    public (IQueryable<T> Data, int Count) GetWithSpec(Specification<T> specifications)
    {
        ArgumentNullException.ThrowIfNull(specifications);
        return SpecificationEvaluator<T>.GetQuery(_entity, specifications);
    }

    public T? GetEntityWithSpec(Specification<T> specifications)
    {
        ArgumentNullException.ThrowIfNull(specifications);
        return SpecificationEvaluator<T>.GetQuery(_entity, specifications).Data.FirstOrDefault();
    }

    public async Task<bool> IsExistAsync(
        Expression<Func<T, bool>> filter, 
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(filter);
        return await _entity.AnyAsync(filter, cancellationToken);
    }

    public async Task<int> CountAsync(
        Expression<Func<T, bool>>? filter = null, 
        CancellationToken cancellationToken = default)
    {
        if (filter == null)
            return await _entity.CountAsync(cancellationToken);
        
        return await _entity.CountAsync(filter, cancellationToken);
    }

    public IReadOnlyList<T> Get()
        => _entity.AsNoTracking().ToList();
}
