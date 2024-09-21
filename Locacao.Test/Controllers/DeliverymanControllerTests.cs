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
        public async Task Post_ReturnsCreatedAtActionResult_WithDeliverymanDto()
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
            Assert.Equal("Get", createdAtActionResult.ActionName);
            Assert.Equal(deliverymanDto.Id, createdAtActionResult.RouteValues["id"]);
            Assert.Equal(deliverymanDto, createdAtActionResult.Value);
        }

        [Fact]
        public async Task Get_ReturnsOkResult_WithListOfDeliverymanDtos()
        {
            // Arrange
            var deliverymen = new List<Deliveryman> { new Deliveryman(), new Deliveryman() };
            var deliverymanDtos = new List<DeliverymanDto> { new DeliverymanDto(), new DeliverymanDto() };
            _mockServices.Setup(s => s.GetAllAsync()).ReturnsAsync(deliverymen);
            _mockMapper.Setup(m => m.Map<IEnumerable<DeliverymanDto>>(deliverymen)).Returns(deliverymanDtos);

            // Act
            var result = await _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<DeliverymanDto>>(okResult.Value);
            Assert.Equal(deliverymanDtos.Count, returnValue.Count);
        }

        [Fact]
        public async Task GetById_ReturnsOkResult_WithDeliverymanDto()
        {
            // Arrange
            var id = Guid.NewGuid();
            var deliveryman = new Deliveryman();
            var deliverymanDto = new DeliverymanDto();
            _mockServices.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(deliveryman);
            _mockMapper.Setup(m => m.Map<DeliverymanDto>(deliveryman)).Returns(deliverymanDto);

            // Act
            var result = await _controller.Get(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(deliverymanDto, okResult.Value);
        }

        [Fact]
        public async Task Put_ReturnsOkResult_WithUpdatedDeliverymanDto()
        {
            // Arrange
            var id = Guid.NewGuid();
            var deliverymanDto = new DeliverymanDto();
            var deliveryman = new Deliveryman();
            _mockMapper.Setup(m => m.Map<Deliveryman>(deliverymanDto)).Returns(deliveryman);
            _mockMapper.Setup(m => m.Map<DeliverymanDto>(deliveryman)).Returns(deliverymanDto);
            _mockServices.Setup(s => s.UpdateAsync(deliveryman)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Put(id, deliverymanDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(deliverymanDto, okResult.Value);
        }

        [Fact]
        public async Task Delete_ReturnsNoContentResult()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockServices.Setup(s => s.DeleteAsync(id)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(id);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}