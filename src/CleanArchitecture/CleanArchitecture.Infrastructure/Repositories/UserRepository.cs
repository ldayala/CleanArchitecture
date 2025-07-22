using CleanArchitecture.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Repositories;

internal sealed class UserRepository : Repository<User,UserId>, IUserRepository
{
    public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
       return await DbContext.Set<User>()
            .FirstOrDefaultAsync(x => x.Email == new Email( email), CancellationToken.None);
    }

    public async Task<bool> IsUserExits(Email email, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<User>()
            .AnyAsync(x => x.Email == email, cancellationToken);
    }
}