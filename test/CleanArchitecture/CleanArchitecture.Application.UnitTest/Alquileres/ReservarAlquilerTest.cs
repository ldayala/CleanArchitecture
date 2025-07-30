using CleanArchitecture.Application.Abstractions.Clock;
using CleanArchitecture.Application.Alquileres.ReservarAlquiler;
using CleanArchitecture.Application.UnitTest.Users;
using CleanArchitecture.Application.UnitTest.Vehiculos;
using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Alquileres;
using CleanArchitecture.Domain.Users;
using CleanArchitecture.Domain.Vehiculos;
using FluentAssertions;
using NSubstitute;
using Xunit;
namespace CleanArchitecture.Application.UnitTest.Alquileres
{
    public class ReservarAlquilerTest
    {
        private readonly ReservarAlquilerCommandHandler _handlerMock;
        private readonly IUserRepository _userRepositoryMock;
        private readonly IVehiculoRepository _vehiculoRepositoryMock;
        private readonly IAlquilerRepository _alquilerRepositoryMock;
        private readonly IUnitOfWork _unitOfWorkMock;
        private readonly DateTime UtcNow = DateTime.UtcNow;

        private readonly ReservarAlquilerCommand Command = new ReservarAlquilerCommand(
            VehiculoId.New(),
            UserId.New(),
            DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)),
            DateOnly.FromDateTime(DateTime.UtcNow.AddDays(2))
            );

        public ReservarAlquilerTest()
        {
            _userRepositoryMock = Substitute.For<IUserRepository>();
            _vehiculoRepositoryMock = Substitute.For<IVehiculoRepository>();
            _alquilerRepositoryMock = Substitute.For<IAlquilerRepository>();
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();
            IDateTimeProvider dateTimeProviderMock = Substitute.For<IDateTimeProvider>();
            dateTimeProviderMock.currentTime.Returns(UtcNow);

            _handlerMock = new ReservarAlquilerCommandHandler(
                _userRepositoryMock,
                _vehiculoRepositoryMock,
                _alquilerRepositoryMock,
                new PrecioService(),
                _unitOfWorkMock,
                dateTimeProviderMock
            );

        }

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenUserIsNull()
        {
            // Arrange
            /* _userRepositoryMock.GetByIdAsync(Arg.Any<UserId>(), Arg.Any<CancellationToken>())
                 .Returns(Task.FromResult<User?>(null));*/
            _userRepositoryMock.GetByIdAsync(Command.UserId, Arg.Any<CancellationToken>())
                 .Returns(Task.FromResult<User?>(null));
            // Act
            var result = await _handlerMock.Handle(Command, CancellationToken.None);
            // Assert
            Assert.Equal(UserErrors.NotFound, result.Error);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenVehiculoIsNull()
        {
            // Arrange
            _userRepositoryMock.GetByIdAsync(Command.UserId, Arg.Any<CancellationToken>())
                .Returns(UserMock.Create());

            _vehiculoRepositoryMock.GetByIdAsync(Command.VehiculoId, Arg.Any<CancellationToken>())
                .Returns(Task.FromResult<Vehiculo?>(null));

            // Act
            var resutl = await _handlerMock.Handle(Command, CancellationToken.None);

            // Assert
            resutl.Error.Should().Be(VehiculoErrors.NotFound);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenAlquilerIsOverlapping()
        {
            // Arrange
            var user = UserMock.Create();
            var vehiculo = VehiculoMock.Create();
            var duracion = DateRange.Create(Command.FechaInicio, Command.FechaFin);

            _userRepositoryMock.GetByIdAsync(Command.UserId, Arg.Any<CancellationToken>())
                .Returns(user);
            _vehiculoRepositoryMock.GetByIdAsync(Command.VehiculoId, Arg.Any<CancellationToken>())
                .Returns(vehiculo);

            _alquilerRepositoryMock.IsOverlappingAsync(vehiculo, duracion, Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(true));
            // Act
            var result = await _handlerMock.Handle(Command, CancellationToken.None);
            // Assert
            result.Error.Should().Be(AlquilerErrors.Overlap);
        }
    }
}
