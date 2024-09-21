using Locacao.Domain.Model;
using Locacao.Repository.Interface;
using Locacao.Services;
using Moq;

namespace Locacao.Tests.Services
{
    public class RentalServicesTests
    {
        private readonly Mock<IBaseRepository<Rental>> _mockRepository;
        private readonly BaseServices<Rental> _service;

        public RentalServicesTests()
        {
            _mockRepository = new Mock<IBaseRepository<Rental>>();
            _service = new BaseServices<Rental>(_mockRepository.Object);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnRental_WhenRentalExists()
        {
            // Arrange
            var rentalId = Guid.NewGuid();
            var rental = new Rental
            {
                Id = rentalId,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(7),
                MotorcycleId = Guid.NewGuid(),
                DeliverymanId = Guid.NewGuid()
            };
            _mockRepository.Setup(repo => repo.GetByIdAsync(rentalId)).ReturnsAsync(rental);

            // Act
            var result = await _service.GetByIdAsync(rentalId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(rentalId, result.Id);
            Assert.Equal(rental.StartDate, result.StartDate);
            Assert.Equal(rental.EndDate, result.EndDate);
            Assert.Equal(rental.MotorcycleId, result.MotorcycleId);
            Assert.Equal(rental.DeliverymanId, result.DeliverymanId);
            _mockRepository.Verify(repo => repo.GetByIdAsync(rentalId), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllRentals()
        {
            // Arrange
            var rentals = new List<Rental>
            {
                new Rental { Id = Guid.NewGuid(), StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddDays(7), MotorcycleId = Guid.NewGuid(), DeliverymanId = Guid.NewGuid() },
                new Rental { Id = Guid.NewGuid(), StartDate = DateTime.UtcNow.AddDays(1), EndDate = DateTime.UtcNow.AddDays(8), MotorcycleId = Guid.NewGuid(), DeliverymanId = Guid.NewGuid() }
            };
            _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(rentals);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, r => r.Id == rentals[0].Id);
            Assert.Contains(result, r => r.Id == rentals[1].Id);
            _mockRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task AddAsync_ShouldAddRental_AndSetCreationDate()
        {
            // Arrange
            var rental = new Rental
            {
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(7),
                MotorcycleId = Guid.NewGuid(),
                DeliverymanId = Guid.NewGuid()
            };

            // Act
            await _service.AddAsync(rental);

            // Assert
            Assert.NotEqual(default, rental.CreationDate);
            Assert.Equal(DateTimeKind.Utc, rental.CreationDate.Kind);
            _mockRepository.Verify(repo => repo.AddAsync(rental), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateRental_AndSetModificationDate()
        {
            // Arrange
            var rental = new Rental
            {
                Id = Guid.NewGuid(),
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(7),
                MotorcycleId = Guid.NewGuid(),
                DeliverymanId = Guid.NewGuid()
            };

            // Act
            await _service.UpdateAsync(rental);

            // Assert
            Assert.NotEqual(default, rental.ModificationDate);
            Assert.Equal(DateTimeKind.Utc, rental.ModificationDate.Kind);
            _mockRepository.Verify(repo => repo.UpdateAsync(rental), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteRental()
        {
            // Arrange
            var rentalId = Guid.NewGuid();

            // Act
            await _service.DeleteAsync(rentalId);

            // Assert
            _mockRepository.Verify(repo => repo.DeleteAsync(rentalId), Times.Once);
        }
    }
}