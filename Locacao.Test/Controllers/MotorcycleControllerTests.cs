using AutoMapper;
using Locacao.Controllers;
using Locacao.Domain.Model;
using Locacao.Dto;
using Locacao.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Locacao.Test.Controllers
{
    public class MotorcycleControllerTests
    {
        private readonly Mock<IMotorcycleService> _mockServices;
        private readonly Mock<IMapper> _mockMapper;
        private readonly MotorcycleController _controller;

        public MotorcycleControllerTests()
        {
            _mockServices = new Mock<IMotorcycleService>();
            _mockMapper = new Mock<IMapper>();
            _controller = new MotorcycleController(_mockServices.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Post_ReturnsCreatedAtActionResult_WhenModelIsValid()
        {
            // Arrange
            var motorcycleDto = new MotorcycleDto { Id = Guid.NewGuid() };
            var motorcycle = new Motorcycle();
            _mockMapper.Setup(m => m.Map<Motorcycle>(motorcycleDto)).Returns(motorcycle);
            _mockMapper.Setup(m => m.Map<MotorcycleDto>(motorcycle)).Returns(motorcycleDto);
            _mockServices.Setup(s => s.AddAsync(motorcycle)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Post(motorcycleDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(nameof(MotorcycleController.Get), createdAtActionResult.ActionName);
            Assert.Equal(motorcycleDto.Id, createdAtActionResult.RouteValues["id"]);
            Assert.Equal(motorcycleDto, createdAtActionResult.Value);
        }

        [Fact]
        public async Task Get_ReturnsOkObjectResult_WithListOfMotorcycles()
        {
            // Arrange
            var motorcycles = new List<Motorcycle> { new Motorcycle(), new Motorcycle() };
            var motorcycleDtos = new List<MotorcycleDto> { new MotorcycleDto(), new MotorcycleDto() };
            _mockServices.Setup(s => s.GetAllAsync()).ReturnsAsync(motorcycles);
            _mockMapper.Setup(m => m.Map<IEnumerable<MotorcycleDto>>(motorcycles)).Returns(motorcycleDtos);

            // Act
            var result = await _controller.Get();

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<MotorcycleDto>>(okObjectResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task Get_ReturnsNotFound_WhenMotorcycleDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockServices.Setup(s => s.GetByIdAsync(id)).ReturnsAsync((Motorcycle)null);

            // Act
            var result = await _controller.Get(id);

            // Assert
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var errorResponse = Assert.IsType<ErrorResponse>(notFoundObjectResult.Value);
            Assert.Equal("Motorcycle not found", errorResponse.Message);
        }

        [Fact]
        public async Task Put_ReturnsOkObjectResult_WhenUpdateIsSuccessful()
        {
            // Arrange
            var id = Guid.NewGuid();
            var motorcycleDto = new MotorcycleDto { Id = id };
            var motorcycle = new Motorcycle { Id = id };
            _mockServices.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(motorcycle);
            _mockMapper.Setup(m => m.Map<Motorcycle>(motorcycleDto)).Returns(motorcycle);
            _mockMapper.Setup(m => m.Map<MotorcycleDto>(motorcycle)).Returns(motorcycleDto);
            _mockServices.Setup(s => s.UpdateAsync(motorcycle)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Put(id, motorcycleDto);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(motorcycleDto, okObjectResult.Value);
        }

        [Fact]
        public async Task Put_ReturnsNotFound_WhenMotorcycleDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();
            var motorcycleDto = new MotorcycleDto { Id = id };
            _mockServices.Setup(s => s.GetByIdAsync(id)).ReturnsAsync((Motorcycle)null);

            // Act
            var result = await _controller.Put(id, motorcycleDto);

            // Assert
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var errorResponse = Assert.IsType<ErrorResponse>(notFoundObjectResult.Value);
            Assert.Equal("Motorcycle not found", errorResponse.Message);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent_WhenDeletionIsSuccessful()
        {
            // Arrange
            var id = Guid.NewGuid();
            var motorcycle = new Motorcycle { Id = id };
            _mockServices.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(motorcycle);
            _mockServices.Setup(s => s.DeleteAsync(id)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(id);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenMotorcycleDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockServices.Setup(s => s.GetByIdAsync(id)).ReturnsAsync((Motorcycle)null);

            // Act
            var result = await _controller.Delete(id);

            // Assert
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            var errorResponse = Assert.IsType<ErrorResponse>(notFoundObjectResult.Value);
            Assert.Equal("Motorcycle not found", errorResponse.Message);
        }
    }
}