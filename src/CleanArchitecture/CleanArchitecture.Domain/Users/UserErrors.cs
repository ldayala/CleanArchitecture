using CleanArchitecture.Domain.Abstractions;

namespace CleanArchitecture.Domain.Users;


public static class UserErrors
{

    public static Error NotFound = new(
        "User.Found",
        "No existe el usuario buscado por este id"
    );

    public static Error InvalidCredentials = new(
        "User.InvalidCredentials",
        "Las credenciales son incorrectas"
    );

    public static Error EmailExist = new(
        "User.EmailExist",
        "There is a user with that email"
    );


}