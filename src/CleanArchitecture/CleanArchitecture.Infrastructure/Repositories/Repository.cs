using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Infrastructure.Extensions;
using CleanArchitecture.Infrastructure.Specification;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace CleanArchitecture.Infrastructure.Repositories;


internal abstract class Repository<TEntity, TEntityId>
where TEntity : Entity<TEntityId>
where TEntityId : class
{
    protected readonly ApplicationDbContext DbContext;

    protected Repository(ApplicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public async Task<TEntity?> GetByIdAsync(
        TEntityId id,
        CancellationToken cancellationToken = default
    )
    {
        return await DbContext.Set<TEntity>()
        .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public virtual void Add(TEntity entity)
    {
        DbContext.Add(entity);
    }

    public IQueryable<TEntity> ApplySpecification(
        ISpecification<TEntity, TEntityId> spec
    )
    {
        return SpecificationEvaluator<TEntity, TEntityId>
            .GetQuery(DbContext.Set<TEntity>().AsQueryable(), spec);
    }

    public async Task<IReadOnlyList<TEntity>> GetAllWithSpec(ISpecification<TEntity, TEntityId> spec)
    {
        return await ApplySpecification(spec).ToListAsync();
    }

    public async Task<int> CountAsync(ISpecification<TEntity, TEntityId> spec)
    {
        return await ApplySpecification(spec).CountAsync();
    }


    public async Task<PagedResults<TEntity, TEntityId>> GetPaginationAsync(
        Expression<Func<TEntity, bool>>? predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? includes,
        int page,
        int pageSize,
        string orderBy,
        bool ascending,
        bool disableTracking = true
        )
    {
        IQueryable<TEntity> query = DbContext.Set<TEntity>();

        if (disableTracking) query = query.AsNoTracking();
        
        if (predicate != null)
        {
            query = query.Where(predicate);
        }
        if (includes != null)
        {
            query = includes(query); // Apply includes if provided
        }
       
        var totalRecords = await query.CountAsync();
        var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
       
        var records= new List<TEntity>();

        if (string.IsNullOrEmpty(orderBy))
        {
            records = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        else
        {
            records= await query
                                .OrderByPropertyOrField(orderBy, ascending)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        return new PagedResults<TEntity, TEntityId>
        {
            PageNumber = page,
            PageSize = pageSize,
            TotalNumberOfpages = totalPages,
            TotalNumberOfRecords = totalRecords,
            Results = records
        };

    }

}
