
using CleanArchitecture.Application.Abstractions.Authentication;
using CleanArchitecture.Application.Abstractions.Data;
using CleanArchitecture.Domain.Users;
using Dapper;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CleanArchitecture.Infrastructure.Authentication
{
    public sealed class JwtProvider(IOptions<JwtOptions> jwtOptions, ISqlConnectionFactory sqlConnectionFactory) : IJwtProvider
    {
        private readonly JwtOptions _jwtOptions=jwtOptions.Value;
        private readonly ISqlConnectionFactory _sqlConnectionFactory= sqlConnectionFactory;
        public async Task<string> GenerateTokenAsync(User user, CancellationToken cancellationToken = default)
        {
            const string  sql = @"
                Select  p.nombre 
                From users u
                LEFT JOIN users_roles ur ON u.id = ur.user_id
                LEFT JOIN roles r ON ur.role_id = r.id
                LEFT JOIN roles_permissions rp ON r.id = rp.role_id
                LEFT JOIN permissions p ON rp.permission_id = p.id
                WHERE u.id = @UserId
"; 
            using var connection = _sqlConnectionFactory.CreateConnection();
            var parameters = new { UserId = user.Id!.Value };
            var permissions = await connection.QueryAsync<string>(sql, parameters);

            var permissionColections = permissions.ToHashSet();


            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id!.Value.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!.Value.ToString()),

            };

            foreach (var permission in permissionColections)
            {
                claims.Add(new Claim(CustomClaims.Permissions, permission));
            }

            var sigingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey!)),
                SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                _jwtOptions.Issuer,
                _jwtOptions.Audience,
                claims,
                null,
                DateTime.UtcNow.AddDays(365),
                sigingCredentials);

            var tokenValue= new JwtSecurityTokenHandler().WriteToken(token);

            return tokenValue;
        }

        public Task<string> GenerateRefreshTokenAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }


        public Task<string?> GetEmailFromTokenAsync(string token, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<string?> GetUserIdFromTokenAsync(string token, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ValidateTokenAsync(string token, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
