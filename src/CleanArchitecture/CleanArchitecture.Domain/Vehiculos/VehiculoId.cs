
namespace CleanArchitecture.Domain.Vehiculos
{
    public record VehiculoId
    {
        public Guid Value { get; init; }
        public VehiculoId(Guid value)
        {
            Value = value;
        }
        public static VehiculoId New() => new(Guid.NewGuid());    
    }
}
