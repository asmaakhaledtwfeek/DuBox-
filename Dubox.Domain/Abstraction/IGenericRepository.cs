using Dubox.Domain.Specification;
using System.Linq.Expressions;

namespace Dubox.Domain.Abstraction;

public interface IGenericRepository<T> where T : class
{
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    Task AddRangeAsync(List<T> entities, CancellationToken cancellationToken = default);
    void Update(T entity);
    void UpdateRange(IEnumerable<T> entities);
    void Delete(T entity);
    void DeleteRange(IEnumerable<T> entity);
    Task<T?> GetByIdAsync<TKey>(TKey id, CancellationToken cancellationToken = default) where TKey : notnull;
    Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default);
    IReadOnlyList<T> Get();
    (IQueryable<T> Data, int Count) GetWithSpec(Specification<T> specification);
    T? GetEntityWithSpec(Specification<T> specification);
    Task<bool> IsExistAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default);
    Task<int> CountAsync(Expression<Func<T, bool>>? filter = null, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<T>> FindAsync(Expression<Func<T, bool>>? filter = null, CancellationToken cancellationToken = default);
}

