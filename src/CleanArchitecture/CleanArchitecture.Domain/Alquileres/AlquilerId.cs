
namespace CleanArchitecture.Domain.Alquileres
{
   public record AlquilerId(Guid id)
    {
        public static AlquilerId New() => new (Guid.NewGuid());
    }
}
