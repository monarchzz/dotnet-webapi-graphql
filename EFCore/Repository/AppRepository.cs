using System.Linq.Expressions;
using EFCore.Context;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Repository;

public class AppRepository<TEntity> : IAppRepository<TEntity> where TEntity : class
{
    private readonly AppDbContext _context;
    private readonly DbSet<TEntity> _dbSet;
    private readonly string? _tenantIdentifier;

    public AppRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();

        _tenantIdentifier = context.TenantIdentifier;
    }

    public string TenantIdentifier()
    {
        return _tenantIdentifier ?? string.Empty;
        ;
    }

    public async Task<TEntity> Add(TEntity entity, CancellationToken cancellationToken = default)
    {
        var entry = await _dbSet.AddAsync(entity, cancellationToken);
        await _saveChangesAsync(cancellationToken);

        return entry.Entity;
    }

    public async Task AddRange(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddRangeAsync(entities, cancellationToken);
        await _saveChangesAsync(cancellationToken);
    }

    public async Task<TEntity> Update(TEntity entity, CancellationToken cancellationToken = default)
    {
        var entry = _dbSet.Update(entity);
        await _saveChangesAsync(cancellationToken);

        return entry.Entity;
    }

    public async Task UpdateRange(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        _dbSet.UpdateRange(entities);
        await _saveChangesAsync(cancellationToken);
    }

    public async Task<TEntity> Remove(TEntity entity, CancellationToken cancellationToken = default)
    {
        var entry = _dbSet.Remove(entity);
        await _saveChangesAsync(cancellationToken);

        return entry.Entity;
    }

    public async Task<TEntity?> Find(Guid id, CancellationToken cancellationToken = default)
    {
        var property = typeof(TEntity).GetProperty("Id");
        if (property == null)
            throw new Exception("Entity does not have Id property");

        var lambdaArg = Expression.Parameter(typeof(TEntity));
        var propertyAccess = Expression.MakeMemberAccess(lambdaArg, property);
        var propertyEquals = Expression.Equal(propertyAccess, Expression.Constant(id, typeof(Guid)));

        var expression = Expression.Lambda<Func<TEntity, bool>>(propertyEquals, lambdaArg);

        return await _noTrackingQuery().FirstOrDefaultAsync(expression, cancellationToken);
    }

    public async Task<bool> Any(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _noTrackingQuery().AnyAsync(predicate, cancellationToken);
    }

    public Task<bool> All(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return _noTrackingQuery().AllAsync(predicate, cancellationToken);
    }

    public IQueryable<TEntity> NoTrackingQuery(Expression<Func<TEntity, bool>>? predicate = null)
    {
        return predicate == null ? _noTrackingQuery() : _noTrackingQuery().Where(predicate);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _saveChangesAsync(cancellationToken);
    }

    private IQueryable<TEntity> _noTrackingQuery()
    {
        return _dbSet.AsNoTracking();
    }

    private async Task _saveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}