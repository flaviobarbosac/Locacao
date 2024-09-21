﻿using Microsoft.AspNetCore.Mvc;
using Locacao.Domain.Model;
using AutoMapper;
using Locacao.Dto;
using Swashbuckle.AspNetCore.Annotations;

namespace Locacao.Controllers
{
    /// <summary>
    /// Controller for managing rentals.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "v1")]
    public class RentalController : ControllerBase
    {
        private readonly Services.Inteface.IBaseServices<Rental> _services;
        private readonly IMapper _mapper;

        public RentalController(Services.Inteface.IBaseServices<Rental> services, IMapper mapper)
        {
            _services = services;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new rental.
        /// </summary>
        /// <param name="data">The rental data.</param>
        /// <returns>The newly created rental.</returns>
        /// <response code="201">Returns the newly created rental.</response>
        /// <response code="400">If the data is invalid.</response>
        [HttpPost]
        [ProducesResponseType(typeof(RentalDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid data", typeof(ErrorResponse))]
        public async Task<ActionResult<RentalDto>> Post([FromBody] RentalDto data)
        {
            try
            {
                var rental = _mapper.Map<Rental>(data);
                await _services.AddAsync(rental);
                var rentalCreatedDto = _mapper.Map<RentalDto>(rental);
                return CreatedAtAction(nameof(Get), new { id = rentalCreatedDto.Id }, rentalCreatedDto);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse { Message = $"Invalid data: {ex.Message}" });
            }
        }

        /// <summary>
        /// Retrieves all rentals.
        /// </summary>
        /// <returns>A list of all rentals.</returns>
        /// <response code="200">Returns the list of rentals.</response>
        /// <response code="500">If there was an internal server error.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<RentalDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<RentalDto>>> Get()
        {
            try
            {
                var result = await _services.GetAllAsync();
                var rentalDtos = _mapper.Map<IEnumerable<RentalDto>>(result);
                return Ok(rentalDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse { Message = $"An error occurred while processing your request: {ex.Message}" });
            }
        }

        /// <summary>
        /// Retrieves a specific rental by id.
        /// </summary>
        /// <param name="id">The id of the rental to retrieve.</param>
        /// <returns>The requested rental.</returns>
        /// <response code="200">Returns the requested rental.</response>
        /// <response code="404">If the rental is not found.</response>
        /// <response code="500">If there was an internal server error.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RentalDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RentalDto>> Get(Guid id)
        {
            try
            {
                var rental = await _services.GetByIdAsync(id);
                if (rental == null)
                {
                    return NotFound(new ErrorResponse { Message = "Rental not found" });
                }
                var rentalDto = _mapper.Map<RentalDto>(rental);
                return Ok(rentalDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse { Message = $"An error occurred while processing your request: {ex.Message}" });
            }
        }

        /// <summary>
        /// Updates an existing rental.
        /// </summary>
        /// <param name="id">The id of the rental to update.</param>
        /// <param name="data">The updated rental data.</param>
        /// <returns>The updated rental.</returns>
        /// <response code="200">Returns the updated rental.</response>
        /// <response code="400">If the data is invalid.</response>
        /// <response code="404">If the rental is not found.</response>
        /// <response code="500">If there was an internal server error.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(RentalDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RentalDto>> Put(Guid id, [FromBody] RentalDto data)
        {
            try
            {
                var existingRental = await _services.GetByIdAsync(id);
                if (existingRental == null)
                {
                    return NotFound(new ErrorResponse { Message = "Rental not found" });
                }

                var rental = _mapper.Map<Rental>(data);
                rental.Id = id;
                await _services.UpdateAsync(rental);
                var updatedRentalDto = _mapper.Map<RentalDto>(rental);
                return Ok(updatedRentalDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse { Message = $"An error occurred while processing your request: {ex.Message}" });
            }
        }

        /// <summary>
        /// Deletes a specific rental.
        /// </summary>
        /// <param name="id">The id of the rental to delete.</param>
        /// <returns>No content.</returns>
        /// <response code="204">If the rental was successfully deleted.</response>
        /// <response code="404">If the rental is not found.</response>
        /// <response code="500">If there was an internal server error.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var rental = await _services.GetByIdAsync(id);
                if (rental == null)
                {
                    return NotFound(new ErrorResponse { Message = "Rental not found" });
                }

                await _services.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse { Message = $"An error occurred while processing your request: {ex.Message}" });
            }
        }
    }
}