
using CleanArchitecture.Application.Abstractions.Authentication;
using CleanArchitecture.Application.Abstractions.Messaging;
using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Users;
using MediatR;

namespace CleanArchitecture.Application.Users.LoginUser
{
    internal sealed class LoginCommandHandler(IUserRepository userRepository, IJwtProvider jwtProvider) : ICommandHandler<LoginCommand, string>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IJwtProvider _jwtProvider = jwtProvider;
        async Task<Result<string>> IRequestHandler<LoginCommand, Result<string>>.Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            //verify user exists in the database
            var user = await _userRepository.GetByEmailAsync(request.Email);

            //find  user by email
            if (user is null)
            {
                return Result.Failure<string>(UserErrors.NotFound);
            }
            //verify password
            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash!.value))
            {
                return Result.Failure<string>(UserErrors.InvalidCredentials);
            }
            // Generate JWT token
            var token = await _jwtProvider.GenerateTokenAsync(user, cancellationToken);

            return Result.Success(token);

        }
    }

}
