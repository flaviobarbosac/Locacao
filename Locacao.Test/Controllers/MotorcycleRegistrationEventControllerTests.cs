using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Locacao.Controllers;
using Locacao.Domain.Model;
using Locacao.Dto;
using Locacao.Services.Inteface;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

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
        public async Task Post_ReturnsCreatedAtActionResult_WhenModelIsValid()
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
            Assert.Equal(nameof(MotorcycleRegistrationEventController.Get), createdAtActionResult.ActionName);
            Assert.Equal(eventDto.Id, createdAtActionResult.RouteValues["id"]);
            Assert.Equal(eventDto, createdAtActionResult.Value);
        }

        [Fact]
        public async Task Get_ReturnsOkObjectResult_WithListOfEvents()
        {
            // Arrange
            var events = new List<MotorcycleRegistrationEvent> { new MotorcycleRegistrationEvent(), new MotorcycleRegistrationEvent() };
            var eventDtos = new List<MotorcycleRegistrationEventDto> { new MotorcycleRegistrationEventDto(), new MotorcycleRegistrationEventDto() };
            _mockServices.Setup(s => s.GetAllAsync()).ReturnsAsync(events);
            _mockMapper.Setup(m => m.Map<IEnumerable<MotorcycleRegistrationEventDto>>(events)).Returns(eventDtos);

            // Act
            var result = await _controller.Get();

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<MotorcycleRegistrationEventDto>>(okObjectResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task Get_ReturnsNotFound_WhenEventDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockServices.Setup(s => s.GetByIdAsync(id)).ReturnsAsync((MotorcycleRegistrationEvent)null);

            // Act
            var result = await _controller.Get(id);

            // Assert
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var errorResponse = Assert.IsType<ErrorResponse>(notFoundObjectResult.Value);
            Assert.Equal("Motorcycle registration event not found", errorResponse.Message);
        }

        [Fact]
        public async Task Get_ReturnsOkObjectResult_WhenEventExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            var eventModel = new MotorcycleRegistrationEvent { Id = id };
            var eventDto = new MotorcycleRegistrationEventDto { Id = id };
            _mockServices.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(eventModel);
            _mockMapper.Setup(m => m.Map<MotorcycleRegistrationEventDto>(eventModel)).Returns(eventDto);

            // Act
            var result = await _controller.Get(id);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(eventDto, okObjectResult.Value);
        }

        [Fact]
        public async Task Put_ReturnsOkObjectResult_WhenUpdateIsSuccessful()
        {
            // Arrange
            var id = Guid.NewGuid();
            var eventDto = new MotorcycleRegistrationEventDto { Id = id };
            var eventModel = new MotorcycleRegistrationEvent { Id = id };
            _mockServices.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(eventModel);
            _mockMapper.Setup(m => m.Map<MotorcycleRegistrationEvent>(eventDto)).Returns(eventModel);
            _mockMapper.Setup(m => m.Map<MotorcycleRegistrationEventDto>(eventModel)).Returns(eventDto);
            _mockServices.Setup(s => s.UpdateAsync(eventModel)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Put(id, eventDto);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(eventDto, okObjectResult.Value);
        }

        [Fact]
        public async Task Put_ReturnsNotFound_WhenEventDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();
            var eventDto = new MotorcycleRegistrationEventDto { Id = id };
            _mockServices.Setup(s => s.GetByIdAsync(id)).ReturnsAsync((MotorcycleRegistrationEvent)null);

            // Act
            var result = await _controller.Put(id, eventDto);

            // Assert
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var errorResponse = Assert.IsType<ErrorResponse>(notFoundObjectResult.Value);
            Assert.Equal("Motorcycle registration event not found", errorResponse.Message);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent_WhenDeletionIsSuccessful()
        {
            // Arrange
            var id = Guid.NewGuid();
            var eventModel = new MotorcycleRegistrationEvent { Id = id };
            _mockServices.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(eventModel);
            _mockServices.Setup(s => s.DeleteAsync(id)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(id);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenEventDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockServices.Setup(s => s.GetByIdAsync(id)).ReturnsAsync((MotorcycleRegistrationEvent)null);

            // Act
            var result = await _controller.Delete(id);

            // Assert
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            var errorResponse = Assert.IsType<ErrorResponse>(notFoundObjectResult.Value);
            Assert.Equal("Motorcycle registration event not found", errorResponse.Message);
        }
    }
}