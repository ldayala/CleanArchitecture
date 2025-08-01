using CleanArchitecture.Domain.Shared;
using CleanArchitecture.Domain.Vehiculos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Configurations;

internal sealed class VehiculoConfiguration : IEntityTypeConfiguration<Vehiculo>
{
    public void Configure(EntityTypeBuilder<Vehiculo> builder)
    {
       builder.ToTable("vehiculos");
       builder.HasKey(vehiculo => vehiculo.Id);
        //como es un object value , debemos convertirlo antes de guardarlo en la base de datos
        // y luego volver a convertirlo al leerlo de la base de datos
        builder.Property(vehiculo => vehiculo.Id)
            .HasConversion(
            vehiculoId => vehiculoId.Value,
            value => new VehiculoId(value)
        );

        builder.OwnsOne(vehiculo => vehiculo.Direccion);

       builder.Property(vehiculo => vehiculo.Modelo) 
        .HasMaxLength(200)
        .HasConversion(modelo => modelo!.Value, value => new Modelo(value));

       builder.Property(vehiculo => vehiculo.Vin)
        .HasMaxLength(500)
        .HasConversion(vin => vin!.Value, value => new Vin(value));

      
       builder.OwnsOne(vehiculo => vehiculo.Precio, priceBuilder => {
        priceBuilder.Property(moneda => moneda.TipoMoneda)
        .HasConversion(tipoMoneda => tipoMoneda.Codigo, codigo => TipoMoneda.FromCodigo(codigo!));
      });

       builder.OwnsOne(vehiculo => vehiculo.Mantenimiento, priceBuilder => {
        priceBuilder.Property(moneda => moneda.TipoMoneda)
        .HasConversion(tipoMoneda => tipoMoneda.Codigo, codigo => TipoMoneda.FromCodigo(codigo!));
      });

      builder.Property<uint>("Version").IsRowVersion();

    }
}