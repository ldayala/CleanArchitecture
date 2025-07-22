namespace CleanArchitecture.Domain.Users;

public interface IUserRepository
{

    Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken = default);

    void Add(User user);
    Task<User?> GetByEmailAsync(string email);
    Task<bool> IsUserExits(Email email, CancellationToken cancellationToken = default);

}