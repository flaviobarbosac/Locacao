using AutoMapper;
using Locacao.Controllers;
using Locacao.Domain.Model;
using Locacao.Dto;
using Locacao.Services.Inteface;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Locacao.Test.Controllers
{
    public class DeliverymanControllerTests
    {
        private readonly Mock<IBaseServices<Deliveryman>> _mockServices;
        private readonly Mock<IMapper> _mockMapper;
        private readonly DeliverymanController _controller;

        public DeliverymanControllerTests()
        {
            _mockServices = new Mock<IBaseServices<Deliveryman>>();
            _mockMapper = new Mock<IMapper>();
            _controller = new DeliverymanController(_mockServices.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Post_ReturnsCreatedAtActionResult_WhenModelIsValid()
        {
            // Arrange
            var deliverymanDto = new DeliverymanDto { Id = Guid.NewGuid() };
            var deliveryman = new Deliveryman();
            _mockMapper.Setup(m => m.Map<Deliveryman>(deliverymanDto)).Returns(deliveryman);
            _mockMapper.Setup(m => m.Map<DeliverymanDto>(deliveryman)).Returns(deliverymanDto);
            _mockServices.Setup(s => s.AddAsync(deliveryman)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Post(deliverymanDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(nameof(DeliverymanController.Get), createdAtActionResult.ActionName);
            Assert.Equal(deliverymanDto.Id, createdAtActionResult.RouteValues["id"]);
            Assert.Equal(deliverymanDto, createdAtActionResult.Value);
        }

        [Fact]
        public async Task Get_ReturnsOkObjectResult_WithListOfDeliverymen()
        {
            // Arrange
            var deliverymen = new List<Deliveryman> { new Deliveryman(), new Deliveryman() };
            var deliverymenDtos = new List<DeliverymanDto> { new DeliverymanDto(), new DeliverymanDto() };
            _mockServices.Setup(s => s.GetAllAsync()).ReturnsAsync(deliverymen);
            _mockMapper.Setup(m => m.Map<IEnumerable<DeliverymanDto>>(deliverymen)).Returns(deliverymenDtos);

            // Act
            var result = await _controller.Get();

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<DeliverymanDto>>(okObjectResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task Get_ReturnsNotFound_WhenDeliverymanDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockServices.Setup(s => s.GetByIdAsync(id)).ReturnsAsync((Deliveryman)null);

            // Act
            var result = await _controller.Get(id);

            // Assert
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var errorResponse = Assert.IsType<ErrorResponse>(notFoundObjectResult.Value);
            Assert.Equal("Deliveryman not found", errorResponse.Message);
        }

        [Fact]
        public async Task Get_ReturnsOkObjectResult_WhenDeliverymanExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            var deliveryman = new Deliveryman { Id = id };
            var deliverymanDto = new DeliverymanDto { Id = id };
            _mockServices.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(deliveryman);
            _mockMapper.Setup(m => m.Map<DeliverymanDto>(deliveryman)).Returns(deliverymanDto);

            // Act
            var result = await _controller.Get(id);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(deliverymanDto, okObjectResult.Value);
        }

        [Fact]
        public async Task Put_ReturnsOkObjectResult_WhenUpdateIsSuccessful()
        {
            // Arrange
            var id = Guid.NewGuid();
            var deliverymanDto = new DeliverymanDto { Id = id };
            var deliveryman = new Deliveryman { Id = id };
            _mockServices.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(deliveryman);
            _mockMapper.Setup(m => m.Map<Deliveryman>(deliverymanDto)).Returns(deliveryman);
            _mockMapper.Setup(m => m.Map<DeliverymanDto>(deliveryman)).Returns(deliverymanDto);
            _mockServices.Setup(s => s.UpdateAsync(deliveryman)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Put(id, deliverymanDto);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(deliverymanDto, okObjectResult.Value);
        }

        [Fact]
        public async Task Put_ReturnsNotFound_WhenDeliverymanDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();
            var deliverymanDto = new DeliverymanDto { Id = id };
            _mockServices.Setup(s => s.GetByIdAsync(id)).ReturnsAsync((Deliveryman)null);

            // Act
            var result = await _controller.Put(id, deliverymanDto);

            // Assert
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var errorResponse = Assert.IsType<ErrorResponse>(notFoundObjectResult.Value);
            Assert.Equal("Deliveryman not found", errorResponse.Message);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent_WhenDeletionIsSuccessful()
        {
            // Arrange
            var id = Guid.NewGuid();
            var deliveryman = new Deliveryman { Id = id };
            _mockServices.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(deliveryman);
            _mockServices.Setup(s => s.DeleteAsync(id)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(id);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenDeliverymanDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockServices.Setup(s => s.GetByIdAsync(id)).ReturnsAsync((Deliveryman)null);

            // Act
            var result = await _controller.Delete(id);

            // Assert
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            var errorResponse = Assert.IsType<ErrorResponse>(notFoundObjectResult.Value);
            Assert.Equal("Deliveryman not found", errorResponse.Message);
        }
    }
}