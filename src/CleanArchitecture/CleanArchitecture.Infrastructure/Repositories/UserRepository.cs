using CleanArchitecture.Application.Paginations;
using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace CleanArchitecture.Infrastructure.Repositories;

internal sealed class UserRepository : Repository<User,UserId>, IUserRepository,IPaginationRepository<User, UserId>
{
    public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
       return await DbContext.Set<User>()
            .FirstOrDefaultAsync(x => x.Email == new Email( email), CancellationToken.None);
    }

    public Task<PagedResults<TE, TId>> GetPaginationAsync<TE, TId>(Expression<Func<TE, bool>> predicate, Func<IQueryable<TE>, IIncludableQueryable<TE, object>> includes, int page, int pageSize, string orderby, bool ascending, bool disableTracking = true)
        where TE : Entity<TId>
        where TId : class
    {
        throw new NotImplementedException();
    }

    public async Task<bool> IsUserExits(Email email, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<User>()
            .AnyAsync(x => x.Email == email, cancellationToken);
    }

    public override void Add(User user)
    {
        foreach (var role in user.Roles!)
        {
            DbContext.Attach(role);
        }
        DbContext.Add(user);
    }
}