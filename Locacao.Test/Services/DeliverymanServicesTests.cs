using Locacao.Domain.Model;
using Locacao.Repository.Interface;
using Locacao.Services;
using Moq;

namespace Locacao.Tests.Services
{
    public class DeliverymanServicesTests
    {
        private readonly Mock<IBaseRepository<Deliveryman>> _mockRepository;
        private readonly BaseServices<Deliveryman> _service;

        public DeliverymanServicesTests()
        {
            _mockRepository = new Mock<IBaseRepository<Deliveryman>>();
            _service = new BaseServices<Deliveryman>(_mockRepository.Object);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnDeliveryman_WhenDeliverymanExists()
        {
            // Arrange
            var deliverymanId = Guid.NewGuid();
            var deliveryman = new Deliveryman { Id = deliverymanId, Name = "John Doe" };
            _mockRepository.Setup(repo => repo.GetByIdAsync(deliverymanId)).ReturnsAsync(deliveryman);

            // Act
            var result = await _service.GetByIdAsync(deliverymanId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(deliverymanId, result.Id);
            Assert.Equal("John Doe", result.Name);
            _mockRepository.Verify(repo => repo.GetByIdAsync(deliverymanId), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllDeliverymen()
        {
            // Arrange
            var deliverymen = new List<Deliveryman>
            {
                new Deliveryman { Id = Guid.NewGuid(), Name = "John Doe" },
                new Deliveryman { Id = Guid.NewGuid(), Name = "Jane Smith" }
            };
            _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(deliverymen);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, d => d.Name == "John Doe");
            Assert.Contains(result, d => d.Name == "Jane Smith");
            _mockRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task AddAsync_ShouldAddDeliveryman_AndSetCreationDate()
        {
            // Arrange
            var deliveryman = new Deliveryman { Name = "New Deliveryman" };

            // Act
            await _service.AddAsync(deliveryman);

            // Assert
            Assert.NotEqual(default, deliveryman.CreationDate);
            Assert.Equal(DateTimeKind.Utc, deliveryman.CreationDate.Kind);
            _mockRepository.Verify(repo => repo.AddAsync(deliveryman), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateDeliveryman_AndSetModificationDate()
        {
            // Arrange
            var deliveryman = new Deliveryman { Id = Guid.NewGuid(), Name = "Updated Deliveryman" };

            // Act
            await _service.UpdateAsync(deliveryman);

            // Assert
            Assert.NotEqual(default, deliveryman.ModificationDate);
            Assert.Equal(DateTimeKind.Utc, deliveryman.ModificationDate.Kind);
            _mockRepository.Verify(repo => repo.UpdateAsync(deliveryman), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteDeliveryman()
        {
            // Arrange
            var deliverymanId = Guid.NewGuid();

            // Act
            await _service.DeleteAsync(deliverymanId);

            // Assert
            _mockRepository.Verify(repo => repo.DeleteAsync(deliverymanId), Times.Once);
        }
    }
}