using Locacao.Domain.Model;
using Locacao.Repository.Interface;
using Locacao.Services;
using Moq;

namespace Locacao.Tests.Services
{
    public class MotorcycleServicesTests
    {
        private readonly Mock<IBaseRepository<Motorcycle>> _mockRepository;
        private readonly BaseServices<Motorcycle> _service;

        public MotorcycleServicesTests()
        {
            _mockRepository = new Mock<IBaseRepository<Motorcycle>>();
            _service = new BaseServices<Motorcycle>(_mockRepository.Object);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnMotorcycle_WhenMotorcycleExists()
        {
            // Arrange
            var motorcycleId = Guid.NewGuid();
            var motorcycle = new Motorcycle { Id = motorcycleId, Model = "Test Model" };
            _mockRepository.Setup(repo => repo.GetByIdAsync(motorcycleId)).ReturnsAsync(motorcycle);

            // Act
            var result = await _service.GetByIdAsync(motorcycleId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(motorcycleId, result.Id);
            Assert.Equal("Test Model", result.Model);
            _mockRepository.Verify(repo => repo.GetByIdAsync(motorcycleId), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllMotorcycles()
        {
            // Arrange
            var motorcycles = new List<Motorcycle>
            {
                new Motorcycle { Id = Guid.NewGuid(), Model = "Model 1" },
                new Motorcycle { Id = Guid.NewGuid(), Model = "Model 2" }
            };
            _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(motorcycles);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, m => m.Model == "Model 1");
            Assert.Contains(result, m => m.Model == "Model 2");
            _mockRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task AddAsync_ShouldAddMotorcycle_AndSetCreationDate()
        {
            // Arrange
            var motorcycle = new Motorcycle { Model = "New Model" };

            // Act
            await _service.AddAsync(motorcycle);

            // Assert
            Assert.NotEqual(default, motorcycle.CreationDate);
            Assert.Equal(DateTimeKind.Utc, motorcycle.CreationDate.Kind);
            _mockRepository.Verify(repo => repo.AddAsync(motorcycle), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateMotorcycle_AndSetModificationDate()
        {
            // Arrange
            var motorcycle = new Motorcycle { Id = Guid.NewGuid(), Model = "Updated Model" };

            // Act
            await _service.UpdateAsync(motorcycle);

            // Assert
            Assert.NotEqual(default, motorcycle.ModificationDate);
            Assert.Equal(DateTimeKind.Utc, motorcycle.ModificationDate.Kind);
            _mockRepository.Verify(repo => repo.UpdateAsync(motorcycle), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteMotorcycle()
        {
            // Arrange
            var motorcycleId = Guid.NewGuid();

            // Act
            await _service.DeleteAsync(motorcycleId);

            // Assert
            _mockRepository.Verify(repo => repo.DeleteAsync(motorcycleId), Times.Once);
        }
    }
}