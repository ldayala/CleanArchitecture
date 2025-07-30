
using CleanArchitecture.Domain.Shared;
using CleanArchitecture.Domain.Vehiculos;

namespace CleanArchitecture.Domain.UnitTest.Vehiculos
{
    internal static class VehiculoMock
    {
        public static Vehiculo Create(Moneda precio, Moneda? mantenimiento) =>
            new(
                VehiculoId.New(),
                new Modelo("Civic"),
                new Vin("489489489"),
                precio,
                mantenimiento ?? Moneda.Zero(precio.TipoMoneda),
                DateTime.UtcNow.AddYears(-1), //para indicar que la fehca es del año pasado
                [],
                new Direccion("Cuba", "cuab", "granma", "guisa", "luis milanes"));
    }
}
