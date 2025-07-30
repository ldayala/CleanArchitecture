using CleanArchitecture.Domain.Alquileres;
using CleanArchitecture.Domain.Alquileres.Events;
using CleanArchitecture.Domain.Shared;
using CleanArchitecture.Domain.UnitTest.Infrastructure;
using CleanArchitecture.Domain.UnitTest.Users;
using CleanArchitecture.Domain.UnitTest.Vehiculos;
using CleanArchitecture.Domain.Users;
using FluentAssertions;
using Xunit;

namespace CleanArchitecture.Domain.UnitTest.Alquileres
{
    public class AlquilerTest:BaseTest
    {
        [Fact]
        public void Reserva_Should_RaiseAlquilerReservaDomainEvent()
        {
            // Arrange
           var user= User.Create(UserMock.Nombre, UserMock.Apellido, UserMock.Email, UserMock.PasswordHash);

            var precio = new Moneda(10.0m, TipoMoneda.Usd);
            var duration= DateRange.Create(
                new DateOnly(2024, 1, 1), new DateOnly(2025, 1, 10)
            );
            var vehiculo = VehiculoMock.Create(precio,null);

            var percioService = new PrecioService();

            // Act
            var alquiler = Alquiler.Reservar(vehiculo,user.Id!,duration, DateTime.UtcNow, percioService);

          //Assert
            alquiler.Should().NotBeNull();
            alquiler.VehiculoId.Should().Be(vehiculo.Id);
            alquiler.UserId.Should().Be(user.Id);
            alquiler.Duracion.Should().Be(duration);
            alquiler.PrecioPorPeriodo!.Monto.Should().Be(precio.Monto*duration.CantidadDias);
            alquiler.Status.Should().Be(AlquilerStatus.Reservado);
            alquiler.FechaCreacion.Should().NotBeNull();
            
            // Check if the domain event was raised
            var domainEvents = AssertDomainEventWasPublished<AlquilerReservadoDomainEvent>(alquiler);

            domainEvents.AlquilerId.Should().Be(alquiler.Id);

        }
    }
}
