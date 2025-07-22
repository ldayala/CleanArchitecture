
using CleanArchitecture.Application.Abstractions.Messaging;
using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Users;
using MediatR;

namespace CleanArchitecture.Application.Users.RegisterUser
{
    internal class RegisterUserCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
        : ICommandHandler<RegisterUserCommand, Guid>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        async Task<Result<Guid>> IRequestHandler<RegisterUserCommand, Result<Guid>>.Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {

            var email = new Email(request.Email);
            var userExists = await _userRepository.IsUserExits(email);
            if (userExists)
            {
                return Result.Failure<Guid>(UserErrors.EmailExist);
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password!);

            var user = User.Create(
               new Nombre(request.Nombre),
               new Apellido(request.Apellidos),
               new Email(request.Email),
               new PasswordHash(passwordHash));

            _userRepository.Add(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(user.Id!.Value);

        }
    }

}
