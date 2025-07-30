using CleanArchitecture.Domain.Users;
using CleanArchitecture.Domain.UnitTest.Infrastructure;
using FluentAssertions;
using Xunit;
using CleanArchitecture.Domain.Users.Events;
using CleanArchitecture.Domain.Roles;

namespace CleanArchitecture.Domain.UnitTest.Users
{
    public class UserTest:BaseTest
    {
        [Fact]
        //Evaluamos que el constructor de la clase User establece las propiedades correctamente.
        public void Create_Should_SetPrepertyValues()
        {
            // Arrange : significa preparar el escenario para la prueba. -->vamos a crear un Mock File -> UserMock.cs

            //Act : significa ejecutar la acción que se va a probar.
            var user = User.Create(
                UserMock.Nombre,
                UserMock.Apellido,
                UserMock.Email,
                UserMock.PasswordHash
            );
            //Assert : significa verificar que el resultado de la acción es el esperado.
            Assert.NotNull(user);
            user.Nombre.Should().Be(UserMock.Nombre);
            user.Apellido.Should().Be(UserMock.Apellido);
            user.Email.Should().Be(UserMock.Email);
            user.PasswordHash.Should().Be(UserMock.PasswordHash);
           
        }

        [Fact]
        public void Create_Should_RaiseUserCreatedDomainEvent()
        {

            var user = User.Create(
                UserMock.Nombre,
                UserMock.Apellido,
                UserMock.Email,
                UserMock.PasswordHash
            );

            // Act
            /*  var domainEvent = user
                .GetDomainEvents().OfType<UserCreatedDomainEvent>().SingleOrDefault();*/
            var domainEvent = AssertDomainEventWasPublished<UserCreatedDomainEvent>(user);
            domainEvent!.UserId.Should().Be(user.Id);

        }

        [Fact]
        public void Create_Should_ContainRoleCliente()
        {
            // Arrange
            var user = User.Create(
                UserMock.Nombre,
                UserMock.Apellido,
                UserMock.Email,
                UserMock.PasswordHash
            );
            // Act & Assert
            user.Roles.Should().ContainSingle(role => role == Role.Cliente);
        }


    }
}
