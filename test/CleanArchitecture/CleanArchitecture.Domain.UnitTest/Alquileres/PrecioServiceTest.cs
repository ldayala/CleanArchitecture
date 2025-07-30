using CleanArchitecture.Domain.Alquileres;
using CleanArchitecture.Domain.Shared;
using CleanArchitecture.Domain.UnitTest.Vehiculos;
using CleanArchitecture.Domain.Vehiculos;
using FluentAssertions;
using Xunit;

namespace CleanArchitecture.Domain.UnitTest.Alquileres
{
    public class PrecioServiceTest
    {
        [Fact]
        public void CalcularPrecio_Should_ReturnCorrectPrecioTotal()
        {
            //arrange
            var precio= new Moneda(10.0m, TipoMoneda.Usd);
            var periodo = DateRange.Create(
                new DateOnly(2024,1,1), new DateOnly(2025,1,10)
            );

            var expectedPrecioTotal = new Moneda(precio.Monto*periodo.CantidadDias,precio.TipoMoneda);

            var vehiculo = VehiculoMock.Create(precio, null);
            var precioService = new PrecioService();

            //Act
            var precioDetalle = precioService.CalcularPrecio(vehiculo, periodo);

            //Assert
            precioDetalle.PrecioTotal.Should().Be(expectedPrecioTotal);
        }
        [Fact]
        public void CalcularPrecio_Should_ReturnCorrectPrecioTotal_WithMantenimiento()
        {
            //arrange
            var precio= new Moneda(10.0m, TipoMoneda.Usd);
            var mantenimiento = new Moneda(5.0m, TipoMoneda.Usd);
            var periodo = DateRange.Create(
                new DateOnly(2024,1,1), new DateOnly(2025,1,10)
            );
            var expectedPrecioTotal = new Moneda(precio.Monto*periodo.CantidadDias + mantenimiento.Monto, precio.TipoMoneda);
            var vehiculo = VehiculoMock.Create(precio, mantenimiento);
            var precioService = new PrecioService();
            //Act
            var precioDetalle = precioService.CalcularPrecio(vehiculo, periodo);
            //Assert
            precioDetalle.PrecioTotal.Should().Be(expectedPrecioTotal);
        }

        [Fact]
        public void CalcularPrecio_Should_ReturnCorrectPrecioTotal_WithAccesorios()
        {
            //arrange
            var precio= new Moneda(10.0m, TipoMoneda.Usd);
            var periodo = DateRange.Create(
                new DateOnly(2024,1,1), new DateOnly(2025,1,10)
            );
            var expectedPrecioTotal = new Moneda(precio.Monto*periodo.CantidadDias + 0.05m*precio.Monto*periodo.CantidadDias, precio.TipoMoneda);
            var vehiculo = VehiculoMock.Create(precio, null);
            vehiculo.Accesorios.Add(Accesorio.AppleCar);
            var precioService = new PrecioService();
            //Act
            var precioDetalle = precioService.CalcularPrecio(vehiculo, periodo);
            //Assert
            precioDetalle.PrecioTotal.Should().Be(expectedPrecioTotal);
        }
    }
}
