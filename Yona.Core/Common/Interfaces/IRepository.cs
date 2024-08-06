namespace Yona.Core.Common.Interfaces;

internal interface IRepository<TEntity, TId>
    where TEntity : class, new()
{
    void Add(TEntity entity);

    void Delete(TEntity entity);

    void Update(TEntity entity);

    TEntity Get(TId id);

    IReadOnlyList<TEntity> Items { get; }
}
