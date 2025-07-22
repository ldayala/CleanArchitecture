
using CleanArchitecture.Domain.Users;

namespace CleanArchitecture.Application.Abstractions.Authentication
{
   public interface IJwtProvider
    {
        Task<string> GenerateTokenAsync(User userId, CancellationToken cancellationToken = default);
        Task<string> GenerateRefreshTokenAsync(CancellationToken cancellationToken = default);
        Task<bool> ValidateTokenAsync(string token, CancellationToken cancellationToken = default);
        Task<string?> GetUserIdFromTokenAsync(string token, CancellationToken cancellationToken = default);
        Task<string?> GetEmailFromTokenAsync(string token, CancellationToken cancellationToken = default);
    }
}
