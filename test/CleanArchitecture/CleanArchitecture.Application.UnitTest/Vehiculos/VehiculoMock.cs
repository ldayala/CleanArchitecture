using CleanArchitecture.Domain.Shared;
using CleanArchitecture.Domain.Vehiculos;

namespace CleanArchitecture.Application.UnitTest.Vehiculos;

internal static class VehiculoMock
{
    public static Vehiculo Create() =>
        new(
            VehiculoId.New(),
            new Modelo("Civic"),
            new Vin("489489489"),
           new Moneda(10.2m, TipoMoneda.Usd),
            Moneda.Zero(TipoMoneda.Usd),
            DateTime.UtcNow,
            [],
            new Direccion("Cuba", "cuab", "granma", "guisa", "luis milanes"));
}
