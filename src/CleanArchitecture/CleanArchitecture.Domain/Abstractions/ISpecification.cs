
using System.Linq.Expressions;

namespace CleanArchitecture.Domain.Abstractions
{
    public interface ISpecification<TEntity,TEntityId>
        where TEntity: Entity<TEntityId>
        where TEntityId : class
    {
        Expression<Func<TEntity,bool>> Criteria { get; }

        //las uniones de sql se reprentan coomo una lista de expressiones
        List<Expression<Func<TEntity, object>>> Includes { get; }

        //order by, order by descending, group by
        Expression<Func<TEntity, object>>? OrderBy { get; }
        Expression<Func<TEntity, object>>? OrderByDescending { get; }
        Expression<Func<TEntity, object>>? GroupBy { get; }
        int Take { get; }
        int Skip { get; }
        bool IsPagingEnabled { get; }
        //bool IsCacheable { get; } //TODO: Implementar cache
    }
}
