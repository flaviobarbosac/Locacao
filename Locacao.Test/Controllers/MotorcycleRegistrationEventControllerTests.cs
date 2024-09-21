using AutoMapper;
using Locacao.Controllers;
using Locacao.Domain.Model;
using Locacao.Dto;
using Locacao.Services.Inteface;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Locacao.Test.Controllers
{
    public class MotorcycleRegistrationEventControllerTests
    {
        private readonly Mock<IBaseServices<MotorcycleRegistrationEvent>> _mockServices;
        private readonly Mock<IMapper> _mockMapper;
        private readonly MotorcycleRegistrationEventController _controller;

        public MotorcycleRegistrationEventControllerTests()
        {
            _mockServices = new Mock<IBaseServices<MotorcycleRegistrationEvent>>();
            _mockMapper = new Mock<IMapper>();
            _controller = new MotorcycleRegistrationEventController(_mockServices.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Post_ReturnsCreatedAtActionResult_WithMotorcycleRegistrationEventDto()
        {
            // Arrange
            var eventDto = new MotorcycleRegistrationEventDto { Id = Guid.NewGuid() };
            var eventModel = new MotorcycleRegistrationEvent();
            _mockMapper.Setup(m => m.Map<MotorcycleRegistrationEvent>(eventDto)).Returns(eventModel);
            _mockMapper.Setup(m => m.Map<MotorcycleRegistrationEventDto>(eventModel)).Returns(eventDto);
            _mockServices.Setup(s => s.AddAsync(eventModel)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Post(eventDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal("Get", createdAtActionResult.ActionName);
            Assert.Equal(eventDto.Id, createdAtActionResult.RouteValues["id"]);
            Assert.Equal(eventDto, createdAtActionResult.Value);
        }

        [Fact]
        public async Task Get_ReturnsOkResult_WithListOfMotorcycleRegistrationEventDtos()
        {
            // Arrange
            var events = new List<MotorcycleRegistrationEvent> { new MotorcycleRegistrationEvent(), new MotorcycleRegistrationEvent() };
            var eventDtos = new List<MotorcycleRegistrationEventDto> { new MotorcycleRegistrationEventDto(), new MotorcycleRegistrationEventDto() };
            _mockServices.Setup(s => s.GetAllAsync()).ReturnsAsync(events);
            _mockMapper.Setup(m => m.Map<IEnumerable<MotorcycleRegistrationEventDto>>(events)).Returns(eventDtos);

            // Act
            var result = await _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<MotorcycleRegistrationEventDto>>(okResult.Value);
            Assert.Equal(eventDtos.Count, returnValue.Count);
        }

        [Fact]
        public async Task GetById_ReturnsOkResult_WithMotorcycleRegistrationEventDto()
        {
            // Arrange
            var id = Guid.NewGuid();
            var eventModel = new MotorcycleRegistrationEvent();
            var eventDto = new MotorcycleRegistrationEventDto();
            _mockServices.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(eventModel);
            _mockMapper.Setup(m => m.Map<MotorcycleRegistrationEventDto>(eventModel)).Returns(eventDto);

            // Act
            var result = await _controller.Get(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(eventDto, okResult.Value);
        }

        [Fact]
        public async Task Put_ReturnsOkResult_WithUpdatedMotorcycleRegistrationEventDto()
        {
            // Arrange
            var id = Guid.NewGuid();
            var eventDto = new MotorcycleRegistrationEventDto();
            var eventModel = new MotorcycleRegistrationEvent();
            _mockMapper.Setup(m => m.Map<MotorcycleRegistrationEvent>(eventDto)).Returns(eventModel);
            _mockMapper.Setup(m => m.Map<MotorcycleRegistrationEventDto>(eventModel)).Returns(eventDto);
            _mockServices.Setup(s => s.UpdateAsync(eventModel)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Put(id, eventDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(eventDto, okResult.Value);
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