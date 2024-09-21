using AutoMapper;
using Locacao.Controllers;
using Locacao.Domain.Model;
using Locacao.Dto;
using Locacao.Services.Inteface;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Locacao.Test.Controllers
{
    public class MotorcycleControllerTests
    {
        private readonly Mock<IBaseServices<Motorcycle>> _mockServices;
        private readonly Mock<IMapper> _mockMapper;
        private readonly MotorcycleController _controller;

        public MotorcycleControllerTests()
        {
            _mockServices = new Mock<IBaseServices<Motorcycle>>();
            _mockMapper = new Mock<IMapper>();
            _controller = new MotorcycleController(_mockServices.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Post_ReturnsCreatedAtActionResult_WithMotorcycleDto()
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
            Assert.Equal("Get", createdAtActionResult.ActionName);
            Assert.Equal(motorcycleDto.Id, createdAtActionResult.RouteValues["id"]);
            Assert.Equal(motorcycleDto, createdAtActionResult.Value);
        }

        [Fact]
        public async Task Get_ReturnsOkResult_WithListOfMotorcycleDtos()
        {
            // Arrange
            var motorcycles = new List<Motorcycle> { new Motorcycle(), new Motorcycle() };
            var motorcycleDtos = new List<MotorcycleDto> { new MotorcycleDto(), new MotorcycleDto() };
            _mockServices.Setup(s => s.GetAllAsync()).ReturnsAsync(motorcycles);
            _mockMapper.Setup(m => m.Map<IEnumerable<MotorcycleDto>>(motorcycles)).Returns(motorcycleDtos);

            // Act
            var result = await _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<MotorcycleDto>>(okResult.Value);
            Assert.Equal(motorcycleDtos.Count, returnValue.Count);
        }

        [Fact]
        public async Task GetById_ReturnsOkResult_WithMotorcycleDto()
        {
            // Arrange
            var id = Guid.NewGuid();
            var motorcycle = new Motorcycle();
            var motorcycleDto = new MotorcycleDto();
            _mockServices.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(motorcycle);
            _mockMapper.Setup(m => m.Map<MotorcycleDto>(motorcycle)).Returns(motorcycleDto);

            // Act
            var result = await _controller.Get(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(motorcycleDto, okResult.Value);
        }

        [Fact]
        public async Task Put_ReturnsOkResult_WithUpdatedMotorcycleDto()
        {
            // Arrange
            var id = Guid.NewGuid();
            var motorcycleDto = new MotorcycleDto();
            var motorcycle = new Motorcycle();
            _mockMapper.Setup(m => m.Map<Motorcycle>(motorcycleDto)).Returns(motorcycle);
            _mockMapper.Setup(m => m.Map<MotorcycleDto>(motorcycle)).Returns(motorcycleDto);
            _mockServices.Setup(s => s.UpdateAsync(motorcycle)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Put(id, motorcycleDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(motorcycleDto, okResult.Value);
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