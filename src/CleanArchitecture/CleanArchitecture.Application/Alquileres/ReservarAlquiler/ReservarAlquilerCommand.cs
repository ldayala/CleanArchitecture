using CleanArchitecture.Application.Abstractions.Messaging;
using CleanArchitecture.Domain.Alquileres;
using CleanArchitecture.Domain.Users;
using CleanArchitecture.Domain.Vehiculos;

namespace CleanArchitecture.Application.Alquileres.ReservarAlquiler;

public record ReservarAlquilerCommand(
    VehiculoId VehiculoId,
    UserId UserId,
    DateOnly FechaInicio,
    DateOnly FechaFin

) : ICommand<AlquilerId>;