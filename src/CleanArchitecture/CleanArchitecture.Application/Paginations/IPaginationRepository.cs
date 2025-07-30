
using CleanArchitecture.Domain.Abstractions;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;


namespace CleanArchitecture.Application.Paginations
{
    public interface IPaginationRepository<TE, TId> where TE : Entity<TId> where TId : class
    {
        Task<PagedResults<TE, TId>> GetPaginationAsync(
           Expression<Func<TE, bool>> predicate,
           Func<IQueryable<TE>, IIncludableQueryable<TE, object>> includes,
            int page,
            int pageSize,
            string orderby,
            bool ascending,
            bool disableTracking = true);
          
    }
}
