using System.Linq.Expressions;

namespace CoreFitness2.Application.Interfaces;

public interface IBaseRepository<TEntity> where TEntity : class
{
    Task AddAsync(TEntity entity, CancellationToken ct = default);

    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default);

    Task<TEntity?> GetOneAsync(
        Expression<Func<TEntity, bool>> predicate,
        bool tracking = false,
        CancellationToken ct = default,
        params Expression<Func<TEntity, object>>[] includes);

    Task<IReadOnlyList<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        bool tracking = false,
        CancellationToken ct = default,
        params Expression<Func<TEntity, object>>[] includes);

    void Delete(TEntity entity);

    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
