using CoreFitness2.Application.Interfaces;
using CoreFitness2.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CoreFitness2.Infrastructure.Repositories;

public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<TEntity> _table;

    public BaseRepository(ApplicationDbContext context)
    {
        _context = context;
        _table = _context.Set<TEntity>();
    }

    public virtual async Task AddAsync(TEntity entity, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(entity);
        await _table.AddAsync(entity, ct);
    }

    public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
    {
        return await _table.AnyAsync(predicate, ct);
    }

    public virtual async Task<TEntity?> GetOneAsync(
        Expression<Func<TEntity, bool>> predicate,
        bool tracking = false,
        CancellationToken ct = default,
        params Expression<Func<TEntity, object>>[] includes)
    {
        return await BuildQuery(tracking, includes)
            .FirstOrDefaultAsync(predicate, ct);
    }

    public virtual async Task<IReadOnlyList<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        bool tracking = false,
        CancellationToken ct = default,
        params Expression<Func<TEntity, object>>[] includes)
    {
        var query = BuildQuery(tracking, includes);

        if (predicate is not null)
            query = query.Where(predicate);

        if (orderBy is not null)
            query = orderBy(query);

        return await query.ToListAsync(ct);
    }

    public virtual void Delete(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        _table.Remove(entity);
    }

    public virtual async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return await _context.SaveChangesAsync(ct);
    }

    private IQueryable<TEntity> BuildQuery(
        bool tracking,
        params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = tracking ? _table.AsTracking() : _table.AsNoTracking();

        foreach (var include in includes)
            query = query.Include(include);

        return query;
    }
}