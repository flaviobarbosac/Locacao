using AutoMapper;
using Locacao.Controllers;
using Locacao.Domain.Enum;
using Locacao.Domain.Model;
using Locacao.Dto;
using Locacao.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Locacao.Tests
{
    public class RentalControllerTests
    {
        private readonly Mock<IRentalService> _mockRentalService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly RentalController _controller;

        public RentalControllerTests()
        {
            _mockRentalService = new Mock<IRentalService>();
            _mockMapper = new Mock<IMapper>();
            _controller = new RentalController(_mockRentalService.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Post_ValidData_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var rentalDto = new RentalDto { /* inicialize com dados válidos */ };
            var rental = new Rental { Id = Guid.NewGuid() };
            _mockMapper.Setup(m => m.Map<Rental>(rentalDto)).Returns(rental);
            _mockMapper.Setup(m => m.Map<RentalDto>(rental)).Returns(rentalDto);
            _mockRentalService.Setup(s => s.AddAsync(It.IsAny<Rental>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Post(rentalDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(nameof(RentalController.Get), createdAtActionResult.ActionName);
            Assert.Equal(rental.Id, createdAtActionResult.RouteValues["id"]);
        }

        [Fact]
        public async Task Get_ReturnsOkResultWithRentals()
        {
            // Arrange
            var rentals = new List<Rental> { new Rental(), new Rental() };
            var rentalDtos = new List<RentalDto> { new RentalDto(), new RentalDto() };
            _mockRentalService.Setup(s => s.GetAllAsync()).ReturnsAsync(rentals);
            _mockMapper.Setup(m => m.Map<IEnumerable<RentalDto>>(rentals)).Returns(rentalDtos);

            // Act
            var result = await _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<RentalDto>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }


        [Fact]
        public async Task Put_ValidData_ReturnsOkResult()
        {
            // Arrange
            var id = Guid.NewGuid();
            var rentalDto = new RentalDto { /* inicialize com dados válidos */ };
            var rental = new Rental { Id = id };
            _mockRentalService.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(rental);
            _mockMapper.Setup(m => m.Map<Rental>(rentalDto)).Returns(rental);
            _mockMapper.Setup(m => m.Map<RentalDto>(rental)).Returns(rentalDto);
            _mockRentalService.Setup(s => s.UpdateAsync(It.IsAny<Rental>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Put(id, rentalDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.IsType<RentalDto>(okResult.Value);
        }

        [Fact]
        public async Task Delete_ExistingRental_ReturnsNoContent()
        {
            // Arrange
            var id = Guid.NewGuid();
            var rental = new Rental { Id = id };
            _mockRentalService.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(rental);
            _mockRentalService.Setup(s => s.DeleteAsync(id)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(id);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void CreateRental_ValidRequest_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var request = new CreateRentalRequest
            {
                MotorcycleId = Guid.NewGuid(),
                DeliverymanId = Guid.NewGuid(),
                RentalPlan = RentalPlan.ThirtyDays
            };
            var rental = new Rental { Id = Guid.NewGuid() };
            _mockRentalService.Setup(s => s.CreateRental(request.MotorcycleId, request.DeliverymanId, request.RentalPlan)).Returns(rental);

            // Act
            var result = _controller.CreateRental(request);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(RentalController.GetRental), createdAtActionResult.ActionName);
            Assert.Equal(rental.Id, createdAtActionResult.RouteValues["id"]);
        }

        [Fact]
        public void ReturnMotorcycle_ValidRequest_ReturnsOkResultWithTotalCost()
        {
            // Arrange
            var id = Guid.NewGuid();
            var request = new ReturnMotorcycleRequest { ReturnDate = DateTime.Now };
            var rental = new Rental { Id = id };
            _mockRentalService.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(rental);
            _mockRentalService.Setup(s => s.CalculateTotalCost(rental, request.ReturnDate)).Returns(100.0m);

            // Act
            var result = _controller.ReturnMotorcycle(id, request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<decimal>(okResult.Value);
            Assert.Equal(100.0m, 100.0m);
        }
    }
}