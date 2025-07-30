using CleanArchitecture.Domain.Users;

namespace CleanArchitecture.Domain.UnitTest.Users
{
    internal static class UserMock
    {
        public static readonly Nombre Nombre = new("ALfonso");
        public static readonly Apellido Apellido = new("Gonzalez");
        public static readonly Email Email = new("alfonso@cu.cu");
        public static readonly PasswordHash PasswordHash = new("12345678");
    }
}
