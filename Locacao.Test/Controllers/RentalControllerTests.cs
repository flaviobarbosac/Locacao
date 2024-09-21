using AutoMapper;
using Locacao.Controllers;
using Locacao.Domain.Model;
using Locacao.Dto;
using Locacao.Services.Inteface;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Locacao.Test.Controllers
{
    public class RentalControllerTests
    {
        private readonly Mock<IBaseServices<Rental>> _mockServices;
        private readonly Mock<IMapper> _mockMapper;
        private readonly RentalController _controller;

        public RentalControllerTests()
        {
            _mockServices = new Mock<IBaseServices<Rental>>();
            _mockMapper = new Mock<IMapper>();
            _controller = new RentalController(_mockServices.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Post_ReturnsCreatedAtActionResult_WithRentalDto()
        {
            // Arrange
            var rentalDto = new RentalDto { Id = Guid.NewGuid() };
            var rental = new Rental();
            _mockMapper.Setup(m => m.Map<Rental>(rentalDto)).Returns(rental);
            _mockMapper.Setup(m => m.Map<RentalDto>(rental)).Returns(rentalDto);
            _mockServices.Setup(s => s.AddAsync(rental)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Post(rentalDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal("Get", createdAtActionResult.ActionName);
            Assert.Equal(rentalDto.Id, createdAtActionResult.RouteValues["id"]);
            Assert.Equal(rentalDto, createdAtActionResult.Value);
        }

        [Fact]
        public async Task Get_ReturnsOkResult_WithListOfRentalDtos()
        {
            // Arrange
            var rentals = new List<Rental> { new Rental(), new Rental() };
            var rentalDtos = new List<RentalDto> { new RentalDto(), new RentalDto() };
            _mockServices.Setup(s => s.GetAllAsync()).ReturnsAsync(rentals);
            _mockMapper.Setup(m => m.Map<IEnumerable<RentalDto>>(rentals)).Returns(rentalDtos);

            // Act
            var result = await _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<RentalDto>>(okResult.Value);
            Assert.Equal(rentalDtos.Count, returnValue.Count);
        }

        [Fact]
        public async Task GetById_ReturnsOkResult_WithRentalDto()
        {
            // Arrange
            var id = Guid.NewGuid();
            var rental = new Rental();
            var rentalDto = new RentalDto();
            _mockServices.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(rental);
            _mockMapper.Setup(m => m.Map<IEnumerable<RentalDto>>(rental)).Returns(new List<RentalDto> { rentalDto });

            // Act
            var result = await _controller.Get(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<RentalDto>>(okResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task Put_ReturnsOkResult_WithUpdatedRentalDto()
        {
            // Arrange
            var id = Guid.NewGuid();
            var rentalDto = new RentalDto();
            var rental = new Rental();
            _mockMapper.Setup(m => m.Map<Rental>(rentalDto)).Returns(rental);
            _mockMapper.Setup(m => m.Map<RentalDto>(rental)).Returns(rentalDto);
            _mockServices.Setup(s => s.UpdateAsync(rental)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Put(id, rentalDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(rentalDto, okResult.Value);
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