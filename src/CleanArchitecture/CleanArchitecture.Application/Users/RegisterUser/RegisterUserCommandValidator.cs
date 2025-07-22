using FluentValidation;

namespace CleanArchitecture.Application.Users.RegisterUser
{
    internal sealed class RegisterUserCommandValidator: AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();
            RuleFor(x => x.Nombre)
                .NotEmpty()
                .MaximumLength(50);
            RuleFor(x => x.Apellidos)
                .NotEmpty()
                .MaximumLength(50);
            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(6);
        }
    }
   
}
