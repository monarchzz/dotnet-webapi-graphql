using System.Linq.Expressions;

namespace EFCore.Repository;

public interface IAppRepository<TEntity> where TEntity : class
{
    public string TenantIdentifier();

    Task<TEntity> Add(TEntity entity, CancellationToken cancellationToken = default);

    Task AddRange(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    Task<TEntity> Update(TEntity entity, CancellationToken cancellationToken = default);

    Task UpdateRange(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    Task<TEntity> Remove(TEntity entity, CancellationToken cancellationToken = default);

    Task<TEntity?> Find(Guid id, CancellationToken cancellationToken = default);

    Task<List<TEntity>> Finds(List<Guid> ids, CancellationToken cancellationToken = default);

    Task<bool> Any(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    Task<bool> All(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    IQueryable<TEntity> NoTrackingQuery(Expression<Func<TEntity, bool>>? predicate = null);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}