
using CleanArchitecture.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Specification
{
    public class SpecificationEvaluator<TEntity,TEntityId>
        where TEntity : Entity<TEntityId>
       where TEntityId : class
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity,TEntityId> specification)
        {
            var query = inputQuery;
            // Apply Criteria
            if (specification.Criteria != null)
            {
                query = query.Where(specification.Criteria);
            }
          
            // Apply Order By
            if (specification.OrderBy != null)
            {
                query = query.OrderBy(specification.OrderBy);
            }
            // Apply Order By Descending
            if (specification.OrderByDescending != null)
            {
                query = query.OrderByDescending(specification.OrderByDescending);
            }
            // Apply Paging
            if (specification.IsPagingEnabled)
            {
                query = query.Skip(specification.Skip).Take(specification.Take);
            }
            // Apply Include Statements

            query = specification.Includes!
                .Aggregate(query, (current, include) => current.Include(include))
                .AsSplitQuery() // esto lo que hace es que las consultas se ejecuten por separado , lo que mejora el rendimiento en algunos casos
                .AsNoTracking(); // esto lo que hace es que no se rastreen los cambios en las entidades, lo que mejora el rendimiento en algunos casos
            return query;
        }
    }
}
