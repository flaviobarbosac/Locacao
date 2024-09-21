using Locacao.Domain.Model;
using Locacao.Repository.Interface;
using Locacao.Services;
using Moq;

namespace Locacao.Tests.Services
{
    public class MotorcycleRegistrationEventServicesTests
    {
        private readonly Mock<IBaseRepository<MotorcycleRegistrationEvent>> _mockRepository;
        private readonly BaseServices<MotorcycleRegistrationEvent> _service;

        public MotorcycleRegistrationEventServicesTests()
        {
            _mockRepository = new Mock<IBaseRepository<MotorcycleRegistrationEvent>>();
            _service = new BaseServices<MotorcycleRegistrationEvent>(_mockRepository.Object);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnEvent_WhenEventExists()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var registrationEvent = new MotorcycleRegistrationEvent { Id = eventId, CreationDate = DateTime.UtcNow, MotorcycleId = Guid.NewGuid() };
            _mockRepository.Setup(repo => repo.GetByIdAsync(eventId)).ReturnsAsync(registrationEvent);

            // Act
            var result = await _service.GetByIdAsync(eventId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(eventId, result.Id);
            Assert.Equal(registrationEvent.CreationDate, result.CreationDate);
            Assert.Equal(registrationEvent.MotorcycleId, result.MotorcycleId);
            _mockRepository.Verify(repo => repo.GetByIdAsync(eventId), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllEvents()
        {
            // Arrange
            var events = new List<MotorcycleRegistrationEvent>
            {
                new MotorcycleRegistrationEvent { Id = Guid.NewGuid(), CreationDate = DateTime.UtcNow, MotorcycleId = Guid.NewGuid() },
                new MotorcycleRegistrationEvent { Id = Guid.NewGuid(), CreationDate = DateTime.UtcNow.AddDays(-1), MotorcycleId = Guid.NewGuid() }
            };
            _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(events);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, e => e.Id == events[0].Id);
            Assert.Contains(result, e => e.Id == events[1].Id);
            _mockRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task AddAsync_ShouldAddEvent_AndSetCreationDate()
        {
            // Arrange
            var registrationEvent = new MotorcycleRegistrationEvent { CreationDate = DateTime.UtcNow, MotorcycleId = Guid.NewGuid() };

            // Act
            await _service.AddAsync(registrationEvent);

            // Assert
            Assert.NotEqual(default, registrationEvent.CreationDate);
            Assert.Equal(DateTimeKind.Utc, registrationEvent.CreationDate.Kind);
            _mockRepository.Verify(repo => repo.AddAsync(registrationEvent), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateEvent_AndSetModificationDate()
        {
            // Arrange
            var registrationEvent = new MotorcycleRegistrationEvent { Id = Guid.NewGuid(), CreationDate = DateTime.UtcNow, MotorcycleId = Guid.NewGuid() };

            // Act
            await _service.UpdateAsync(registrationEvent);

            // Assert
            Assert.NotEqual(default, registrationEvent.ModificationDate);
            Assert.Equal(DateTimeKind.Utc, registrationEvent.ModificationDate.Kind);
            _mockRepository.Verify(repo => repo.UpdateAsync(registrationEvent), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteEvent()
        {
            // Arrange
            var eventId = Guid.NewGuid();

            // Act
            await _service.DeleteAsync(eventId);

            // Assert
            _mockRepository.Verify(repo => repo.DeleteAsync(eventId), Times.Once);
        }
    }
}